using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingPoint : MonoBehaviour
{
    public Transform playerTransform; // Assign the player's transform in the Inspector
    private Vector3 mousePos;

    private void Update()
    {
        RotateAimingPoint();
    }

    private void RotateAimingPoint()
    {
        // Get the mouse position in world coordinates
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure the same Z position as the aiming point

        // Calculate the direction from the player to the mouse position
        Vector3 aimDirection = (mousePos - playerTransform.position).normalized;

        // Calculate the angle in degrees
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Rotate the aiming point accordingly
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

