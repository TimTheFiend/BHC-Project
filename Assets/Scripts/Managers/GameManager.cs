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

    [Tooltip("Contains a list of rooms the player has visited, and is not currently in.")]
    public HashSet<RoomObject> exploredRooms = new HashSet<RoomObject>();  //Only contains unique.
    public HashSet<RoomObject> completedRooms = new HashSet<RoomObject>();  //Only contains unique.

    public GameObject mob;

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
        DungeonGenerator.instance.GenerateDungeon();

        OnDungeonGenerationComplete();
    }

    #endregion Awake, Start

    /// <summary>
    /// Helper method for moving Player and Camera to the start room.
    /// </summary>
    private void OnDungeonGenerationComplete() {
        Vector3 v = DungeonLayout.GetRoomCenterWorldPosition(DungeonGenerator.instance.startRoom);
        playerObj.transform.position = new Vector3(v.x, v.y, 0f);
        Camera.main.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, Camera.main.transform.position.z);
    }

    public void UpdateCurrentPlayerPosition(RoomObject newRoom) {
        /*
         * How to organise rooms
         * 1. Get newRoom.
         * 1a. if playerRoomPosition == null, it's the starting room -> OPEN
         * 2. If RoomType == (Normal || Boss) -> CLOSE and ActivateRoom
         * 3. Add playerRoomPosition to completedRooms.
         * 4. Set newRoom to be playerRoomPosition
         */
        Debug.Assert(activePlayerRoom != newRoom, "activePlayerPosition is the same as the room that's attempting to be entered");
        //Mark newRoom as explored
        exploredRooms.Add(newRoom);

        activeRoom = GameObject.Find(newRoom.ToString()).transform;
        Debug.Assert(activeRoom != null, "activePlayerRoom (Transform) is null");

        activePlayerRoom = newRoom;

        Debug.Log($"{activePlayerRoom} - {activePlayerRoom.type}");
        /*newRoom has been:
         *      - Added to exploredRooms
         *      - set to be the activePlayerRoom
         *Found and set the value for activeRoom
         */

        switch (activePlayerRoom.type) {
            case RoomType.Null:
                Debug.LogError("activePlayerPosition is of type Null, something went wrong.");
                break;

            case RoomType.StartRoom:
                canUseDoors = true;
                break;

            case RoomType.Normal:
                canUseDoors = completedRooms.Contains(activePlayerRoom);
                break;

            case RoomType.Boss:
                canUseDoors = completedRooms.Contains(activePlayerRoom);
                break;

            case RoomType.Item:
                canUseDoors = true;
                break;

            default:
                break;
        }

        //If this is the first room
        if (completedRooms.Count == 0) {
            //TODO
        }

        /* POST start-of-level room fuckery. */
        if (canUseDoors) {
            //If true, only the doors needs to be activated.
        }
    }

    private void AttemptActivateRoom(bool isRoomCompleted) {
        foreach (Transform child in activeRoom) {
            if (isRoomCompleted) {
            }
        }
    }

    private void ActivateMob(Transform transform) {
    }

    private void ActivateDoor(Transform transform) {
    }

    #region In regards to movement between rooms, and activation.

    //When moving between rooms, set current playerRoomPosition, and add room to exploredRooms
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

    public void AttemptActivateRoom(RoomObject room) {
        if (room.type == RoomType.StartRoom || room.type == RoomType.Item) {
            canUseDoors = true;
        }
        else {
            canUseDoors = completedRooms.Contains(room);
        }
        //ActivateDoors(canUseDoors);
    }

    private void CompleteCurrentRoom() {
        completedRooms.Add(activePlayerRoom);
    }

    //WIP
    public void ActivateCurrentRoom() {
        activeRoom = GameObject.Find(activePlayerRoom.ToString()).transform;

        //Find transform for activePlayerRoom;
        foreach (Transform obj in activeRoom) {
            if (obj.gameObject.tag == "SpawnerMob") {
                GameObject spawn = Instantiate(mob, obj.position, Quaternion.identity);
                spawn.transform.SetParent(activeRoom);
                spawn.gameObject.GetComponent<EnemyAttackController>().playerPosition = player.transform;
                Destroy(obj.gameObject);
            }
        }
    }

    /// <summary>
    /// Checks if the room is completed, and opens the door(s) if <see langword="true"/>.
    /// </summary>
    public void IsActiveRoomCompleted() {
        print("check room");
        int enemyCounter = -1;
        foreach (Transform obj in activeRoom) {
            if (obj.gameObject.tag == "Enemy") {
                enemyCounter++;
            }
        }
        if (enemyCounter == 0) {
            canUseDoors = true;
            foreach (Transform door in activeRoom) {
                if (door.gameObject.tag == "Door") {
                    door.gameObject.GetComponent<DoorObject>().ActiveRoomIsCompleted();
                }
            }
        }
    }

    /// <summary>
    /// Prepares the movement between rooms, by calculating which room the player is going towards.
    /// </summary>
    public void PrepareMovementBetweenRooms() {
        Vector2 newRoom = DungeonLayout.GetActivatedDoorDirection(playerObj.transform.position, activePlayerRoom);
        //Set new room.
        SetCurrentPlayerPosition(new RoomObject((int)newRoom.x + activePlayerRoom.x, (int)newRoom.y + activePlayerRoom.y));

        CameraManager.instance.MoveToRoom(newRoom);
    }

    #endregion In regards to movement between rooms, and activation.
}