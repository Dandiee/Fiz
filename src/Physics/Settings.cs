using Common;

namespace Physics
{
    public static class Settings
    {
        public static float AngularSlop = (2.0f/180.0f*MathUtil.Pi);
        public static float LinearSlop = 0.005f;
        public static float MaxAngularCorrection = (8.0f/180.0f*MathUtil.Pi);
        public static float Baumgarte = 0.2f;
        public static float MaxLinearCorrection = 0.2f;
        public static float VelocityThreshold = 1.0f;
        // public static Vector2 Gravity = new Vector2(0, -10);
        public static int VelocitySolverIterations = 12;
        public static int PositionSolverIterations = 8;
        public static float TimeStep = 1/60f;
        public static bool IsBlockSolverEnabled = true;
        public static bool IsBoundingBoxCollisionCheckEnabled = true;
        public static int MaximumEpaIteratins = 100;
        public static int BodyConnectionCacheSize = 2048;
    }
}
