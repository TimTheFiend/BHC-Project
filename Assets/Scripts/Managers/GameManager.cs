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

    [Header("Active room information")]
    public RoomObject activePlayerRoom = new RoomObject(-1, -1);  //Used to keep track of Player's position in relation to the minimap.
    private Transform activeRoom;  //Player room transform, for access to objects in said room.
    private int mobsInActiveRoom;

    [Header("Information about where the player has been.")]
    public HashSet<RoomObject> exploredRooms = new HashSet<RoomObject>();  //Only contains unique.
    public HashSet<RoomObject> completedRooms = new HashSet<RoomObject>();  //Only contains unique.

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
    public void StartNewDungeonFloor() {
        DungeonGenerator.instance.GenerateDungeon();

        canUseDoors = true;
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
        Debug.Log(string.Format("{0} - {1}", activePlayerRoom, activePlayerRoom.type));

        switch (activePlayerRoom.type) {
            case RoomType.Normal:
                SpawnManager.instance.ActivateNormalRoom(activeRoom, out mobsInActiveRoom);
                break;

            case RoomType.Boss:
                SpawnManager.instance.ActivateBossRoom(activeRoom, out mobsInActiveRoom);
                //SpawnManager.instance.ActivateBossRoom(activeRoom);
                break;

            case RoomType.Item:
                SpawnManager.instance.ActivateItemRoom(activeRoom);
                break;

            default:
                Debug.Assert(activePlayerRoom.type != RoomType.Null, "activePlayerRoom type is \"Null\".");
                break;
        }

        ActivateRoomDoors();
    }

    /// <summary>
    /// Activates the active room's doors.
    /// </summary>
    private void ActivateRoomDoors() {
        foreach (Transform obj in activeRoom) {
            if (obj.gameObject.tag == "Door") {
                obj.gameObject.GetComponent<DoorObject>().SetDoorState(canUseDoors);
            }
        }
    }

    /// <summary>
    /// Checks if the room is completed, and opens the door(s) if <see langword="true"/>.
    /// </summary>
    public void IsActiveRoomCompleted(Vector3 mobPosition) {
        mobsInActiveRoom--;
        SpawnManager.instance.MobPickupDrop(mobPosition);

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

    /// <summary>
    /// Handles boss room completion.
    /// </summary>
    public void BossRoomIsCompleted() {
        completedRooms.Add(activePlayerRoom);
        SpawnManager.instance.SpawnDungeonExit(activeRoom.position);
        SetCanUseDoors();
        ActivateRoomDoors();
    }

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

    /// <summary>
    /// Handles the player moving between rooms.
    /// </summary>
    /// <param name="room"></param>
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
        UIManager.instance.MoveMinimap(newRoom);
        PlayerActivatedRoomMovement(DungeonGenerator.instance.GetActiveRoom(new RoomObject((int)newRoom.x + activePlayerRoom.x, (int)newRoom.y + activePlayerRoom.y)));

        CameraManager.instance.MoveToRoom(newRoom);
    }

    #endregion In regards to movement between rooms, and activation.
}