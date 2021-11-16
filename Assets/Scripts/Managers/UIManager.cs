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

    [Header("Toggle-ables")]
    [SerializeField] private Image statsMenu;
    [SerializeField] private Image mapMenu;
    public GameObject mapObject;
    public GameObject roomObject;
    public Canvas mapCanvas;

    [Header("Minimap UI Test")]
    public RenderTexture image;
    public Sprite sprite;

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
        InitializeToggleUI();

        GameObject newRoom = Instantiate(roomObject, mapObject.transform.position, Quaternion.identity);

        newRoom.transform.SetParent(mapCanvas.transform, false);
        newRoom.transform.position = new Vector3(20f, 20f, 0);

        RectTransform rect = newRoom.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(20f, 20f);

        //newRoom.transform.position = new Vector3(50f, 20f, 0);
        Debug.Log(rect.anchoredPosition);
    }

    //Gemmer toggle ui væk på startup
    private void InitializeToggleUI() {
        statsMenu.gameObject.SetActive(false);
        mapMenu.gameObject.SetActive(false);
    }

    public void ToggleStats(bool isActive) {
        statsMenu.gameObject.SetActive(isActive);
    }

    public void ToggleMap(bool isActive) {
        mapMenu.gameObject.SetActive(isActive);
    }

    public void DEVUpdateHPBar(float healthPercentage) {
        hpBar.value = healthPercentage;
    }

    public float UpdateHPBar {
        set {
            hpBar.value = value;
        }
    }

    //DEPRECATED
    public void DEVUpdateEnergyBar(float energyPercentage) {
        energyBar.value = energyPercentage;
    }

    public float UpdateEnergyBar {
        set {
            energyBar.value = value;
        }
    }
}