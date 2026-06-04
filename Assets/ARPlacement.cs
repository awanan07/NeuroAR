using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlacement : MonoBehaviour
{
    public GameObject brainPrefab; 
    private GameObject spawnedBrain;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public bool canPlace = false; // Controlled mathematically by our UI State Machine

    void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (!canPlace || spawnedBrain != null) return; 

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (arRaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;
                spawnedBrain = Instantiate(brainPrefab, hitPose.position, hitPose.rotation);
                
                // Trigger the UI State Machine to transition to Interaction Mode
                FindObjectOfType<AppFlowManager>()?.ShowInteractionUI();
            }
        }
    }
}
