using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileEntity : MonoBehaviour {
    //public float projectileSpeed = 2.5f;
    public float damage = 25f;
    public LayerMask layer;

    private const float MAXSPEED = 8f;

    // TODO 
    [SerializeField]private float projectileSpeed;
    

    public float ProjectileSpeed {
        get { return projectileSpeed; }
        set {
            if(projectileSpeed + value > MAXSPEED) {
                print("MAX SPEED");
                projectileSpeed = MAXSPEED;
            }
            else {
                projectileSpeed += value;

            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    print("HEWEE");
    //    if(collision.gameObject.layer == layer) {
    //        if(collision.gameObject.tag == "Enemy") {
    //            collision.gameObject.GetComponent<CharacterObject>().LoseHealth(damage);
    //        }
    //        OnHit();
    //    }
    //}


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<CharacterObject>().LoseHealth(damage);
        }
        OnHit();
    }

    private void OnHit() {
        Destroy(gameObject);
    }
}
