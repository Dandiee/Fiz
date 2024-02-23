using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class VaryingRestitutionScenario : BaseScenario
    {
        private const int BallCount = 100;
        private const float Radius = 0.5f;
        private const float Distance = 0.2f;
        private const float Height = 10f;
        

        public VaryingRestitutionScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {
            

            //Ground = new Polygon(GroundSize, new Vector2(0, -GroundSize.Y / 2f), BodyDefinition.DefaultLocked.WithRestitution(1));
            Add(Ground);

            //Add(new Polygon(GroundSize, new Vector2(0, -GroundSize.Y / 2f), BodyDefinition.DefaultLocked));

            const float step = 1f/BallCount;
            const float xStart = ((BallCount - 1)*(Radius*2 + Distance)/-2f);
            for (var i = 0; i < BallCount; i++)
            {
                var ball = BodyBuilder.BuildCircle(new Vector2(xStart + i*(Radius*2 + Distance), Height), Radius, 
                    BodyDefinition.Default.WithRestitution(i*step));
                Add(ball);
            }
        }
    }
}
