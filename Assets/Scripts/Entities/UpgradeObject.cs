using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/Upgrade Item", order = 1)]
public class UpgradeObject : ScriptableObject
{
    public Sprite sprite;
    public bool IsWeaponUpgrade;

    public WeaponStats weaponStats => new WeaponStats(Damage, Speed);

    [Header("Weapon")]
    public float Damage;
    public float Speed;

    [Header("Character")]
    public float Life;
    public float MovementSpeed;
}