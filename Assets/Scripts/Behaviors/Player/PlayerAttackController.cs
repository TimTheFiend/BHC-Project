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

    /// <summary>
    /// Sets the weapon to have the default player stats.
    /// </summary>
    protected override void SetWeaponStats() {
        activeWeapon.weaponStats = WeaponStats.PlayerDefault();
    }

    /// <summary>
    /// Updates the data so it follows the cursor position.
    /// </summary>
    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}