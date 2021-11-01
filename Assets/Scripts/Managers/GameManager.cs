using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; // Singleton

    [Tooltip("Used to keep track of Player's position in relation to the minimap.")]
    public RoomObject playerRoomPosition;
    [Tooltip("The player object.")]
    public GameObject player;

    private void Start() {
        DungeonGenerator.Instance.GenerateDungeon();

        Vector3 v = DungeonLayout.GetRoomCenterWorldPosition(DungeonGenerator.Instance.startRoom);
        player.transform.position = new Vector3(v.x, v.y, 0f);
        Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
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

        if (player == null) {
            player = GameObject.Find("Player");
        }
    }

    public void MovePlayerToRoom() {
        Vector2 newRoom = DungeonLayout.GetActivatedDoorDirection(player.transform.position, playerRoomPosition);

        playerRoomPosition = new RoomObject((int)newRoom.x + playerRoomPosition.x, (int)newRoom.y + playerRoomPosition.y);

        CameraManager.instance.MoveToRoom(newRoom);
    }
}