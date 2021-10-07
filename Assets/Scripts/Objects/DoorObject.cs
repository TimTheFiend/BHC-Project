using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorObject : MonoBehaviour
{
    public bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (isOpen) {
            print("Trigger: " + collision.name);
        }
    }
}
