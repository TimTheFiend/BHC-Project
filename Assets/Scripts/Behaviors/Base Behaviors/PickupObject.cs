using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupObject : MonoBehaviour {
    // Pickup
    // Remove


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            OnPickup(collision.gameObject);
        }
        print(collision.gameObject.name);
    }

    protected virtual void OnPickup(GameObject playerObj) {
        Destroy(gameObject);
    }
}