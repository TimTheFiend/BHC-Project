using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : CharacterObject
{
    public PlayerMovementController pMovement;
    public PlayerAttackController pAttack;
    public int coins = 0;

    //For moving between rooms
    public bool canUseDoors = true;  //True because the player starts in an empty room.

    private void Start() {
        pMovement = GetComponent<PlayerMovementController>();
        pAttack = GetComponent<PlayerAttackController>();

        //TODO fix
        currentHP = maxHP;
    }

    public void OnMovement(InputAction.CallbackContext _context) {
        pMovement.UpdateMoveDirection(_context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext _context) {
        if (!_context.canceled) {
            pAttack.AttemptAttack();
        }
    }

    public void OnDash(InputAction.CallbackContext _context) {
        pMovement.AttemptDash();
    }
}