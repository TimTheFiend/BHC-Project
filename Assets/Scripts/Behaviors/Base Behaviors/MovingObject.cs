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
        rbody.MovePosition(rbody.position + newPosition * movementSpeed * Time.deltaTime);
        //rbody.velocity = newPosition * movementSpeed * Time.deltaTime;
    }

    public void UpdateMoveDirection(Vector2 newDirection) {
        moveDirection = newDirection;
    }
}