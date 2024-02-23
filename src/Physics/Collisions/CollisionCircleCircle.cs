using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;
using Physics.Collisions.Manifolds;

namespace Physics.Collisions
{
    public static class CollisionCircleCircle
    {
        public static Manifold Detect(Circle circle1, Circle circle2)
        {
            var distanceSquared = Vector2.DistanceSquared(circle1.Position, circle2.Position);
            var radiusSum = circle1.Radius + circle2.Radius;
            if (distanceSquared > radiusSum*radiusSum)
                return null;

            var distance = Math.Sqrt(distanceSquared);

            if (distance <= radiusSum)
            {
                var normal = Vector2.Normalize(circle2.Position - circle1.Position);

                var globalIncidentCollisionPoint = circle2.Position - (normal * circle2.Radius);
                var globalReferenceCollisionPoint = circle1.Position + (normal * circle1.Radius);

                var localIncidentCollisionPoint = circle2.ToLocal(globalIncidentCollisionPoint);
                var localReferenceCollisionPoint = circle1.ToLocal(globalReferenceCollisionPoint);

                var localNormalByReference = Vector2.Rotate(TrigonoUtil.NegateRotationVector(circle1.RotationVector), normal);

                return new Manifold
                {
                    ReferenceBody = circle1,
                    IncidentBody = circle2,
                    Normal = normal,
                    Tangent = Vector2.Cross(normal, 1),
                    ReferenceEdgeLocalMiddlePoint = localReferenceCollisionPoint,
                    ReferenceEdgeLocalNormal = -localNormalByReference,
                    Points = new List<ManifoldPoint>(1)
                    {
                        new ManifoldPoint(globalIncidentCollisionPoint, localIncidentCollisionPoint)
                    }
                };
            }

            return null;
        }
    }
}
