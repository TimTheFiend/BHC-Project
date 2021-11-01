using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class DoorObject : MonoBehaviour
{
    public bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isOpen) {
            GameManager.instance.MovePlayerToRoom();
        }
    }
}
