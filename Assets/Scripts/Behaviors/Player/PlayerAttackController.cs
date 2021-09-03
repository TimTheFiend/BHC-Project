using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : AttackingObject
{
    public bool flipAbility = false;

    private void Start() {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
        //weaponTransform = transform.GetChild(0).transform;
        //ChangeProjectile();
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}