using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : AttackingObject
{
    public Transform playerPosition;
    public bool enableAutoAttack = false;
    [Range(0.1f, 10f)] public float autoAttackTimer;

    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        activeWeapon = transform.GetChild(0).GetComponent<WeaponEntity>();
        StartCoroutine(AutoAttackCoroutine());

        SetWeaponStats();
    }

    public IEnumerator AutoAttackCoroutine() {
        while (true) {
            float time = 0f;
            while (time < autoAttackTimer) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (enableAutoAttack) {
                activeWeapon.AttemptAttack();
            }
        }
    }

    public override void UpdateTrackingData() {
        UpdateTrackingData(playerPosition.position);
    }

    private void Update() {
        float laserLength = 50f;
        int objectLayers = LayerMask.GetMask("Objects", "Walls");

        // playerPosition.position has to be changed to realPlayerPosition by using InverseTransformPoint
        Vector2 realPlayerPosition = transform.InverseTransformPoint(playerPosition.position);

        // creates a ray from an enemy position aiming at the player position
        RaycastHit2D hit = Physics2D.Raycast(transform.position, realPlayerPosition, laserLength, objectLayers);

        // if the ray hits the player
        if (hit.collider.tag == "Player") {
            Debug.DrawRay(transform.position, realPlayerPosition * laserLength, Color.green);
            enableAutoAttack = true;
        }

        // if the ray hits an enemy
        else if (hit.collider.tag == "Enemy") {
            float newOriginX = hit.point.x;
            float newOriginY = hit.point.y;

            // following ifs changes newOriginX and newOriginY to be slightly inside the enemy the ray has hit.
            if (playerPosition.position.x > hit.point.x) {
                // right
                newOriginX = newOriginX + 0.1f;
            }
            else {
                // left
                newOriginX = newOriginX - 0.1f;
            }

            if (playerPosition.position.y > hit.point.y) {
                // up
                newOriginY = newOriginY + 0.1f;
            }
            else {
                // down
                newOriginY = newOriginY - 0.1f;
            }

            Vector2 newOriginPoint = new Vector2(newOriginX, newOriginY);

            // creates a new ray but where the origin is based on the point where the previous ray hit the enemy, but is slightly altered to be inside the enemy rather that outside or on the enemy.
            RaycastHit2D newHit = Physics2D.Raycast(newOriginPoint, realPlayerPosition, laserLength, objectLayers);

            // if the new ray hits the player
            if (newHit.collider.tag == "Player") {
                Debug.DrawRay(newOriginPoint, realPlayerPosition * laserLength, Color.green);
                enableAutoAttack = true;
            }

            // if the new ray does not hit the player
            else {
                Debug.DrawRay(newOriginPoint, realPlayerPosition * laserLength, Color.red);
                enableAutoAttack = false;
            }
        }

        // if the ray does not hit the player
        else {
            Debug.DrawRay(transform.position, realPlayerPosition * laserLength, Color.red);
            enableAutoAttack = false;
        }
    }
}