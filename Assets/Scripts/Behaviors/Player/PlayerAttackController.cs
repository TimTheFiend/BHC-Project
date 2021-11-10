using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : AttackingObject
{
    private void Start() {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
        //weaponTransform = transform.GetChild(0).transform;
        //ChangeProjectile();
        SetWeaponStats();
    }

    protected override void SetWeaponStats() {
        activeWeapon.weaponStats = WeaponStats.PlayerDefault();
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}