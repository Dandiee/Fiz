using System;
using System.Runtime.CompilerServices;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class DistanceJoint : GenericJoint<float, float>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly bool IsSoft;

        public readonly float Length;
        public readonly float DampingRatio;
        public readonly float Omega;

        private Vector2 _n;
        private float _r1Cn;
        private float _r2Cn;
        private float _bias;
        private float _gamma;

        public DistanceJoint(Body body1, Body body2, Vector2 localAnchor1, Vector2 localAnchor2, float frequency = 0, float dampingRatio = 0)
            : base(body1, body2)
        {
            R1 = localAnchor1;
            R2 = localAnchor2;
            IsSoft = frequency > 0;
            
            if (IsSoft)
            {
                DampingRatio = dampingRatio;
                Omega = MathUtil.TwoPi*frequency;
            }

            Length = Vector2.Length(Body1.ToGlobal(R1) - Body2.ToGlobal(R2));
        }

        public override void InitializeVelocityConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            var u = Body2.Position + r2 - Body1.Position - r1;

            _n = Vector2.Normalize(u);
            _r1Cn = Vector2.Cross(r1, _n);
            _r2Cn = Vector2.Cross(r2, _n);

            InverseMass = GetInverseMass(_r1Cn, _r2Cn);

            if (IsSoft)
            {
                var mass = 1.0f / InverseMass;
                var k = mass*Omega*Omega;
                _gamma = 1.0f/(Settings.TimeStep*((2.0f*mass*DampingRatio*Omega) + Settings.TimeStep*k));
                _bias = (u.Length() - Length)*Settings.TimeStep*k*_gamma;
                InverseMass += _gamma;
            }

            ApplyImpulse(AccumulatedImpulse);
        }

        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = Vector2.Dot(_n, v2 - v1) - w1*_r1Cn + w2*_r2Cn;
            var impulse = -(cDot + _bias + _gamma*AccumulatedImpulse)/InverseMass;
            AccumulatedImpulse += impulse;

            ApplyImpulse(impulse);
        }

        private float GetInverseMass(float r1Cn, float r2Cn)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            return m1 + m2 + r1Cn * r1Cn * i1 + r2Cn * r2Cn * i2;
        }

        protected override void ApplyImpulse(float lambda)
        {
            if (Math.Abs(lambda) < 0.00001f)
                return;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= _n*lambda*m1;
            Body1.AngularVelocity -= i1*lambda*_r1Cn;
            Body2.Velocity += _n*lambda*m2;
            Body2.AngularVelocity += i2*lambda*_r2Cn;
        }

        public override bool SolvePositionConstraints()
        {
            if (IsSoft) return true;

            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            var u = (r1 + Body1.Position) - (r2 + Body2.Position);
            
            var c = MathUtil.Clamp(u.Length() - Length, -0.5f, 0.5f);
            
            var n = Vector2.Normalize((r1 + Body1.Position) - (r2 + Body2.Position));
            var r1Cn = Vector2.Cross(r1, n);
            var r2Cn = Vector2.Cross(r2, n);
            var impulse = GetInverseMass(r1Cn, r2Cn)*c;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-n*impulse*m1, -i1*impulse*r1Cn);
            Body2.AddSpatials( n*impulse*m2,  i2*impulse*r2Cn);
            //Body1.Position -= n*impulse*m1;
            //Body1.Rotation -= i1*impulse*r1Cn;
            //Body2.Position += n*impulse*m2;
            //Body2.Rotation += i2*impulse*r2Cn;

            return Math.Abs(c) < 0.005f;
        }
    }
}
