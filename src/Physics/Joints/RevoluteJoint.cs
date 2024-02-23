using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class RevoluteJoint : GenericJoint<Matrix2x2, Vector2>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;

        private Vector2 _r1;
        private Vector2 _r2;

        public RevoluteJoint(Body body1, Body body2, Vector2 globalAnchor)
            : base(body1, body2)
        {
            R1 = body1.ToLocal(globalAnchor);
            R2 = body2.ToLocal(globalAnchor);
        }

        public override void InitializeVelocityConstraints()
        {
            _r1 = Vector2.Rotate(Body1.RotationVector, R1);
            _r2 = Vector2.Rotate(Body2.RotationVector, R2);

            InverseMass = GetInverseMass(_r1, _r2);
            
            ApplyImpulse(AccumulatedImpulse);
        }

        protected override void ApplyImpulse(Vector2 impulse)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= m1*impulse;
            Body1.AngularVelocity -= i1*Vector2.Cross(_r1, impulse);
            Body2.Velocity += m2*impulse;
            Body2.AngularVelocity += i2*Vector2.Cross(_r2, impulse);
        }

        private Matrix2x2 GetInverseMass(Vector2 r1, Vector2 r2)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            var m11 = m1 + m2 + i1*r1.Y*r1.Y + i2*r2.Y*r2.Y;
            var m12 = -i1*r1.X*r1.Y - i2*r2.X*r2.Y;
            var m21 = m12;
            var m22 = m1 + m2 + i1*r1.X*r1.X + i2*r2.X*r2.X;

            return new Matrix2x2(m11, m12, m21, m22);
        }

        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = v2 + Vector2.Cross(w2, _r2) - v1 - Vector2.Cross(w1, _r1);
            var impulse = InverseMass.Solve(-cDot);

            AccumulatedImpulse += impulse;

            ApplyImpulse(impulse);
        }

    
        public override bool SolvePositionConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);

            var c = Body2.ToGlobal(R2) - Body1.ToGlobal(R1);
            var impulse = GetInverseMass(r1, r2).Solve(-c);

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-m1*impulse, -i1*Vector2.Cross(r1, impulse));
            Body2.AddSpatials(+m2*impulse, +i2*Vector2.Cross(r2, impulse));
            //Body1.Position -= m1*impulse;
            //Body1.Rotation -= i1*Vector2.Cross(r1, impulse);
            //Body2.Position += m2*impulse;
            //Body2.Rotation += i2*Vector2.Cross(r2, impulse);

            return c.Length() < 0.005f;
        }
    }
}
