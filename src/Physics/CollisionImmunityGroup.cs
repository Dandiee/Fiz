namespace Physics
{
    public class CollisionImmunityGroup
    {
        private static int _instanceCounter;
        public string Name { get; private set; }
        public readonly int Id;

        public CollisionImmunityGroup(string name)
        {
            _instanceCounter++;

            Name = name;
            Id = _instanceCounter;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
