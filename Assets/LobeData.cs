using UnityEngine;

public class LobeData : MonoBehaviour
{
    [Header("Information")]
    public string lobeName = "Brain Part";
    [TextArea(3, 5)] 
    public string lobeDescription = "Description goes here.";

    [Header("Media")]
    public AudioSource audioSource;
    public ParticleSystem sparks;
    
    private Renderer meshRenderer;
    private Color originalColor;

    void Awake()
    {
        meshRenderer = GetComponentInChildren<Renderer>();
        if (meshRenderer != null) originalColor = meshRenderer.material.color;
    }

    public void SelectLobe()
    {
        if (meshRenderer != null) meshRenderer.material.color = new Color(0.2f, 0.8f, 1f, 1f); 
        if (sparks != null) sparks.Play();
        Handheld.Vibrate();
    }

    public void DeselectLobe()
    {
        if (meshRenderer != null) meshRenderer.material.color = originalColor;
        if (audioSource != null) audioSource.Stop();
    }

    public void ToggleAudio()
    {
        if (audioSource == null) return;
        if (audioSource.isPlaying) audioSource.Stop();
        else audioSource.Play();
    }
}