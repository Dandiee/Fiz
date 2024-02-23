using System;

namespace Common
{
    public struct Matrix4x4
    {
        public static readonly Matrix4x4 Zero = new Matrix4x4();
        public static readonly Matrix4x4 Identity = new Matrix4x4(
            new Vector4(1, 0, 0, 0), 
            new Vector4(0, 1, 0, 0), 
            new Vector4(0, 0, 1, 0), 
            new Vector4(0, 0, 0, 1) 
            );

        public Vector4 Column1;
        public Vector4 Column2;
        public Vector4 Column3;
        public Vector4 Column4;

        public Matrix4x4(Vector4 column1, Vector4 column2, Vector4 column3, Vector4 column4) : this()
        {
            Column1 = column1;
            Column2 = column2;
            Column3 = column3;
            Column4 = column4;
        }

        public static Matrix4x4 PerspectiveFovRH(float fov, float aspect, float znear, float zfar)
        {
            var num1 = (float)(1.0 / Math.Tan(fov * 0.5));
            var num2 = num1 / aspect;
            var right = znear / num2;
            var top = znear / num1;
            return PerspectiveOffCenterRH(-right, right, -top, top, znear, zfar);
        }

        public static Matrix4x4 PerspectiveOffCenterRH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            var matrix = PerspectiveOffCenterLH(left, right, bottom, top, znear, zfar);
            var multipler = new Vector4(1, 1, -1, 1);
            matrix.Column1 *= multipler;
            matrix.Column2 *= multipler;
            matrix.Column3 *= multipler;
            matrix.Column4 *= multipler;
            return matrix;
        }

        public static Matrix4x4 PerspectiveOffCenterLH(float left, float right, float bottom, float top, float znear, float zfar)
        {
            var num = zfar / (zfar - znear);
            return new Matrix4x4(
                 new Vector4((float)(2.0 * znear / (right - (double)left)), 0, (float)((left + (double)right) / (left - right)), 0),
                 new Vector4(0, (float)(2.0 * znear / (top - (double)bottom)), (float)((top + (double)bottom) / (bottom - (double)top)), 0), 
                 new Vector4(0, 0, num, -znear * num), 
                 Vector4.UnitZ
                );
        }

        public static Matrix4x4 LookAtRH(Vector3 eye, Vector3 target, Vector3 up)
        {
            var result1 = Vector3.Normalize(eye - target);
            var result2 = Vector3.Normalize(Vector3.Cross(up, result1));
            var result3 = Vector3.Cross(result1, result2);

            return new Matrix4x4(
                new Vector4(result2, -Vector3.Dot(result2, eye)),
                new Vector4(result3, -Vector3.Dot(result3, eye)),
                new Vector4(result1, -Vector3.Dot(result1, eye)), 
                Vector4.UnitW
                );
        }

        public static bool operator ==(Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            return matrix1.Column1 == matrix2.Column1 &&
                   matrix1.Column2 == matrix2.Column2 &&
                   matrix1.Column3 == matrix2.Column3 &&
                   matrix1.Column4 == matrix2.Column4;
        }

        public static bool operator !=(Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            return !(matrix1 == matrix2);
        }

        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            var col1 = new Vector4(
                left.Column1.X * right.Column1.X + left.Column2.X * right.Column1.Y + left.Column3.X * right.Column1.Z + left.Column4.X * right.Column1.W,
                left.Column1.Y * right.Column1.X + left.Column2.Y * right.Column1.Y + left.Column3.Y * right.Column1.Z + left.Column4.Y * right.Column1.W,
                left.Column1.Z * right.Column1.X + left.Column2.Z * right.Column1.Y + left.Column3.Z * right.Column1.Z + left.Column4.Z * right.Column1.W,
                left.Column1.W * right.Column1.X + left.Column2.W * right.Column1.Y + left.Column3.W * right.Column1.Z + left.Column4.W * right.Column1.W);

            var col2 = new Vector4(
                left.Column1.X * right.Column2.X + left.Column2.X * right.Column2.Y + left.Column3.X * right.Column2.Z + left.Column4.X * right.Column2.W,
                left.Column1.Y * right.Column2.X + left.Column2.Y * right.Column2.Y + left.Column3.Y * right.Column2.Z + left.Column4.Y * right.Column2.W,
                left.Column1.Z * right.Column2.X + left.Column2.Z * right.Column2.Y + left.Column3.Z * right.Column2.Z + left.Column4.Z * right.Column2.W,
                left.Column1.W * right.Column2.X + left.Column2.W * right.Column2.Y + left.Column3.W * right.Column2.Z + left.Column4.W * right.Column2.W);

            var col3 = new Vector4(
                left.Column1.X * right.Column3.X + left.Column2.X * right.Column3.Y + left.Column3.X * right.Column3.Z + left.Column4.X * right.Column3.W,
                left.Column1.Y * right.Column3.X + left.Column2.Y * right.Column3.Y + left.Column3.Y * right.Column3.Z + left.Column4.Y * right.Column3.W,
                left.Column1.Z * right.Column3.X + left.Column2.Z * right.Column3.Y + left.Column3.Z * right.Column3.Z + left.Column4.Z * right.Column3.W,
                left.Column1.W * right.Column3.X + left.Column2.W * right.Column3.Y + left.Column3.W * right.Column3.Z + left.Column4.W * right.Column3.W);

            var col4 = new Vector4(
                left.Column1.X * right.Column4.X + left.Column2.X * right.Column4.Y + left.Column3.X * right.Column4.Z + left.Column4.X * right.Column4.W,
                left.Column1.Y * right.Column4.X + left.Column2.Y * right.Column4.Y + left.Column3.Y * right.Column4.Z + left.Column4.Y * right.Column4.W,
                left.Column1.Z * right.Column4.X + left.Column2.Z * right.Column4.Y + left.Column3.Z * right.Column4.Z + left.Column4.Z * right.Column4.W,
                left.Column1.W * right.Column4.X + left.Column2.W * right.Column4.Y + left.Column3.W * right.Column4.Z + left.Column4.W * right.Column4.W);

            return new Matrix4x4(col1, col2, col3, col4);
        }

        public static Matrix4x4 Invert(Matrix4x4 value)
        {
            var num1 = (float)(value.Column1.Z * (double)value.Column2.W - value.Column2.Z * (double)value.Column1.W);
            var num2 = (float)(value.Column1.Z * (double)value.Column3.W - value.Column3.Z * (double)value.Column1.W);
            var num3 = (float)(value.Column4.Z * (double)value.Column1.W - value.Column1.Z * (double)value.Column4.W);
            var num4 = (float)(value.Column2.Z * (double)value.Column3.W - value.Column3.Z * (double)value.Column2.W);
            var num5 = (float)(value.Column4.Z * (double)value.Column2.W - value.Column2.Z * (double)value.Column4.W);
            var num6 = (float)(value.Column3.Z * (double)value.Column4.W - value.Column4.Z * (double)value.Column3.W);
            var num7 = (float)((double)value.Column2.Y * num6 + (double)value.Column3.Y * num5 + (double)value.Column4.Y * num4);
            var num8 = (float)((double)value.Column1.Y * num6 + (double)value.Column3.Y * num3 + (double)value.Column4.Y * num2);
            var num9 = (float)(value.Column1.Y * -(double)num5 + (double)value.Column2.Y * num3 + (double)value.Column4.Y * num1);
            var num10 = (float)((double)value.Column1.Y * num4 + value.Column2.Y * -(double)num2 + (double)value.Column3.Y * num1);
            var num11 = (float)((double)value.Column1.X * num7 - (double)value.Column2.X * num8 + (double)value.Column3.X * num9 - (double)value.Column4.X * num10);
            if (Math.Abs(num11) == 0.0)
            {
                return Zero;
            }
             
            var num12 = 1f / num11;
            var num13 = (float)(value.Column1.X * (double)value.Column2.Y - value.Column2.X * (double)value.Column1.Y);
            var num14 = (float)(value.Column1.X * (double)value.Column3.Y - value.Column3.X * (double)value.Column1.Y);
            var num15 = (float)(value.Column4.X * (double)value.Column1.Y - value.Column1.X * (double)value.Column4.Y);
            var num16 = (float)(value.Column2.X * (double)value.Column3.Y - value.Column3.X * (double)value.Column2.Y);
            var num17 = (float)(value.Column4.X * (double)value.Column2.Y - value.Column2.X * (double)value.Column4.Y);
            var num18 = (float)(value.Column3.X * (double)value.Column4.Y - value.Column4.X * (double)value.Column3.Y);
            var num19 = (float)(value.Column2.X * (double)num6  + value.Column3.X * (double)num5 + value.Column4.X * (double)num4);
            var num20 = (float)(value.Column1.X * (double)num6  + value.Column3.X * (double)num3 + value.Column4.X * (double)num2);
            var num21 = (float)(value.Column1.X *-(double)num5  + value.Column2.X * (double)num3 + value.Column4.X * (double)num1);
            var num22 = (float)(value.Column1.X * (double)num4  + value.Column2.X * -(double)num2 + value.Column3.X * (double)num1);
            var num23 = (float)(value.Column2.W * (double)num18 + value.Column3.W * (double)num17 + value.Column4.W * (double)num16);
            var num24 = (float)(value.Column1.W * (double)num18 + value.Column3.W * (double)num15 + value.Column4.W * (double)num14);
            var num25 = (float)(value.Column1.W *-(double)num17 + value.Column2.W * (double)num15 + value.Column4.W * (double)num13);
            var num26 = (float)(value.Column1.W * (double)num16 + value.Column2.W * -(double)num14 + value.Column3.W * (double)num13);
            var num27 = (float)(value.Column2.Z * (double)num18 + value.Column3.Z * (double)num17 + value.Column4.Z * (double)num16);
            var num28 = (float)(value.Column1.Z * (double)num18 + value.Column3.Z * (double)num15 + value.Column4.Z * (double)num14);
            var num29 = (float)(value.Column1.Z *-(double)num17 + value.Column2.Z * (double)num15 + value.Column4.Z * (double)num13);
            var num30 = (float)(value.Column1.Z * (double)num16 + value.Column2.Z * -(double)num14 + value.Column3.Z * (double)num13);

            return new Matrix4x4(
                new Vector4(num7 * num12, -num8 * num12, num9 * num12, -num10 * num12),
                new Vector4(-num19 * num12, num20 * num12, -num21 * num12, num22 * num12),
                new Vector4(num23 * num12, -num24 * num12, num25 * num12, -num26 * num12),
                new Vector4(-num27 * num12, num28 * num12, -num29 * num12, num30 * num12)
                );
        }

        public static Matrix4x4 Transpose(Matrix4x4 value)
        {
            return new Matrix4x4(
                new Vector4(value.Column1.X, value.Column2.X, value.Column3.X, value.Column4.X),
                new Vector4(value.Column1.Y, value.Column2.Y, value.Column3.Y, value.Column4.Y),
                new Vector4(value.Column1.Z, value.Column2.Z, value.Column3.Z, value.Column4.Z),
                new Vector4(value.Column1.W, value.Column2.W, value.Column3.W, value.Column4.W));
        }

    }
}
