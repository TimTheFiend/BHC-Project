using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEntity : MonoBehaviour
{
    public GameObject parent;
    public WeaponStats stats;
    private string parentTag;  //In case the parent dies prior to this object destroying.

    /// <summary>
    /// Sets the object's properties and references.
    /// </summary>
    /// <param name="parentObject">Object that spawns this.</param>
    /// <param name="stats">Stats of this object</param>
    /// <param name="weaponTransfrom">Position in which to spawn this object.</param>
    /// <param name="forceMode">How this object is pushed, currently only one kind is used.</param>
    public void SpawnProjectile(GameObject parentObject, WeaponStats stats, Transform weaponTransfrom, ForceMode2D forceMode) {
        parent = parentObject;
        parentTag = parent.gameObject.tag;
        this.stats = stats;

        if (parent.tag == "Enemy") {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

        GetComponent<Rigidbody2D>().AddForce(weaponTransfrom.right * this.stats.speed, forceMode);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == parentTag) {
            return;
        }

        if (collision.tag == "Player" || collision.tag == "Enemy" || collision.tag == "BreakableObject") {
            collision.gameObject.GetComponent<CharacterObject>().LoseHealth(stats.damage);
        }
        OnHit();
    }

    private void OnHit() {
        Destroy(gameObject);
    }
}