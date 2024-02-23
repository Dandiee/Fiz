using SharpDX;

namespace TestBed.WinForms.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Vector4 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Common.Vector2 AsPhysicsVector2(this Vector2 v)
        {
            return new Common.Vector2(v.X, v.Y);
        }

        public static Common.Vector3 AsPhysicsVector3(this Vector3 v)
        {
            return new Common.Vector3(v.X, v.Y, v.Z);
        }

        public static Vector2 AsSharpDxVector2(this Common.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }

        public static Vector3 AsSharpDxVector3(this Common.Vector3 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static Matrix AsSharpDxMatrix(this Common.Matrix4x4 v)
        {
            return new Matrix(
                v.Column1.X, v.Column2.X, v.Column3.X, v.Column4.X,
                v.Column1.Y, v.Column2.Y, v.Column3.Y, v.Column4.Y,
                v.Column1.Z, v.Column2.Z, v.Column3.Z, v.Column4.Z,
                v.Column1.W, v.Column2.W, v.Column3.W, v.Column4.W);
        }
    }
}
