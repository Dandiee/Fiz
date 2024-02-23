using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios.Implementations
{
    public sealed class DominoScenario : BaseScenario
    {
        public DominoScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            for (var i = 0; i < 13; ++i)
            {
                Add(BodyBuilder.BuildBox(new Vector2(0.2f, 2.0f), new Vector2(-6.0f + 1.0f*i, 12.125f)));
            }

            var topSlider = BodyBuilder.BuildBox(new Vector2(14.0f, 0.5f), new Vector2(1.0f, 6.0f), BodyDefinition.DefaultLocked);
            var box = BodyBuilder.BuildBox(new Vector2(2.0f, 2.0f), new Vector2(6.0f, 2.5f));

            var dominoShelf = BodyBuilder.BuildBox(new Vector2(12.0f, 0.5f), new Vector2(-1.5f, 10.0f), BodyDefinition.DefaultLocked);
            var verticalWall = BodyBuilder.BuildBox(new Vector2(0.5f, 3.0f), new Vector2(-7.0f, 4.0f), BodyDefinition.DefaultLocked);
            var breakingBall = BodyBuilder.BuildRegularPolygon(8, 0.5f, new Vector2(-10.0f, 15.0f));

            var teeter = BodyBuilder.BuildBox(new Vector2(12.0f, 0.25f), new Vector2(-0.9f, 1.0f));
            var boxCap = BodyBuilder.BuildBox(new Vector2(2.0f, 0.2f), new Vector2(6.0f, 3.6f));

            Add(new RevoluteJoint(Ground, teeter, new Vector2(-2.0f, 1.0f)));
            Add(new RevoluteJoint(Ground, box, new Vector2(6.0f, 2.6f)));
            Add(new RevoluteJoint(verticalWall, breakingBall, new Vector2(-7.0f, 15.0f)));

            Add(dominoShelf);
            Add(breakingBall);
            Add(teeter);
            Add(box);
            Add(boxCap);
            Add(topSlider);
            Add(verticalWall);

            Add(new RevoluteJoint(box, boxCap, new Vector2(7.0f, 3.5f)));
        }
    }
}
