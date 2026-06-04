using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacement : MonoBehaviour
{
    public GameObject brainPrefab;
    private GameObject spawnedBrain;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public bool canPlace = false; 

    void Awake()
    {
        // Safely connecting the manager inside the brackets
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // If the UI hasn't given permission, or the brain is already spawned, stop here.
        if (!canPlace || spawnedBrain != null) return;

        bool isInputDetected = false;
        Vector2 inputPosition = Vector2.zero;

        // EnhancedTouch handles physical screen taps
        if (Touch.activeTouches.Count > 0)
        {
            var activeTouch = Touch.activeTouches[0];
            if (activeTouch.phase == TouchPhase.Began)
            {
                isInputDetected = true;
                inputPosition = activeTouch.screenPosition;
            }
        }
        // Explicit fallback for Unity Editor Mouse clicks
        else if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            isInputDetected = true;
            inputPosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }

        // 3. Fire the Raycast
        if (isInputDetected)
        {
            // Diagnostic: Vibrate the phone immediately so the tester knows the tap registered
            Handheld.Vibrate();

            // Relaxed the strict polygon requirement to "Planes" for easier mobile tapping
            if (arRaycastManager.Raycast(inputPosition, hits, TrackableType.Planes))
            {
                var hitPose = hits[0].pose;
                spawnedBrain = Instantiate(brainPrefab, hitPose.position, hitPose.rotation);
                
                // Switch the UI to the Interaction Dashboard
                FindFirstObjectByType<AppFlowManager>()?.ShowInteractionUI();
            }
        }
    }
}