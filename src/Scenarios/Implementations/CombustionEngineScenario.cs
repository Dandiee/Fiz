using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public class CombustionEngineScenario : BaseScenario
    {
        public int CylinderCount { get; private set; }
        public Vector2 WinchPoint { get; private set; }

        private Tuple<Polygon, float, Vector2>[] _mapping;
        private Polygon _lastCylinder;

        public CombustionEngineScenario(World world)
            : base(world, false)
        {
            AddCylinder();
        }

        private void AddGround()
        {
            Clear();
            Ground = BodyBuilder.BuildBox(new Vector2(100, 10), new Vector2(0, -20), BodyDefinition.DefaultLocked);
            Add(Ground);
        }

        private void CreateStarEngine(int cylinderCount)
        {
            const float winchLength = 2f;
            const float rodLength = 10f;

            _mapping = new Tuple<Polygon, float, Vector2>[cylinderCount];

            var winchOrigoPoint = Vector2.Zero;
            var winchDirection = Vector2.Rotate(0.123f, Vector2.UnitY);
            var winchPoint = winchOrigoPoint + winchDirection * winchLength;
            var alphaStep = MathUtil.TwoPi/cylinderCount;

            // winch
            var winch = BodyBuilder.BuildCircle(winchOrigoPoint, winchLength);
            Add(winch);
            Add(new RevoluteJoint(Ground, winch, winchOrigoPoint));
            Add(new AngularMotorJoint(Ground, winch)
            {
                MaximumMotorTorque = 10000000,
                MotorSpeed = MathUtil.Pi
            });

            winch.Rotating += (sender, args) =>
            {
                return;
                var prevAlpha = MathUtil.ToTwoPi(args.PreviousRotation);
                var newAlpha = MathUtil.ToTwoPi(args.NewRotation);

                var cylinders = _mapping.Where(m => m.Item2 >= prevAlpha && m.Item2 <= newAlpha).ToList();

                if (cylinders.Count == 1)
                {
                    var targetCylinder = cylinders[0];
                    if (targetCylinder.Item1 != _lastCylinder)
                    {
                        targetCylinder.Item1.Force = -targetCylinder.Item3*50000;
                        _lastCylinder = targetCylinder.Item1;
                    }
                }
                else if (cylinders.Count > 1)
                {
                    
                }



            };

            for (var i = 0; i < cylinderCount; i++)
            {
                var alpha = alphaStep*i;
                var cylinderOrientation = Vector2.Rotate(alphaStep*i, Vector2.UnitY);
                var tangentOrientation = Vector2.Cross(1, cylinderOrientation);

                var intersectionDistance = MathUtil.RayIntersectsSphere1(winchOrigoPoint, cylinderOrientation, winchPoint, rodLength).Value;
                if (intersectionDistance < 0)
                    intersectionDistance += rodLength * 2;

                var cylinderPinPoint = winchOrigoPoint + cylinderOrientation * intersectionDistance;
                var cylinderTop = winchOrigoPoint + cylinderOrientation * (rodLength + winchLength + 2.06f);

                // cylinder
                var cylinder = BodyBuilder.BuildBar(cylinderPinPoint, cylinderPinPoint + cylinderOrientation*2, new Vector2(.5f, 0.05f), BodyDefinition.Default.WithFriction(0));
                cylinder.Tag = new List<float>();
                Add(cylinder);
                Add(new LineJoint(Ground, cylinder, Ground.ToLocal(winchOrigoPoint), Vector2.Zero, cylinderOrientation));
                Add(new AngleJoint(Ground, cylinder));
                _mapping[i] = new Tuple<Polygon, float, Vector2>(cylinder, winch.Rotation + alpha, cylinderOrientation);
                
                cylinder.Moving += (sender, args) =>
                {
                    //var oldT = (List<float>) cylinder.Tag;
                    //var t = Vector2.Dot(cylinder.Position - winchOrigoPoint, cylinderOrientation);
                };

                // rod
                var rod = BodyBuilder.BuildBar(winchPoint, cylinderPinPoint, new Vector2(0.2f), BodyDefinition.DefaultCollisionImmune);
                Add(rod);
                Add(new RevoluteJoint(cylinder, rod, cylinderPinPoint));
                Add(new RevoluteJoint(winch, rod, winchPoint));

                // cylinder cap
                var cylinderCap = BodyBuilder.BuildBar(cylinderTop, cylinderTop + cylinderOrientation*0.2f, new Vector2(.5f, 0.05f), BodyDefinition.DefaultLocked);
                Add(cylinderCap);

                // left wall
                var q1 = cylinderTop + tangentOrientation*0.5f;
                Add(BodyBuilder.BuildSegment(q1, q1 - cylinderOrientation*6));

                // right wall
                var q2 = cylinderTop - tangentOrientation*0.5f;
                Add(BodyBuilder.BuildSegment(q2, q2 - cylinderOrientation*6));
            }
        }

        public void AddCylinder()
        {
            CylinderCount++;
            AddGround();
            CreateStarEngine(CylinderCount);
        }

        public void RemoveCylinder()
        {
            if (CylinderCount > 1)
                CylinderCount--;

            AddGround();
            CreateStarEngine(CylinderCount);
        }
    }
}
