using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MovingObject
{
    //public void OnMovement(InputAction.CallbackContext _context) {
    //    UpdateMoveDirection(_context.ReadValue<Vector2>());
    //}

    protected override IEnumerator DashCooldown() {
        float time = 0f;
        while (time < cooldownDash) {
            time += Time.deltaTime;
            UIManager.instance.UpdateEnergyBar = (time / cooldownDash) * 100;
            yield return new WaitForEndOfFrame();
        }

        isDashing = false;
    }
}