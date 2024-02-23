using System;

namespace Common
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return MathUtil.Lerp(min, max, (float)random.NextDouble());
        }

        public static double NextDouble(this Random random, double min, double max)
        {
            return MathUtil.Lerp(min, max, random.NextDouble());
        }

        public static long NextLong(this Random random)
        {
            var buffer = new byte[sizeof(long)];
            random.NextBytes(buffer);
            return BitConverter.ToInt64(buffer, 0);
        }

        public static long NextLong(this Random random, long min, long max)
        {
            var buf = new byte[sizeof(long)];
            random.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min + 1)) + min);
        }

        public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y));
        }

        public static Vector3 NextVector3(this Random random, Vector3 min, Vector3 max)
        {
            return new Vector3(random.NextFloat(min.X, max.X), random.NextFloat(min.Y, max.Y), random.NextFloat(min.Z, max.Z));
        }

        public static TimeSpan NextTime(this Random random, TimeSpan min, TimeSpan max)
        {
            return TimeSpan.FromTicks(random.NextLong(min.Ticks, max.Ticks));
        }
    }
}
