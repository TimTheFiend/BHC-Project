using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MovingObject
{
    //public void OnMovement(InputAction.CallbackContext _context) {
    //    UpdateMoveDirection(_context.ReadValue<Vector2>());


    //Dash cooldown UI
    protected override IEnumerator DashCooldown() {
        UnityEngine.UI.Slider slider = GetComponent<PlayerController>().uiEnergi;

        float time = 0f;
        while (time < dashCooldown) {
            time += Time.deltaTime;
            slider.value = (time / dashCooldown) * 100;
            yield return new WaitForEndOfFrame();
        }

        isDashing = false;
    }
}