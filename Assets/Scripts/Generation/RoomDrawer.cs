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


        PrintContent();
    }

    //Printer indhold af tilmap, startende med venstre bund hjørne, og arbejder sig op, venstre mod højre
    //X incrementeres først, når den når til "højre-væg", går vi y+1, osv.
    private void PrintContent() {
        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null) {
                    gridWalls.SetTile(new Vector3Int(x + width, y, 0), tile);
                }
            }
        }
    }
}
