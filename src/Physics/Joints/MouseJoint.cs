using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class MouseJoint : GenericJoint<Matrix2x2, Vector2>
    {
        public override bool IsVisible
        {
            get { return true; }
        }

        public Vector2 Target { get; set; }
        private readonly float _maximumForce;
        public readonly Vector2 R;
        public readonly float DampingRatio;
        public readonly float Omega;

        private Vector2 _r;
        private Vector2 _bias;
        private float _gamma;

        public MouseJoint(Body body, Vector2 globalAnchor)
            : base(body, null)
        {
            R = Body1.ToLocal(globalAnchor);
            Target = globalAnchor;
            _maximumForce = 1000.0f * body.Mass;
            DampingRatio = 0.7f;
            Omega = MathUtil.TwoPi * 5f;
        }

        public override void InitializeVelocityConstraints()
        {
            _r = Vector2.Rotate(Body1.RotationVector, R);
            InverseMass = GetInverseMass(_r);
            
            var k = Body1.Mass*Omega*Omega;
            _gamma = 1.0f/(Settings.TimeStep*((2.0f*Body1.Mass*DampingRatio*Omega) + Settings.TimeStep*k));
            _bias = (Body1.ToGlobal(R) - Target)*Settings.TimeStep*k*_gamma;

            InverseMass += new Matrix2x2(_gamma, 0, 0, _gamma);

            ApplyImpulse(AccumulatedImpulse);
        }

        private Matrix2x2 GetInverseMass(Vector2 r)
        {
            var m = Body1.InverseMass;
            var i = Body1.InverseInertia;

            var a11 = m + i*r.Y*r.Y;
            var a12 = -i*r.X*r.Y;
            var a21 = a12;
            var a22 = m + i*r.X*r.X;
        
            return new Matrix2x2(a11, a12, a21, a22);
        }


        protected override void ApplyImpulse(Vector2 impulse)
        {
            if (impulse.LengthSquared() < 0.00001f)
                return;

            var m = Body1.InverseMass;
            var i = Body1.InverseInertia;

            Body1.Velocity += m*impulse;
            Body1.AngularVelocity += i*(impulse.Y*_r.X - impulse.X*_r.Y);
        }

        public override void SolveVelocityConstraints()
        {
            var v = Body1.Velocity;
            var w = Body1.AngularVelocity;

            var cDot = v + Vector2.Cross(w, _r);
            var impulse = InverseMass.Solve(-(cDot + _bias + _gamma*AccumulatedImpulse));
            
            var oldImpulse = AccumulatedImpulse;
            AccumulatedImpulse += impulse;
            var maxImpulse = Settings.TimeStep*_maximumForce;
            if (AccumulatedImpulse.Length() > maxImpulse)
                AccumulatedImpulse *= maxImpulse / AccumulatedImpulse.Length();

            impulse = AccumulatedImpulse - oldImpulse;

            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            return true;
        }
    }
}
