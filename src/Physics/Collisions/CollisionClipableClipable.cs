using System;
using Physics.Bodies;
using Physics.Collisions.Manifolds;
using Physics.Collisions.Polygons.Clipping;
using Physics.Collisions.Polygons.Detector;

namespace Physics.Collisions
{
    public class CollisionClipableClipable
    {
        public static Manifold Detect(ClipableBody clipable1, ClipableBody clipable2)
        {
            if (Settings.IsBoundingBoxCollisionCheckEnabled)
            {
                var aabbCheck = (Math.Abs(clipable1.Position.X - clipable2.Position.X) * 2 <
                                 (clipable1.GlobalSize.X + clipable2.GlobalSize.X)) &&
                                (Math.Abs(clipable1.Position.Y - clipable2.Position.Y) * 2 <
                                 (clipable1.GlobalSize.Y + clipable2.GlobalSize.Y));

                if (!aabbCheck)
                    return null;
            }


            var gjkResult = Gjk.IsColliding(clipable1, clipable2);
            if (gjkResult.IsColliding)
            {
                var epaResult = Epa.GetPenetration(gjkResult.Simplex, clipable1, clipable2);
                var manifold = ClippingManifoldSolver.GetManifold(epaResult.Normal, epaResult.Depth, clipable1, clipable2);
                
                return manifold;
            }
            
            return null;
        }
    }
}
