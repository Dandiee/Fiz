using Physics.Bodies;

namespace Physics.Joints
{
    public abstract class Joint
    {
        public virtual bool IsVisible { get; set; }

        public readonly Body Body1;
        public readonly Body Body2;

        public string Name { get; set; }

        public abstract void InitializeVelocityConstraints();
        public abstract void SolveVelocityConstraints();
        public abstract bool SolvePositionConstraints();

        protected Joint(Body body)
        {
            Body1 = body;
        }

        protected Joint(Body body1, Body body2)
        {
            Body1 = body1;
            Body2 = body2;
        }
    }
}
