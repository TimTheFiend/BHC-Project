using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DoorObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.name);
    }
    
}
