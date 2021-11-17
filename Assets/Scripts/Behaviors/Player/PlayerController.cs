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

        DontDestroyOnLoad(gameObject);
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

    public void OpenMinimap(InputAction.CallbackContext _context) {
        switch (_context.phase) {
            case InputActionPhase.Started:
                UIManager.instance.ToggleMap(true);
                break;

            case InputActionPhase.Canceled:
                UIManager.instance.ToggleMap(false);
                break;
        }
    }

    public override void LoseHealth(float healthLost) {
        base.LoseHealth(healthLost);
        UIManager.instance.UpdateHPBar = currentHP / maxHP * 100;
        Debug.Log(currentHP / maxHP * 100);
    }
}