using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class VerticalCircleStack : BaseScenario
    {
        public VerticalCircleStack(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            

            for (var i = 0; i < 20; i++)
            {
                var circle = BodyBuilder.BuildCircle(new Vector2(0, 0.2f + i * 1.1f), 0.5f);
                Add(circle);
            }
        }
    }
}
