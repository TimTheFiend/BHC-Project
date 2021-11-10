using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorObject : MonoBehaviour
{
    public bool isOpen = false;

    private SpriteRenderer sRenderer;
    [SerializeField] private Sprite doorOpen;
    [SerializeField] private Sprite doorClosed;

    private void Start() {
        sRenderer = GetComponent<SpriteRenderer>();
        ActivateDoor(false);
    }

    public void SetDoorState(bool isOpen) {
        this.isOpen = isOpen;
        GetComponent<SpriteRenderer>().sprite = this.isOpen ? doorOpen : doorClosed;
    }

    public void ActivateDoor(bool hasChanged) {
        if (hasChanged) {
            isOpen = !isOpen;
        }
        GetComponent<SpriteRenderer>().sprite = isOpen ? doorOpen : doorClosed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (GameManager.instance.canUseDoors && collision.gameObject.tag == "Player") {
            GameManager.instance.PrepareMovementBetweenRooms();
        }
    }
}