using Newtonsoft.Json;
using System.Text;
using UnityEngine;
using DPM.Infrastructure;
using System.Linq;

namespace DPM.Domain
{
    public class Map
    {
        public readonly int MapRows;
        public readonly int MapColumns;
        public readonly int[][] Tiles;

        //  ^ y
        //  |
        //  |      x
        //  +------>

        [JsonConstructor]
        public Map([JsonProperty("Tiles")] int[][] tiles)
        {
            MapRows = tiles.Length;
            MapColumns = tiles[0].Length;
            Tiles = tiles;
        }

        public TileType GetTile(Vector2Int pos)
        {
            var newRow = MapRows - 1 - pos.y; //������������� ��� y
            var tile = new Vector2Int(pos.x, newRow);
            return (TileType)Tiles[tile.y][tile.x];
        }

        /// <summary>
        /// �������� �� �� ��� ������ ��������� � �������� �������� ����
        /// </summary>
        public bool InMapBounds(Vector2Int cell)
        {
            //Debug.Log($"{GetTile(cell)} {cell.x} {cell.y}");
            //(x,y) = (column, row)
            if (cell.y >= MapRows - 1 || cell.y < -1 ||
                cell.x >= MapColumns - 1 || cell.x < -1)
                return false;
            return true;
        }

        public bool IsHole(Vector2Int cell)
        {
            return GetTile(cell) == TileType.Hole;
        }

        public bool IsWall(Vector2Int cell)
        {
            return GetTile(cell) == TileType.Wall;
        }

        public bool IsGround(Vector2Int cell)
        {
            return GetTile(cell) == TileType.Ground;
        }

        public override string ToString()
        {
            var result = new StringBuilder("Tiles:\n{\n");
            for (var row = 0; row < MapRows; row++)
                for (var column = 0; column < MapColumns; column++)
                {
                    var newRow = MapRows - 1 - row;//����������� Y �� ������ ����� ������ ����
                    var tile = GetTile(new Vector2Int(column, newRow));
                    result.Append((int)tile);
                    if (column == MapColumns - 1)
                        result.Append("\n");
                }
            result.Append("}\n");
            return result.ToString();
        }
    }
}
