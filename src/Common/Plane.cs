namespace Common
{
    public struct Plane
    {
        public Vector3 Normal;
        public float D;

        public Plane(Vector3 normal, float d)
        {
            Normal = normal;
            D = d;
        }

    }
}
