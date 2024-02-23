using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;
using Physics.Collisions.Manifolds;

namespace Physics.Collisions.Polygons.Clipping
{
    public class ClippingManifoldSolver
    {
        // csak counter-clockwise
        public static Manifold GetManifold(Vector2 penetrationDirection, float penetrationDepth, ClipableBody body1, ClipableBody body2)
        {
            var reference = GetBest(penetrationDirection, body1.GlobalVertices);
            var incident = GetBest(-penetrationDirection, body2.GlobalVertices);

            var perpendicular1 = Math.Abs(Vector2.Dot(reference.Direction, penetrationDirection));
            var perpendicular2 = Math.Abs(Vector2.Dot(incident.Direction, penetrationDirection));

            var flip = perpendicular1 > perpendicular2;

            if (flip)
            {
                var temp = reference;
                reference = incident;
                incident = temp;
            }

            var referencePolygon = flip ? body2 : body1;
            var incidentPolygon = flip ? body1 : body2;

            var dir1 = Vector2.Normalize(reference.V1.Vertex - reference.V2.Vertex);
            var offset1 = Vector2.Dot(dir1, reference.V1.Vertex);
            var clipping1Result = Clip(incident.V1, incident.V2, dir1, offset1, reference.V1.Index);
            if (clipping1Result.Count != 2)
                return null;

            var dir2 = Vector2.Normalize(reference.V2.Vertex - reference.V1.Vertex);
            var offset2 = Vector2.Dot(dir2, reference.V2.Vertex);
            var clipping2Result = Clip(clipping1Result[0], clipping1Result[1], dir2, offset2, reference.V2.Index);
            if (clipping2Result.Count != 2)
                return null;

            var frontOffset = Vector2.Dot(-reference.Normal, reference.V1.Vertex);
            var manifold = new Manifold
            {
                Normal = flip ? -reference.Normal : reference.Normal

            };
            manifold.Tangent = Vector2.Cross(manifold.Normal, 1);

            var refLocalE1 = referencePolygon.LocalVertices[reference.V1.Index];
            var refLocalE2 = referencePolygon.LocalVertices[reference.V2.Index];

            var refLocalDir = Vector2.Normalize(refLocalE2 - refLocalE1);
            var refLocalNormal = new Vector2(-refLocalDir.Y, refLocalDir.X);

            var referenceEdgeLocalNormal = refLocalNormal;
            var isFlipped = flip;
            var referencEdgeLocalMiddlePoint = (refLocalE1 + refLocalE2) / 2f;

            foreach (var vertex in clipping2Result)
            {
                var depth = Vector2.Dot(-reference.Normal, vertex.Vertex) - frontOffset;

                if (depth >= 0)
                {
                    var v = flip ? vertex.Vertex : vertex.Vertex - depth * -manifold.Normal;
                    var localVertex = incidentPolygon.LocalVertices[vertex.Index];
                    manifold.Points.Add(new ManifoldPoint(v, localVertex));
                }
            }

            manifold.IsFlipped = isFlipped;
            manifold.ReferenceEdgeLocalNormal = referenceEdgeLocalNormal;
            manifold.ReferenceBody = referencePolygon;
            manifold.IncidentBody = incidentPolygon;
            manifold.ReferenceEdgeLocalMiddlePoint = referencEdgeLocalMiddlePoint;

            if (manifold.Points.Count == 0)
                return null;

            return manifold;
        }

        private static List<ClippingVertexResult> Clip(ClippingVertexResult v1, ClippingVertexResult v2, Vector2 n, float offset, int referenceClipperPointIndex)
        {
            var points = new List<ClippingVertexResult>(2);

            var d1 = Vector2.Dot(n, v1.Vertex) - offset;
            var d2 = Vector2.Dot(n, v2.Vertex) - offset;

            if (d1 <= 0.0f) points.Add(v1);
            if (d2 <= 0.0f) points.Add(v2);

            if (d1 * d2 < 0.0f)
            {
                var e = (v2.Vertex - v1.Vertex) * (d1 / (d1 - d2)) + v1.Vertex;

                if (d1 > 0.0)
                {
                    points.Add(new ClippingVertexResult(e, v1.Index)
                    {
                        ClippedByReferenceIndex = referenceClipperPointIndex
                    });
                }
                else
                {
                    points.Add(new ClippingVertexResult(e, v2.Index)
                    {
                        ClippedByReferenceIndex = referenceClipperPointIndex
                    });
                }
            }

            return points;
        }

        private static ClipingEdgeResult GetBest(Vector2 direction, Vector2[] vertices)
        {
            var maxIndex = 0;
            var maxVertex = vertices[0];
            var maxDistance = Vector2.Dot(maxVertex, direction);

            for (var i = 1; i < vertices.Length; i++)
            {
                var distance = Vector2.Dot(vertices[i], direction);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxVertex = vertices[i];
                    maxIndex = i;
                }
            }

            var prevIndex = maxIndex == 0 ? vertices.Length - 1 : maxIndex - 1;
            var nextIndex = maxIndex == vertices.Length - 1 ? 0 : maxIndex + 1;

            var prevVertex = vertices[prevIndex];
            var nextVertex = vertices[nextIndex];

            var dir1 = Vector2.Normalize(prevVertex - maxVertex);
            var dir2 = Vector2.Normalize(maxVertex - nextVertex);

            var norm1 = Vector2.Cross(1, dir1);
            var norm2 = Vector2.Cross(1, dir2);

            var proj1 = Vector2.Dot(norm1, direction);
            var proj2 = Vector2.Dot(norm2, direction);

            return proj1 > proj2
                ? new ClipingEdgeResult(new ClippingVertexResult(prevVertex, prevIndex),
                    new ClippingVertexResult(maxVertex, maxIndex), norm1, -dir1)
                : new ClipingEdgeResult(new ClippingVertexResult(maxVertex, maxIndex),
                    new ClippingVertexResult(nextVertex, nextIndex), norm2, -dir2);

        }
    }
}
