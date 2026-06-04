using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class BrainTapManager : MonoBehaviour
{
    public bool canTap = false;
    public LobeData selectedLobe = null; 

    private Vector2 touchStartPos;
    private float tapThresholdPixels; 
    private Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        
        // Calculate physical DPI threshold (0.15 inches)
        float dpi = Screen.dpi;
        if (dpi == 0) dpi = 160f; // Fallback for editor or unknown devices
        tapThresholdPixels = 0.15f * dpi;
    }

    void Update()
    {
        if (!canTap) return;

        bool hasInput = false;
        Vector2 currentScreenPos = Vector2.zero;
        bool isBegan = false;
        bool isEnded = false;

        if (Touch.activeTouches.Count > 0)
        {
            var touch = Touch.activeTouches[0];
            hasInput = true;
            currentScreenPos = touch.screenPosition;
            isBegan = (touch.phase == TouchPhase.Began);
            isEnded = (touch.phase == TouchPhase.Ended);
        }
        else if (UnityEngine.InputSystem.Mouse.current != null)
        {
            var mouse = UnityEngine.InputSystem.Mouse.current;
            if (mouse.leftButton.wasPressedThisFrame || mouse.leftButton.wasReleasedThisFrame || mouse.leftButton.isPressed)
            {
                hasInput = true;
                currentScreenPos = mouse.position.ReadValue();
                isBegan = mouse.leftButton.wasPressedThisFrame;
                isEnded = mouse.leftButton.wasReleasedThisFrame;
            }
        }

        if (!hasInput) return;

        if (isBegan)
        {
            touchStartPos = currentScreenPos;
        }
        else if (isEnded)
        {
            float swipeDist = (currentScreenPos - touchStartPos).magnitude;

            if (swipeDist < tapThresholdPixels) 
            {
                ProcessTap(currentScreenPos);
            }
        }
    }

    private void ProcessTap(Vector2 screenPos)
    {
        Ray ray = mainCam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out LobeData tappedLobe))
            {
                if (selectedLobe != null && selectedLobe != tappedLobe)
                {
                    selectedLobe.DeselectLobe();
                }

                selectedLobe = tappedLobe;
                selectedLobe.SelectLobe();
                
                // Sync the 2D UI with the newly tapped lobe
                FindFirstObjectByType<AppFlowManager>()?.UpdateTextIfOpen();
            }
        }
    }
}