using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterObject
{
    [SerializeField] private EnemyAttackController eAttack;
    [SerializeField] private EnemyMovementController eMovement;

    protected override void Start() {
        base.Start();

        eMovement = GetComponent<EnemyMovementController>();
        eAttack = GetComponent<EnemyAttackController>();
    }

    protected override void Die() {
        Vector3 pos = transform.position;
        if (gameObject.name.StartsWith("Boss")) {
            GameManager.instance.BossRoomIsCompleted();
            base.Die();
            return;
        }
        base.Die();
        GameManager.instance.IsActiveRoomCompleted(pos);
    }
}