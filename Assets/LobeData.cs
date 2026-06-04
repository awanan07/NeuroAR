using UnityEngine;

public class LobeData : MonoBehaviour
{
    public GameObject textPanel; 
    public AudioSource audioSource; 
    public ParticleSystem sparks; 

    public bool audioModeActive = true;
    public bool textModeActive = true;

    public void OnLobeTapped()
    {
        Handheld.Vibrate();
        
        if (textPanel != null) {
            textPanel.SetActive(textModeActive);
            textPanel.transform.LookAt(textPanel.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
        
        if (audioModeActive && audioSource != null) audioSource.Play();
        if (sparks != null) sparks.Play();
    }
}
