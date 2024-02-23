using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class PrismaticJoint : GenericJoint<Matrix2x2, Vector2>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly float Angle;
        public readonly Vector2 T;

        private Vector2 _t;
        private float _tCr1U;
        private float _r2Ct;

        public PrismaticJoint(Body body1, Body body2, Vector2 localAnchor1, Vector2 localAnchor2, Vector2 axis)
            : base(body1, body2)
        {
            R1 = localAnchor1;
            R2 = localAnchor2;
            T = Vector2.Cross(-1, axis);
            Angle = body1.Rotation - body2.Rotation;
        }

        public override void InitializeVelocityConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            _t = Vector2.Rotate(Body2.RotationVector, T);
            _tCr1U = Vector2.Cross(_t, (r1 + (Body2.Position + r2) - (Body1.Position + r1)));
            _r2Ct = Vector2.Cross(r2, _t);

            InverseMass = GetInverseMass(_tCr1U, _r2Ct);

            ApplyImpulse(AccumulatedImpulse);
        }

        protected override void ApplyImpulse(Vector2 impulse)
        {
            if (impulse.LengthSquared() < 0.00001f)
                return;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= m1*impulse.X*_t;
            Body1.AngularVelocity += i1*impulse.X*_tCr1U + i1*impulse.Y;
            Body2.Velocity += m2*impulse*_t.X;
            Body2.AngularVelocity += i2*impulse.X*_r2Ct - i2*impulse.Y;
        }

        private Matrix2x2 GetInverseMass(float tCr1U, float r2Ct)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            var m11 = m1 + m2 + tCr1U*tCr1U*i1 + r2Ct*r2Ct*i2;
            var m12 = tCr1U*i1 - r2Ct*i2;
            var m21 = m12;
            var m22 = i1 + i2;

            return new Matrix2x2(m11, m12, m21, m22);
        }

        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDotLine = Vector2.Dot(-_t, v1) + w1*_tCr1U + Vector2.Dot(_t, v2) + w2*_r2Ct;
            var cDotAngle = w1 - w2;
            var cDot = new Vector2(cDotLine, cDotAngle);
            var impulse = InverseMass.Solve(-cDot);

            AccumulatedImpulse += impulse;
            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            var t = Vector2.Rotate(Body2.RotationVector, T);
            var u = (Body2.Position + r2) - (Body1.Position + r1);
            var tCr1U = Vector2.Cross(t, (r1 + u));
            var r2Ct = Vector2.Cross(r2, t);

            var cLine = Vector2.Dot(t, u);
            var cAngle = Body1.Rotation - Body2.Rotation - Angle;
            var c = new Vector2(cLine, cAngle);
            var impulse = -GetInverseMass(tCr1U, r2Ct).Solve(c);

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-m1*impulse.X*t, i1*impulse.X*tCr1U + i1*impulse.Y);
            Body2.AddSpatials(+m2*impulse*t.X, i2*impulse.X*r2Ct - i2*impulse.Y);
            //Body1.Position -= m1*impulse.X*t;
            //Body1.Rotation += i1*impulse.X*tCr1U + i1*impulse.Y;
            //Body2.Position += m2*impulse*t.X;
            //Body2.Rotation += i2*impulse.X*r2Ct - i2*impulse.Y;

            return Math.Abs(cLine) <= 0.005f && Math.Abs(cAngle) <= (2.0f / 180.0f * MathUtil.Pi);
        }
    }
}
