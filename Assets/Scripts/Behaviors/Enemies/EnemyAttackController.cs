using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackingObject
{
    public Transform playerPosition;
    public bool enableAutoAttack = false;
    [Range(2f, 10f)] public float autoAttackTimer;

    Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
        StartCoroutine(AutoAttackCoroutine());
    }

    /// <summary>
    /// Enables autoattacking
    /// </summary>
    /// <returns></returns>
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

    private void Update() {
        // put in seperate function

        float laserLength = 50f;

        // do some mathmagic on playerPosition.position so the raycast actualyl aims at the correct direction
        Vector2 realPosition = transform.InverseTransformPoint(playerPosition.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, realPosition, laserLength);

        if(hit.collider != null) {
            Debug.Log("Hitting: " + hit.collider.tag);
            Debug.Log(hit.point);

            if(hit.collider.tag == "Enemy") {

            }
            else if (hit.collider.tag == "Player") {
            }
        }
        Debug.DrawRay(transform.position, realPosition * laserLength, Color.red);
    }
}
