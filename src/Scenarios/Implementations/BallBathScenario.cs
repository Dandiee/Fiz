using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class BallBathScenario : BaseScenario
    {
        public BallBathScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            var leftWall  = BodyBuilder.BuildBox(new Vector2(2, 10), new Vector2(-5, 5), BodyDefinition.DefaultLocked);
            var rightWall = BodyBuilder.BuildBox(new Vector2(2, 10), new Vector2(+5, 5), BodyDefinition.DefaultLocked);
            Add(leftWall);
            Add(rightWall);

            var ballCount = 30;
            var ballRadius = 0.3f;

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(0, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(1, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(2, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(3, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(-1, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(-2, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }

            for (var i = 0; i < ballCount; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(-3, 1f + i * 1.1f), ballRadius);
                Add(circle);
            }
  
        }
    }
}
