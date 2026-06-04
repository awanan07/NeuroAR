using UnityEngine;
using UnityEngine.SceneManagement;

public class AppFlowManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomeScreen;
    public GameObject scanningScreen;
    public GameObject interactionScreen;

    [Header("System References")]
    public ARPlacement arPlacement;
    public BrainTapManager tapManager;

    // Global variables ensure absolute mathematical state synchronization across all structures
    private bool isGlobalAudioOn = true;
    private bool isGlobalTextOn = true;

    void Start()
    {
        welcomeScreen.SetActive(true);
        scanningScreen.SetActive(false);
        interactionScreen.SetActive(false);
        arPlacement.canPlace = false;
        tapManager.canTap = false;
    }

    public void TriggerScanningState()
    {
        welcomeScreen.SetActive(false);
        scanningScreen.SetActive(true);
        arPlacement.canPlace = true;
    }

    public void ShowInteractionUI()
    {
        scanningScreen.SetActive(false);
        interactionScreen.SetActive(true);
        arPlacement.canPlace = false;
        tapManager.canTap = true;
    }

    public void ResetApp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExplodeButtonPressed()
    {
        FindObjectOfType<BrainInteraction>()?.ToggleExplodedView();
    }

    public void ToggleAudioMode()
    {
        isGlobalAudioOn = !isGlobalAudioOn;
        LobeData[] allLobes = FindObjectsOfType<LobeData>();
        foreach(LobeData lobe in allLobes)
        {
            lobe.audioModeActive = isGlobalAudioOn;
            if (!isGlobalAudioOn && lobe.audioSource != null && lobe.audioSource.isPlaying)
            {
                lobe.audioSource.Stop();
            }
        }
    }

    public void ToggleTextMode()
    {
        isGlobalTextOn = !isGlobalTextOn;
        LobeData[] allLobes = FindObjectsOfType<LobeData>();
        foreach(LobeData lobe in allLobes)
        {
            lobe.textModeActive = isGlobalTextOn;
            if (!isGlobalTextOn && lobe.textPanel != null) lobe.textPanel.SetActive(false);
        }
    }
}