using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UpgradeEntity : MonoBehaviour
{
    public WeaponStats weaponUpgrade;
    public UpgradeObject upgradeObject;

    //Kun for at give weaponUpgrade v?rdier
    private void Start() {
        weaponUpgrade = new WeaponStats(25f, 12f);

        if (upgradeObject.sprite != null) {
            GetComponent<SpriteRenderer>().sprite = upgradeObject.sprite;
        }
    }

    /// <summary>
    /// Sets the values of the object.
    /// </summary>
    /// <param name="stats">The upgrade.</param>
    public void SetValues(UpgradeObject stats) {
        upgradeObject = stats;

        GetComponent<SpriteRenderer>().sprite = upgradeObject.sprite;
    }

    /// <summary>
    /// Handles upgrading the player.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;

            //If the upgrade is a weapon upgrade.
            if (upgradeObject.IsWeaponUpgrade) {
                (player.GetComponent<PlayerAttackController>().activeWeapon as RangedWeaponEntity).weaponStats.UpgradeStats(upgradeObject.weaponStats);
            }

            Destroy(gameObject);
        }
    }
}