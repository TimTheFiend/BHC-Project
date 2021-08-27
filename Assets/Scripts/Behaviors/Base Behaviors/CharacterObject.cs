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

    public void LoseHealth(float healthLost) {
        currentHP -= healthLost;

        if(isDead) {
            Die();
        }
    }

    protected virtual void Die() {
        Destroy(gameObject);
    }

    public void RecoverHealth(float healthRecovered) {
        currentHP += healthRecovered;
        if(currentHP > maxHP) {
            currentHP = maxHP;
        }
    }
}
