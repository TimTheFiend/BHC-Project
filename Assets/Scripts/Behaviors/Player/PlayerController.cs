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

    /// <summary>
    /// Handles Movement inputs.
    /// </summary>
    /// <param name="_context"></param>
    public void OnMovement(InputAction.CallbackContext _context) {
        pMovement.UpdateMoveDirection(_context.ReadValue<Vector2>());
    }

    /// <summary>
    /// Handles attack inputs.
    /// </summary>
    /// <param name="_context"></param>
    public void OnAttack(InputAction.CallbackContext _context) {
        if (!_context.canceled) {
            pAttack.AttemptAttack();
        }
    }

    /// <summary>
    /// Handles Dashing inputs.
    /// </summary>
    /// <param name="_context"></param>
    public void OnDash(InputAction.CallbackContext _context) {
        pMovement.AttemptDash();
    }

    /// <summary>
    /// Handles minimap UI input.
    /// </summary>
    /// <param name="_context"></param>
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

    /// <summary>
    /// Updates the player's health, and the UI.
    /// </summary>
    /// <param name="healthLost"></param>
    public override void LoseHealth(float healthLost) {
        base.LoseHealth(healthLost);
        UIManager.instance.UpdateHPBar = currentHP / maxHP * 100;
        Debug.Log(currentHP / maxHP * 100);
    }
}