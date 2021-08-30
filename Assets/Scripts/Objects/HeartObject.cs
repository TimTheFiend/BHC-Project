using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartObject : PickupObject {

    public float amountToRecover = 10f;

    protected override void OnPickup(GameObject playerObj) {
        playerObj.GetComponent<PlayerController>().RecoverHealth(amountToRecover);
        Destroy(gameObject);
    }
}
