using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitObject : MonoBehaviour
{
    public bool isDebug = true;

    public string hubWorld = "HubWorld";
    public string gameplay = "Dungeon";

    private void OnEnable() {
        Debug.Log("Enabled");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        Debug.Log("Disabled");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        GameManager.instance.StartNewDungeonFloor();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public Vector3 hubWorldPos = new Vector3(0.5f, 3.5f, 0);

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            if (isDebug) {
                SceneManager.LoadScene(gameplay);
                GameManager.instance.StartNewDungeonFloor();
                return;
            }

            if (sceneName == hubWorld) {
                SceneManager.LoadScene(gameplay);
            }
            else {
                SceneManager.LoadScene(hubWorld);
                GameManager.instance.playerObj.transform.position = hubWorldPos;
                CameraManager.instance.gameObject.transform.position = hubWorldPos;
            }
        }
    }
}