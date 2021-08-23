using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponObject : WeaponObject
{
    [Header("Ranged components")]
    public GameObject projectile;

    [Header("Ranged weapon vars")]
    public bool canShoot = true;
    public float shotCooldown = 0.5f;

    protected virtual void Shoot() {
        if (canShoot) {
            StartCoroutine(ShootRoutine());
        }
    }

    protected virtual IEnumerator ShootRoutine() {
        canShoot = false;

        print("Hello world");

        //Spawn Projectile
        GameObject _projectile = Instantiate(projectile, weaponTransform.position, weaponTransform.rotation);
        //Add force
        _projectile.GetComponent<Rigidbody2D>().AddForce(transform.right * 2.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(shotCooldown);

        canShoot = true;
    }
}