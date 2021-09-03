using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerMovementController pMovement;
    public PlayerAttackController pAttack;
    public Slider uiEnergi;

    public float maxEnergy = 1f;
    public float currentEnergy;

    private void Start() {
        pMovement = GetComponent<PlayerMovementController>();
        pAttack = GetComponent<PlayerAttackController>();

        uiEnergi = FindObjectOfType<Slider>();
    }

    public void OnMovement(InputAction.CallbackContext _context) {
        pMovement.UpdateMoveDirection(_context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext _context) {
        if(!_context.canceled) {
            pAttack.AttemptAttack();
        }
    }

    

    public void OnDash(InputAction.CallbackContext _context) {
        pMovement.AttemptDash();

    }
}
