using Physics;
using Common;
using Physics.Helpers;

namespace Scenarios.Implementations
{
    public class BoundingVolumeHierarchyScenario : BaseScenario
    {
        public BoundingVolumeHierarchyScenario(World world, bool useDefaultGround = false) : base(world, useDefaultGround)
        {
            //world.Gravity = Vector2.Zero;   
        }

        public void CreateBox(Vector2? position)
        {
            if (position != null)
            {
                World.Add(BodyBuilder.BuildBox(new Vector2(2), position.Value));
            }
        }
    }
}
