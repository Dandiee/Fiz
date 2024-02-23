using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;
using Physics.Collisions.Manifolds;

namespace Physics.Collisions
{
    public static class CollisionSegmentCircle
    {
        public static Manifold Detect(Segment segment, Circle circle, bool isFlipped)
        {
            var l2 = segment.LengthSquared;
            var p = circle.Position;
            var v = segment.P1;
            var w = segment.P2;
            var t = Math.Max(0, Math.Min(1, ((p.X - v.X) * (w.X - v.X) + (p.Y - v.Y) * (w.Y - v.Y)) / l2));
          
            var closestPoint = new Vector2(v.X + t * (w.X - v.X), v.Y + t * (w.Y - v.Y));
            var distance = Vector2.Distance(closestPoint, circle.Position);

            if (distance < circle.Radius)
            {
                var collisionNormal = Vector2.Normalize(circle.Position - closestPoint);
                var globalCircleCollisionPoint = circle.Position - collisionNormal * circle.Radius;
                var localCircleCollisionPoint = circle.ToLocal(globalCircleCollisionPoint);

                return new Manifold
                {
                    IncidentBody = circle,
                    ReferenceBody = segment,
                    ReferenceEdgeLocalNormal = -collisionNormal,
                    ReferenceEdgeLocalMiddlePoint = Vector2.Zero,
                    Normal = collisionNormal,
                    Tangent = Vector2.Cross(collisionNormal, 1),
                    IsFlipped = isFlipped,
                    Points = new List<ManifoldPoint>
                    {
                        new ManifoldPoint(globalCircleCollisionPoint, localCircleCollisionPoint)
                    }
                };
            }

            return null;
        }
    }
}
