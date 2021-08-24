using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEntity : MonoBehaviour
{
    public float projectileSpeed = 2.5f;


    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.gameObject.tag);

        OnHit();
    }

    

    private void OnHit() {
        Destroy(gameObject);
    }
}
