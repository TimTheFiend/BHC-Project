using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class UpgradeEntity : MonoBehaviour
{
    public WeaponUpgrade weaponUpgrade;

    //Kun for at give weaponUpgrade værdier
    private void Start()
    {
        weaponUpgrade = new WeaponUpgrade(100, 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;

            (player.GetComponent<PlayerAttackController>().activeWeapon as RangedWeaponEntity).UpgradeProjectile(weaponUpgrade);

            Destroy(gameObject);
        }
    }
}


/* STRUCTS */
public struct WeaponUpgrade
{
    public float damage;
    public float speed;

    public WeaponUpgrade(float damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
    }
}

public struct CharacterUpgrade
{
    //TODO
}