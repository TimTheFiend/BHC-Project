using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : RangedWeaponObject
{
    public void OnAttack(InputAction.CallbackContext _context) {
        if (AttemptAttack()) {
            StartCoroutine(ShootRoutine());
        }
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}