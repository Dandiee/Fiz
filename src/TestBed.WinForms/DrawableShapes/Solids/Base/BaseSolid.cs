namespace TestBed.WinForms.DrawableShapes.Solids.Base
{
    public abstract class BaseSolid : BaseShape
    {
        protected BaseSolid()
        {
            Drawer.RegisterSolid(this);
        }

        public override void Dispose()
        {
            Drawer.DeregisterSolid(this);
        }
    }
}
