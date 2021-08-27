using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupObject : MonoBehaviour {
    // Pickup
    // Remove

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            OnPickup();
        }
    }

    protected virtual void OnPickup() {
        Destroy(gameObject);
    }
}