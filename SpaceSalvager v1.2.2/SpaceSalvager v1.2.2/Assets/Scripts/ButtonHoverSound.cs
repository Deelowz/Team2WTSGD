//////////////////////////////////////////////////////
// Assignment/Lab/Project: 2D Game
//Name: Ahmed Suoamra
//Section: 2023SU.SGD.113.0073
//Instructor: George Cox
// Date: 10/17/2023
//////////////////////////////////////////////////////
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Color m_MouseOverColor = Color.red;

    //This stores the GameObject’s original color
    Color m_OriginalColor;

    //Get the GameObject’s mesh renderer to access the GameObject’s material and color
    MeshRenderer m_Renderer;
    public AudioClip hoverSound;
    public AudioClip clickSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //Fetch the mesh renderer component from the GameObject
        m_Renderer = GetComponent<MeshRenderer>();
        //Fetch the original color of the GameObject
        m_OriginalColor = m_Renderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
            m_Renderer.material.color = m_MouseOverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_Renderer.material.color = m_OriginalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSound != null)
        {
            if (gameObject.activeInHierarchy) 
            {
                StartCoroutine(PlayClickSoundWithDelay());
            }
        }
    }

    private System.Collections.IEnumerator PlayClickSoundWithDelay()
    {
        audioSource.enabled = true;
        audioSource.PlayOneShot(clickSound);
        yield return new WaitForSeconds(clickSound.length);
        audioSource.enabled = false;
    }
}
