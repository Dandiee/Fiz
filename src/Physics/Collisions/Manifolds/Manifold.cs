using System.Collections.Generic;
using System.Linq;
using Common;
using Physics.Bodies;

namespace Physics.Collisions.Manifolds
{
    public class Manifold
    {
        public List<ManifoldPoint> Points { get; set; }

        public bool IsFlipped { get; set; }
        public Vector2 Normal { get; set; }
        public Vector2 Tangent { get; set; }
        public Vector2 ReferenceEdgeLocalNormal { get; set; }
        public Vector2 ReferenceEdgeLocalMiddlePoint { get; set; }
        
        public Body ReferenceBody { get; set; }
        public Body IncidentBody { get; set; }

        public Matrix2x2 EffectiveMass { get; set; }
        public Matrix2x2 ContactMass { get; set; }

        public void Update(Manifold newManifold)
        {
            var mergedManifoldPoints = new List<ManifoldPoint>();

            foreach (var newPoint in newManifold.Points)
            {
                // TODO: valami jobb indexelési módja a cachenek
                var existingContact = Points.FirstOrDefault(oldContact => Vector2.Distance(newPoint.GlobalVertex, oldContact.GlobalVertex) < 0.1f);

                if (existingContact != null)
                {
                    newPoint.ContactImpulse = existingContact.ContactImpulse;
                    newPoint.FrictionImpulse = existingContact.FrictionImpulse;
                    newPoint.VelocityBias = existingContact.VelocityBias;

                    newPoint.Warmed = true;
                }
                else
                {
                    newPoint.Warmed = false;
                }


                mergedManifoldPoints.Add(newPoint);
            }

            IsFlipped = newManifold.IsFlipped;
            ReferenceEdgeLocalNormal = newManifold.ReferenceEdgeLocalNormal;
            ReferenceBody = newManifold.ReferenceBody;
            IncidentBody = newManifold.IncidentBody;
            ReferenceEdgeLocalMiddlePoint = newManifold.ReferenceEdgeLocalMiddlePoint;
            Normal = newManifold.Normal;
            Tangent = newManifold.Tangent;
            Points = mergedManifoldPoints;
        }

        

        public void UpdateNormal(Body b1, Body b2)
        {
            Normal = Vector2.Rotate(ReferenceBody.RotationVector, ReferenceEdgeLocalNormal) * -(IsFlipped ? -1 : 1);
            Tangent = Vector2.Cross(Normal, 1);
        }

        public Manifold(List<ManifoldPoint> points, Vector2 normal)
        {
            Points = points;
            Normal = normal;
            Tangent = Vector2.Cross(Normal, 1);
        }

        public Manifold()
        {
            Points = new List<ManifoldPoint>(2);
        }
    }
}
