using Common;
using Physics.Bodies;

namespace Physics.Collisions.Polygons.Detector
{
    public class MinkowskiSum
    {
        public ClipableBody Body1 { get; private set; }
        public ClipableBody Body2 { get; private set; }

        public MinkowskiSum(ClipableBody body1, ClipableBody body2)
        {
            Body1 = body1;
            Body2 = body2;
        }

        public Vector2 GetSupportPoint(Vector2 direction)
        {
            var v1 = GetFarthestPoint(direction, Body1.GlobalVertices);
            direction = -direction;

            var v2 = GetFarthestPoint(direction, Body2.GlobalVertices);

            return v1 - v2;
        }

        private static Vector2 GetFarthestPoint(Vector2 direction, Vector2[] convex)
        {
            var maxVertex = convex[0];
            var maxDistance = Vector2.Dot(maxVertex, direction);

            for (var i = 1; i < convex.Length; i++)
            {
                var distance = Vector2.Dot(convex[i], direction);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxVertex = convex[i];
                }
            }

            return maxVertex;
        }
    }
}
