using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null; // Singleton

    [Header("Gameplay constants")]
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider energyBar;
    [SerializeField] private GameObject mainUI;

    [Header("Toggle-ables")]
    [SerializeField] private Image statsMenu;
    [SerializeField] private Image mapMenu;
    public GameObject mapObject;
    public GameObject _panel;
    public Canvas mapCanvas;

    [Header("Minimap UI Test")]
    public Sprite normalRoom;
    public Sprite bossRoom;
    public Sprite itemRoom;
    public Sprite unknownRoom;
    public Dictionary<Vector2, GameObject> minimapRooms = new Dictionary<Vector2, GameObject>();
    private const float roomSize = 20f;

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

        DontDestroyOnLoad(mainUI);
    }

    private void Start() {
        InitializeToggleUI();
    }

    //Gemmer toggle ui væk på startup
    private void InitializeToggleUI() {
        statsMenu.gameObject.SetActive(false);
        mapMenu.gameObject.SetActive(false);
    }

    #region Toggle UI

    public void ToggleStats(bool isActive) {
        statsMenu.gameObject.SetActive(isActive);
    }

    public void ToggleMap(bool isActive) {
        mapMenu.gameObject.SetActive(isActive);
    }

    #endregion Toggle UI

    #region Updating player bars

    public float UpdateHPBar {
        set {
            hpBar.value = value;
        }
    }

    public float UpdateEnergyBar {
        set {
            energyBar.value = value;
        }
    }

    #endregion Updating player bars

    /* Minimap UI */

    public void DrawInitialMinimap(List<RoomObject> rooms) {
        RoomObject startRoom = GameManager.instance.activePlayerRoom;

        foreach (RoomObject room in rooms) {
            RoomObject tempRoom = room - startRoom;

            Vector2 position = new Vector2(tempRoom.x * roomSize * -1, tempRoom.y * roomSize * -1);  //*-1 to reverse the mirroring.

            /* Spawner _panel */
            GameObject newRoom = Instantiate(_panel, mapObject.transform.position, Quaternion.identity);
            newRoom.name = room.type.ToString();
            /*temp*/
            minimapRooms.Add(position, newRoom);
            //newRoom.GetComponent<Image>().sprite = TempDrawProperRoom(room);
            /* temp slut*/

            newRoom.transform.SetParent(mapCanvas.transform, false);
            (newRoom.transform as RectTransform).sizeDelta = new Vector2(roomSize, roomSize);
            (newRoom.transform as RectTransform).anchoredPosition = position;
        }

        UpdateCurrentMinimapRoom();
    }

    private void UpdateCurrentMinimapRoom() {
        //Finde GameObject på Canvas position
        GameObject roomUI = minimapRooms[(mapCanvas.transform as RectTransform).anchoredPosition * -1];
        //Hente RoomType baseret på GameObject.name
        RoomType roomType;
        Debug.Assert(System.Enum.TryParse(roomUI.name, out roomType), "Unable to convert string to RoomType");
        //Ændr Image.sprite

        Sprite spriteToUse;

        switch (roomType) {
            case RoomType.Boss:
                spriteToUse = bossRoom;
                break;

            case RoomType.Item:
                spriteToUse = itemRoom;
                break;

            default:
                spriteToUse = normalRoom;
                break;
        }

        roomUI.GetComponent<Image>().sprite = spriteToUse;
    }

    public void MoveMinimap(Vector2 dir) {
        Vector2 moveDir = Vector2.zero;
        if (dir == Vector2.up) {
            moveDir = Vector2.down * roomSize;
        }
        else if (dir == Vector2.down) {
            moveDir = Vector2.up * roomSize;
        }
        else if (dir == Vector2.left) {
            moveDir = Vector2.right * roomSize;
        }
        else if (dir == Vector2.right) {
            moveDir = Vector2.left * roomSize;
        }

        (mapCanvas.transform as RectTransform).anchoredPosition += moveDir;
        UpdateCurrentMinimapRoom();
    }
}
