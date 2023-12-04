public class Map
{
    public readonly int MapRows;
    public readonly int MapColumns;
    private readonly int[,] Tiles;

    public Map(int[,] tiles)
    {
        MapColumns = tiles.GetLength(0);
        MapRows = tiles.Length / MapColumns;
        Tiles = tiles;
    }

    public TileType GetTile(int x, int y)
    {
        return (TileType)Tiles[x, y];
    }

    public override string ToString()
    {
        var result = "";
        for (var row = 0; row < MapRows; row++)
            for (var column = 0; column < MapColumns; column++)
            {
                if (column == MapColumns - 1)
                    result += $"{Tiles[row, column]}\n";
                else if (row == 8 && column == 4)
                    result += "O ";
                else
                    result += $"{Tiles[row, column]} ";
            }
        return result;
    }
}
