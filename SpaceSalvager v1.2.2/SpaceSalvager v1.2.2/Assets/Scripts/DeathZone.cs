using UnityEngine;
using System.Collections; // Add this using directive to resolve the IEnumerator issue.
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public float respawnDelay = 3.0f;
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RespawnAfterDelay(other.transform));
        }
    }

    private IEnumerator RespawnAfterDelay(Transform playerTransform)
    {
        // Optionally perform any other actions during the delay (e.g., player animation).
        // ...

        // Wait for the specified respawn delay.
        yield return new WaitForSeconds(respawnDelay);

        // Reset the player's position to the assigned respawn point.
         int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneBuildIndex);
    }
}
