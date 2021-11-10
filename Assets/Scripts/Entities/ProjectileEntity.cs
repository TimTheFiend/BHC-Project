using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEntity : MonoBehaviour
{
    public GameObject parent;
    public WeaponStats stats;
    public ProjectileFlags projectileFlags;

    public void SpawnProjectile(GameObject spawnObject, WeaponStats stats, Transform weaponTransfrom, ForceMode2D forceMode) {
        parent = spawnObject;
        this.stats = stats;

        if (parent.tag == "Enemy") {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        GetComponent<Rigidbody2D>().AddForce(weaponTransfrom.right * this.stats.speed, forceMode);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        //If parent is not dead
        if (parent != null) {
            //Check if projectile hits the object that spawned it.
            if (collision.gameObject.tag == parent.tag) {
                return;
            }
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "BreakableObject") {
            collision.gameObject.GetComponent<CharacterObject>().LoseHealth(stats.damage);
        }
        OnHit();
    }

    private void OnHit() {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        print(collision.gameObject);
    }
}

[System.Flags]
public enum ProjectileFlags
{
    None = 0,
    Piercing = 1,
    Etheral = 2,
    Temp1 = 4,
    Temp2 = 8,
    temp3 = 16
}