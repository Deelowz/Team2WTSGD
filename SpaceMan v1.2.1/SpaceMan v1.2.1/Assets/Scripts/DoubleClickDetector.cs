using UnityEngine;

public class DoubleClickDetector : MonoBehaviour
{
    private float doubleClickTime = 0.2f; // Adjust this time as needed.
    private float lastClickTime = -1f;
    private bool isDoubleClick = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float currentTime = Time.time;

            if (currentTime - lastClickTime <= doubleClickTime)
            {
                // This is a double click.
                isDoubleClick = true;
                Debug.Log("Double Click Detected");
            }
            else
            {
                // This is a single click.
                isDoubleClick = false;
            }

            lastClickTime = currentTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (!isDoubleClick)
            {
                // Handle a single Spacebar press.
                Debug.Log("Single Click Detected");
            }
        }
    }
}
