namespace Common
{
    public struct Matrix3x3
    {
        public Vector3 Column1;
        public Vector3 Column2;
        public Vector3 Column3;

        public Matrix3x3(Vector3 column1, Vector3 column2, Vector3 column3)
        {
            Column1 = column1;
            Column2 = column2;
            Column3 = column3;
        }

        public Vector3 Solve33(Vector3 b)
        {
	        var det = Vector3.Dot(Column1, Vector3.Cross(Column2, Column3));
	        if (det != 0.0f)
		        det = 1.0f / det;

	        var a1 = det * Vector3.Dot(b, Vector3.Cross(Column2, Column3));
	        var a2 = det * Vector3.Dot(Column1, Vector3.Cross(b, Column3));
	        var a3 = det * Vector3.Dot(Column1, Vector3.Cross(Column2, b));
            return new Vector3(a1, a2, a3);
        }

        public Vector2 Solve22(Vector2 b)
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

        public Matrix3x3 GetInverse22(Matrix3x3 matrix)
        {
            var a = Column1.X;
            var b = Column2.X;
            var c = Column1.Y;
            var d = Column2.Y;

	        var det = a * d - b * c;
	        if (det != 0.0f)
		        det = 1.0f / det;

            matrix.Column1.X = det * d; 
            matrix.Column2.X = -det * b; 
            matrix.Column1.Y = -det * c; 
            matrix.Column2.Y = det * a; 

            return matrix;
        }

        public Matrix3x3 GetSymInverse33(Matrix3x3 matrix)
        {
	        var det = Vector3.Dot(Column1, Vector3.Cross(Column2, Column3));
	        if (det != 0.0f)
		        det = 1.0f / det;

            var a11 = Column1.X;
            var a12 = Column2.X;
            var a13 = Column3.X;
            var a22 = Column2.Y;
            var a23 = Column3.Y;
	        var a33 = Column3.Z;

	        matrix.Column1.X = det * (a22 * a33 - a23 * a23);
	        matrix.Column1.Y = det * (a13 * a23 - a12 * a33);
	        matrix.Column1.Z = det * (a12 * a23 - a13 * a22);

	        matrix.Column2.X = matrix.Column1.Y;
	        matrix.Column2.Y = det * (a11 * a33 - a13 * a13);
	        matrix.Column2.Z = det * (a13 * a12 - a11 * a23);

	        matrix.Column3.X = matrix.Column1.Z;
	        matrix.Column3.Y = matrix.Column2.Z;
	        matrix.Column3.Z = det * (a11 * a22 - a12 * a12);

            return matrix;
        }

        public static Vector2 Mul22(Matrix3x3 matrix, Vector2 vector)
        {
            return new Vector2(matrix.Column1.X * vector.X + matrix.Column2.X * vector.Y, matrix.Column1.Y * vector.X + matrix.Column2.Y * vector.Y);
        }

        public static Vector3 Mul(Matrix3x3 matrix, Vector3 vector)
        {
	        return vector.X * matrix.Column1 + vector.Y * matrix.Column2 + vector.Z * matrix.Column3;
        }

    }
}
