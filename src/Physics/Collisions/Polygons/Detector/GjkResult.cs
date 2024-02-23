using System.Collections.Generic;
using Common;

namespace Physics.Collisions.Polygons.Detector
{
    public sealed class GjkResult
    {
        public static readonly GjkResult NoCollision = new GjkResult();

        public bool IsColliding { get; private set; }
        public List<Vector2> Simplex { get; private set; }

        public GjkResult(List<Vector2> simplex)
        {
            IsColliding = true;
            Simplex = simplex;
        }

        private GjkResult()
        {
            
        }
    }
}
