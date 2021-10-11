using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyMovementController : MovingObject
{
    public bool enableAutoMove = false;
    [Range(2f, 10f)] public float autoMoveTimer;


    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        StartCoroutine(AutoMoveCoroutine());
    }

    public IEnumerator AutoMoveCoroutine() {
        while(true) {
            if(enableAutoMove) {
                Vector2 newDirection = RandomDirection();
                UpdateMoveDirection(newDirection);
            }
            float time = 0f;
            while(time < autoMoveTimer) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            break;
        }
    }


    public Vector2 RandomDirection() {
        float min = -1;
        float max = 1;

        return new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));
        //return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

}