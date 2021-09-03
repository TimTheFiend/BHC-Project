using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedWeaponEntity : WeaponEntity
{
    public GameObject projectileObject;
    public ProjectileEntity projectile;

    private void Start() {
        projectile = projectileObject.GetComponent<ProjectileEntity>();
        // TODO 
        projectile.damage = 15f;
        projectile.ProjectileSpeed = 15f;
    }

    public override void AttemptAttack() {
        if (!isAttacking) {
            //TODO attack
            StartCoroutine(Attack());
        }
    }

    protected override IEnumerator Attack() {
        isAttacking = true;

        GameObject toInstantiate = Instantiate(projectileObject, transform.position, transform.rotation);
        
        toInstantiate.GetComponent<Rigidbody2D>().AddForce(transform.right * projectile.ProjectileSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCooldown);

        projectile.ProjectileSpeed = 5f;

        isAttacking = false;
    }
}