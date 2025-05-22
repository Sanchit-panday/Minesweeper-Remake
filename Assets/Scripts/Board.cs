using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    // public Tile tileUnknown;
    // public Tile tileEmpty;
    // public Tile tileMine;
    // public Tile tileExploded;
    // public Tile tileFlag;
    // public Tile tileNum1;
    // public Tile tileNum2;
    // public Tile tileNum3;
    // public Tile tileNum4;
    // public Tile tileNum5;
    // public Tile tileNum6;
    // public Tile tileNum7;
    // public Tile tileNum8;

    [System.Serializable]
    public struct TileSet
    {
        public Tile TileUnknown;
        public Tile TileEmpty;
        public Tile TileMine;
        public Tile TileExploded;
        public Tile TileFlag;
        public List<Tile> TileNum; // Use a list for number tiles
    }

    public TileSet tiles;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();

        if (tiles.TileNum == null)
        {
            tiles.TileNum = new List<Tile>();
        }
    }



    public void Draw(Cell[,] state)
    {
        int width = state.GetLength(0);
        int height = state.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];
                tilemap.SetTile(cell.position, GetTile(cell));
            }
        }
    }
    private Tile GetTile(Cell cell)
    {
        if (cell.revealed)
        {
            return GetRevealedTile(cell);
        }
        else if (cell.flagged)
        {
            return tiles.TileFlag;
        }
        else
        {
            return tiles.TileUnknown;
        }
    }

    private Tile GetRevealedTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Empty:
                return tiles.TileEmpty;

            case Cell.Type.Mine:
                return cell.exploded ? tiles.TileExploded : tiles.TileMine;

            case Cell.Type.Number:
                return GetNumberTile(cell.number);

            default:
                return null;
        }
    }

    private Tile GetNumberTile(int number)
    {
        if (tiles.TileNum != null && number > 0 && number <= tiles.TileNum.Count)
        {
            return tiles.TileNum[number - 1]; // Adjust index for 0-based list
        }
        else
        {
            Debug.LogError("Invalid number requested or number tiles not set up.");
            return null; // Or return a default tile if appropriate
        }
    }
}
