using System.Collections.Generic;
using System.Linq;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.DrawableShapes.Wireframes.Base;
using TestBed.WinForms.Extensions;
using MathUtil = Common.MathUtil;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Wireframes
{
    public sealed class Circle : BaseWireframe
    {
        static Circle()
        {
            SetResolution(32);
        }

        #region Static Helpers
        private static IList<Vector2> _points;
        private static int _resolution;
        public static void SetResolution(int resolution)
        {
            _resolution = resolution;
            _points = new List<Vector2>();

            var step = MathUtil.TwoPi / resolution;
            var start = Vector2.UnitY;
            var prev = start;

            for (var i = 1; i < resolution + 1; i++)
            {
                var next = Vector2.Rotate(-i*step, start);
                _points.Add(prev);
                _points.Add(next);
                prev = next;
            }
        }
        #endregion

        public Vector2 Origo { get; set; }
        public float Radius { get; set; }

        public Circle(Vector2 origo, float radius)
        {
            Color = SharpDX.Color.Blue;
            Origo = origo;
            Radius = radius;
        }

        public Circle(Vector2 origo, float radius, Color color)
        {
            Color = color;
            Origo = origo;
            Radius = radius;
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            return _points.Select(p => new Vertex2DPositionColor
            {
                Color = Color,
                Position = (p * Radius + Origo).AsSharpDxVector2()
            });
        }
    }
}
