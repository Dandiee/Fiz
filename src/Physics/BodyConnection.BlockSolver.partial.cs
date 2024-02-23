using Common;

namespace Physics
{
    // Erin Catto féle block solver Cpp-ről migrálva
    partial class BodyConnection
    {
        private void SolveBlockVelocityConstraints(Vector2 n)
        {
            var cp1 = Manifold.Points[0];
            var cp2 = Manifold.Points[1];

            var a = new Vector2(cp1.ContactImpulse, cp2.ContactImpulse);

            // Relative velocity at contact
            var dv1 = Body2.Velocity + Vector2.Cross(Body2.AngularVelocity, cp1.R2) - Body1.Velocity - Vector2.Cross(Body1.AngularVelocity, cp1.R1);
            var dv2 = Body2.Velocity + Vector2.Cross(Body2.AngularVelocity, cp2.R2) - Body1.Velocity - Vector2.Cross(Body1.AngularVelocity, cp2.R1);

            // Compute Normal velocity
            var vn1 = Vector2.Dot(dv1, n);
            var vn2 = Vector2.Dot(dv2, n);

            var b = new Vector2(vn1 - cp1.VelocityBias, vn2 - cp2.VelocityBias);

            // Compute b'
            b -= Manifold.EffectiveMass * a;

            for (; ; )
            {
                // Case 1: vn = 0
                // 0 = A * x + b'
                // Solve for x:
                // x = - inv(A) * b'
                var x = -(Manifold.ContactMass * b);

                if (x.X >= 0.0f && x.Y >= 0.0f)
                {
                    // Get the incremental impulse
                    var d = x - a;

                    // Apply incremental impulse
                    var p1 = d.X * n;
                    var p2 = d.Y * n;
                    Body1.Velocity -= Body1.InverseMass * (p1 + p2);
                    Body1.AngularVelocity -= Body1.InverseInertia * (Vector2.Cross(cp1.R1, p1) + Vector2.Cross(cp2.R1, p2));

                    Body2.Velocity += Body2.InverseMass * (p1 + p2);
                    Body2.AngularVelocity += Body2.InverseInertia * (Vector2.Cross(cp1.R2, p1) + Vector2.Cross(cp2.R2, p2));

                    // Accumulate
                    cp1.ContactImpulse = x.X;
                    cp2.ContactImpulse = x.Y;

                    break;
                }

                //
                // Case 2: vn1 = 0 and x2 = 0
                //
                //   0 = a11 * x1 + a12 * 0 + b1' 
                // vn2 = a21 * x1 + a22 * 0 + b2'
                //
                x.X = -cp1.ContactMass * b.X;
                x.Y = 0.0f;
                vn2 = Manifold.EffectiveMass.Column1.Y * x.X + b.Y;

                if (x.X >= 0.0f && vn2 >= 0.0f)
                {
                    // Get the incremental impulse
                    var d = x - a;

                    // Apply incremental impulse
                    var P1 = d.X * n;
                    var P2 = d.Y * n;
                    Body1.Velocity -= Body1.InverseMass * (P1 + P2);
                    Body1.AngularVelocity -= Body1.InverseInertia * (Vector2.Cross(cp1.R1, P1) + Vector2.Cross(cp2.R1, P2));

                    Body2.Velocity += Body2.InverseMass * (P1 + P2);
                    Body2.AngularVelocity += Body2.InverseInertia * (Vector2.Cross(cp1.R2, P1) + Vector2.Cross(cp2.R2, P2));

                    // Accumulate
                    cp1.ContactImpulse = x.X;
                    cp2.ContactImpulse = x.Y;

                    break;
                }


                //
                // Case 3: vn2 = 0 and x1 = 0
                //
                // vn1 = a11 * 0 + a12 * x2 + b1' 
                //   0 = a21 * 0 + a22 * x2 + b2'
                //
                x.X = 0.0f;
                x.Y = -cp2.ContactMass * b.Y;
                vn1 = Manifold.EffectiveMass.Column2.X * x.Y + b.X;
                vn2 = 0.0f;

                if (x.Y >= 0.0f && vn1 >= 0.0f)
                {
                    // Resubstitute for the incremental impulse
                    var d = x - a;

                    // Apply incremental impulse
                    var P1 = d.X * n;
                    var P2 = d.Y * n;
                    Body1.Velocity -= Body1.InverseMass * (P1 + P2);
                    Body1.AngularVelocity -= Body1.InverseInertia * (Vector2.Cross(cp1.R1, P1) + Vector2.Cross(cp2.R1, P2));

                    Body2.Velocity += Body2.InverseMass * (P1 + P2);
                    Body2.AngularVelocity += Body2.InverseInertia * (Vector2.Cross(cp1.R2, P1) + Vector2.Cross(cp2.R2, P2));

                    // Accumulate
                    cp1.ContactImpulse = x.X;
                    cp2.ContactImpulse = x.Y;


                    break;
                }

                //
                // Case 4: x1 = 0 and x2 = 0
                // 
                // vn1 = b1
                // vn2 = b2;
                x.X = 0.0f;
                x.Y = 0.0f;
                vn1 = b.X;
                vn2 = b.Y;

                if (vn1 >= 0.0f && vn2 >= 0.0f)
                {
                    // Resubstitute for the incremental impulse
                    var d = x - a;

                    // Apply incremental impulse
                    var P1 = d.X * n;
                    var P2 = d.Y * n;
                    Body1.Velocity -= Body1.InverseMass * (P1 + P2);
                    Body1.AngularVelocity -= Body1.InverseInertia * (Vector2.Cross(cp1.R1, P1) + Vector2.Cross(cp2.R1, P2));

                    Body2.Velocity += Body2.InverseMass * (P1 + P2);
                    Body2.AngularVelocity += Body2.InverseInertia * (Vector2.Cross(cp1.R2, P1) + Vector2.Cross(cp2.R2, P2));

                    // Accumulate
                    cp1.ContactImpulse = x.X;
                    cp2.ContactImpulse = x.Y;

                    break;
                }

                // No solution, give up. This is hit sometimes, but it doesn't seem to matter.
                break;
            }
        }

        private void InitializeBlockVelocityConstraints()
        {
            var vcp1 = Manifold.Points[0];
            var vcp2 = Manifold.Points[1];

            var rn1A = Vector2.Cross(vcp1.R1, Manifold.Normal);
            var rn1B = Vector2.Cross(vcp1.R2, Manifold.Normal);
            var rn2A = Vector2.Cross(vcp2.R1, Manifold.Normal);
            var rn2B = Vector2.Cross(vcp2.R2, Manifold.Normal);

            var k11 = Body1.InverseMass + Body2.InverseMass + Body1.InverseInertia * rn1A * rn1A + Body2.InverseInertia * rn1B * rn1B;
            var k22 = Body1.InverseMass + Body2.InverseMass + Body1.InverseInertia * rn2A * rn2A + Body2.InverseInertia * rn2B * rn2B;
            var k12 = Body1.InverseMass + Body2.InverseMass + Body1.InverseInertia * rn1A * rn2A + Body2.InverseInertia * rn1B * rn2B;

            // Ensure a reasonable condition number.
            const float kMaxConditionNumber = 1000.0f;
            if (k11 * k11 < kMaxConditionNumber * (k11 * k22 - k12 * k12))
            {
                Manifold.EffectiveMass = new Matrix2x2(new Vector2(k11, k12), new Vector2(k12, k22));
                Manifold.ContactMass = Manifold.EffectiveMass.Invert();
            }
            else
            {
                // TODO: csak a legmélyebbet kéne visszaadni?
                Manifold.EffectiveMass = new Matrix2x2(new Vector2(k11, k12), new Vector2(k12, k22));
                Manifold.ContactMass = Manifold.EffectiveMass.Invert();
            }
        }
    }
}
