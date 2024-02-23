using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;
using Physics.Collisions.Manifolds;

namespace Physics.Collisions
{
    public static class CollisionPolygonCircle
    {
        public static Manifold Detect(Polygon polygon, Circle circle, bool isFlipped)
        {
            var distance = float.PositiveInfinity;
            var globalPolyogonCollisionPoint = Vector2.Zero;

            for (var i = 0; i < polygon.GlobalVertices.Length; i++)
            {
                var j = i == polygon.GlobalVertices.Length - 1 ? 0 : i + 1;
                var p1 = polygon.GlobalVertices[i];
                var p2 = polygon.GlobalVertices[j];

                var dir = p2 - p1;

                var cP = circle.Position - p1;
                var dirLength = dir.Length();

                var projectionLength = Vector2.Dot(cP, Vector2.Normalize(dir));

                if (projectionLength < 0)
                {
                    var dist = Vector2.Distance(p1, circle.Position);
                    if (distance > dist)
                    {
                        distance = dist;
                        globalPolyogonCollisionPoint = p1;
                    }
                }
                else if (projectionLength > dirLength)
                {
                    var dist = Vector2.Distance(p2, circle.Position);

                    if (distance > dist)
                    {
                        distance = dist;
                        globalPolyogonCollisionPoint = p2;
                    }
                }
                else
                {
                    var edgeVector = p2 - p1;
                    var edgeDirection = Vector2.Normalize(edgeVector);
                    var edgeNormal = new Vector2(-edgeDirection.Y, edgeDirection.X);
                    var offsettedCirclePosition = circle.Position - p1;
                    
                    var normalProjection = Math.Abs(Vector2.Dot(offsettedCirclePosition, edgeNormal));
                    
                    if (normalProjection <= circle.Radius)
                    {
                        if (distance > normalProjection)
                        {
                            distance = normalProjection;
                            globalPolyogonCollisionPoint = circle.Position + edgeNormal*normalProjection;
                        }
                    }
                }
            }

            if (distance - circle.Radius > 0.05f)
                return null;

            var penetrationDirection = Vector2.Normalize(globalPolyogonCollisionPoint - circle.Position);

            var globalCircleCollisionPoint = circle.Position + penetrationDirection*circle.Radius;

            var localPolygonCollisionPoint = polygon.ToLocal(globalPolyogonCollisionPoint);
            var localCircleCollisionPoint = circle.ToLocal(globalCircleCollisionPoint);
           
            var normal = isFlipped ? penetrationDirection : -penetrationDirection;

            var localReferenceEdgeNormal = Vector2.Rotate(TrigonoUtil.NegateRotationVector(polygon.RotationVector), penetrationDirection);

            return new Manifold
            {
                IncidentBody = circle,
                ReferenceBody = polygon,
                IsFlipped = isFlipped,
                Normal = normal,
                Tangent = Vector2.Cross(normal, 1),
                ReferenceEdgeLocalNormal = localReferenceEdgeNormal,
                ReferenceEdgeLocalMiddlePoint = localPolygonCollisionPoint,

                Points = new List<ManifoldPoint>
                {
                    new ManifoldPoint(globalCircleCollisionPoint, localCircleCollisionPoint)
                }
            };
        }
    }
}
