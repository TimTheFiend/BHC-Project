using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour {
    public static RoomDrawer instance = null;

    //Indeholder rum til tegning.
    public List<GameObject> dungeonRooms;


    public TileBase door;
    public List<Vector2Int> doorPos;

    //Object vi tegner på.
    public GameObject activeGrid;
    private Grid grid;  //NOTE: bliver nok ikke brugt
    //Tilemap for Walls
    private Tilemap gridWalls;
    //Tilemap for Doors
    private Tilemap gridDoors;
    //Tilemap for Floor
    private Tilemap gridFloor;

    //Liste af sprites
    public List<TileBase> floorSprites;

    //Et rums dimensioner er (16x12)
    private const int totalWidth = 16;
    private const int totalHeight = 12;
    private int halfWidth;
    private int halfHeight;

    public List<Vector2Int> minimapRoomPos = new List<Vector2Int>() {
        new Vector2Int(1, 1),
        new Vector2Int(1, 2),
        new Vector2Int(1, 3),
        new Vector2Int(0, 3),
        new Vector2Int(0, 1),
        //new Vector2Int(-2, -0)
    };

    public List<Vector2Int> OverUnderHøjreVenstre = new List<Vector2Int>() {
        new Vector2Int (0,1),
        new Vector2Int (0,-1),
        new Vector2Int (1,0),
        new Vector2Int (-1,0),
    };

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
        gridDoors = activeGrid.transform.Find("Doors").GetComponent<Tilemap>();

        //Sæt half værdier
        halfHeight = (int)totalHeight / 2;
        halfWidth = (int)totalWidth / 2;

        DrawDungeonRooms(minimapRoomPos);
        
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

        DoorPlacement();

        //Tilføjelse af `roomObjects` til `room`
        foreach (GameObject item in roomObjects) {
            Vector3 itemPos = new Vector3(item.transform.position.x + (totalWidth * roomCenter.x), item.transform.position.y + (totalHeight * roomCenter.y));
            //TODO, ændr
            List<Sprite> objectSprites = new List<Sprite>();
            item.GetComponent<SpriteRenderer>().sprite = objectSprites[Random.Range(0, objectSprites.Count)];

            Instantiate(item, itemPos, Quaternion.identity);
        }
    }

    private void SpawnDoorsAsGameObjects(Vector2Int roomCenter) {
        //doorPos = new List<Vector2>() {
        //    new Vector2(0f, 5.5f),
        //    new Vector2(0f, -5.5f),
        //    new Vector2(7.5f, 0f),
        //    new Vector2(-7.5f, 0f),
        //};


        foreach (Vector2 pos in doorPos) {
            Vector2 vector2 = new Vector2(pos.x + (totalWidth * roomCenter.x), pos.y + (totalHeight * roomCenter.y));
            
            Instantiate(door, vector2, Quaternion.identity);
        }
    }

    private void DrawDoors(Vector2Int roomCenter) {
        //Liste af hvor dørene skal tegnes henne
        List<Vector2Int> doorPos = new List<Vector2Int>() {
            new Vector2Int(0, 5), //Up
            new Vector2Int(-1, 5), //Up
            new Vector2Int(0, -6), //Down
            new Vector2Int(-1, -6), //Down
            new Vector2Int(-8, 0), //Left
            new Vector2Int(-8, -1), //Left
            new Vector2Int(7, 0), //Right
            new Vector2Int(7, -1), //Right
        };

        foreach (Vector2Int pos in doorPos) {
            //Vector2Int vector2Int = new Vector2Int(pos.x + (totalWidth * roomCenter.x), pos.y + (totalHeight * roomCenter.y));
            gridDoors.SetTile(new Vector3Int(pos.x + (totalWidth * roomCenter.x), pos.y + (totalHeight * roomCenter.y), 0), door);
        }

        
        
        //gridDoors.SetTile(new Vector3Int(pos.x + (totalWidth * roomCenter.x), pos.y + (totalHeight * roomCenter.y), 0), door);

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

    public void DoorPlacement() {
        foreach (var Pos in minimapRoomPos) {
            foreach (var retning in OverUnderHøjreVenstre) {
                //minimapRoomPos.Contains(Pos + retning);
                if (minimapRoomPos.Contains(Pos + retning) == true) {
                    print(Pos + retning);
                }
            }
        }
    }
}
