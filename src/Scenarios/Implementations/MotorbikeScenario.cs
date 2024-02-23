using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;
using Scenarios.Helpers;

namespace Scenarios.Implementations
{
    public sealed class MotorbikeScenario : BaseScenario
    {
        public Body BikeFrame { get; private set; }
        private Body _frontWheel;
        private Body _backWheel;

        private AngularMotorJoint _engine;

        public MotorbikeScenario(World world)
            : base(world, false)
        {
            CreateBike();
            AddObjects();
            CreateTerrain();
        }

        #region Creation
        private void CreateBike()
        {
            var offset = new Vector2(0, 50);

            // Body
            BikeFrame = BodyBuilder.BuildIrregularPolygon(new[]
            {
                new Vector2(-4, 1) + offset, 
                new Vector2(2, 2.5f) + offset, 
                new Vector2(3, 0.5f) + offset,
                new Vector2(1, -2.5f) + offset,
                new Vector2(-1, -2.5f) + offset
            }, BodyDefinition.Default.WithDensity(10));

            var backMudguard = BodyBuilder.BuildIrregularPolygon(new[]
            {
                new Vector2(-4.01f, 1) + offset, 
                new Vector2(-6.2f, 1) + offset, 
                new Vector2(-6f, 0.8f) + offset, 
                new Vector2(-3.3f, 0.2f) + offset 
            });

            var frontMudguard = BodyBuilder.BuildIrregularPolygon(new[]
            {
                new Vector2(3, 0.5f) + offset, 
                new Vector2(6, 0.8f) + offset, 
                new Vector2(2.7f, 1.1f) + offset 
            });

            var windscreen = BodyBuilder.BuildIrregularPolygon(new[]
            {
                new Vector2(2, 2.5f) + offset, 
                new Vector2(1.8f, 3.5f) + offset, 
                new Vector2(1.5f, 2.35f) + offset
            });


            _backWheel = BodyBuilder.BuildCircle(new Vector2(-4.5f, -3.5f) + offset, 2, BodyDefinition.Default.WithFriction(5));
            _frontWheel = BodyBuilder.BuildCircle(new Vector2(5, -3.5f) + offset, 2, BodyDefinition.Default.WithFriction(1));

            Add(BikeFrame);
            Add(_frontWheel);
            Add(_backWheel);
            Add(backMudguard);
            Add(frontMudguard);
            Add(windscreen);

            Add(new WeldJoint(BikeFrame, backMudguard));
            Add(new WeldJoint(BikeFrame, frontMudguard));
            Add(new WeldJoint(BikeFrame, windscreen));

            // Front wheel suspension
            var frontSuspensionPoint = BikeFrame.ToLocal(new Vector2(3, 0.5f) + offset);
            var frontSuspensionDirection = Vector2.Normalize(new Vector2(2, 2.5f) - new Vector2(3, 0.5f));
            var frontWheelLineJoint = new LineJoint(BikeFrame, _frontWheel, frontSuspensionPoint, Vector2.Zero, frontSuspensionDirection);
            var frontWheelDistanceJoint = new DistanceJoint(BikeFrame, _frontWheel, frontSuspensionPoint, Vector2.Zero, 3, .5f)
            {
                IsVisible = true
            };

            Add(frontWheelLineJoint);
            Add(frontWheelDistanceJoint);

            // Back wheel suspension
            var backSuspensionPoint = BikeFrame.ToLocal(new Vector2(-0.5f, -2f) + offset);
            var backTopSuspensionPoint = BikeFrame.ToLocal(new Vector2(-3, 0) + offset);
            var backWheelHorizontalDistanceJoint = new DistanceJoint(BikeFrame, _backWheel, backSuspensionPoint, Vector2.Zero)
            {
                IsVisible = true
            };

            var backWheelVerticalDistanceJoint = new DistanceJoint(BikeFrame, _backWheel, backTopSuspensionPoint, Vector2.Zero, 3, .5f);
            Add(backWheelHorizontalDistanceJoint);
            Add(backWheelVerticalDistanceJoint);

            // Engine
            _engine = new AngularMotorJoint(BikeFrame, _backWheel);
            Add(_engine);
        }

        private void CreateTerrain()
        {
            foreach (var segment in SegmentHelper.GetRandomSegmentGround(-400, 400, 3, 3))
                Add(segment);
        }

        private void AddObjects()
        {
            for (var i = 0; i < 25; i++)
            {
                Add(BodyBuilder.BuildBox(new Vector2(Random.NextFloat(0.2f, 0.8f)), new Vector2(Random.NextFloat(-100, 100), 20)));
            }

            for (var i = 0; i < 25; i++)
            {
                Add(BodyBuilder.BuildCircle(new Vector2(Random.NextFloat(-100, 100), 20), Random.NextFloat(0.1f, 0.8f)));
            }
        }
        #endregion

        #region Handlers
        public void RotateRight()
        {
            BikeFrame.Torque = -100000;
        }

        public void RotateLeft()
        {
            BikeFrame.Torque = 100000;
        }

        public void Accelerate()
        {
            if (_engine != null)
            {
                _engine.MaximumMotorTorque = 14500;
                _engine.MotorSpeed = -40;
            }
        }

        public void Idling()
        {
            if (_engine != null)
            {
                _engine.MaximumMotorTorque = 0;
                _engine.MotorSpeed = -4;
            }
        }

        public void Break()
        {
            if (_engine != null)
            {
                _engine.MaximumMotorTorque = 10000;
                _engine.MotorSpeed = 0;
            }
        }
        #endregion
    }
}
