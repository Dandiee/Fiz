using System;

namespace Common
{
    public struct Vector2
    {
        #region static instances
        public static readonly Vector2 One = new Vector2(1);
        public static readonly Vector2 Zero = new Vector2();
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);

        public static readonly Vector2 TopLeft = Normalize(new Vector2(1, -1));
        public static readonly Vector2 TopRight = Normalize(new Vector2(1, 1));
        public static readonly Vector2 BottomLeft = Normalize(new Vector2(-1, -1));
        public static readonly Vector2 BottomRight = Normalize(new Vector2(-1, 1));
        #endregion 

        public float X;
        public float Y;

        #region ctors
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2(float xy)
        {
            X = xy;
            Y = xy;
        }

        public Vector2(Vector2 vector)
        {
            X = vector.X;
            Y = vector.Y;
        }
        #endregion

        #region instance helpers
        public float Length()
        {
            return (float) Math.Sqrt(X*X + Y*Y);
        }

        public float LengthSquared()
        {
            return X*X + Y*Y;
        }
        #endregion

        #region static helpers
        public static Vector2 Swap(Vector2 vector)
        {
            return new Vector2(vector.Y, vector.X);
        }

        public static Vector2 Rotate(float angle, Vector2 vector)
        {
            var rotationVector = TrigonoUtil.RotationVector(angle);
            return new Vector2(rotationVector.Y * vector.X - rotationVector.X * vector.Y, rotationVector.X * vector.X + rotationVector.Y * vector.Y);
        }

        public static Vector2 Rotate(Vector2 rotationVector, Vector2 vector)
        {
            return new Vector2(rotationVector.Y*vector.X - rotationVector.X*vector.Y, rotationVector.X*vector.X + rotationVector.Y*vector.Y);
        }

        public static Vector2 Rotation(float angle)
        {
            return TrigonoUtil.RotationVector(angle);
        }

        public static Vector2 Normalize(Vector2 vector)
        {
            var length = vector.Length();
            return vector/length;
        }

        public static Vector2 Normalize(float x, float y)
        {
            var length = (float)Math.Sqrt(x * x + y * y);
            return new Vector2(x/length, y/length);
        }

        public static float Length(Vector2 vector)
        {
            return vector.Length();
        }

        public static float LengthSquared(Vector2 vector)
        {
            return vector.LengthSquared();
        }

        public static float Dot(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X*vector2.X + vector1.Y*vector2.Y;
        }

        public static float Determinant(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.Y - vector2.X * vector1.Y;
        }

        public static Vector2 Minimum(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.X <= vector2.X ? vector1.X : vector2.X,
                vector1.Y <= vector2.Y ? vector1.Y : vector2.Y);
        }

        public static Vector2 Maximum(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(
                vector1.X >= vector2.X ? vector1.X : vector2.X,
                vector1.Y >= vector2.Y ? vector1.Y : vector2.Y);
        }

        public static float Distance(Vector2 vector1, Vector2 vector2)
        {
            var component1 = vector1.X - vector2.X;
            var component2 = vector1.Y - vector2.Y;

            return (float)Math.Sqrt(component1 * component1 + component2 * component2);
        }

        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2 Cross(Vector2 a, float s)
        {
            return new Vector2(s * a.Y, -s * a.X);
        }

        public static Vector2 Cross(float s, Vector2 a)
        {
            return new Vector2(-s * a.Y, s * a.X);
        }

        public static Vector2 Add(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2 Substract(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public static Vector2 Multiplay(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }

        public static Vector2 Add(ref Vector2 vector1, ref Vector2 vector2)
        {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2 Substract(ref Vector2 vector1, ref Vector2 vector2)
        {
            return new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public static Vector2 Multiplay(ref Vector2 vector1, ref Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }

        public static float DistanceSquared(Vector2 vector1, Vector2 vector2)
        {
            var r1 = vector1.X - vector2.X;
            var r2 = vector1.Y - vector2.Y;

            return r1*r1 + r2*r2;
        }
        #endregion

        #region operators
        public static Vector2 operator -(Vector2 vector)
        {
            return new Vector2(-vector.X, -vector.Y);
        }

        public static Vector2 operator *(Vector2 vector, float scalar)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2 operator /(Vector2 vector, float scalar)
        {
            return new Vector2(vector.X / scalar, vector.Y / scalar);
        }

        public static Vector2 operator *(float scalar, Vector2 vector)
        {
            return new Vector2(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector2 operator *(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X * vector2.X, vector1.Y * vector2.Y);
        }

        public static Vector2 operator +(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector2 operator -(Vector2 vector1, Vector2 vector2)
        {
            return new Vector2(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public static bool operator ==(Vector2 vector1, Vector2 vector2)
        {
            return vector2.X == vector1.X && vector2.Y == vector1.Y;
        }

        public static bool operator !=(Vector2 vector1, Vector2 vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool operator <(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X < vector2.X && vector1.Y < vector2.Y;
        }

        public static bool operator >(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X > vector2.X && vector1.Y > vector2.Y;
        }

        public static bool operator <=(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X <= vector2.X && vector1.Y <= vector2.Y;
        }

        public static bool operator >=(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X >= vector2.X && vector1.Y >= vector2.Y;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("X:{0} Y:{1}", X, Y);
        }
    }
}
