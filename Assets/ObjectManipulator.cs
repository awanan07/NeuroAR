using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        // 2 Fingers to Scale (Pinch) - Allows pulling closer on Z-axis
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(t1.position, t2.position);
                initialScale = transform.localScale;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(t1.position, t2.position);
                if (Mathf.Approximately(initialDistance, 0)) return;
                transform.localScale = initialScale * (currentDistance / initialDistance);
            }
        }
        
        // 1 Finger swipe to Rotate
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
            transform.Rotate(0, -touchDelta.x * 0.2f, 0, Space.World);
        }
    }
}
