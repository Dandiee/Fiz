using System;
using Common;

namespace Physics.Bodies
{
    public sealed class Segment : ClipableBody
    {
        public Vector2 P1 { get; private set; }
        public Vector2 P2 { get; private set; }
        public float LengthSquared { get; private set; }
        public Vector2 NormalizedDirection { get; private set; }
        public Vector2 DenormalizedDirection { get; private set; }
        public Vector2 Normal { get; private set; }

        public override bool IsPointInside(Vector2 point)
        {
            return false;
        }

        public override void AddSpatials(Vector2 position, float rotation) { }

        public Segment(Vector2 p1, Vector2 p2)
        {
            Density = 0.2f;
            Friction = 1f;
            Restitution = 0.3f;
            IsLocked = true;

            P1 = p1;
            P2 = p2;
            Position = (p1 + p2)/2f;
            LocalVertices = new[]
            {
                P1 - Position, P2 - Position
            };

            GlobalVertices = new []
            {
                P1, P2
            };


            RotationVector = TrigonoUtil.RotationVector(0);
            
            GlobalMin = new Vector2(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y));
            GlobalMax = new Vector2(Math.Max(p1.X, p2.X), Math.Max(p1.Y, p2.Y));
            GlobalSize = GlobalMax - GlobalMin;
            
            LengthSquared = Vector2.DistanceSquared(p1, p2);
            DenormalizedDirection = p2 - p1;
            NormalizedDirection = Vector2.Normalize(DenormalizedDirection);
            Normal = Vector2.Cross(1, NormalizedDirection);
            BoundingCircleRadius = Vector2.Distance(p1, p2)/2f;
        }
    }
}
