using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : PickupObject
{
    public int coinWorth = 5;

    protected override void OnPickup(GameObject playerObj) {
        playerObj.GetComponent<PlayerController>().coins += coinWorth;
        Destroy(gameObject);
    }
}
