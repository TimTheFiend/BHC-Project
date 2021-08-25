using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEntity : MonoBehaviour
{
    public float projectileSpeed = 2.5f;
    public ProjectileFlags projectileFlags;

    private void OnTriggerEnter2D(Collider2D collision) {
        print(collision.gameObject.tag);

        OnHit();
    }

    private void OnHit() {
        // Check to see if destroy self.
        if (projectileFlags.HasFlag(ProjectileFlags.Piercing)) {
            return;
        }
        Destroy(gameObject);
    }

    public void ChangeFlags(ProjectileFlags flag, bool state) {
        switch (state) {
            case true:
                projectileFlags |= flag;
                return;

            case false:
                projectileFlags &= flag;
                return;
        }
    }
}

[System.Flags]
public enum ProjectileFlags
{
    None = 0,
    Piercing = 1,
    Etheral = 2
}