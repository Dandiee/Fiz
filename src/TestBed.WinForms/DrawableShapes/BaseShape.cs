using System;
using System.Collections.Generic;
using SharpDX;
using TestBed.WinForms.DrawableShapes.Vertices;

namespace TestBed.WinForms.DrawableShapes
{
    public abstract class BaseShape : IDisposable
    {
        public virtual Color4 Color { get; set; }
        public abstract void Dispose();
        public abstract IEnumerable<Vertex2DPositionColor> GetVertices();
    }
}
