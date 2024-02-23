﻿namespace Common
{
    public struct Ray
    {
        public Vector3 Position;
        public Vector3 Direction;

        public Ray(Vector3 position, Vector3 direction) : this()
        {
            Position = position;
            Direction = direction;
        }
    }
}
