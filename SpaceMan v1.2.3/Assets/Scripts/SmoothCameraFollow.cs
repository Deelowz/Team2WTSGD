using UnityEngine;
using Cinemachine;

public class SmoothCameraFollow : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Assign your player GameObject to this field.
    public CinemachineVirtualCamera virtualCamera; // Assign your Cinemachine Virtual Camera to this field.

    private void Start()
    {
        if (virtualCamera != null)
        {
            // Set the Virtual Camera's Follow and LookAt targets to your player's targets.
            virtualCamera.Follow = player;
            virtualCamera.LookAt = player;
        }
    }
}
