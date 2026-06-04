using UnityEngine;

public class BrainTapManager : MonoBehaviour
{
    public bool canTap = false; 
    private Vector2 touchStartPos;
    private float tapThreshold = 20f; // Math logic: If the finger moved >20 pixels, it's a rotation swipe, NOT a tap.

    void Update()
    {
        if (!canTap || Input.touchCount != 1) return;
        
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            touchStartPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            // Calculates the mathematical vector distance between touch start and end
            if (Vector2.Distance(touchStartPos, touch.position) < tapThreshold)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    hit.collider.GetComponent<LobeData>()?.OnLobeTapped();
                }
            }
        }
    }
}
