using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour {
    public static RoomDrawer instance = null;

    //Indeholder rum til tegning.
    public List<GameObject> dungeonRooms;


    public GameObject door;
    public List<Vector2> doorPos;

    //Object vi tegner på.
    public GameObject activeGrid;
    private Grid grid;  //NOTE: bliver nok ikke brugt
    //Tilemap for Walls
    private Tilemap gridWalls;
    //Tilemap for Floor
    private Tilemap gridFloor;

    //Liste af sprites
    public List<TileBase> floorSprites;

    //Et rums dimensioner er (16x12)
    private const int totalWidth = 16;
    private const int totalHeight = 12;
    private int halfWidth;
    private int halfHeight;

    private void Awake() {
        #region Singleton Pattern
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        #endregion
    }

    private void Start() {
        grid = activeGrid.GetComponent<Grid>();
        gridWalls = activeGrid.transform.Find("Walls").GetComponent<Tilemap>();
        gridFloor = activeGrid.transform.Find("Floor").GetComponent<Tilemap>();

        //Sæt half værdier
        halfHeight = (int)totalHeight / 2;
        halfWidth = (int)totalWidth / 2;
    }

    /* Funktioner */

    //`GameObject room` == prefab NewRoom
    private void DrawRoom(Vector2Int roomCenter, GameObject room) {
        #region Hentning af variabler fra `room`
        Tilemap tilemap = room.GetComponent<Tilemap>();
        List<GameObject> roomObjects = new List<GameObject>();

        foreach (Transform obj in room.transform.GetChild(0).transform) {
            roomObjects.Add(obj.gameObject);
        }
        #endregion

        #region Tegning af `room` ind i `gridWalls` samt `floor`
        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    gridWalls.SetTile(new Vector3Int(x + (totalWidth * roomCenter.x), y + (totalHeight * roomCenter.y), 0), tile);
                }
                //Tegner gulv en efter en
                gridFloor.SetTile(new Vector3Int(x + (totalWidth * roomCenter.x), y + (totalHeight * roomCenter.y), 0), floorSprites[0]);
            }
        }
        #endregion

        //NOTE: Joakim har kode til rum skiftning.
        SpawnDoorsInRoom(roomCenter);

        //Tilføjelse af `roomObjects` til `room`
        foreach (GameObject item in roomObjects) {
            Vector3 itemPos = new Vector3(item.transform.position.x + (totalWidth * roomCenter.x), item.transform.position.y + (totalHeight * roomCenter.y));
            //TODO, ændr
            List<Sprite> objectSprites = new List<Sprite>();
            item.GetComponent<SpriteRenderer>().sprite = objectSprites[Random.Range(0, objectSprites.Count)];

            Instantiate(item, itemPos, Quaternion.identity);
        }
    }

    private void SpawnDoorsInRoom(Vector2Int roomCenter) {
        doorPos = new List<Vector2>() {
            new Vector2(0f, 5.5f),
            new Vector2(0f, -5.5f),
            new Vector2(7.5f, 0f),
            new Vector2(-7.5f, 0f),
        };
        foreach (Vector2 pos in doorPos) {
            Vector2 vector2 = new Vector2(pos.x + (totalWidth * roomCenter.x), pos.y + (totalHeight * roomCenter.y));         
            Instantiate(door, vector2, Quaternion.identity);
        }
    }

    public void DrawDungeonRooms(List<Vector2Int> roomPositions) {
        foreach (Vector2Int pos in roomPositions) {
            //Hent tilfældigt rum fra `dungeonRooms`
            int index = Random.Range(0, dungeonRooms.Count);
            //Hentning af rummet
            GameObject roomToDraw = dungeonRooms[index];
            //Fjern den fra listen (så den ikke bliver tegnet to gange
            dungeonRooms.RemoveAt(index);
            //Kald DrawRoom med roomPosition og `room`
            DrawRoom(pos, roomToDraw);
            //Gør dette indtil alle `roomPositions` er blevet tegnet.
        }
    }
}
