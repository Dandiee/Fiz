using Common;

namespace Physics.Collisions.Manifolds
{
    public class ManifoldPoint
    {
        public Vector2 GlobalVertex { get; private set; }
        public Vector2 LocalVertex { get; private set; }

        public bool Warmed { get; set; }

        public float VelocityBias { get; set; }
        
        public Vector2 R1 { get; set; }
        public Vector2 R2 { get; set; }

        public float ContactMass { get; set; }
        public float FrictionMass { get; set; }

        public float ContactImpulse { get; set; }
        public float FrictionImpulse { get; set; }

        public ManifoldPoint(Vector2 globalVertex, Vector2 localVertex)
        {
            GlobalVertex = globalVertex;
            LocalVertex = localVertex;
        }
    }
}
