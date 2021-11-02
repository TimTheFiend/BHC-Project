using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Singleton

    [Tooltip("Used to keep track of Player's position in relation to the minimap.")]
    public RoomObject playerRoomPosition;

    [Tooltip("The player object.")]
    public GameObject playerObj;

    public PlayerController player;

    public GameObject mob; // TEMP
    private Transform roomHolder;

    private void Start() {
        DungeonGenerator.Instance.GenerateDungeon();

        Vector3 v = DungeonLayout.GetRoomCenterWorldPosition(DungeonGenerator.Instance.startRoom);
        playerObj.transform.position = new Vector3(v.x, v.y, 0f);
        Camera.main.transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, Camera.main.transform.position.z);
    }

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

    public void PrepareMovementBetweenRooms() {

        #region Figure out where to

        Vector2 newRoom = DungeonLayout.GetActivatedDoorDirection(playerObj.transform.position, playerRoomPosition);

        playerRoomPosition = new RoomObject((int)newRoom.x + playerRoomPosition.x, (int)newRoom.y + playerRoomPosition.y);

        //Addition
        CurrentDungeon.UpdatePlayerPosition(playerRoomPosition);

        #endregion Figure out where to

        //So the player isn't in a loop between the two rooms
        player.canUseDoors = false;

        #region Prepare spawning of mobs

        roomHolder = GameObject.Find(playerRoomPosition.ToString()).transform;

        foreach (Transform obj in roomHolder) {
            obj.gameObject.SetActive(false);
        }

        #endregion Prepare spawning of mobs

        //Move camera
        CameraManager.instance.MoveToRoom(newRoom);
    }

    public void ActivateCurrentRoom() {
        print(playerRoomPosition);
        foreach (Transform obj in roomHolder) {
            if (obj.gameObject.tag == "SpawnerMob") {
                GameObject instance = Instantiate(mob, obj.position, Quaternion.identity);
                instance.transform.SetParent(roomHolder);
                instance.gameObject.GetComponent<EnemyAttackController>().playerPosition = player.transform;
                Destroy(obj.gameObject);
            }
        }
    }

    public void DeactivateCurrentRoom() {
    }
}