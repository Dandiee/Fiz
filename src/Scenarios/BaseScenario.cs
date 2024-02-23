using System;
using Common;
using Physics;
using Physics.Bodies;
using Physics.Helpers;
using Physics.Joints;

namespace Scenarios
{
    public abstract class BaseScenario
    {
        protected World World { get; private set; }
        protected Body Ground { get; set; }
        protected Random Random { get; private set; }

        protected BaseScenario(World world, bool useDefaultGround = true)
        {
            World = world;
            Random = new Random();
            
            if (useDefaultGround)
            {
                Ground = BodyBuilder.BuildBox(new Vector2(100, 20), new Vector2(0, -10), BodyDefinition.DefaultLocked);
                Add(Ground);
            }
        }

        protected void Add(Joint joint)
        {
            World.Add(joint);
        }

        protected void Add(Body body)
        {
            World.Add(body);
        }

        protected void Clear()
        {
            World.Clear();
        }
    }
}
