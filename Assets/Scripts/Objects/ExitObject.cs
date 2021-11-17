using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitObject : MonoBehaviour
{
    public bool isHubworld = false;

    public string hubWorld = "HubWorld";
    public string gameplay = "Movement";

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            if (isHubworld) {
                SceneManager.LoadScene(gameplay);
            }
            else {
                SceneManager.LoadScene(hubWorld);
            }
        }
    }
}