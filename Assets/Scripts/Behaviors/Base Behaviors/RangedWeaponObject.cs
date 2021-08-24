using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponObject : WeaponObject
{
    [Header("Ranged components")]
    public GameObject projectile;

    protected virtual IEnumerator ShootRoutine() {
        canAttack = false;

        //Spawn Projectile
        GameObject _projectile = Instantiate(projectile, weaponTransform.position, weaponTransform.rotation);
        //Add force
        _projectile.GetComponent<Rigidbody2D>().AddForce(weaponTransform.right * 2.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }
}