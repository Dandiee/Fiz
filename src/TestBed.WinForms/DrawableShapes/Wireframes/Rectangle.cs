using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Wireframes
{
    public sealed class Rectangle : BaseWireframe
    {
        public Vector2 Min { get; set; }
        public Vector2 Max { get; set; }

        public Rectangle(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;

            Color = Color4.White;
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            // Left
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = Min.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = new Vector2(Min.X, Max.Y).AsSharpDxVector2()
            };

            // Top
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = new Vector2(Min.X, Max.Y).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = Max.AsSharpDxVector2()
            };

            // Right
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = Max.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = new Vector2(Max.X, Min.Y).AsSharpDxVector2()
            };

            // Bottom
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = new Vector2(Max.X, Min.Y).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = Min.AsSharpDxVector2()
            };
        }
    }
}
