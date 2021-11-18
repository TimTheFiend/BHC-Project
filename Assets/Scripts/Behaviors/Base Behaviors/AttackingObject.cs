using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingObject : MonoBehaviour
{
    public WeaponEntity activeWeapon;

    public Transform weaponTransform;
    public float weaponAngle;

    public bool canAttack = true;
    [Range(0f, 5f)] public float attackCooldown = 0.5f;

    private float rotationRadius = 0.5f;
    protected Vector2 trackingPosition;

    private void FixedUpdate() {
        UpdateTrackingData();
        MoveWeapon();
    }

    /// <summary>
    /// Sets the values of the weapon.
    /// </summary>
    protected virtual void SetWeaponStats() {
        activeWeapon.weaponStats = WeaponStats.EnemyDefault();
    }

    /// <summary>
    /// Attempts to active with <see cref="activeWeapon"/>
    /// </summary>
    public virtual void AttemptAttack() {
        activeWeapon.AttemptAttack();
    }

    /// <summary>
    /// Must be overwritten.
    /// </summary>
    public virtual void UpdateTrackingData() {
    }

    /// <summary>
    /// Updates the position of where to point the weapon.
    /// </summary>
    /// <param name="newTrackingPosition">the position.</param>
    protected virtual void UpdateTrackingData(Vector2 newTrackingPosition) {
        trackingPosition = newTrackingPosition;
    }

    /// <summary>
    /// Moves the weapon around.
    /// </summary>
    protected virtual void MoveWeapon() {
        CalculateWeaponAngle();

        weaponTransform.position = GetWeaponPosition();
        weaponTransform.rotation = GetWeaponRotation();
    }

    #region Maths

    protected virtual void CalculateWeaponAngle() {
        Vector2 relativePosition = transform.InverseTransformPoint(trackingPosition);
        weaponAngle = Mathf.Atan2(relativePosition.y, relativePosition.x);
    }

    protected Vector2 GetWeaponPosition() {
        float x = transform.position.x + Mathf.Cos(weaponAngle) * rotationRadius;
        float y = transform.position.y + Mathf.Sin(weaponAngle) * rotationRadius;

        return new Vector2(x, y);
    }

    protected Quaternion GetWeaponRotation() {
        return Quaternion.AngleAxis(weaponAngle * Mathf.Rad2Deg, Vector3.forward);
    }

    #endregion Maths
}