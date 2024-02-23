using System.Collections.Generic;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public class SuspendedVehicleScenario: BaseScenario
    {
        public SuspendedVehicleScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            

            //Add(new Circle(new Vector2(0, 3), 1));
            //return;

            const float yPos = 5f;
            const float wheelRadius = 0.5f;
            const float suspensionHeight = 0.5f;
            const float wheelOffset = 0.1f;
            const float wheelsCount = 40;
            const float bodyLengthOversize = 0.5f;
            const int bodyHeight = 4;

            Add(BodyBuilder.BuildIrregularPolygon(new List<Vector2>() { new Vector2(1, 0), new Vector2(0, 0.4f), new Vector2(-1, 0) }, BodyDefinition.DefaultLocked));

            var bodySize =
                new Vector2(wheelsCount*(wheelRadius*2) + (wheelsCount - 1)*(wheelOffset) + bodyLengthOversize*2,
                    bodyHeight);

            var vehicleBody = BodyBuilder.BuildBox(bodySize, Vector2.UnitY * yPos, BodyDefinition.Default.WithDensity(100));

            var wheelCenterHeight = yPos - bodyHeight/2f - suspensionHeight - wheelRadius;
            var fromHorizontal = (bodySize.X/-2f) + bodyLengthOversize;
            var step = wheelRadius*2 + wheelOffset;

            for (var i = 0; i <= wheelsCount; i++)
            {
                var wheelPos = new Vector2(fromHorizontal + i*step, wheelCenterHeight);
                var wheel = BodyBuilder.BuildCircle(wheelPos, wheelRadius);
                var globalAnchor = wheelPos + Vector2.UnitY*(wheelRadius + suspensionHeight);
                var localAnchor = vehicleBody.ToLocal(globalAnchor);
                Add(wheel);
                Add(new LineJoint(vehicleBody, wheel, localAnchor, Vector2.Zero, Vector2.UnitY));
                Add(new DistanceJoint(vehicleBody, wheel, localAnchor, Vector2.Zero, 15, 1));
                //Add(new DaniMotorJoint(vehicleBody, wheel){ MaximumMotorTorque = 100, MotorSpeed = 1});
                var globalAnchorL = wheelPos + Vector2.UnitY * (wheelRadius + suspensionHeight) - Vector2.UnitX * 1f;
                var globalAnchorR = wheelPos + Vector2.UnitY * (wheelRadius + suspensionHeight) + Vector2.UnitX * 1f;

                var localAncorL = vehicleBody.ToLocal(globalAnchorL);
                var localAncorR = vehicleBody.ToLocal(globalAnchorR);

                //Add(new DistanceJoint(vehicleBody, wheel, localAncorL, Vector2.Zero, 10, 1));
                //Add(new DistanceJoint(vehicleBody, wheel, localAncorR, Vector2.Zero, 10, 1));
            }

            Add(vehicleBody);
        }
    }
}
