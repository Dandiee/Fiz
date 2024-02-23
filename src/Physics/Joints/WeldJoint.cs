using System;
using Common;
using Physics.Bodies;

namespace Physics.Joints
{
    public sealed class WeldJoint : GenericJoint<Matrix3x3, Vector3>
    {
        public readonly Vector2 R1;
        public readonly Vector2 R2;
        public readonly float Angle;

        private Vector2 _r1;
        private Vector2 _r2;

        public WeldJoint(Body body1, Body body2, Vector2 globalAnchor)
            : base(body1, body2)
        {
            R1 = body1.ToLocal(globalAnchor);
            R2 = body2.ToLocal(globalAnchor);

            Angle = body1.Rotation - body2.Rotation;
        }

        public WeldJoint(Body body1, Body body2)
            : base(body1, body2)
        {
            var globalAnchor = (body1.Position + body2.Position)/2;

            R1 = body1.ToLocal(globalAnchor);
            R2 = body2.ToLocal(globalAnchor);

            Angle = body1.Rotation - body2.Rotation;
        }

        public override void InitializeVelocityConstraints()
        {
            _r1 = Vector2.Rotate(Body1.RotationVector, R1);
            _r2 = Vector2.Rotate(Body2.RotationVector, R2);
            InverseMass = GetInverseMass(_r1, _r2);

            ApplyImpulse(AccumulatedImpulse);
        }

        private Matrix3x3 GetInverseMass(Vector2 r1, Vector2 r2)
        {
            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            var a11 = m1 + m2 + i1*r1.Y*r1.Y + i2*r2.Y*r2.Y;
            var a12 = -i1*r1.Y*r1.X - i2*r2.Y*r2.X;
            var a13 = r1.Y*i1 - r2.Y*i2;
            var a21 = -i1*r1.Y*r1.X - i2*r2.Y*r2.X;
            var a22 = m1 + m2 + i1*r1.X*r1.X + i2*r2.X*r2.X;
            var a23 = r2.X*i2 - r1.X*i1;
            var a31 = r1.Y*i1 + r2.Y*i2;
            var a32 = -r1.X*i1 - r2.X*i2;
            var a33 = i1 + i2;

            return new Matrix3x3(
                    new Vector3(a11, a21, a31),
                    new Vector3(a12, a22, a32),
                    new Vector3(a13, a23, a33));
        }

        protected override void ApplyImpulse(Vector3 impulse)
        {
            if (impulse.LengthSquared() < 0.00001f)
                return;

            var impulseXy = new Vector2(impulse.X, impulse.Y);

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.Velocity -= m1*impulseXy;
            Body1.AngularVelocity += impulse.X*_r1.Y*i1 - impulse.Y*_r1.X*i1 + i1*impulse.Z;
            Body2.Velocity += m2*impulseXy;
            Body2.AngularVelocity += impulse.Y*_r2.X*i2 - impulse.X*_r2.Y*i2 - i2*impulse.Z;
        }

        public override void SolveVelocityConstraints()
        {
            var v1 = Body1.Velocity;
            var v2 = Body2.Velocity;
            var w1 = Body1.AngularVelocity;
            var w2 = Body2.AngularVelocity;

            var cDotRevolute = v2 + Vector2.Cross(w2, _r2) - v1 - Vector2.Cross(w1, _r1);
            var cDotAngle = w1 - w2;
            var cDot = new Vector3(cDotRevolute, cDotAngle);

            var impulse = InverseMass.Solve33(-cDot);
            AccumulatedImpulse += impulse;
            
            ApplyImpulse(impulse);
        }

        public override bool SolvePositionConstraints()
        {
            var r1 = Vector2.Rotate(Body1.RotationVector, R1);
            var r2 = Vector2.Rotate(Body2.RotationVector, R2);

            var cRevolute = Body2.ToGlobal(R2) - Body1.ToGlobal(R1);
            var cAngle = Body1.Rotation - Body2.Rotation - Angle;
            var c = new Vector3(cRevolute, cAngle);

            var lambda = -GetInverseMass(r1, r2).Solve33(c);
            var lambdaXy = new Vector2(lambda.X, lambda.Y);

            var m1 = Body1.InverseMass;
            var m2 = Body2.InverseMass;
            var i1 = Body1.InverseInertia;
            var i2 = Body2.InverseInertia;

            Body1.AddSpatials(-m1*lambdaXy, lambda.X*r1.Y*i1 - lambda.Y*r1.X*i1 + i1*lambda.Z);
            Body2.AddSpatials(+m2*lambdaXy, lambda.Y*r2.X*i2 - lambda.X*r2.Y*i2 - i2*lambda.Z);
            //Body1.Position -= m1*lambdaXy;
            //Body1.Rotation += lambda.X*r1.Y*i1 - lambda.Y*r1.X*i1 + i1*lambda.Z;
            //Body2.Position += m2*lambdaXy;
            //Body2.Rotation += lambda.Y*r2.X*i2 - lambda.X*r2.Y*i2 - i2*lambda.Z;

            return lambdaXy.Length() < 0.02f && Math.Abs(lambda.Z) <= (2.0f/180.0f*MathUtil.Pi);
        }
    }
}
