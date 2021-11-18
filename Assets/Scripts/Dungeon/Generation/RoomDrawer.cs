using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class RoomDrawer : MonoBehaviour
{
    public static RoomDrawer instance = null;

    //Indeholder rum til tegning.
    [Header("Pre-made rooms")]
    public List<GameObject> dungeonRooms;

    [Header("Floor sprites")]
    [SerializeField] private List<TileBase> floorSprites;

    [Header("Door-related variables")]
    [SerializeField] private GameObject doorObj;

    //Object vi tegner på.
    public GameObject activeGrid;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap gridWalls;
    [SerializeField] private Tilemap gridFloor;

    [Header("Room Size")]
    private const int totalWidth = 16;
    private const int totalHeight = 12;
    private int halfWidth => totalWidth / 2;
    private int halfHeight => totalHeight / 2;

    /* Temp */
    private Transform roomHolder;

    private void Awake() {

        #region Singleton Pattern

        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        #endregion Singleton Pattern

        InitialiseTilemaps();
    }

    private void InitialiseTilemaps() {
        activeGrid = GameObject.Find("Grid_");
        Debug.Assert(activeGrid != null);

        gridWalls = activeGrid.transform.Find("Walls").GetComponent<Tilemap>();
        gridFloor = activeGrid.transform.Find("Floor").GetComponent<Tilemap>();
    }

    //Metode der bliver kaldt n�r et nyt level skal tegnes.
    public void DrawDungeonRooms(List<RoomObject> roomPositions) {
        InitialiseTilemaps();
        foreach (RoomObject room in roomPositions) {

            #region RoomHolder

            //Laver nyt transform til at holde fast i potentielle objekter indeni.
            roomHolder = new GameObject(room.ToString()).transform;
            //Sætter `roomHolder` til at være i midten af rummet den hører til.
            roomHolder.position = DungeonLayout.GetRoomCenterWorldPosition(room);

            #endregion RoomHolder

            //Hent tilf�ldigt rum fra `dungeonRooms`
            int index = Random.Range(0, dungeonRooms.Count);
            //Hentning af rummet
            GameObject roomToDraw = dungeonRooms[index];
            //Fjern den fra listen (s� den ikke bliver tegnet to gange
            //Kald DrawRoom med roomPosition og `room`
            DrawRoom(room, roomToDraw);
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
        DrawDoorsAsObjects(roomObj);

        if (roomObj == DungeonGenerator.instance.startRoom) {
            return;
        }
        SpawnObjects(roomObj, roomObjects);
    }

    private void DrawDoorsAsObjects(RoomObject newRoom) {
        foreach (Vector2 pos in DungeonLayout.GetDoorPositions(newRoom)) {
            GameObject toInstantiate = Instantiate(doorObj, DungeonLayout.GetWorldPosition(pos, newRoom), Quaternion.identity);
            toInstantiate.transform.SetParent(roomHolder);
        }
        //Sådan spawner man dør
    }

    private void SpawnObjects(RoomObject newRoom, List<GameObject> objectsToSpawn) {
        if (newRoom.type != RoomType.Normal) {
            return;
        }
        foreach (GameObject item in objectsToSpawn) {
            GameObject toInstantiate = Instantiate(item, DungeonLayout.GetWorldPosition(item.transform.position, newRoom), Quaternion.identity);
            toInstantiate.transform.SetParent(roomHolder);
            //Instantiate(item, new Vector3(item.transform.position.x + (totalWidth * newRoom.x), item.transform.position.y + (totalHeight * newRoom.y)), Quaternion.identity);
        }
    }
}