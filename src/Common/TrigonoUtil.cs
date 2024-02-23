using System;

namespace Common
{
    public static class TrigonoUtil
    {
        private const int Magnitude = 100000;
        private const int Steps = (int)(MathUtil.TwoPi * Magnitude);
        private static readonly float[] SinTable;
        private static readonly float[] CosTable;

        static TrigonoUtil()
        {
            SinTable = new float[Steps];
            CosTable = new float[Steps];

            for (var i = 0; i < Steps; i++)
            {
                SinTable[i] = (float) Math.Sin((float)i/Magnitude);
                CosTable[i] = (float) Math.Cos((float) i/Magnitude);
            }
        }

        public static float Sin2(float alpha)
        {
            var x = (int) (alpha*Magnitude);
            var negative = x < 0;
            if (negative) 
                x = -x;

            var mod = x%Steps;
            if (negative)
                return -SinTable[mod];

            return SinTable[mod];
        }

        public static float Sin(float alpha)
        {
            return Cos(alpha - MathUtil.PiOverTwo);
        }

        public static float Cos(float alpha)
        {
            var x = (int) (alpha*Magnitude);
            var mod = ((x ^ (x >> 31)) - (x >> 31))%Steps;
            return CosTable[mod];
        }

        public static Vector2 RotationVector(float alpha)
        {
            return new Vector2(Sin(alpha), Cos(alpha));
        }

        public static Vector2 NegateRotationVector(Vector2 rotationVector)
        {
            return new Vector2(-rotationVector.X, rotationVector.Y);
        }
    }
}
