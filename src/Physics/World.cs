using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Physics.Bodies;
using Physics.Joints;

namespace Physics
{
    public class World
    {
        public static int IterationCounter;

        private readonly List<Body> _bodies;
        private readonly List<Joint> _joints;
        private ConcurrentBag<int> _activeBodyConnectionIndicies;
        private BodyConnection[] _bodyConnections;
        //public Tree _tree;
        public readonly Vector2 Gravity = new Vector2(0, -10);

      

        public World()
        {
            _bodies = new List<Body>();
            _joints = new List<Joint>();
            _bodyConnections = new BodyConnection[Settings.BodyConnectionCacheSize * Settings.BodyConnectionCacheSize];
            _activeBodyConnectionIndicies = new ConcurrentBag<int>();
            //_tree = new Tree();
        }

        public void Clear()
        {
            _bodies.Clear();
            _joints.Clear();
            //_tree = new Tree();
            _activeBodyConnectionIndicies = new ConcurrentBag<int>();
            _bodyConnections = new BodyConnection[Settings.BodyConnectionCacheSize * Settings.BodyConnectionCacheSize];
        }

        //public Tree GetTree()
        //{
        //    return _tree;
        //}

        public void Add(Body body)
        {
            _bodies.Add(body);

            Parallel.For(0, _bodies.Count, i =>
            {
                var iOffset = i*Settings.BodyConnectionCacheSize;
                var j = _bodies.Count - 1;
                var ij = iOffset + j;
                    
                _bodyConnections[ij] = new BodyConnection(_bodies[i], _bodies[j]);
            });

            //_tree.Add(body);
        }

        public void Add(Joint joint)
        {
            _joints.Add(joint);
        }

        public void Remove(Joint joint)
        {
            _joints.Remove(joint);
        }

        public IEnumerable<Body> GetBodies()
        {
            return _bodies.AsEnumerable();
        }

        public IEnumerable<Joint> GetJoints()
        {
            return _joints.AsEnumerable();
        }

        public IEnumerable<BodyConnection> GetBodyConnections()
        {
            return _activeBodyConnectionIndicies.Select(ij => _bodyConnections[ij]);
        }


        public void Step(float deltaTime)
        {
            deltaTime = Settings.TimeStep;

            CalculateTranslationAndOrientationChange();

            IntegrateForces(deltaTime);

            BroadPhase();

            InitializeVelocityConstraintsAndWarmStart();

            SolveVelocityConstraints();

            IntegrateVelocities(deltaTime);
            
            SolvePositionConstraints();

            // foreach(var body in _bodies)
            // {
            //     _tree.UpdateBody(body);
            // }

            IterationCounter++;
        }

        

        private void CalculateTranslationAndOrientationChange()
        {
            foreach (var body in _bodies)
            {
                body.PrevRotation = body.CurrentRotation;
                body.PrevPosition = body.CurrentPosition;

                body.CurrentRotation = body.Rotation;
                body.CurrentPosition = body.Position;

                body.TriggerChanges();
            }
        }

        public void IntegrateForces(float deltaTime)
        {
            Parallel.ForEach(_bodies, body =>
            {
                if (!body.IsLocked)
                {
                    body.Velocity += deltaTime * (Gravity + body.InverseMass * body.Force);
                    body.AngularVelocity += deltaTime * body.InverseInertia * body.Torque;
                }
            });
        }

        public void BroadPhase()
        {
            _activeBodyConnectionIndicies = new ConcurrentBag<int>();
            
            //Parallel.For(0, _bodies.Count, i =>
            for (var i = 0; i < _bodies.Count; i++)
            {
                var body1 = _bodies[i];
                var iOffset = i * Settings.BodyConnectionCacheSize;
                if (!body1.IsCollisionImmune)
                {
                    for (var j = i + 1; j < _bodies.Count; ++j)
                    {
                        var body2 = _bodies[j];
                        var ij = iOffset + j;
                        if (body2.IsCollisionImmune)
                            continue;

                        //if (body2.IsInAnyCollisionImmunityGroup(body1.GetCollisionImmunityGroups()))
                        //    continue;

                        if (body1.IsLocked && body2.IsLocked)
                        {
                            _bodyConnections[ij] = null;
                            continue;
                        }

                        var connection = _bodyConnections[ij];
                        var newManifold = connection.GetManifold();

                        if (newManifold != null)
                        {
                            _activeBodyConnectionIndicies.Add(ij);

                            if (connection.Manifold == null)
                            {
                                connection.Manifold = newManifold;
                            }
                            else
                            {
                                connection.Manifold.Update(newManifold);
                            }
                        }
                        else connection.Manifold = null;
                    }
                }

            }
            //);
        }

        public void InitializeVelocityConstraintsAndWarmStart()
        {
            Parallel.ForEach(_activeBodyConnectionIndicies, index =>
            //foreach (var index in _activeBodyConnectionIndicies)
            {
                _bodyConnections[index].InitializeVelocityConstraints();
            });


            Parallel.ForEach(_activeBodyConnectionIndicies, index =>
            //foreach (var index in _activeBodyConnectionIndicies)
            {
                _bodyConnections[index].WarmStart();
            });

            Parallel.ForEach(_joints, joint =>
            //foreach (var joint in _joints)
            {
                joint.InitializeVelocityConstraints();
            });
        }

        public void SolveVelocityConstraints()
        {
            for (var i = 0; i < Settings.VelocitySolverIterations; i++)
            {
                foreach (var joint in _joints)
                {
                    joint.SolveVelocityConstraints();
                }

                foreach (var ij in _activeBodyConnectionIndicies)
                {
                    _bodyConnections[ij].SolveVelocityConstraints();
                }
            }
        }

        public void IntegrateVelocities(float deltaTime)
        {
            foreach (var body in _bodies)
            {
                body.AddSpatials(deltaTime * body.Velocity, deltaTime * body.AngularVelocity);
                //body.Position += deltaTime * body.Velocity;
                //body.Rotation += deltaTime * body.AngularVelocity;

                body.Force = Vector2.Zero;
                body.Torque = 0;
            }
        }

        public void SolvePositionConstraints()
        {
            for (var i = 0; i < Settings.PositionSolverIterations; i++)
            {
                var isArbiterOkay = true;
                //foreach (var index in _activeBodyConnectionIndicies)
                Parallel.ForEach(_activeBodyConnectionIndicies, index =>
                {
                    isArbiterOkay &= _bodyConnections[index].SolvePositionConstraints();
                });

                var isJointsOkay = true;
                foreach (var joint in _joints)
                {
                    isJointsOkay &= joint.SolvePositionConstraints();
                }

                if (isArbiterOkay && isJointsOkay)
                    break;
            }
        }
    }
}
