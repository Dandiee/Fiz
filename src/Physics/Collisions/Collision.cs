using System;
using Physics.Bodies;
using Physics.Collisions.Manifolds;

namespace Physics.Collisions
{
    public static class Collision
    {
        public static Manifold Detect(Body body1, Body body2)
        {
            if (body1 is ClipableBody && body2 is ClipableBody)
                return CollisionClipableClipable.Detect(body1 as ClipableBody, body2 as ClipableBody);
            if (body1 is Circle && body2 is Circle)
                return CollisionCircleCircle.Detect(body1 as Circle, body2 as Circle);
            if (body1 is Polygon && body2 is Circle)
                return CollisionPolygonCircle.Detect(body1 as Polygon, body2 as Circle, false);
            if (body1 is Circle && body2 is Polygon)
                return CollisionPolygonCircle.Detect(body2 as Polygon, body1 as Circle, true);
            if (body1 is Circle && body2 is Segment)
                return CollisionSegmentCircle.Detect(body2 as Segment, body1 as Circle, true);
            if (body1 is Segment && body2 is Circle)
                return CollisionSegmentCircle.Detect(body1 as Segment, body2 as Circle, false);

            throw new NotSupportedException();
        }
    }
}
