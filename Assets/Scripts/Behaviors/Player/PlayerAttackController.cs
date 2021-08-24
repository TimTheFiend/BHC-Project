using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : AttackingObject
{
    //public void OnAttack(InputAction.CallbackContext _context) {
    //    if (AttemptAttack()) {
    //    }
    //}
    private void Start() {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}