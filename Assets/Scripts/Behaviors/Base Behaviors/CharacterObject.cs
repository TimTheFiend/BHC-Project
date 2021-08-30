using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour
{
    public float maxHP = 100f;
    public float currentHP;

    private void Start() {
        currentHP = maxHP;
    }

    public bool isDead {
        get {
            return currentHP <= 0;
        }
    }

    public void LoseHealth(float damageTaken) {
        //float damageResult = maxHP - damageTaken;
        currentHP -= damageTaken;

        if(isDead) {
            Die();
        }
    }

    private void Die() {
        throw new NotImplementedException();

    }

    public void RecoverHealth(float healthRecovered) {
        currentHP += healthRecovered;
        if(currentHP > maxHP) {
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
    public IEnumerator RecoverHealthOvertime(float totalAmount, float totalDuration, int totalTicks){
        //timeUntilHeal is to calculate how long it will take to gaining the total health
        float timeUntilHeal = totalDuration / totalTicks;
        for (int i = 0; i < totalTicks; i++){
            yield return new WaitForSeconds(timeUntilHeal);

            RecoverHealth(totalAmount / totalTicks);
        }
    }

    /// <summary>
    /// Loseing your health from effects overtime
    /// </summary>
    /// <param name="totalAmount">How much dmg you will lose in total</param>
    /// <param name="totalDuration">How long it will take/be active</param>
    /// <param name="totalTicks">how many times you will take dmg</param>
    /// <returns></returns>
    public IEnumerator LoseHealthOvertime(float totalAmount, float totalDuration, int totalTicks){
        //timeUntilDamaged is to calculate how long will take to take the total dmg
        float timeUntilDamaged = totalDuration / totalTicks;
        for (int i = 0; i < totalTicks; i++) {
            yield return new WaitForSeconds(timeUntilDamaged);

            LoseHealth(totalAmount / totalTicks);
        }
        

    }
}