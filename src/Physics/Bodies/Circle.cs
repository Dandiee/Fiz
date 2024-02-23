using System;
using Common;

namespace Physics.Bodies
{
    public sealed class Circle : Body
    {
        public float Radius { get; set; }

        public Circle(Vector2 position, float radius, BodyDefinition definition = null)
        {
            if (definition == null)
                definition = BodyDefinition.Default;

            Radius = radius;
            Area = radius * radius * MathUtil.Pi;
            Position = position;
            Rotation = definition.Rotation;
            Restitution = definition.Restitution;
            Friction = definition.Friction;
            Density = definition.Density;
            BoundingCircleRadius = radius;
            IsCollisionImmune = definition.IsCollisionImmune;
            RotationVector = TrigonoUtil.RotationVector(Rotation);

            GlobalMin = position - new Vector2(radius);
            GlobalMax = position + new Vector2(radius);
            GlobalSize = GlobalMax - GlobalMin;

            if (definition.IsLocked)
            {
                Inertia = float.MaxValue;
                Mass = float.MaxValue;
                IsLocked = true;
            }
            else
            {
                Mass = Area * Density;
                Inertia = (MathUtil.Pi * (float)Math.Pow(radius, 4)) / 4f;
                InverseMass = 1f / Mass;
                InverseInertia = 1f / Inertia;
            }
        }

        public override bool IsPointInside(Vector2 point)
        {
            return Vector2.Distance(point, Position) < Radius;
        }

        public override void AddSpatials(Vector2 position, float rotation)
        {
            var wasRotated = Math.Abs(rotation) > .0001f;
            if(wasRotated)
                RotationVector = TrigonoUtil.RotationVector(Rotation);

            AddSpatialsValues(position, rotation);

            GlobalMax += position;
            GlobalMin += position;
        }
    }
}
