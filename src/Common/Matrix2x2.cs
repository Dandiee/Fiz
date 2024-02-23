using System;

namespace Common
{
    public struct Matrix2x2
    {
        public Vector2 Column1;
        public Vector2 Column2;

        public Matrix2x2(Vector2 column1, Vector2 column2)
        {
            Column1 = column1;
            Column2 = column2;
        }

        public Matrix2x2(float m11, float m12, float m21, float m22)
        {
            Column1 = new Vector2(m11, m12);
            Column2 = new Vector2(m21, m22);
        }

        public Matrix2x2(float angle)
        {
            var rotationVector = TrigonoUtil.RotationVector(angle);
            Column1 = new Vector2(rotationVector.Y, rotationVector.X);
            Column2 = new Vector2(-rotationVector.X, rotationVector.Y);
        }

        public static Matrix2x2 operator +(Matrix2x2 a, Matrix2x2 b)
        {
            return new Matrix2x2(a.Column1 + b.Column1, a.Column2 + b.Column2);
        }

        public static Matrix2x2 operator *(Matrix2x2 a, Matrix2x2 b)
        {
            return new Matrix2x2(a * b.Column1, a * b.Column2);
        }

        public static Vector2 operator *(Matrix2x2 a, Vector2 v)
        {
            return new Vector2(a.Column1.X * v.X + a.Column2.X * v.Y, a.Column1.Y * v.X + a.Column2.Y * v.Y);
        }

        public Matrix2x2 Transpose()
        {
            return new Matrix2x2(new Vector2(Column1.X, Column2.X), new Vector2(Column1.Y, Column2.Y));
        }

        public Matrix2x2 Invert()
        {
            var a = Column1.X;
            var b = Column2.X;
            var c = Column1.Y;
            var d = Column2.Y;

            var det = 1.0f / (a * d - b * c);

            return new Matrix2x2(det * d, -det * c, -det * b, det * a);
        }

        public Matrix2x2 Invert2()
        {
            var a = Column1.X;
            var b = Column2.X;
            var c = Column1.Y;
            var d = Column2.Y;

            var det = (a * d - b * c);

            return new Matrix2x2(det * d, -det * c, -det * b, det * a);
        }

        public Vector2 Solve(Vector2 b)
        {
            var a11 = Column1.X;
            var a12 = Column2.X;
            var a21 = Column1.Y;
            var a22 = Column2.Y;
            var det = a11 * a22 - a12 * a21;
            if (det != 0.0f)
                det = 1.0f / det;

            var a1 = det * (a22 * b.X - a12 * b.Y);
            var a2 = det * (a11 * b.Y - a21 * b.X);
            return new Vector2(a1, a2);
        }
    }
}
