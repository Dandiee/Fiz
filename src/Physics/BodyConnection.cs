using System;
using Common;
using Physics.Bodies;
using Physics.Collisions;
using Physics.Collisions.Manifolds;

namespace Physics
{
    public partial class BodyConnection
    {
        public Manifold Manifold { get; set; }

        public Body Body1 { get; private set; }
        public Body Body2 { get; private set; }
        public float Friction { get; private set; }
        public float Restitution { get; private set; }

        public BodyConnection(Body b1, Body b2)
        {
            Body1 = b1;
            Body2 = b2;

            Friction = (float)Math.Sqrt(Body1.Friction * Body2.Friction);
            Restitution = Math.Min(Body1.Restitution, Body2.Restitution);
        }
        
        public Manifold GetManifold()
        {
            return Collision.Detect(Body1, Body2);
        }

        public void InitializeVelocityConstraints()
        {
            Manifold.UpdateNormal(Body1, Body2);
            foreach (var point in Manifold.Points)
            {
                point.R1 = point.GlobalVertex - Body1.Position;
                point.R2 = point.GlobalVertex - Body2.Position;
                
                point.ContactMass = GetMass(point.R1, point.R2, Manifold.Normal, 1);
                point.FrictionMass = GetMass(point.R1, point.R2, Manifold.Tangent, 1);

                var vRel = Vector2.Dot(Manifold.Normal, Body2.Velocity + Vector2.Cross(Body2.AngularVelocity, point.R2) - Body1.Velocity - Vector2.Cross(Body1.AngularVelocity, point.R1));
                point.VelocityBias = vRel < -Settings.VelocityThreshold ? -Restitution*vRel : 0;
            }

            // TODO: Eron Catto féle BlockSolver
            if (Manifold.Points.Count == 2 && Settings.IsBlockSolverEnabled)
            {
                InitializeBlockVelocityConstraints();
            }
        }

        public void WarmStart()
        {
            foreach (var point in Manifold.Points)
            {
                ApplyImpulse(point, point.ContactImpulse * Manifold.Normal + point.FrictionImpulse * Manifold.Tangent);
            }
        }

        public void SolveVelocityConstraints()
        {
            // solve friction constraint
            foreach (var point in Manifold.Points)
            {
                var lambda = GetLambda(point, Manifold.Tangent, point.FrictionMass, 0);
                var maxFriction = Friction * point.ContactImpulse;
                
                var newImpulse = MathUtil.Clamp(point.FrictionImpulse - lambda, -maxFriction, maxFriction);
                lambda = newImpulse - point.FrictionImpulse;
                point.FrictionImpulse = newImpulse;

                ApplyImpulse(point, lambda * Manifold.Tangent);
            }

            // solve contact constraint
            if (Manifold.Points.Count == 1 || Settings.IsBlockSolverEnabled == false)
            {
                foreach (var point in Manifold.Points)
                {
                    var lambda = GetLambda(point, Manifold.Normal, point.ContactMass, point.VelocityBias);

                    var newImpulse = Math.Max(point.ContactImpulse - lambda, 0.0f);
                    lambda = newImpulse - point.ContactImpulse;
                    point.ContactImpulse = newImpulse;

                    ApplyImpulse(point, lambda * Manifold.Normal);
                }
            }
            else
            {
                SolveBlockVelocityConstraints(Manifold.Normal);
            }
        }

        public bool SolvePositionConstraints()
        {
            var minSeparation = 0f;
            
            foreach (var maniPoint in Manifold.Points)
            {
                var currentCollisionPoint = Manifold.IncidentBody.ToGlobal(maniPoint.LocalVertex);
                var currentNormal = -Vector2.Rotate(Manifold.ReferenceBody.RotationVector, Manifold.ReferenceEdgeLocalNormal);
                var currentSeparation = Vector2.Dot(currentCollisionPoint - Manifold.ReferenceBody.ToGlobal(Manifold.ReferenceEdgeLocalMiddlePoint), currentNormal);

                var r1 = currentCollisionPoint - Body1.Position;
                var r2 = currentCollisionPoint - Body2.Position;

                var c = MathUtil.Clamp(Settings.Baumgarte*(currentSeparation + Settings.LinearSlop), -Settings.MaxLinearCorrection, 0.0f);
                var p = GetMass(r1, r2, currentNormal, -c) * currentNormal * (Manifold.IsFlipped ? -1 : 1);

                Body1.AddSpatials(-Body1.InverseMass*p, -Body1.InverseInertia*Vector2.Cross(r1, p));
                Body2.AddSpatials( Body2.InverseMass*p,  Body2.InverseInertia*Vector2.Cross(r2, p));
                //Body1.Position -= Body1.InverseMass * p;
                //Body2.Position += Body2.InverseMass * p;
                //Body1.Rotation -= Body1.InverseInertia * Vector2.Cross(r1, p);
                //Body2.Rotation += Body2.InverseInertia * Vector2.Cross(r2, p);

                minSeparation = Math.Min(minSeparation, currentSeparation);
            }

            return minSeparation >= -3.0f * Settings.LinearSlop;
        }
    }
}
