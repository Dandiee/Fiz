using System;
using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Solids.Base;
using TestBed.WinForms.DrawableShapes.Vertices;
using TestBed.WinForms.Extensions;
using MathUtil = Common.MathUtil;
using Vector2 = Common.Vector2;

namespace TestBed.WinForms.DrawableShapes.Solids
{
    public sealed class Triangle : BaseSolid
    {
        public Vector2 V1 { get; set; }
        public Vector2 V2 { get; set; }
        public Vector2 V3 { get; set; }

        public static Tuple<Vector2, Vector2, Vector2> GetTriangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            return MathUtil.GetWindingOrder(v1, v2, v3) == MathUtil.WindingOrder.Clockwise 
                ? new Tuple<Vector2, Vector2, Vector2>(v1, v2, v3) 
                : new Tuple<Vector2, Vector2, Vector2>(v3, v2, v1);
        }

        public Triangle(Tuple<Vector2, Vector2, Vector2> tupple)
        {
            V1 = tupple.Item1;
            V2 = tupple.Item2;
            V3 = tupple.Item3;
        }

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Color4 color)
        {
            Color = color;

            if (MathUtil.GetWindingOrder(v1, v2, v3) == MathUtil.WindingOrder.Clockwise)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
            }
            else
            {
                V1 = v3;
                V2 = v2;
                V3 = v1;
            }
        }

        public Triangle(Vector2 v1, Vector2 v2, Vector2 v3)
        {
            Color = SharpDX.Color.White;

            if (MathUtil.GetWindingOrder(v1, v2, v3) == MathUtil.WindingOrder.Clockwise)
            {
                V1 = v1;
                V2 = v2;
                V3 = v3;
            }
            else
            {
                V1 = v3;
                V2 = v2;
                V3 = v1;
            }
        }

        public override IEnumerable<Vertex2DPositionColor> GetVertices()
        {
            yield return new Vertex2DPositionColor { Color = Color, Position = V1.AsSharpDxVector2() };
            yield return new Vertex2DPositionColor { Color = Color, Position = V2.AsSharpDxVector2() };
            yield return new Vertex2DPositionColor { Color = Color, Position = V3.AsSharpDxVector2() };
        }
    }
}
