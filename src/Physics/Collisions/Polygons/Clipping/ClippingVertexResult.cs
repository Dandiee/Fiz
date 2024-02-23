using Common;

namespace Physics.Collisions.Polygons.Clipping
{
    public struct ClippingVertexResult
    {
        public Vector2 Vertex;
        public int Index;
        public int? ClippedByReferenceIndex;

        public ClippingVertexResult(Vector2 vertex, int index)
        {
            Vertex = vertex;
            Index = index;
            ClippedByReferenceIndex = null;
        }
    }
}
