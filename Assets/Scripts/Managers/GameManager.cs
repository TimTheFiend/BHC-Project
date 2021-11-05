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
    public RoomObject playerRoomPosition = new RoomObject(-1, -1);

    public Transform activePlayerRoom;

    [Tooltip("Contains a list of rooms the player has visited, and is not currently in.")]
    public HashSet<RoomObject> exploredRooms = new HashSet<RoomObject>();  //Only contains unique.

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

        Vector3 v = DungeonLayout.GetRoomCenterWorldPosition(DungeonGenerator.instance.startRoom);
        playerObj.transform.position = new Vector3(v.x, v.y, 0f);
        Camera.main.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, Camera.main.transform.position.z);
    }

    #endregion Awake, Start

    #region In regards to movement between rooms, and activation.

    //When moving between rooms, set current playerRoomPosition, and add room to exploredRooms
    public void SetCurrentPlayerPosition(RoomObject room) {
        if (exploredRooms.Count == 0) {
            playerRoomPosition = room;

            exploredRooms.Add(playerRoomPosition);
            canUseDoors = true;
            return;
        }

        //false if the room isn't in the list; true if it is.
        canUseDoors = !exploredRooms.Add(room);
        playerRoomPosition = room;
    }

    //WIP
    public void ActivateCurrentRoom() {
        activePlayerRoom = GameObject.Find(playerRoomPosition.ToString()).transform;

        //Find transform for activePlayerRoom;
        foreach (Transform obj in activePlayerRoom) {
            if (obj.gameObject.tag == "SpawnerMob") {
                GameObject spawn = Instantiate(mob, obj.position, Quaternion.identity);
                spawn.transform.SetParent(activePlayerRoom);
                spawn.gameObject.GetComponent<EnemyAttackController>().playerPosition = player.transform;
                Destroy(obj.gameObject);
            }
            //TODO fix doors
            //else if (obj.gameObject.tag == "Door") {
            //    obj.GetComponent<DoorObject>().ActivateDoor(false);
            //}
        }
    }

    /// <summary>
    /// Checks if the room is completed, and opens the door(s) if <see langword="true"/>.
    /// </summary>
    public void IsActiveRoomCompleted() {
        print("check room");
        int enemyCounter = -1;
        foreach (Transform obj in activePlayerRoom) {
            if (obj.gameObject.tag == "Enemy") {
                enemyCounter++;
            }
        }
        if (enemyCounter == 0) {
            canUseDoors = true;
            foreach (Transform door in activePlayerRoom) {
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
        Vector2 newRoom = DungeonLayout.GetActivatedDoorDirection(playerObj.transform.position, playerRoomPosition);
        //Set new room.
        SetCurrentPlayerPosition(new RoomObject((int)newRoom.x + playerRoomPosition.x, (int)newRoom.y + playerRoomPosition.y));

        CameraManager.instance.MoveToRoom(newRoom);
    }

    #endregion In regards to movement between rooms, and activation.
}