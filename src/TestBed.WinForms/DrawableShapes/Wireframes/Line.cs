using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Wireframes
{
    public sealed class Line : BaseWireframe
    {
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }


        public Line(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
            
            Color = SharpDX.Color.Red;
        }

        public Line(Vector2 p1, Vector2 p2, Color4 color)
        {
            P1 = p1;
            P2 = p2;

            Color = color;
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = P1.AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = P2.AsSharpDxVector2()
            };
        }
    }
}
