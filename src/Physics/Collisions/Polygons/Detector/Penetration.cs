using Common;

namespace Physics.Collisions.Polygons.Detector
{
    public struct Penetration
    {
        public Vector2 Normal;
        public float Depth;

        public Penetration(Vector2 normal, float depth)
        {
            Normal = normal;
            Depth = depth;
        }
    }
}
