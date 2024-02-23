using Common;
using Physics.Collisions.Manifolds;

namespace Physics
{
    partial class BodyConnection
    {
        private void ApplyImpulse(ManifoldPoint point, Vector2 impulse)
        {
            if (impulse.LengthSquared() < 0.00001f)
                return;

            Body1.Velocity -= Body1.InverseMass * impulse;
            Body2.Velocity += Body2.InverseMass * impulse;
            Body1.AngularVelocity -= Body1.InverseInertia * Vector2.Cross(point.R1, impulse);
            Body2.AngularVelocity += Body2.InverseInertia * Vector2.Cross(point.R2, impulse);
        }

        private float GetMass(Vector2 r1, Vector2 r2, Vector2 direction, float nominator)
        {
            var rd1 = Vector2.Cross(r1, direction);
            var rd2 = Vector2.Cross(r2, direction);

            var impulse = Body1.InverseMass + Body2.InverseMass + Body1.InverseInertia * rd1 * rd1 + Body2.InverseInertia * rd2 * rd2;

            return impulse > 0.0f ? nominator / impulse : 0.0f;
        }

        private float GetLambda(ManifoldPoint point, Vector2 direction, float mass, float bias)
        {
            var deltaV = Body2.Velocity + Vector2.Cross(Body2.AngularVelocity, point.R2) - Body1.Velocity - Vector2.Cross(Body1.AngularVelocity, point.R1);
            var vDirection = Vector2.Dot(deltaV, direction);
            return mass * (vDirection - bias);
        }
    }
}
