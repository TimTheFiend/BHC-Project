using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackingObject
{
    public Transform playerPosition;
    public bool enableAutoAttack = false;
    [Range(2f, 10f)] public float autoAttackTimer;

    // Start is called before the first frame update
    void Start()
    {
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();

        StartCoroutine(AutoAttackCoroutine());
    }

    public IEnumerator AutoAttackCoroutine() {
        while(true) {
            float time = 0f;
            while(time < autoAttackTimer) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if(enableAutoAttack) {
                activeWeapon.AttemptAttack();
            }
        }
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(playerPosition.position);
    }
}
