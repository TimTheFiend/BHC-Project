using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : WeaponObject
{
    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}