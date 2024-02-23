using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public class AngleJoint : GenericJoint<float, float>
    {
        public readonly float Angle;
        public readonly float DampingRatio;
        public readonly float Omega;
        public readonly bool IsSoft;

        private float _bias;
        private float _gamma;

        public AngleJoint(Body body1, Body body2
            , float frequency = 0, float dampingRatio = 0)
            : base(body1, body2)
        {
            Angle = body1.Rotation - body2.Rotation;
            IsSoft = frequency > 0;

            if (IsSoft)
            {
                DampingRatio = dampingRatio;
                Omega = MathUtil.TwoPi * frequency;
            }
        }

        public override void InitializeVelocityConstraints()
        {
            InverseMass = GetInverseMass();

            var cDot = Body1.Rotation - Body2.Rotation - Angle;

            if (IsSoft)
            {
                var mass = (1.0f/InverseMass);
                var k = mass*Omega*Omega;
                _gamma = 1.0f/(Settings.TimeStep*((2.0f*mass*DampingRatio*Omega) + Settings.TimeStep*k));
                _bias = (cDot - Angle)*Settings.TimeStep*k*_gamma;
                InverseMass += _gamma;
            }

            ApplyImpulse(AccumulatedImpulse);
        }

        protected override void ApplyImpulse(float impulse)
        {
            if (Math.Abs(impulse) < 0.00001f)
                return;

            Body1.AngularVelocity += impulse*Body1.InverseInertia;
            Body2.AngularVelocity -= impulse*Body2.InverseInertia;
        }

        private float GetInverseMass()
        {
            return Body1.InverseInertia + Body2.InverseInertia;
        }

        public override void SolveVelocityConstraints()
        {
            var cDot = Body1.AngularVelocity - Body2.AngularVelocity;
            var impulse = -(cDot + _bias + _gamma*AccumulatedImpulse)/InverseMass;
            AccumulatedImpulse += impulse;

            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            if (true || IsSoft) return true;

            var c = Body1.Rotation - Body2.Rotation - Angle;
            var lamda = -GetInverseMass() * c * 100001;

            Body1.AddSpatials(Vector2.Zero, +lamda * Body1.InverseInertia);
            Body2.AddSpatials(Vector2.Zero, -lamda * Body2.InverseInertia);
            //Body1.Rotation += lamda * Body1.InverseInertia;
            //Body2.Rotation -= lamda * Body2.InverseInertia;

            return Math.Abs(c) < (2.0f / 180.0f * MathUtil.Pi);
        }
    }
}
