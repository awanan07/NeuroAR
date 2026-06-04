using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems; 
using TMPro; 

public class AppFlowManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject welcomeScreen;
    public GameObject scanningScreen;
    public GameObject interactionScreen;

    [Header("2D Information UI")]
    public GameObject infoPanel2D;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("System References")]
    public ARPlacement arPlacement;
    public BrainTapManager tapManager;
    public ARPlaneManager planeManager;

    void Start()
    {
        // Initial setup
        welcomeScreen.SetActive(true);
        scanningScreen.SetActive(false);
        interactionScreen.SetActive(false);
        if (infoPanel2D != null) infoPanel2D.SetActive(false);
        
        // Ensure alpha is set correctly initially
        UIAnimator.Instance.FadeIn(welcomeScreen, 0f); // Instant fade in for setup
        
        arPlacement.canPlace = false;
        tapManager.canTap = false;

        if (planeManager == null) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.requestedDetectionMode = PlaneDetectionMode.None;
            HideAllPlanes();
        }
    }

    public void TriggerScanningState()
    {
        UIAnimator.Instance.FadeOut(welcomeScreen);
        UIAnimator.Instance.FadeIn(scanningScreen);
        UIAnimator.Instance.StartPulse(scanningScreen, 1.03f, 2f); // Gentle pulse effect
        
        arPlacement.canPlace = true;

        if (planeManager == null) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.enabled = true;
            planeManager.requestedDetectionMode = PlaneDetectionMode.Horizontal | PlaneDetectionMode.Vertical;
        }
    }

    public void ShowInteractionUI()
    {
        UIAnimator.Instance.StopPulse(scanningScreen);
        UIAnimator.Instance.FadeOut(scanningScreen);
        UIAnimator.Instance.FadeIn(interactionScreen);
        
        arPlacement.canPlace = false;
        tapManager.canTap = true;

        if (planeManager == null) planeManager = FindFirstObjectByType<ARPlaneManager>();
        if (planeManager != null)
        {
            planeManager.requestedDetectionMode = PlaneDetectionMode.None;
            planeManager.enabled = false;
            foreach (var plane in planeManager.trackables) Destroy(plane.gameObject);
        }

        ARPlaneMeshVisualizer[] roguePlanes = FindObjectsByType<ARPlaneMeshVisualizer>(FindObjectsSortMode.None);
        foreach (var visualizer in roguePlanes) Destroy(visualizer.gameObject);
    }

    private void HideAllPlanes()
    {
        ARPlaneMeshVisualizer[] allVisualizers = FindObjectsByType<ARPlaneMeshVisualizer>(FindObjectsSortMode.None);
        foreach (var visualizer in allVisualizers) visualizer.gameObject.SetActive(false);
    }

    public void ResetApp()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // --- LOCKED UI BUTTON LOGIC ---

    public void ToggleAudioMode()
    {
        if (tapManager != null && tapManager.selectedLobe != null)
        {
            tapManager.selectedLobe.ToggleAudio();
        }
    }

    public void ToggleTextMode()
    {
        if (tapManager != null && tapManager.selectedLobe != null)
        {
            if (infoPanel2D.activeSelf)
            {
                UIAnimator.Instance.FadeOut(infoPanel2D, 0.2f);
            }
            else
            {
                UIAnimator.Instance.PopIn(infoPanel2D);
                UpdateTextIfOpen();
            }
        }
    }

    public void UpdateTextIfOpen()
    {
        if (infoPanel2D.activeSelf && tapManager.selectedLobe != null)
        {
            titleText.text = tapManager.selectedLobe.lobeName;
            descriptionText.text = tapManager.selectedLobe.lobeDescription;
            
            // Pop it in when text updates to draw attention
            UIAnimator.Instance.PopIn(infoPanel2D, 0.25f);
        }
        else if (tapManager.selectedLobe == null)
        {
            UIAnimator.Instance.FadeOut(infoPanel2D, 0.2f);
        }
    }
}