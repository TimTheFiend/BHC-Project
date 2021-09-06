using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackingObject
{
    // Start is called before the first frame update
    void Start()
    {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
    }

    // Update is called once per frame
    public override void UpdateTrackingData() {


        UpdateTrackingData(Camera.main.ScreenToWorldPoint(GameObject.Find("Player").transform.position));

        // Fra PlayerAttackController
        // UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}
