using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Physics.Bodies
{
    public sealed class Polygon : ClipableBody
    {
        public Polygon(IList<Vector2> vertices, BodyDefinition definition = null)
        {
            if (definition == null)
                definition = BodyDefinition.Default;

            if (MathUtil.GetWindingOrder(vertices[0], vertices[1], vertices[2]) == MathUtil.WindingOrder.Clockwise)
            {
                vertices = vertices.Reverse().ToList();
            }

            Position = vertices.Aggregate(Vector2.Zero, (current, vertex) => current + vertex) / vertices.Count;
            LocalVertices = new Vector2[vertices.Count];
            var farthestVertexDistance = float.NegativeInfinity;

            for(var i = 0; i < vertices.Count; i++)
            {
                var localVertex = vertices[i] - Position;
                var length = localVertex.Length();
                if (length > farthestVertexDistance)
                    farthestVertexDistance = length;
                LocalVertices[i] = localVertex;
            }
            GlobalVertices = new Vector2[LocalVertices.Length];
            InitializeGlobalVertices();
            BoundingCircleRadius = farthestVertexDistance;

            Area = GetArea();
            Rotation = definition.Rotation;
            Restitution = definition.Restitution;
            Friction = definition.Friction;
            Density = definition.Density;
            IsCollisionImmune = definition.IsCollisionImmune;

            if (definition.IsLocked)
            {
                Inertia = float.MaxValue;
                Mass = float.MaxValue;
                IsLocked = true;
            }
            else
            {
                Mass = Area * Density;
                InverseMass = 1f / Mass;
                Inertia = CalculateInertie();
                if (Inertia < 0)
                {
                    Inertia *= -1;
                }
                InverseInertia = 1f / Inertia;
            }
        }

        public override bool IsPointInside(Vector2 point)
        {
            for (var i = 0; i < GlobalVertices.Length; i++)
            {
                var p1 = GlobalVertices[i];
                var p2 = GlobalVertices[i == GlobalVertices.Length - 1 ? 0 : i + 1];
                if (MathUtil.PointInTriangle(p1, Position, p2, point))
                {
                    return true;
                }
            }

            return false;
        }

        
        public override void AddSpatials(Vector2 position, float rotation)
        {
            AddSpatialsValues(position, rotation);
            var wasRotated = Math.Abs(rotation) >.0001f;
            UpdateGlobalVertices(wasRotated, position);
        }

        private float CalculateInertie()
        {
            var sumOfCounter = 0f;
            var sumOfDenominator = 0f;

            for (var i = 0; i < LocalVertices.Length; i++)
            {
                var j = i == LocalVertices.Length - 1 ? 0 : i + 1;

                var x1 = LocalVertices[i].X;
                var y1 = LocalVertices[i].Y;

                var x2 = LocalVertices[j].X;
                var y2 = LocalVertices[j].Y;

                var a = (x1*x1 + y1*y1 + x1*x2 + y1*y2 + x2*x2 + y2*y2);
                var b = (x1*y2 - x2*y1);

                sumOfCounter += a * b;
                sumOfDenominator += b;
            }

            return (Mass/6f)*(sumOfCounter/sumOfDenominator);
        }

        private float GetArea()
        {
            var area = Vector2.Determinant(LocalVertices[LocalVertices.Length - 1], LocalVertices[0]);
            for (var i = 1; i < LocalVertices.Length; i++)
            {
                area += Vector2.Determinant(LocalVertices[i - 1], LocalVertices[i]);
            }
            return area / 2f;
        }

        private void UpdateGlobalVertices(bool wasRotated, Vector2 translation)
        {
            if (wasRotated)
            {
                InitializeGlobalVertices();
            }
            else
            {
                GlobalMax = GlobalMax + translation;
                GlobalMin = GlobalMin + translation;

                for (var i = 0; i < GlobalVertices.Length; i++)
                {
                    GlobalVertices[i] += translation;
                }
            }
        }

        private void InitializeGlobalVertices()
        {
            RotationVector = TrigonoUtil.RotationVector(Rotation);

            var minX = float.PositiveInfinity;
            var minY = float.PositiveInfinity;
            var maxX = float.NegativeInfinity;
            var maxY = float.NegativeInfinity;

            for (var i = 0; i < GlobalVertices.Length; i++)
            {
                var globalVertex = ToGlobal(LocalVertices[i]);
                GlobalVertices[i] = globalVertex;

                if (globalVertex.X < minX) minX = globalVertex.X;
                if (globalVertex.Y < minY) minY = globalVertex.Y;
                if (globalVertex.X > maxX) maxX = globalVertex.X;
                if (globalVertex.Y > maxY) maxY = globalVertex.Y;
            }

            GlobalMin = new Vector2(minX, minY);
            GlobalMax = new Vector2(maxX, maxY);
            GlobalSize = GlobalMax - GlobalMin;
        }
    }
}
