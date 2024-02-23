using Common;

namespace Physics.Collisions.Polygons.Clipping
{
    public struct ClipingEdgeResult
    {
        public ClippingVertexResult V1;
        public ClippingVertexResult V2;

        public Vector2 Normal;
        public Vector2 Direction;

        public ClipingEdgeResult(ClippingVertexResult v1, ClippingVertexResult v2, Vector2 normal, Vector2 direction)
        {
            V1 = v1;
            V2 = v2;
            Normal = normal;
            Direction = direction;
        }
    }
}
