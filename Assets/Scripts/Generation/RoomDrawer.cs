using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{
    public GameObject roomToDraw;
    public GameObject gridObj;

    TileBase tile;

    public Tilemap tilemap;
    public Grid grid;
    public Tilemap gridWalls;

    public const int width = 16;
    public const int height = 12;
    public int halfWidth;
    public int halfHeight;

    public Vector2 newRoom = Vector2.zero;

    //Et rums dimensioner er (16x12)
    private void Start() {
        tilemap = roomToDraw.GetComponent<Tilemap>();
        grid = gridObj.GetComponent<Grid>();
        gridWalls = gridObj.transform.Find("Walls").GetComponent<Tilemap>();

        tile = tilemap.GetTile(new Vector3Int(-8, 0, 0));

        //gridWalls.SetTile(new Vector3Int(-9, 0, 0), tile);

        //Sæt half værdier
        halfHeight = (int)height / 2;
        halfWidth = (int)width / 2;


        DrawRoom(new Vector2Int(0, 1));
        DrawRoom(new Vector2Int(-1, -1));
        DrawRoom(new Vector2Int(1, 2));
        
    }

    //
    private void DrawRoom(Vector2Int roomCenter) {
        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null) {
                    continue;
                }
                gridWalls.SetTile(new Vector3Int(x + (width * roomCenter.x) , y + (height * roomCenter.y), 0), tile);
            }
        }
    }
}
