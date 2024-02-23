using System;
using System.Collections.Generic;
using Common;
using Physics.Bodies;
using Physics.Helpers;

namespace Scenarios.Helpers
{
    public static class SegmentHelper
    {
        private static readonly Random Random = new Random();

        public static IEnumerable<Segment> GetRandomSegmentGround(int xFrom, int xTo, float horizontalStep, float noise)
        {
            var startPoint = new Vector2(xFrom, 0);
            var steps = (xTo - xFrom) / horizontalStep;

            for (var i = 0; i < steps; i++)
            {
                var v1 = startPoint;
                var v2 = startPoint + new Vector2(horizontalStep, Random.NextFloat(-noise, noise));
                startPoint = v2;
                yield return BodyBuilder.BuildSegment(v1, v2);
            }
        }
    }
}
