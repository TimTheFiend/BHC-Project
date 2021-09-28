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
        // projectile entities from enemies damage other enemies
        // "newHit" rayline casting seem to randomly change based on angle
        //      possible issue is that the hit.point is outside of "Enemy" instead of on or inside, and will therefore see the "Enemy" it's supposed to ignore as an obstacle

        float laserLength = 50f;
        int objectLayers = LayerMask.GetMask("Objects", "Walls");
        Vector2 realPlayerPosition = transform.InverseTransformPoint(playerPosition.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, realPlayerPosition, laserLength, objectLayers);

        if(hit.collider.tag == "Player") {
            Debug.DrawRay(transform.position, realPlayerPosition * laserLength, Color.green);
            enableAutoAttack = true;
        }

        else if(hit.collider.tag == "Enemy") {
            RaycastHit2D newHit = Physics2D.Raycast(hit.point, realPlayerPosition, laserLength, objectLayers);
            print(newHit.point);

            // TODO
            // check direction that the enemy is aiming, add/subtract number of the hit.point coordinate used below, based on direction, so that newHit origin is inside "Enemy" instead of outside
            if(newHit.collider.tag == "Player") {
                Debug.DrawRay(hit.point, realPlayerPosition * laserLength, Color.green);
                enableAutoAttack = true;
            }

            else {
                Debug.DrawRay(hit.point, realPlayerPosition * laserLength, Color.red);
                enableAutoAttack = false;
            }
        }

        else {
            Debug.DrawRay(transform.position, realPlayerPosition * laserLength, Color.red);
            enableAutoAttack = false;            
        }
    }
}
