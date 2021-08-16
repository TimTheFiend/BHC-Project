using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool canShoot = true;
    [Range(0.0f, 5.0f)] public float shootCooldown = 2.5f;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
    }

    public void OnMovement(InputAction.CallbackContext value) {
        Vector2 inputMovement = value.ReadValue<Vector2>();
        print(inputMovement);
    }

    public void OnShoot(InputAction.CallbackContext value) {
        print(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

        if (canShoot) {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot() {
        canShoot = false;

        print("pew pew");
        yield return new WaitForSeconds(shootCooldown);
        print("You can shoot again");
        canShoot = true;
    }
}