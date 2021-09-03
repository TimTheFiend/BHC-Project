using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour
{
    [Header("Component references")]
    [SerializeField] private Rigidbody2D rbody;

    [Header("Movement vars")]
    [Range(1.0f, 10f)] public float movementSpeed = 5f;
    [Range(1.0f, 10f)] public float dashSpeed = 5f;
    [Range(0f, 1.0f)] public float dashLengthInSeconds = 0.5f;
    [Range(0.01f, 10f)] public float dashCooldown = 5f;
    public bool canMove = true;  //Determines if the player can actively move the GameObject
    public bool isDashing = false;  //Determines if the player can dash

    public Vector2 moveDirection;

    private void Start() {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        Move(moveDirection);
    }

    /// <summary>
    /// Moves the <see cref="MovingObject"/>'s position based on input.
    /// </summary>
    /// <param name="newPosition">New position.</param>
    protected virtual void Move(Vector2 newPosition) {
        if (!canMove) {
            return;
        }
        rbody.MovePosition(rbody.position + newPosition * movementSpeed * Time.deltaTime);
    }

    public void UpdateMoveDirection(Vector2 newDirection) {
        moveDirection = newDirection;
    }

    

    /// <summary>
    /// checking if you can dash
    /// </summary>
    public virtual void AttemptDash() {
        if (!isDashing && moveDirection != Vector2.zero) {
            StartCoroutine(Dash());
        }
    }

    /// <summary>
    /// Looking at the way u want to dash, and dashes
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Dash() {
        print("dash");

        //GetComponent<PlayerMovementController>().DashCooldownUI();

        Vector2 dashDir = moveDirection;
        canMove = false;
        isDashing = true;

        GetComponent<SpriteRenderer>().color = new Color(1f, 0, 0);

        float time = 0f;
        while (time < dashLengthInSeconds) {
            time += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3)dashDir, dashSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
        canMove = true;
        StartCoroutine(DashCooldown());
    }

    /// <summary>
    /// kigger på hvor lang en cooldown du har efter du har dashed
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator DashCooldown() {
        float time = 0f;
        while (time < dashCooldown) {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
       
        isDashing = false;
    }
}