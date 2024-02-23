using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;

namespace Physics.Collisions.Polygons.Detector
{
    public static class Epa
    {
        public static Penetration GetPenetration(List<Vector2> simplex, ClipableBody body1, ClipableBody body2)
        {
            var minkowskiSum = new MinkowskiSum(body1, body2);
            var expandingSimplex = new ExpandingSimplex(simplex);

            ExpandingSimplexEdge edge = null;
            var point = Vector2.Zero;

            for (var i = 0; i < Settings.MaximumEpaIteratins; i++)
            {
                edge = expandingSimplex.GetClosestEdge();
                point = minkowskiSum.GetSupportPoint(edge.Normal);

                var projection = Vector2.Dot(point, edge.Normal);
                if ((projection - edge.Distance) < 1e-2)
                    return new Penetration(edge.Normal, projection);

                expandingSimplex.Expand(point);
            }

            Console.WriteLine("Kudarcot vallott ez a kurva epa");
            return new Penetration(edge.Normal, Vector2.Dot(point, edge.Normal));
        }
    }
}
