using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponEntity : MonoBehaviour
{
    public bool isAttacking = false;
    public float attackCooldown = .5f;
    public float attackDamage = .5f;


    public void AttemptAttack() {
        if(!isAttacking) {
            //TODO attack
            StartCoroutine(Attack());
        }
    }

    protected virtual IEnumerator Attack() {
        isAttacking = true;

        //TODO attack
        print("attack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

}