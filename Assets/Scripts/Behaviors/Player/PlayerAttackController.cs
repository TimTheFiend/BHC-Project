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

        ChangeProjectile();
    }

    public void ChangeProjectile() {
        if (activeWeapon.GetType() == typeof(RangedWeaponEntity)) {
            (activeWeapon as RangedWeaponEntity).projectile.GetComponent<ProjectileEntity>().ChangeFlags(ProjectileFlags.Piercing, true);
        }
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}