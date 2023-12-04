public enum Direction
{
    Top,
    Left,
    Bottom,
    Right
}

static class Extensions
{
    public static int ToInt(this Direction value)
    {
        return (int)value;
    }
}