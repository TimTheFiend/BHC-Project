using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class ExitObject : MonoBehaviour
{
    public Scene gameplay;
    public Scene hubworld;

    private void OnTriggerEnter2D(Collider2D collision) {
        /*
         * File > Build Settings > Scenes in Build
         */
        if (collision.tag == "Player") {
            SceneManager.LoadScene("New Room");
        }
    }
}