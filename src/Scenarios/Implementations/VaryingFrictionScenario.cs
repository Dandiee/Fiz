using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public sealed class VaryingFrictionScenario : BaseScenario
    {
        public VaryingFrictionScenario(World world)
            : base(world)
        {
            InitializeObjects();
        }

        private void InitializeObjects()
        {

            

            var slider1 = BodyBuilder.BuildBox(new Vector2(13.0f, 0.25f), new Vector2(-2.0f, 11.0f),
                BodyDefinition.DefaultLocked.WithRotation(-0.25f).WithFriction(0.1f));
            
            var slider2 = BodyBuilder.BuildBox(new Vector2(13.0f, 0.25f), new Vector2(2.0f, 7.0f), 
                BodyDefinition.DefaultLocked.WithRotation(0.25f).WithFriction(0.15f));
            
            var slider3 = BodyBuilder.BuildBox(new Vector2(13.0f, 0.25f), new Vector2(-2.0f, 3.0f), 
                BodyDefinition.DefaultLocked.WithRotation(-0.25f));
            
            var wall1 = BodyBuilder.BuildBox(new Vector2(0.25f, 1.0f), new Vector2(5.25f, 9.5f), BodyDefinition.DefaultLocked);
            var wall2 = BodyBuilder.BuildBox(new Vector2(0.25f, 1.0f), new Vector2(-5.25f, 5.5f), BodyDefinition.DefaultLocked);
            
            var friction = new[] { 1, 0.5f, 0.3f, 0.1f, 0.0f };
            for (var i = 0; i < 5; ++i)
            {
                var box = BodyBuilder.BuildBox(new Vector2(0.5f, 0.5f), new Vector2(-7.5f + 2.0f*i, 14.0f),
                    BodyDefinition.Default.WithFriction(friction[i]).WithRotation(-0.25f).WithRestitution(0));
            
                Add(box);
            }
            
            Add(slider1);
            Add(slider2);
            Add(slider3);
            Add(wall1);
            Add(wall2);
        }
    }
}
