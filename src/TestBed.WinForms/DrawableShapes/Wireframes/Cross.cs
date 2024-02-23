using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Wireframes
{
    public sealed class Cross : BaseWireframe
    {
        public float Length = 1;

        public Vector2 Point { get; set; }

        public Cross(Vector2 point, float length = 1)
        {
            Point = point;
            Color = SharpDX.Color.Red;
            Length = length;

        }

        public Cross(Vector2 point, Color4 color, float length = 1)
        {
            Point = point;
            Color = color;
            Length = length;
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (Point + new Vector2(Length, Length)).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (Point + new Vector2(-Length, -Length)).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (Point + new Vector2(-Length, Length)).AsSharpDxVector2()
            };

            yield return new Vertex2DPositionColor
            {
                Color = Color,
                Position = (Point + new Vector2(Length, -Length)).AsSharpDxVector2()
            };
        }
    }
}
