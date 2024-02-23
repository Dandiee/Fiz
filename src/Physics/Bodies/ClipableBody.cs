using Common;

namespace Physics.Bodies
{
    public abstract class ClipableBody : Body
    {
        public Vector2[] GlobalVertices { get; protected set; }
        public Vector2[] LocalVertices { get; protected set; }
    }
}
