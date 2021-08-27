using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackController : AttackingObject
{
    public bool flipAbility = false;
    public int counter = 0;

    private void Start() {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
        //weaponTransform = transform.GetChild(0).transform;
        //ChangeProjectile();
    }

    public override void AttemptAttack() {
        counter++;

        if (counter > 5) {
            ChangeProjectile();
            print("FLIP ABILITY");
            flipAbility = !flipAbility;
            counter = 0;
        }
        base.AttemptAttack();
    }

    public void ChangeProjectile() {
        if (activeWeapon.GetType() == typeof(RangedWeaponEntity)) {
            (activeWeapon as RangedWeaponEntity).projectile.GetComponent<ProjectileEntity>().ChangeFlags(ProjectileFlags.Piercing, flipAbility);
            print((activeWeapon as RangedWeaponEntity).projectile.GetComponent<ProjectileEntity>().projectileFlags.ToString());
        }
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
    }
}