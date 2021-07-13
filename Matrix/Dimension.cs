namespace Molytho.Matrix
{
    public sealed record Dimension(int Height, int Width)
    {
        public bool IsValid => Height > 0 && Width > 0;
    }
}
