namespace Common
{
    public struct Int2
    {
        public int X;
        public int Y;

        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Int2(int xy)
            : this()
        {
            X = Y = xy;
        }

    }
}
