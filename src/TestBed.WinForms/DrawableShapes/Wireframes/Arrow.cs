using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Wireframes
{
    public sealed class Arrow : BaseWireframe
    {
        public Vector2 From { get; set; }
        public Vector2 To { get; set; }

        public static float ArrowLength = 5;
        public static float ArrowAngle = 0.2f;


        public Arrow(Vector2 from, Vector2 to)
        {
            From = from;
            To = to;
            Color = SharpDX.Color.Green;
        }


        public Arrow(Vector2 from, Vector2 to, Color color)
        {
            From = from;
            To = to;
            Color = color;
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = From.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = To.AsSharpDxVector2()
            };

          
            var norm = Vector2.Normalize(From - To) * ArrowLength;
            var d1 = Vector2.Rotate(-ArrowAngle, norm);
            var d2 = Vector2.Rotate(ArrowAngle, norm);

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = To.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (To + d1).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = To.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (To + d2).AsSharpDxVector2()
            };
        }
    }
}
