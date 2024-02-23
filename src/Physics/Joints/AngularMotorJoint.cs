using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class AngularMotorJoint : GenericJoint<float, float>
    {
        public float MotorSpeed { get; set; }
        public float MaximumMotorTorque { get; set; }

        public AngularMotorJoint(Body body1, Body body2)
            : base(body1, body2) { }

        protected override void ApplyImpulse(float impulse)
        {
            if (Math.Abs(impulse) < 0.00001f)
                return;

            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AngularVelocity -= i1*impulse;
            Body2.AngularVelocity += i2*impulse;
        }

        private float GetInverseMass()
        {
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            return 1.0f / (i1 + i2);
        }

        public override void InitializeVelocityConstraints()
        {
            InverseMass = GetInverseMass();
            ApplyImpulse(AccumulatedImpulse);
        }

        public override void SolveVelocityConstraints()
        {
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = w2 - w1 - MotorSpeed;
            var impulse = -InverseMass*cDot;
            var oldImpulse = AccumulatedImpulse;
            var maxImpulse = Settings.TimeStep*MaximumMotorTorque;
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
