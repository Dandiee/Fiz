using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;

namespace Physics.Collisions.Polygons.Detector
{
    public static class Gjk
    {
        public static int MaxTriesCount = 100;

        //public static GjkResult IsColliding(Polygon polygon1, Polygon polygon2)
        //{
        //    var minkowskiSum = new MinkowskiSum(polygon1, polygon2);
        //    var direction = polygon2.Position - polygon1.Position;

        //    if (Math.Abs(direction.X) < 0.001f && Math.Abs(direction.Y) < 0.001f)
        //        direction = Vector2.UnitY;

        //    var simplex = new List<Vector2>(3);
        //    simplex.Add(minkowskiSum.GetSupportPoint(direction));
            
        //    if (Vector2.Dot(simplex[0], direction) <= 0)
        //        return GjkResult.NoCollision;


        //    direction = -direction;
        //    var triesCounter = 0;
        //    while (true)
        //    {
        //        simplex.Add(minkowskiSum.GetSupportPoint(direction));
        //        if (Vector2.Dot(simplex[simplex.Count - 1], direction) <= 0)
        //            return GjkResult.NoCollision;

        //        if (CheckSimplex(simplex, ref direction))
        //            return new GjkResult(simplex);

        //        if (triesCounter++ > MaxTriesCount)
        //            return GjkResult.NoCollision;
        //    }
        //}

        public static GjkResult IsColliding(ClipableBody body1, ClipableBody body2)
        {
            var minkowskiSum = new MinkowskiSum(body1, body2);
            var direction = body2.Position - body1.Position;

            if (Math.Abs(direction.X) < 0.001f && Math.Abs(direction.Y) < 0.001f)
                direction = Vector2.UnitY;

            var simplex = new List<Vector2>(3);
            simplex.Add(minkowskiSum.GetSupportPoint(direction));

            if (Vector2.Dot(simplex[0], direction) <= 0)
                return GjkResult.NoCollision;


            direction = -direction;
            var triesCounter = 0;
            while (true)
            {
                simplex.Add(minkowskiSum.GetSupportPoint(direction));
                if (Vector2.Dot(simplex[simplex.Count - 1], direction) <= 0)
                    return GjkResult.NoCollision;

                if (CheckSimplex(simplex, ref direction))
                    return new GjkResult(simplex);

                if (triesCounter++ > MaxTriesCount)
                    return GjkResult.NoCollision;
            }
        }

        private static bool CheckSimplex(List<Vector2> simplex, ref Vector2 direction)
        {
            var a = simplex[simplex.Count - 1];
            var ao = -a;
            
            if (simplex.Count == 3)
            {
                var b = simplex[1];
                var c = simplex[0];
                
                var ab = b - a;
                var ac = c - a;
                
                var abPerp = TripleProduct(ac, ab, ab);
                var acPerp = TripleProduct(ab, ac, ac);
                
                var acLocation = Vector2.Dot(acPerp, ao);
                if (acLocation >= 0)
                {
                    simplex.RemoveAt(1);
                    direction = acPerp;
                }
                else
                {
                    var abLocation = Vector2.Dot(abPerp, ao);
                    if (abLocation < 0)
                        return true;
                    
                    simplex.RemoveAt(0);
                    direction = abPerp;
                }
            }
            else
            {
                var b = simplex[0];
                var ab = b - a;
                
                direction = TripleProduct(ab, ao, ab);
                if (direction.LengthSquared() <= 0.00001f)
                    direction = new Vector2(ab.Y, -ab.X);
            }

            return false;
        }

        private static Vector2 TripleProduct(Vector2 a, Vector2 b, Vector2 c)
        {
            return b * Vector2.Dot(a, c) - a * Vector2.Dot(b, c);
        }
    }
}
