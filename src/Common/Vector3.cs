using System;
using System.Runtime.Remoting.Messaging;

namespace Common
{
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3();
        public static readonly Vector3 One = new Vector3(1);
        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);

        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 xy, float z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }

        public Vector3(float xyz)
        {
            X = Y = Z = xyz;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public Vector3 Normalize()
        {
            return this/Length();
        }

        public static Vector3 Normalize(Vector3 vector)
        {
            return vector.Normalize();
        }

        public static float Length(Vector2 vector)
        {
            return vector.Length();
        }

        public static float Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

       public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4x4 worldViewProjection)
       {
           var coordinate = new Vector3();
           var result1 = Matrix4x4.Invert(worldViewProjection);

           coordinate.X = (float)((vector.X - (double)x) / width * 2.0 - 1.0);
           coordinate.Y = (float)-((vector.Y - (double)y) / height * 2.0 - 1.0);
           coordinate.Z = (float)((vector.Z - (double)minZ) / (maxZ - (double)minZ));
           return TransformCoordinate(coordinate, result1);
       }

       public static Vector3 TransformCoordinate(Vector3 coordinate, Matrix4x4 transform)
       {
           var vector = new Vector4
           {
               X =
                   (float)
                       (coordinate.X*(double) transform.Column1.X + coordinate.Y*(double) transform.Column1.Y +
                        coordinate.Z*(double) transform.Column1.Z) + transform.Column1.W,
               Y =
                   (float)
                       (coordinate.X*(double) transform.Column2.X + coordinate.Y*(double) transform.Column2.Y +
                        coordinate.Z*(double) transform.Column2.Z) + transform.Column2.W,
               Z =
                   (float)
                       (coordinate.X*(double) transform.Column3.X + coordinate.Y*(double) transform.Column3.Y +
                        coordinate.Z*(double) transform.Column3.Z) + transform.Column3.W,
               W =
                   (float)
                       (1.0/
                        (coordinate.X*(double) transform.Column4.X +
                         coordinate.Y*(double) transform.Column4.Y +
                         coordinate.Z*(double) transform.Column4.Z + transform.Column4.W))
           };
           return new Vector3(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W);
       }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.Y * vector2.Z - vector1.Z * vector2.Y, vector1.Z * vector2.X - vector1.X * vector2.Z, vector1.X * vector2.Y - vector1.Y * vector2.X);
        }

        public static Vector3 operator /(Vector3 vector, float scalar)
        {
            return new Vector3(vector.X / scalar, vector.Y / scalar, vector.Z / scalar);
        }

        public static Vector3 operator *(Vector3 vector, float scalar)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator *(float scalar, Vector3 vector)
        {
            return new Vector3(vector.X * scalar, vector.Y * scalar, vector.Z * scalar);
        }

        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.X + vector2.X, vector1.Y + vector2.Y, vector1.Z + vector2.Z);
        }

        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            return new Vector3(vector1.X - vector2.X, vector1.Y - vector2.Y, vector1.Z - vector2.Z);
        }

        public static Vector3 operator -(Vector3 vector)
        {
            return new Vector3(-vector.X, -vector.Y, -vector.Z);
        }

        public float LengthSquared()
        {
            return X*X + Y*Y + Z*Z;
        }
    }
}
