namespace TestBed.WinForms.DrawableShapes.Wireframes.Base
{
    public abstract class BaseWireframe : BaseShape
    {
        protected BaseWireframe()
        {
            Drawer.RegisterWireframe(this);
        }

        public override void Dispose()
        {
            Drawer.DeregisterWireframe(this);
        }
    }
}
