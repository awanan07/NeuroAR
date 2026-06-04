using UnityEngine;
using System.Collections;

public class BrainInteraction : MonoBehaviour
{
    public Transform[] brainParts; 
    public float explosionDistance = 0.5f; 
    private bool isExploded = false;
    private Vector3[] originalPositions;

    void Start()
    {
        originalPositions = new Vector3[brainParts.Length];
        for (int i = 0; i < brainParts.Length; i++)
        {
            originalPositions[i] = brainParts[i].localPosition;
        }
    }

    public void ToggleExplodedView()
    {
        isExploded = !isExploded;
        for (int i = 0; i < brainParts.Length; i++)
        {
            Vector3 direction = brainParts[i].localPosition.normalized;
            Vector3 targetPos = isExploded ? originalPositions[i] + (direction * explosionDistance) : originalPositions[i];
            StartCoroutine(SmoothMove(brainParts[i], targetPos));
        }
        Handheld.Vibrate(); // Adds Tactile Modality 
    }

    IEnumerator SmoothMove(Transform part, Vector3 target)
    {
        float timeElapsed = 0;
        Vector3 startPos = part.localPosition;
        while (timeElapsed < 1.0f) 
        {
            // Mathematical Smooth-Step implementation (t^2(3-2t)) for organic floating effect
            float t = timeElapsed / 1.0f;
            float smoothStep = t * t * (3f - 2f * t); 
            
            part.localPosition = Vector3.Lerp(startPos, target, smoothStep);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        part.localPosition = target;
    }
}
