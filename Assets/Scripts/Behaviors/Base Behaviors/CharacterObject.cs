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

    void LoseHealth(float damageTaken) {
        //float damageResult = maxHP - damageTaken;
        currentHP -= damageTaken;

        if(isDead) {
            die();
        }
    }

    private void die() {
        throw new NotImplementedException();

    }

    void RecoverHealth(float healthRecovered) {
        currentHP += healthRecovered;
        if(currentHP > maxHP) {
            currentHP = maxHP;
        }
    }
}
