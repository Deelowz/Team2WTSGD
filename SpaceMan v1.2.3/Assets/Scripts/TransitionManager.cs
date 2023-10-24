using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public string targetSceneName;
    public Animator transitionAnimator;
    public float transitionDuration = 3f; // Specify the duration in seconds

    public void StartTransition()
    {
        StartCoroutine(LoadSceneWithTransition());
    }

    IEnumerator LoadSceneWithTransition()
    {
        // Trigger the transition animation if you have one.
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger("StartTransition");

            // Wait for the specified duration.
            yield return new WaitForSeconds(transitionDuration);
        }

        // Load the target scene.
        SceneManager.LoadScene(targetSceneName);
    }
}
