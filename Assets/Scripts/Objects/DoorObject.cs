using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Door-object used in the Dungeon.
/// </summary>
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

    /// <summary>
    /// Change the door sprite to either Open or Closed.
    /// </summary>
    /// <param name="isOpen">The state the door should be.</param>
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