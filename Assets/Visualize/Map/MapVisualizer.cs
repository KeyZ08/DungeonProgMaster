using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DPM.Domain;
using DPM.Infrastructure;

namespace DPM.UI
{
    public class MapVisualizer : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;

        [Header("Tiles")]
        [SerializeField] private RuleTile ground;
        [SerializeField] private Tile hole;
        [SerializeField] private Tile wall;
        [SerializeField] private Tile finish;

        private Dictionary<TileType, TileBase> tileByType;

        public void Construct()
        {
            tileByType = new Dictionary<TileType, TileBase>()
        {
            { TileType.Ground, ground },
            { TileType.Hole, hole },
            { TileType.Wall, wall },
            { TileType.Finish, finish },
        };
        }

        public void DrawMap(Map map)
        {
            tilemap.ClearAllTiles();
            if (tileByType == null) Construct();
            for (int row = 0; row < map.MapRows; row++)
            {
                for (int column = 0; column < map.MapColumns; column++)
                {
                    var tile = map.GetTile(new Vector2Int(column, row));
                    tilemap.SetTile(new Vector3Int(column, row), tileByType[tile]);
                }
            }
        }

        public Vector2 GetCellCenter(Vector2Int cell)
        {
            return tilemap.GetCellCenterWorld((Vector3Int)cell);
        }

        public Vector2 GetCellSize()
        {
            return tilemap.cellSize;
        }

        /// <summary>
        /// Находит ячейку по мировым координатам
        /// </summary>
        public Vector2Int GetCellByWorlPoint(Vector3 worldPoint)
        {
            return (Vector2Int)tilemap.WorldToCell(worldPoint);
        }
    }

}