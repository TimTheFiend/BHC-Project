using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingObject : MonoBehaviour
{
    [Header("Component references")]
    [SerializeField] protected Rigidbody2D rbody;

    [Header("Movement vars")]
    [Range(1.0f, 10f)] public float movementSpeed = 5f;
    [Range(1.0f, 10f)] public float dashSpeed = 5f;
    [Range(0f, 1.0f)] public float dashLengthInSeconds = 0.5f;
    [Range(0.5f, 5.0f)] public float cooldownDash = 2.5f;
    public bool canMove = true;  //Determines if the moveing object can actively move
    public bool isDashing = false;


    public Vector2 moveDirection;

    protected virtual void Start() {
        rbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate() {
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
        //rbody.velocity = newPosition * movementSpeed * Time.deltaTime;
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
    /// Looking at the way you want to dash, and dashes
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Dash() {
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

    //Dash cooldown UI
    protected virtual IEnumerator DashCooldown() {
        float time = 0f;
        while (time < cooldownDash) {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isDashing = false;
    }
}