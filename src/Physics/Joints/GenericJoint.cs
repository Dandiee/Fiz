using Physics.Bodies;

namespace Physics.Joints
{
    public abstract class GenericJoint<TMass, TImpulse> : Joint
    {
        protected TImpulse AccumulatedImpulse;
        protected TMass InverseMass;

        protected GenericJoint(Body body) : base(body) { }
        protected GenericJoint(Body body1, Body body2) : base(body1, body2) { }

        protected abstract void ApplyImpulse(TImpulse impulse);
    }
}
