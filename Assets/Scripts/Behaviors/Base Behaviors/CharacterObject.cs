using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;

    protected virtual void Start() {
        currentHP = maxHP;
    }

    /// <summary>
    /// Checks if the object is dead.
    /// </summary>
    public bool isDead {
        get {
            return currentHP <= 0;
        }
    }

    /// <summary>
    /// Removes hp from the object, and checks if the object dies as a result.
    /// </summary>
    /// <param name="healthLost">The amount of damage received.</param>
    public virtual void LoseHealth(float healthLost) {
        currentHP -= healthLost;
        if (isDead) {
            Die();
        }
    }

    /// <summary>
    /// Handles what happens on object death.
    /// </summary>
    protected virtual void Die() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Adds hp to the object, and ensures it doesn't go above maxHP.
    /// </summary>
    /// <param name="healthRecovered">Amount of hp.</param>
    public void RecoverHealth(float healthRecovered) {
        currentHP += healthRecovered;
        if (currentHP > maxHP) {
            currentHP = maxHP;
        }
    }

    /// <summary>
    /// Regaining your health overtime
    /// </summary>
    /// <param name="totalAmount">How much you will heal in total</param>
    /// <param name="totalDuration">How long it will take/be active</param>
    /// <param name="totalTicks">how many times you will get health</param>
    /// <returns></returns>
    public IEnumerator RecoverHealthOvertime(float totalAmount, float totalDuration, int totalTicks) {
        //timeUntilHeal is to calculate how long it will take to gaining the total health
        float timeUntilHeal = totalDuration / totalTicks;
        for (int i = 0; i < totalTicks; i++) {
            yield return new WaitForSeconds(timeUntilHeal);

            RecoverHealth(totalAmount / totalTicks);
        }
    }

    /// <summary>
    /// Losing your health from effects overtime
    /// </summary>
    /// <param name="totalAmount">How much dmg you will lose in total</param>
    /// <param name="totalDuration">How long it will take/be active</param>
    /// <param name="totalTicks">how many times you will take dmg</param>
    /// <returns></returns>
    public IEnumerator LoseHealthOvertime(float totalAmount, float totalDuration, int totalTicks) {
        //timeUntilDamaged is to calculate how long will take to take the total dmg
        float timeUntilDamaged = totalDuration / totalTicks;
        for (int i = 0; i < totalTicks; i++) {
            yield return new WaitForSeconds(timeUntilDamaged);

            LoseHealth(totalAmount / totalTicks);
        }
    }
}