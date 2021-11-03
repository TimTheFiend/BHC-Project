using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    [Header("Spaghetti values")]
    private readonly float playerMoveIntoRoom = 3f;  //it kinda fits.

    [Header("Object reference")]
    private Transform player;
    private Camera gameCam;

    [Header("Transition variables")]
    [Range(0.5f, 5f)] public float transitionTimeInSeconds = 2.5f;
    public bool isCameraTransitioning = false;

    [Header("Camera variables")]
    [SerializeField] float cameraWidth;
    [SerializeField] float cameraHeight;
    public float totalWidth => cameraWidth * 2;
    public float totalHeight => cameraHeight * 2;

    [Header("Aspect ratio")]
    private readonly Vector2Int aspectRatio = new Vector2Int(4, 3);

    #region Awake & Start
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


    }

    private void Start() {
        //Get player transform reference
        if (player == null) {
            player = GameManager.instance.playerObj.transform;
        }
        gameCam = Camera.main;

        //Set camera size
        cameraHeight = gameCam.orthographicSize;
        cameraWidth = cameraHeight + (cameraHeight / aspectRatio.y);
        //Position set
    }

    #endregion

    public void MoveToRoom(Vector2 room) {
        Vector3 newPlayerPos = player.position;
        bool isHorizontal = Mathf.Abs(room.x) > room.y;
        bool isPositive = isHorizontal ? room.x > 0f : room.y > 0f;
        if (isHorizontal) {
            newPlayerPos.x += isPositive ? playerMoveIntoRoom : -playerMoveIntoRoom;
        }
        else {
            newPlayerPos.y += isPositive ? playerMoveIntoRoom : -playerMoveIntoRoom;
        }


        StartCoroutine(MoveCamera(new Vector3(room.x * totalWidth, room.y * totalHeight), newPlayerPos));
    }

    private IEnumerator MoveCamera(Vector3 newPos) {
        #region Transitions camera to new room
        Vector3 startPos = transform.position;
        Vector3 position = transform.position + newPos;

        float timer = 0f;
        isCameraTransitioning = true;

        while (timer <= transitionTimeInSeconds) {
            transform.position = Vector3.Lerp(startPos, position, (timer / transitionTimeInSeconds));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = position;
        isCameraTransitioning = false;
        #endregion
    }

    private IEnumerator MoveCamera(Vector3 newPos, Vector3 movePlayerToPos) {
        Debug.Assert(movePlayerToPos != player.position, "The position to move the player is the same as the current");

        player.gameObject.SetActive(false);
        player.position = movePlayerToPos;

        #region Transitions camera to new room
        Vector3 startPos = transform.position;
        Vector3 position = transform.position + newPos;

        float timer = 0f;
        isCameraTransitioning = true;

        while (timer <= transitionTimeInSeconds) {
            transform.position = Vector3.Lerp(startPos, position, (timer / transitionTimeInSeconds));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = position;
        isCameraTransitioning = false;
        #endregion

        player.gameObject.SetActive(true);
    }
}
