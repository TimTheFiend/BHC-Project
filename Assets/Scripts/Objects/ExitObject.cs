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

    /// <summary>
    /// Sets a delegate on <see cref="SceneManager.sceneLoaded"/> based on where the object is.
    /// </summary>
    private void Start() {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName.ToLower().Contains("hub")) {
            SceneManager.sceneLoaded += OnSceneLoaded_Dungeon;
        }
        else {
            SceneManager.sceneLoaded += OnSceneLoaded_HubWorld;
        }
    }

    /// <summary>
    /// Handles scene preparation for going to the Dungeon.
    /// </summary>
    private void OnSceneLoaded_Dungeon(Scene scene, LoadSceneMode mode) {
        GameManager.instance.StartNewDungeonFloor();
        SceneManager.sceneLoaded -= OnSceneLoaded_Dungeon;
    }

    /// <summary>
    /// Handles scene preparation for going to the hubworld.
    /// </summary>
    private void OnSceneLoaded_HubWorld(Scene scene, LoadSceneMode mode) {
        GameManager.instance.playerObj.transform.position = hubWorldPos;
        CameraManager.instance.gameObject.transform.position = hubWorldPosCam;

        SceneManager.sceneLoaded -= OnSceneLoaded_HubWorld;
    }

    /// <summary>
    /// Loads the next scene based on the current scene's name.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Scene currentScene = SceneManager.GetActiveScene();
            string sceneName = currentScene.name;

            if (sceneName == hubWorld) {
                SceneManager.LoadScene(gameplay);
            }
            else {
                SceneManager.LoadScene(hubWorld);
            }
        }
    }
}