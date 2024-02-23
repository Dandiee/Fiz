using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class LinearMotorJoint : GenericJoint<float, float>
    {
        public float MotorSpeed { get; set; }
        public float MaximumMotorForce { get; set; }

        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly Vector2 N;

        private Vector2 _n;
        private float _nCr1U;
        private float _r2Cn;


        public LinearMotorJoint(Body b1, Body b2, Vector2 localAnchor1, Vector2 localAnchor2, Vector2 axis)
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
            _n = Vector2.Rotate(Body1.RotationVector, N);
            _nCr1U = Vector2.Cross(_n, (r1 + (Body2.Position + r2) - (Body1.Position + r1)));
            _r2Cn = Vector2.Cross(r2, _n);

            InverseMass = GetInverseMass(_nCr1U, _r2Cn);

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

            Body1.Velocity -= m1*impulse*_n;
            Body1.AngularVelocity += i1*impulse*_nCr1U;
            Body2.Velocity += m2*impulse*_n;
            Body2.AngularVelocity += i2*impulse*_r2Cn;
        }

        private float GetInverseMass(float nCr1U, float r2Cn)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            return m1 + m2 + nCr1U*nCr1U*i1 + r2Cn*r2Cn*i2;
        }


        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = Vector2.Dot(-_n, v1) + w1*_nCr1U + Vector2.Dot(_n, v2) + w2*_r2Cn;
            var impulse = (MotorSpeed - cDot)/InverseMass;
            var oldImpulse = AccumulatedImpulse;
            var maxImpulse = Settings.TimeStep*MaximumMotorForce;
            AccumulatedImpulse = MathUtil.Clamp(AccumulatedImpulse + impulse, -maxImpulse, maxImpulse);
            impulse = AccumulatedImpulse - oldImpulse;

            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            return true;
        }
    }
}
