using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WeaponStats
{
    /* FIELDS */
    public float damage;
    public float speed;

    /* Constructors */

    public WeaponStats(float damage, float speed) {
        this.damage = damage;
        this.speed = speed;
    }

    /* Methods */

    public void UpgradeStats(WeaponStats upgrade) {
        damage += upgrade.damage;
        speed += upgrade.speed;
    }

    public override string ToString() {
        return $"DMG: {damage} | SPD: {speed}";
    }

    /* Static gets */

    public static WeaponStats PlayerDefault() {
        return new WeaponStats(100f, 10f);
    }

    public static WeaponStats EnemyDefault() {
        return new WeaponStats(10f, 5f);
    }
}