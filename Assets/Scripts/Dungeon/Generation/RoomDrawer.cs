using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{
    public static RoomDrawer Instance = null;

    private Dictionary<DoorLayout, List<Vector2Int>> doorPositions;

    //Indeholder rum til tegning.
    [Header("Pre-made rooms")]
    public List<GameObject> dungeonRooms;

    [Header("Door-related variables")]
    public TileBase door;  //TileBase for door

    public GameObject doorObj; //NOTE: bruges til at tegne d�re som gameobject SKAL IKKE BRUGES ENDNU

    //Object vi tegner på.
    public GameObject activeGrid;

    [SerializeField] private Grid grid;  //NOTE: bliver nok ikke brugt

    //Tilemap for Walls
    [SerializeField] private Tilemap gridWalls;

    //Tilemap for Doors
    [SerializeField] private Tilemap gridDoors;

    //Tilemap for Floor
    [SerializeField] private Tilemap gridFloor;

    //Liste af sprites
    public List<TileBase> floorSprites;

    //Et rums dimensioner er (16x12)
    [SerializeField] public readonly int totalWidth = 16;

    [SerializeField] public readonly int totalHeight = 12;
    [SerializeField] private int halfWidth;
    [SerializeField] private int halfHeight;

    private void Awake() {

        #region Singleton Pattern

        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        #endregion Singleton Pattern

        grid = activeGrid.GetComponent<Grid>();
        gridWalls = activeGrid.transform.Find("Walls").GetComponent<Tilemap>();
        gridFloor = activeGrid.transform.Find("Floor").GetComponent<Tilemap>();
        gridDoors = activeGrid.transform.Find("Doors").GetComponent<Tilemap>();

        //Sæt half værdier
        halfHeight = (int)totalHeight / 2;
        halfWidth = (int)totalWidth / 2;

        doorPositions = new Dictionary<DoorLayout, List<Vector2Int>>();
        doorPositions.Add(DoorLayout.Up, new List<Vector2Int>() { new Vector2Int(0, 5), new Vector2Int(-1, 5) });
        doorPositions.Add(DoorLayout.Down, new List<Vector2Int>() { new Vector2Int(0, -6), new Vector2Int(-1, -6) });
        doorPositions.Add(DoorLayout.Left, new List<Vector2Int>() { new Vector2Int(-8, 0), new Vector2Int(-8, -1) });
        doorPositions.Add(DoorLayout.Right, new List<Vector2Int>() { new Vector2Int(7, 0), new Vector2Int(7, -1) });
    }

    //Metode der bliver kaldt n�r et nyt level skal tegnes.
    public void DrawDungeonRooms(List<RoomObject> roomPositions) {
        foreach (RoomObject room in roomPositions) {
            Transform roomHolder = new GameObject(room.ToString()).transform;

            //Hent tilf�ldigt rum fra `dungeonRooms`
            int index = Random.Range(0, dungeonRooms.Count);
            //Hentning af rummet
            GameObject roomToDraw = dungeonRooms[index];
            //Fjern den fra listen (s� den ikke bliver tegnet to gange
            //dungeonRooms.RemoveAt(index);
            //Kald DrawRoom med roomPosition og `room`
            DrawRoom(room, roomToDraw);
            //G�r dette indtil alle `roomPositions` er blevet tegnet.
        }
    }

    private void DrawRoom(RoomObject roomObj, GameObject dungeonRoom) {

        #region Hentning af variabler fra `room`

        Tilemap tilemap = dungeonRoom.GetComponent<Tilemap>();
        List<GameObject> roomObjects = new List<GameObject>();

        foreach (Transform obj in dungeonRoom.transform.GetChild(0).transform) {
            roomObjects.Add(obj.gameObject);
        }

        #endregion Hentning af variabler fra `room`

        #region Tegning af `room` ind i `gridWalls` samt `floor`

        for (int x = -halfWidth; x < halfWidth; x++) {
            for (int y = -halfHeight; y < halfHeight; y++) {
                TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tile != null) {
                    gridWalls.SetTile(DungeonLayout.GetRoomCenterWorldPosition(roomObj, x, y), tile);
                }
                //Tegner gulv en efter en
                gridFloor.SetTile(DungeonLayout.GetRoomCenterWorldPosition(roomObj, x, y), floorSprites[0]);
            }
        }

        #endregion Tegning af `room` ind i `gridWalls` samt `floor`

        /* Tegner d�re */
        DrawDoors(roomObj);

        //TODO PlaceObjects() temp navn
        if (roomObj == DungeonGenerator.instance.startRoom) {
            return;
        }
        SpawnObjects(roomObj, roomObjects);
        //SpawnObjects(roomObj, dungeonRoom);
    }

    //BRUGER TILEMAP
    private void DrawDoors(RoomObject newRoom) {
        foreach (Vector2Int pos in DungeonLayout.GetTilemapDoorPosition(newRoom)) {
            gridDoors.SetTile(DungeonLayout.GetRoomCenterWorldPosition(newRoom, pos.x, pos.y), door);
        }
    }

    private void SpawnObjects(RoomObject newRoom, List<GameObject> objectsToSpawn) {
        Transform roomHolder = GameObject.Find(newRoom.ToString()).transform;

        foreach (GameObject item in objectsToSpawn) {
            GameObject toSpawn = Instantiate(item, DungeonLayout.GetWorldPosition(item.transform.position, newRoom), Quaternion.identity);
            toSpawn.transform.SetParent(roomHolder);
        }
    }

    private void SpawnObjects(RoomObject newRoom, GameObject dungeonRoom) {
        List<GameObject> objectsToSpawn = new List<GameObject>();
        //Hent children fra `dungeonRoom`s f�rste child
        foreach (GameObject child in dungeonRoom.transform.GetChild(0).transform) {
            objectsToSpawn.Add(child);
        }
        print(objectsToSpawn.Count);
        //Identificer typen af gameobject
        //
        //Initialise gameobject
    }
}