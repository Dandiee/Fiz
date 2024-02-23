using System.Collections.Generic;
using Common;
using Physics.Bodies;

namespace Physics.Helpers
{
    public static class BodyBuilder
    {
        public static Circle BuildCircle(Vector2 position, float radius, BodyDefinition definition = null)
        {
            return new Circle(position, radius, definition);
        }

        public static Segment BuildSegment(Vector2 p1, Vector2 p2)
        {
            return new Segment(p1, p2);
        }

        public static Polygon BuildIrregularPolygon(IList<Vector2> vertices, BodyDefinition definition = null)
        {
            return new Polygon(vertices, definition);
        }

        public static Polygon BuildRegularPolygon(int verticesCount, float radius, Vector2 position, BodyDefinition definition = null)
        {
            var vertices = new Vector2[verticesCount];
            var step = MathUtil.TwoPi / verticesCount;
            var directionVector = Vector2.UnitY * radius;
            for (var i = 0; i < verticesCount; i++)
            {
                vertices[i] = Vector2.Rotate(step * i, directionVector);
            }

            return new Polygon(vertices, definition);
        }

        public static Polygon BuildBox(Vector2 size, Vector2 position, BodyDefinition definition = null)
        {
            var h = 0.5f * size;
            var vertices = new[]
            {
                -h + position,
                new Vector2(+h.X, -h.Y) + position,
                h + position,
                new Vector2(-h.X, +h.Y) + position,
            };

            return new Polygon(vertices, definition);
        }

        public static Polygon BuildBar(Vector2 p1, Vector2 p2, Vector2 offset, BodyDefinition definition = null)
        {
            var dir = Vector2.Normalize(p2 - p1);
            var tan = Vector2.Cross(1, dir);

            var v1 = p1 + tan * offset.X - dir * offset.Y;
            var v2 = p2 + tan * offset.X + dir * offset.Y;
            var v3 = p2 - tan * offset.X + dir * offset.Y;
            var v4 = p1 - tan * offset.X - dir * offset.Y;

            return BuildIrregularPolygon(new[]
            {
                v1, v4, v3, v2,  
            }, definition);
        }
    }
}
