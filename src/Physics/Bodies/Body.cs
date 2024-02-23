using System.Collections.Generic;
using System.Linq;
using Common;

namespace Physics.Bodies
{
    public abstract class Body
    {
        private readonly HashSet<CollisionImmunityGroup> _collisionImmunityGroups;
        private static int _instanceCounter;

        public Vector2 GlobalMin { get; protected set; }
        public Vector2 GlobalMax { get; protected set; }
        public Vector2 GlobalSize { get; protected set; }

        //public Node Node { get; set; }

        protected Body()
        {
            _instanceCounter++;
            Id = _instanceCounter;
            _collisionImmunityGroups = new HashSet<CollisionImmunityGroup>();
        }

        public void AddToCollisionImmunityGroup(CollisionImmunityGroup collisionImmunityGroup)
        {
            _collisionImmunityGroups.Add(collisionImmunityGroup);
        }

        public void RemoveFromCollisionImmunityGroup(CollisionImmunityGroup collisionImmunityGroup)
        {
            _collisionImmunityGroups.Remove(collisionImmunityGroup);
        }

        public bool IsInCollisionImmunityGroup(CollisionImmunityGroup collisionImmunityGroup)
        {
            return _collisionImmunityGroups.Contains(collisionImmunityGroup);
        }

        public bool IsInAnyCollisionImmunityGroup(IEnumerable<CollisionImmunityGroup> collisionImmunityGroups)
        {
            return collisionImmunityGroups.Any(collisionImmunityGroup => _collisionImmunityGroups.Contains(collisionImmunityGroup));
        }

        public IEnumerable<CollisionImmunityGroup> GetCollisionImmunityGroups()
        {
            return _collisionImmunityGroups;
        }

        public int Id { get; set; }

        public object Tag { get; set; }

        public abstract bool IsPointInside(Vector2 point);
        public abstract void AddSpatials(Vector2 position, float rotation);

        public Vector2 Velocity { get; set; }
        public Vector2 Force { get; set; }
        public string Name { get; set; }
        public float AngularVelocity { get; set; }
        public float Torque { get; set; }

        public Vector2 Position { get; protected set; }
        
        public float Rotation { get; protected set; }
        public Vector2 RotationVector { get; protected set; }


        public Vector2 PrevPosition { get; set; }
        public float PrevRotation { get; set; }


        public Vector2 CurrentPosition { get; set; }
        public float CurrentRotation { get; set; }



        public float BoundingCircleRadius { get; protected set; }
        public float Friction { get; protected set; }
        public float Restitution { get; protected set; }
        public float Mass { get; protected set; }
        public float InverseMass { get; protected set; }
        public float Inertia { get; protected set; }
        public float InverseInertia { get; protected set; }
        public float Density { get; protected set; }
        public float Area { get; protected set; }
        public bool IsLocked { get; protected set; }
        public bool IsCollisionImmune { get; protected set; }

        public event BodyMove Moving;
        public event BodyRotate Rotating;

        public Vector2 ToGlobal(Vector2 localVector)
        {
            return new Vector2(RotationVector.Y * localVector.X - RotationVector.X * localVector.Y + Position.X, RotationVector.X * localVector.X + RotationVector.Y * localVector.Y + Position.Y);
        }

        public Vector2 ToLocal(Vector2 globalVector)
        {
            return Vector2.Rotate(TrigonoUtil.NegateRotationVector(RotationVector), globalVector - Position);
        }

        protected virtual void AddSpatialsValues(Vector2 position, float rotation)
        {
            Position += position;
            Rotation += rotation;
        }

        public void TriggerChanges()
        {
            if (Moving != null && CurrentPosition != PrevPosition)
                    Moving.Invoke(this, new BodyMoveArgs(PrevPosition, CurrentPosition));
            if (Rotating != null && CurrentRotation != PrevRotation)
                Rotating.Invoke(this, new BodyRotateArgs(PrevRotation, CurrentRotation));
        }
    }

    public delegate void BodyMove(object sender, BodyMoveArgs args);
    public delegate void BodyRotate(object sender, BodyRotateArgs args);

    public class BodyMoveArgs
    {
        public Vector2 PreviousPosition { get; private set; }
        public Vector2 NewPosition { get; private set; }

        public BodyMoveArgs(Vector2 previousPosition, Vector2 newPosition)
        {
            PreviousPosition = previousPosition;
            NewPosition = newPosition;
        }
    }

    public class BodyRotateArgs
    {
        public float PreviousRotation { get; private set; }
        public float NewRotation { get; private set; }

        public BodyRotateArgs(float previousRotation, float newRotation)
        {
            PreviousRotation = previousRotation;
            NewRotation = newRotation;
        }

    }
}
