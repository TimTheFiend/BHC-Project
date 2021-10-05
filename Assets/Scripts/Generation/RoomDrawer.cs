using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{
    //`Room` der bliver tegnet
    public GameObject roomObj;
    public Tilemap roomToDraw;
    //Object vi tegner på.
    public GameObject gridGameObject;
    public Grid grid;
    public Tilemap gridWalls;

    //Liste af gameobjects fra `roomObj`
    List<GameObject> objectsToDraw = new List<GameObject>();

    //Liste af sprites
    public List<Sprite> sprites;

    //`Room` størrelse(r)
    private const int width = 16;
    private const int height = 12;
    private int halfWidth;
    private int halfHeight;

    //Et rums dimensioner er (16x12)
    private void Start() {
        roomToDraw = roomObj.GetComponent<Tilemap>();
        grid = gridGameObject.GetComponent<Grid>();
        gridWalls = gridGameObject.transform.Find("Walls").GetComponent<Tilemap>();

        //Sæt half værdier
        halfHeight = (int)height / 2;
        halfWidth = (int)width / 2;

        DrawRoom(new Vector2Int(1, 1), roomObj);
    }

    //Finder nyt "midt punkt" så vi kan tegner en ny bane/map.
    //hvor den så tegner en ny bane som ligner den gamle.
    private void DrawRoom(Vector2Int roomCenter) {
        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                TileBase tile = roomToDraw.GetTile(new Vector3Int(x, y, 0));
                if (tile == null) {
                    continue;
                }
                gridWalls.SetTile(new Vector3Int(x + (width * roomCenter.x) , y + (height * roomCenter.y), 0), tile);
            }
        }
    }

    //`GameObject room` == prefab NewRoom
    private void DrawRoom(Vector2Int roomCenter, GameObject room) {
        #region Hentning af variabler fra `room`
        Tilemap tilemap = room.GetComponent<Tilemap>();
        List<GameObject> roomObjects = new List<GameObject>();

        foreach (Transform obj in room.transform.GetChild(0).transform) {
            roomObjects.Add(obj.gameObject);
        }
        #endregion


        #region Tegning af `room` ind i `gridWalls`
        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile == null) {
                    continue;
                }
                gridWalls.SetTile(new Vector3Int(x + (width * roomCenter.x), y + (height * roomCenter.y), 0), tile);
            }
        }
        #endregion

        //Tilføjelse af `roomObjects` til `room`
        foreach (GameObject item in roomObjects) {
            Vector3 itemPos = new Vector3(item.transform.position.x + (width * roomCenter.x), item.transform.position.y + (height * roomCenter.y));

            item.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];

            Instantiate(item, itemPos, Quaternion.identity);
        }
    }



    public void DrawRooms(params Vector2Int[] rooms) {
        foreach (Vector2Int room in rooms) {
            DrawRoom(room);
        }
    }
}
