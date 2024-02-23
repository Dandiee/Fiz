namespace Physics.Bodies
{
    public class BodyDefinition
    {
        public static readonly BodyDefinition Default = new BodyDefinition
        {
            Density = 15f,
            Friction = 0.5f,
            Restitution = 0.3f,
        };

        public static readonly BodyDefinition DefaultLocked = new BodyDefinition
        {
            Density = 0.2f,
            Friction = 0.5f,
            Restitution = 0.3f,
            IsLocked = true,
        };

        public static readonly BodyDefinition DefaultCollisionImmune = new BodyDefinition
        {
            Density = 0.2f,
            Friction = 0.5f,
            Restitution = 0.3f,
            IsCollisionImmune = true,
        };

        public float Density { get; set; }
        public float Rotation { get; set; }
        public float Restitution { get; set; }
        public float Friction { get; set; }
        public bool IsLocked { get; set; }
        public bool IsCollisionImmune { get; set; }

        public BodyDefinition AsLocked()
        {
            var copy = Copy();
            copy.IsLocked = true;
            return copy;
        }

        public BodyDefinition WithRotation(float angle)
        {
            var copy = Copy();
            copy.Rotation = angle;
            return copy;
        }

        public BodyDefinition WithFriction(float friction)
        {
            var copy = Copy();
            copy.Friction = friction;
            return copy;
        }

        public BodyDefinition WithRestitution(float restitution)
        {
            var copy = Copy();
            copy.Restitution = restitution;
            return copy;
        }

        public BodyDefinition WithDensity(float density)
        {
            var copy = Copy();
            copy.Density = density;
            return copy;
        }

        private BodyDefinition Copy()
        {
            return new BodyDefinition
            {
                Density = Density,
                Friction = Friction,
                IsLocked = IsLocked,
                Restitution = Restitution,
                Rotation = Rotation
            };
        }
    }
}
