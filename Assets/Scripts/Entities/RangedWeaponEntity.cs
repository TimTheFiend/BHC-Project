using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeaponEntity : WeaponEntity
{
    public GameObject projectile;

    public override void AttemptAttack() {
        if (!isAttacking) {
            //TODO attack
            StartCoroutine(Attack());
        }
    }

    protected override IEnumerator Attack() {
        isAttacking = true;
        GameObject toInstantiate = Instantiate(projectile, transform.position, transform.rotation);
        // TODO change static value
        toInstantiate.GetComponent<Rigidbody2D>().AddForce(transform.right * 2.5f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}