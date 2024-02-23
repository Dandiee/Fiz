using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class PulleyJoint : GenericJoint<float, float>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly Vector2 A1;
        public readonly Vector2 A2;
        public readonly float R;
        public readonly float Length;

        private float _r1Cn1;
        private float _r2Cn2;
        private Vector2 _n1;
        private Vector2 _n2;

        public PulleyJoint(Body body1, Body body2, Vector2 localAnchor1, Vector2 localAnchor2, Vector2 anchor1, Vector2 anchor2, float ratio)
            : base(body1, body2)
        {
            R1 = localAnchor1;
            R2 = localAnchor2;
            A1 = anchor1;
            A2 = anchor2;
            R = ratio;
            Length = R*Vector2.Length(Body1.ToGlobal(R1) - A1) + Vector2.Length(Body2.ToGlobal(R2) - A2);
        }

        public override void InitializeVelocityConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            _n1 = Vector2.Normalize(Body1.Position + r1 - A1);
            _n2 = Vector2.Normalize(Body2.Position + r2 - A2);
            _r1Cn1 = Vector2.Cross(r1, _n1);
            _r2Cn2 = Vector2.Cross(r2, _n2);

            InverseMass = GetInverseMass(_r1Cn1, _r2Cn2);

            ApplyImpulse(AccumulatedImpulse);
        }

        private float GetInverseMass(float r1Cn1, float r2Cn2)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            return m1*R*R + m2 + R*R*i1*r1Cn1*r1Cn1 + i2*r2Cn2*r2Cn2;
        }

        protected override void ApplyImpulse(float impulse)
        {
            if (Math.Abs(impulse) < 0.00001f)
                return;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= m1*R*_n1*impulse;
            Body1.AngularVelocity -= i1*R*_r1Cn1*impulse;
            Body2.Velocity -= m2*_n2*impulse;
            Body2.AngularVelocity -= i2*_r2Cn2*impulse;
        }

        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDot = -R*Vector2.Dot(_n1, v1) - w1*R*_r1Cn1 + Vector2.Dot(-_n2, v2) - w2*_r2Cn2;
            var impulse = -cDot/InverseMass;
            AccumulatedImpulse += impulse;

            ApplyImpulse(impulse);
            
        }

        public override bool SolvePositionConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);
            var n1 = Vector2.Normalize(Body1.Position + r1 - A1);
            var n2 = Vector2.Normalize(Body2.Position + r2 - A2);
            var r1Cn1 = Vector2.Cross(r1, n1);
            var r2Cn2 = Vector2.Cross(r2, n2);
            var length = R*Vector2.Length(Body1.ToGlobal(R1) - A1) + Vector2.Length(Body2.ToGlobal(R2) - A2);

            var c = MathUtil.Clamp(length - Length, -2.5f, 2.54f);
            var impulse = GetInverseMass(r1Cn1, r2Cn2)*c;

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-m1*R*n1*impulse, -i1*R*r1Cn1*impulse);
            Body2.AddSpatials(-m2*n2*impulse, -i2*r2Cn2*impulse);
            //Body1.Position -= m1*R*n1*impulse;
            //Body1.Rotation -= i1*R*r1Cn1*impulse;
            //Body2.Position -= m2*n2*impulse;
            //Body2.Rotation -= i2*r2Cn2*impulse;

            return c < 0.005f;
        }
    }
}
