namespace Common
{
    public struct Vector4
    {
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);

        public static readonly Vector4 One = new Vector4(1);

        public float X;
        public float Y;
        public float Z;
        public float W;

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector3 xyz, float w)
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
            W = w;
        }

        public Vector4(float xyzw)
        {
            X = Y = Z = W = xyzw;
        }

        public static Vector4 operator *(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X * vector2.X, vector1.Y * vector2.Y, vector1.Z * vector2.Z, vector1.W * vector2.W);
        }

        public static Vector4 operator /(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X / vector2.X, vector1.Y / vector2.Y, vector1.Z / vector2.Z, vector1.W / vector2.W);
        }

        public static Vector4 operator +(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z, vector1.W + vector2.W);
        }

        public static Vector4 operator -(Vector4 vector1, Vector4 vector2)
        {
            return new Vector4(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z, vector1.W - vector2.W);
        }

        public static Vector4 operator -(Vector4 vector)
        {
            return new Vector4(-vector.X, -vector.Y, -vector.Z , -vector.W);
        }

        public static bool operator ==(Vector4 vector1, Vector4 vector2)
        {
            return vector1.X == vector2.X &&
                   vector1.Y == vector2.Y &&
                   vector1.Z == vector2.Z &&
                   vector1.W == vector2.W;
        }

        public static bool operator !=(Vector4 vector1, Vector4 vector2)
        {
            return !(vector1 == vector2);
        }
    }
}
