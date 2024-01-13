using UnityEngine;

namespace DPM.Infrastructure
{
    public enum Direction
    {
        Top = 0,
        Left = 1,
        Bottom = 2,
        Right = 3
    }

    static class Extensions
    {
        private static Vector2Int[] vectors = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.left,
        Vector2Int.down,
        Vector2Int.right,
        };

        public static int ToInt(this Direction value)
        {
            return (int)value;
        }

        public static Vector2Int Vector(this Direction dir)
        {
            return vectors[dir.ToInt()];
        }
    }
}