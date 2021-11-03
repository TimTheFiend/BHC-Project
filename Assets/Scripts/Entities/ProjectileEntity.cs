using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEntity : MonoBehaviour
{
    public GameObject parent;
    public float damage = 10f;
    public float projectileSpeed = 2.5f;
    public ProjectileFlags projectileFlags;

    public void SpawnProjectile(GameObject spawnObject, Transform weaponTransfrom, ForceMode2D forceMode) {
        parent = spawnObject;
        if(parent.tag == "Enemy") {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        GetComponent<Rigidbody2D>().AddForce(weaponTransfrom.right * projectileSpeed, forceMode);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == parent.tag) {
            return;
        }

        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<CharacterObject>().LoseHealth(damage);
        }
        OnHit();


        //print(collision.gameObject.tag);
    }

    private void OnHit() {
        // Check to see if destroy self.
        if (projectileFlags.HasFlag(ProjectileFlags.Piercing)) {
            return;
        }
        
        Destroy(gameObject);
    }

    //NOTE: Not in use
    //public void ChangeFlags(ProjectileFlags flag, bool state) {
    //    switch (state) {
    //        case true:
    //            projectileFlags |= flag;
    //            return;

    //        case false:
    //            projectileFlags &= ~flag;
    //            return;
    //    }
    //}

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