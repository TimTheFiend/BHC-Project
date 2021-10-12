using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class EnemyMovementController : MovingObject
{
    public bool enableAutoMove = false;
    public bool isHittingWall = false;
    private List<Vector2> availableDirections = new List<Vector2>();
    private List<Vector2> directions = new List<Vector2>();
    private int wallLayer;
    private float laserLength;


    [Range(2f, 10f)] public float autoMoveTimer;


    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        #region adds directions to directions list

        var upRightAngle = new Vector2(1f, 1f).normalized;
        var upLeftAngle = new Vector2(-1f, 1f).normalized;
        var downRightAngle = new Vector2(1f, -1f).normalized;
        var downLeftAngle = new Vector2(-1f, -1f).normalized;

        var upAngle = new Vector2(0f, 1f).normalized;
        var rightAngle = new Vector2(1f, 0f).normalized;
        var downAngle = new Vector2(0, -1f).normalized;
        var leftAngle = new Vector2(-1f, 0f).normalized;

        directions.Add(upAngle);
        directions.Add(upRightAngle);
        directions.Add(rightAngle);
        directions.Add(downRightAngle);
        directions.Add(downAngle);
        directions.Add(downLeftAngle);
        directions.Add(leftAngle);
        directions.Add(upLeftAngle);

        #endregion
        wallLayer = LayerMask.GetMask("Walls");

        StartCoroutine(AutoMoveCoroutine());
    }

    public IEnumerator AutoMoveCoroutine() {
        while(true) {
            if(enableAutoMove) {

                Vector2 newDirection = RandomDirection();

                // if isHittingWall is set to false
                if(!isHittingWall) {
                    // set newDirection to a random direction
                    newDirection = RandomDirection();
                }
                // if isHittingWall is set to false
                else {
                    // set newDirection to a random direction within a range of available directions
                    newDirection = RandomAvailableDirection();

                }

                UpdateMoveDirection(newDirection);
            }
            float time = 0f;
            while(time < autoMoveTimer) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    // is a completely random direction
    public Vector2 RandomDirection() {
        float min = -1;
        float max = 1;

        return new Vector2(Random.Range(min, max), Random.Range(min, max));
    }

    // is a random direction within a range of available directions
    public Vector2 RandomAvailableDirection() {
        return availableDirections[Random.Range(0, availableDirections.Count)];
    }

    /// <summary>
    /// creates 8 raylines in different directions and checks if there are walls within the rayline.
    /// if there is a well within the rayline, set isHittingWall to true.
    /// if there isn't a wall within the rayline, set isHittingWall to false.
    /// isHittingWall is checked later.
    /// </summary>
    public void CheckForWallHit() {
        for(int i = 0; i < directions.Count; i++) {
            if((directions[i].x != 0 && directions[i].y == 0) || (directions[i].x == 0 && directions[i].y != 0)) {
                laserLength = 1.41f;
            }
            else {
                laserLength = 2f;
            }

            Debug.DrawRay(transform.position, directions[i] * laserLength, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], laserLength, wallLayer);

            // if the collider hits something (can only detect walls)
            if(hit.collider == true) {
                Debug.DrawRay(transform.position, directions[i] * laserLength, Color.red);

                // if the list of available directions already contains the current direction
                if(availableDirections.Contains(directions[i])) {
                    // remove this direction as an available direction, as it is not available anymore
                    availableDirections.Remove(directions[i]);
                }
                isHittingWall = true;

            }
            // if the collider does not hit something
            else {
                // if the list of available directions does not contain the current direction
                if(availableDirections.Contains(directions[i]) == false) {
                    // add that direction to the list of available directions
                    availableDirections.Add(directions[i]);
                }
                isHittingWall = false;
            }
        }
    }

    private void Update() {
        CheckForWallHit();
    }
}