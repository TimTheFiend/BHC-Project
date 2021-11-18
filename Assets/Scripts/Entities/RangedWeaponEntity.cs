using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponEntity : WeaponEntity
{
    public GameObject projectileObject;
    [SerializeField] private GameObject parent;

    private void Start() {
        parent = transform.parent.gameObject;
    }

    public override void AttemptAttack() {
        if (!isAttacking) {
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// Spawns a projectile from the object.
    /// </summary>
    protected override IEnumerator Attack() {
        isAttacking = true;

        GameObject toInstantiate = (GameObject)Instantiate(projectileObject, transform.position, transform.rotation);
        toInstantiate.GetComponent<ProjectileEntity>().SpawnProjectile(parent, weaponStats, transform, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}