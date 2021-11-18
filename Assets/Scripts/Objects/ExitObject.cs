using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitObject : MonoBehaviour
{
    public bool isDebug = true;

    public string hubWorld = "HubWorld";
    public string gameplay = "Dungeon";
    private Vector3 hubWorldPosCam = new Vector3(0.5f, 3.5f, -10f);
    private Vector3 hubWorldPos = new Vector3(0.5f, 3.5f, 0f);

    private void OnEnable() {
        Debug.Log("Enabled");
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName.ToLower().Contains("hub")) {
            Debug.Log("GO TO DUNGEON");
            SceneManager.sceneLoaded += OnSceneLoaded_Dungeon;
        }
        else {
            Debug.Log("GO TO HUBWORLD");
            SceneManager.sceneLoaded += OnSceneLoaded_HubWorld;
        }
    }

    private void OnDisable() {
        Debug.Log("Disabled");
    }

    private void OnSceneLoaded_Dungeon(Scene scene, LoadSceneMode mode) {
        GameManager.instance.StartNewDungeonFloor();
        SceneManager.sceneLoaded -= OnSceneLoaded_Dungeon;
    }

    private void OnSceneLoaded_HubWorld(Scene scene, LoadSceneMode mode) {
        GameManager.instance.playerObj.transform.position = hubWorldPos;
        CameraManager.instance.gameObject.transform.position = hubWorldPosCam;

        SceneManager.sceneLoaded -= OnSceneLoaded_HubWorld;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            if (sceneName == hubWorld) {
                SceneManager.LoadScene(gameplay);
            }
            else {
                SceneManager.LoadScene(hubWorld);
                //GameManager.instance.playerObj.transform.position = hubWorldPos;
                //CameraManager.instance.gameObject.transform.position = hubWorldPos;
            }
        }
    }
}