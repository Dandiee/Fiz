using System;
using Common;

namespace Physics.Collisions.Polygons.Detector
{
    public class ExpandingSimplexEdge : IComparable<ExpandingSimplexEdge>
    {
        public Vector2 Point1 { get; set; }
        public Vector2 Point2 { get; set; }
        public Vector2 Normal { get; set; }
        public double Distance { get; set; }

        public ExpandingSimplexEdge(Vector2 point1, Vector2 point2, int winding)
        {
            Normal = new Vector2(point2.X - point1.X, point2.Y - point1.Y);
            
            Normal = winding < 0 ? new Vector2(-Normal.Y, Normal.X) : new Vector2(Normal.Y, -Normal.X);

            Normal = Vector2.Normalize(Normal);
            Distance = Math.Abs(point1.X * Normal.X + point1.Y * Normal.Y);
            Point1 = point1;
            Point2 = point2;
        }



        public int CompareTo(ExpandingSimplexEdge o)
        {
            if (Distance < o.Distance) return -1;
            if (Distance > o.Distance) return 1;
            return 0;
        }
    }
}