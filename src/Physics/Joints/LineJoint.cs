using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class LineJoint : GenericJoint<float, float>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly Vector2 N;

        private Vector2 _t;
        private float _tCr1U;
        private float _r2Ct;

        public LineJoint(Body b1, Body b2, Vector2 localAnchor1, Vector2 localAnchor2, Vector2 axis)
            : base(b1, b2)
        {
            R1 = localAnchor1;
            R2 = localAnchor2;
            N = axis;
        }

        public override void InitializeVelocityConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            _t = Vector2.Cross(1, Vector2.Rotate(Body1.RotationVector, N));
            _tCr1U = Vector2.Cross(_t, (r1 + (Body2.Position + r2) - (Body1.Position + r1)));
            _r2Ct = Vector2.Cross(r2, _t);

            InverseMass = GetInverseMass(_tCr1U, _r2Ct);

            ApplyImpulse(AccumulatedImpulse);
        }

        protected override void ApplyImpulse(float impulse)
        {
            if (Math.Abs(impulse) < 0.00001f)
                return;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= m1*impulse*_t;
            Body1.AngularVelocity += i1*impulse*_tCr1U;
            Body2.Velocity += m2*impulse*_t;
            Body2.AngularVelocity += i2*impulse*_r2Ct;
        }

        private float GetInverseMass(float tCr1U, float r2Ct)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            return m1 + m2 + tCr1U*tCr1U*i1 + r2Ct*r2Ct*i2;
        }


        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = Vector2.Dot(-_t, v1) + w1*_tCr1U + Vector2.Dot(_t, v2) + w2*_r2Ct;
            var impulse = -cDot/InverseMass;
            AccumulatedImpulse += impulse;

            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            var t = Vector2.Cross(1, Vector2.Rotate(Body1.RotationVector, N));
            var u = (Body2.Position + r2) - (Body1.Position + r1);
            var tCr1U = Vector2.Cross(t, (r1 + u));
            var r2Ct = Vector2.Cross(r2, t);

            var c = MathUtil.Clamp(Vector2.Dot(t, u), -0.2f, 0.2f);
            var impulse = -GetInverseMass(tCr1U, r2Ct)*c;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-m1*impulse*t, i1*impulse*tCr1U);
            Body2.AddSpatials(m2*impulse*t, i2*impulse*r2Ct);
            //Body1.Position -= m1*impulse*t;
            //Body1.Rotation += i1*impulse*tCr1U;
            //Body2.Position += m2*impulse*t;
            //Body2.Rotation += i2*impulse*r2Ct;

            return Math.Abs(c) < 0.02f;
        }
    }
}
