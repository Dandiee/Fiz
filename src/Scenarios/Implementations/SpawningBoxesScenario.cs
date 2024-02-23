using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class SpawningBoxesScenario : BaseScenario
    {
        public SpawningBoxesScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            Add(BodyBuilder.BuildSegment(new Vector2(0), new Vector2(-100, 100)));
            Add(BodyBuilder.BuildSegment(new Vector2(0), new Vector2(+100, 100)));
        }

        public void SpawnBox()
        {
            //var velcotyAngle = Random.NextFloat(-MathUtil.Pi/7f, MathUtil.Pi/7f);
            var velcotyAngle = Random.NextFloat(-1.1f, -0.8f);
            var velocity = Vector2.Rotate(velcotyAngle, Vector2.Normalize(-1, 1))*Random.NextFloat(45, 50);
            var angularVelocity = Random.NextFloat(-1, 1);
            var circle = BodyBuilder.BuildCircle(new Vector2(0, 10), 0.3f);
            circle.Velocity = velocity;
            circle.AngularVelocity = angularVelocity;
            Add(circle);
        }
    }
}
