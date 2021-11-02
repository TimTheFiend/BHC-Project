using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementController : MovingObject
{
    public bool enableAutoMove = false;
    public bool isHittingWall = false;
    private List<Vector2> availableDirections = new List<Vector2>();
    private List<Vector2> directions = new List<Vector2>();
    private int wallLayer;
    private float laserLength;

    [Range(2f, 10f)] public float autoMoveTimer;

    public bool isMovingCurrently = false;

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

        #endregion adds directions to directions list

        wallLayer = LayerMask.GetMask("Walls");

        StartCoroutine(AutoMoveCoroutine());
    }

    public IEnumerator AutoMoveCoroutine() {
        while (true) {
            Vector2 newDirection = RandomDirection();

            // if isHittingWall is set to false
            if (!isHittingWall) {
                // set newDirection to a random direction
                newDirection = RandomDirection();
            }
            // if isHittingWall is set to false
            else {
                // set newDirection to a random direction within a range of available directions
                newDirection = RandomAvailableDirection();
            }

            UpdateMoveDirection(newDirection);
            float time = 0f;
            while (time < autoMoveTimer) {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public IEnumerator AutoMoveRoutine() {
        isMovingCurrently = true;

        Vector2 newDirection = RandomDirection();

        // if isHittingWall is set to false
        if (!isHittingWall) {
            // set newDirection to a random direction
            newDirection = RandomDirection();
        }
        // if isHittingWall is set to false
        else {
            // set newDirection to a random direction within a range of available directions
            newDirection = RandomAvailableDirection();
        }

        UpdateMoveDirection(newDirection);
        float time = 0f;
        while (time < autoMoveTimer) {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isMovingCurrently = false;
    }

    //public IEnumerator AutoMoveCoroutine() {
    //    while (true) {
    //        if (enableAutoMove) {
    //            Vector2 newDirection = RandomDirection();

    //            // if isHittingWall is set to false
    //            if (!isHittingWall) {
    //                // set newDirection to a random direction
    //                newDirection = RandomDirection();
    //            }
    //            // if isHittingWall is set to false
    //            else {
    //                // set newDirection to a random direction within a range of available directions
    //                newDirection = RandomAvailableDirection();
    //            }

    //            UpdateMoveDirection(newDirection);
    //        }
    //        float time = 0f;
    //        while (time < autoMoveTimer) {
    //            time += Time.deltaTime;
    //            yield return new WaitForEndOfFrame();
    //        }
    //    }
    //}

    // is a completely random direction
    public Vector2 RandomDirection() {
        float min = -1;
        float max = 1;

        return new Vector2(Random.Range((int)min, (int)max + 1), Random.Range((int)min, (int)max + 1));
    }

    /// is a random direction within a range of available directions
    /// TODO
    /// i stedet for at v�lge �n af de mindst 1 retninger, v�lg en retning som er tilf�ldigt valgt
    /// fx, hvis alle h�jreretninger er blokeret(op og til h�jre(1, 1), h�jre(1, 0), ned og til h�jre(1, -1)),
    /// i stedet for KUN at g� op (0, 1), op og til venstre(-1, 1), venstre(-1, 0), ned og til venstre (-1, -1) eller ned(0, -1). v�lg en float fra op (0, 1) til ned (0, -1)
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
        for (int i = 0; i < directions.Count; i++) {
            // cardinal directions
            if ((directions[i].x != 0 && directions[i].y == 0) || (directions[i].x == 0 && directions[i].y != 0)) {
                ///
                ///the distance from one point to the same point in an adjacent square of the same size is different depending on if you're moving diagonally or in a cardinal direction.
                ///a square in a cardinal direction is 0.7 distance away, a square in a diagonal direction is 1 distance away.
                ///in this example, we want the distance from the middle point to an adjacent diagonal square to be 2 distance away, ie. twice of 1, so 0.7 is doubled to 1.4.
                /// an extra 0.01 is added to 1.4 to make 1.41 to make sure that the cardinal rayline hits the same point if standing next to an obstacle
                ///(for some reason if the 0.01 isn't added, the diagonal rayline triggers before the cardinal one)
                ///
                laserLength = 1.41f;
            }
            // diagonal directions
            else {
                laserLength = 2f;
            }

            Debug.DrawRay(transform.position, directions[i] * laserLength, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], laserLength, wallLayer);

            // if the collider hits something (can only detect walls)
            if (hit.collider == true) {
                Debug.DrawRay(transform.position, directions[i] * laserLength, Color.red);

                // if the list of available directions already contains the current direction
                if (availableDirections.Contains(directions[i])) {
                    // remove this direction as an available direction, as it is not available anymore
                    availableDirections.Remove(directions[i]);
                }
                isHittingWall = true;
            }
            // if the collider does not hit something
            else {
                // if the list of available directions does not contain the current direction
                if (availableDirections.Contains(directions[i]) == false) {
                    // add that direction to the list of available directions
                    availableDirections.Add(directions[i]);
                }
                isHittingWall = false;
            }
        }
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        CheckForWallHit();
        //if (isMovingCurrently == false) {
        //    StartCoroutine(AutoMoveRoutine());
        //}
    }
}