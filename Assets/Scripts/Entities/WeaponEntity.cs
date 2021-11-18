using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEntity : MonoBehaviour
{
    public bool isAttacking = false;
    public WeaponStats weaponStats;

    public float attackCooldown = .5f;
    public float attackDamage = .5f;

    /// <summary>
    /// Attempts to attack, and attacks if it isn't on cooldown.
    /// </summary>
    public virtual void AttemptAttack() {
        if (!isAttacking) {
            StartCoroutine(Attack());
        }
    }

    /// <summary>
    /// The weapon attacks.
    /// </summary>
    protected virtual IEnumerator Attack() {
        isAttacking = true;

        print("attack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }
}