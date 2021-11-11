using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Singleton

    [Tooltip("The player object.")]
    public GameObject playerObj;

    public PlayerController player;

    [Header("Room-to-Room movement")]
    public bool canUseDoors = true;  //`true` because the player starts in an empty room.

    [Tooltip("Used to keep track of Player's position in relation to the minimap.")]
    public RoomObject activePlayerRoom = new RoomObject(-1, -1);

    public Transform activeRoom;
    [SerializeField] private int mobsInActiveRoom;

    [Tooltip("Contains a list of rooms the player has visited, and is not currently in.")]
    public HashSet<RoomObject> exploredRooms = new HashSet<RoomObject>();  //Only contains unique.
    public HashSet<RoomObject> completedRooms = new HashSet<RoomObject>();  //Only contains unique.

    public GameObject mob;
    public GameObject itemUpgrade;

    public List<UpgradeObject> upgradeObjects = new List<UpgradeObject>();
    public List<GameObject> bosses = new List<GameObject>();

    #region Awake, Start

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

        if (playerObj == null) {
            playerObj = GameObject.Find("Player");
            player = playerObj.GetComponent<PlayerController>();
        }
    }

    private void Start() {
        StartNewDungeonFloor();
    }

    #endregion Awake, Start

    /// <summary>
    /// Starts generating a new dungeon floor.
    /// </summary>
    private void StartNewDungeonFloor() {
        DungeonGenerator.instance.GenerateDungeon();

        OnDungeonGenerationComplete();
    }

    /// <summary>
    /// Helper method for moving Player and Camera to the start room.
    /// </summary>
    private void OnDungeonGenerationComplete() {
        Vector3 v = DungeonLayout.GetRoomCenterWorldPosition(DungeonGenerator.instance.startRoom);
        playerObj.transform.position = new Vector3(v.x, v.y, 0f);
        Camera.main.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, Camera.main.transform.position.z);
    }

    /// <summary>
    /// Sets the value of canUseDoors.
    /// </summary>
    private void SetCanUseDoors() {
        Debug.Log(activePlayerRoom.type);
        canUseDoors = activePlayerRoom.type == RoomType.StartRoom
                   || activePlayerRoom.type == RoomType.Item
                   || completedRooms.Contains(activePlayerRoom);
    }

    #region Activate Rooms

    /// <summary>
    /// Activates GameObjects inside the activeRoom. Is called from <see cref="CameraManager"/>
    /// </summary>
    public void ActivatePlayerRoom() {
        mobsInActiveRoom = 0;

        switch (activePlayerRoom.type) {
            case RoomType.Null:
                Debug.LogError("Current Room is set as type: NULL");
                break;

            case RoomType.StartRoom:
                break;

            case RoomType.Normal:
                ActivatePlayerRoomNormal();
                break;

            case RoomType.Boss:
                ActivatePlayerRoomBoss();
                break;

            case RoomType.Item:
                ActivatePlayerRoomItem();
                break;

            default:
                break;
        }

        foreach (Transform obj in activeRoom) {
            if (obj.gameObject.tag == "Door") {
                obj.gameObject.GetComponent<DoorObject>().SetDoorState(canUseDoors);
            }
        }
    }

    /// <summary>
    /// Checks if the room is completed, and opens the door(s) if <see langword="true"/>.
    /// </summary>
    public void IsActiveRoomCompleted() {
        mobsInActiveRoom--;

        if (mobsInActiveRoom == 0) {
            completedRooms.Add(activePlayerRoom);
            SetCanUseDoors();

            foreach (Transform door in activeRoom) {
                if (door.gameObject.CompareTag("Door")) {
                    door.gameObject.GetComponent<DoorObject>().SetDoorState(canUseDoors);
                }
            }
        }
    }

    #region Activate different types of room

    private void ActivatePlayerRoomNormal() {
        foreach (Transform obj in activeRoom) {
            if (obj.gameObject.tag == "SpawnerMob" && canUseDoors == false) {
                GameObject spawn = Instantiate(mob, obj.position, Quaternion.identity);
                spawn.transform.SetParent(activeRoom);
                spawn.gameObject.GetComponent<EnemyAttackController>().playerPosition = player.transform;
                Destroy(obj.gameObject);

                mobsInActiveRoom++;
            }
        }
    }

    private void ActivatePlayerRoomBoss() {
        GameObject bossToSpawn = Instantiate(bosses[0], activeRoom.position, Quaternion.identity);

        bossToSpawn.GetComponent<EnemyAttackController>().playerPosition = player.transform;
    }

    private void ActivatePlayerRoomItem() {
        GameObject upgradeToSpawn = Instantiate(itemUpgrade, activeRoom.position, Quaternion.identity);

        //TODO Add randomness
        //int index = Random.Range(0, upgradeObjects.Count + 1);
        //Remove the upgrade from the pool afterwards.
        upgradeToSpawn.GetComponent<UpgradeEntity>().SetValues(upgradeObjects[0]);
    }

    #endregion Activate different types of room

    #endregion Activate Rooms

    #region In regards to movement between rooms, and activation.

    /// <summary>
    /// Sets the newRoom to be the activePlayerRoom, and handles any rooms that aren't of type <see cref="RoomType.Normal"/>.
    /// </summary>
    /// <param name="newRoom">The room the player is going to enter.</param>
    public void PlayerActivatedRoomMovement(RoomObject newRoom) {
        activePlayerRoom = newRoom;
        exploredRooms.Add(activePlayerRoom);

        if (activePlayerRoom.type == RoomType.StartRoom || activePlayerRoom.type == RoomType.Item) {
            completedRooms.Add(activePlayerRoom);
        }

        activeRoom = GameObject.Find(activePlayerRoom.ToString()).transform;

        SetCanUseDoors();
    }

    //NOTE: Only gets called once after the generation of the dungeon.
    public void SetCurrentPlayerPosition(RoomObject room) {
        if (exploredRooms.Count == 0) {
            activePlayerRoom = room;

            exploredRooms.Add(activePlayerRoom);
            canUseDoors = true;
            return;
        }

        //false if the room isn't in the list; true if it is.
        canUseDoors = !exploredRooms.Add(room);
        activePlayerRoom = room;
    }

    /// <summary>
    /// Prepares the movement between rooms, by calculating which room the player is going towards.
    /// </summary>
    public void PrepareMovementBetweenRooms() {
        Vector2 newRoom = DungeonLayout.GetActivatedDoorDirection(playerObj.transform.position, activePlayerRoom);
        //Set new room.
        //SetCurrentPlayerPosition(new RoomObject((int)newRoom.x + activePlayerRoom.x, (int)newRoom.y + activePlayerRoom.y));
        PlayerActivatedRoomMovement(DungeonGenerator.instance.GetActiveRoom(new RoomObject((int)newRoom.x + activePlayerRoom.x, (int)newRoom.y + activePlayerRoom.y)));

        CameraManager.instance.MoveToRoom(newRoom);
    }

    #endregion In regards to movement between rooms, and activation.
}