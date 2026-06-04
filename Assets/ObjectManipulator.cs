using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class ObjectManipulator : MonoBehaviour
{
    private float initialDistance;
    private Vector3 initialScale;
    private float dpiRotationSensitivity;

    void Awake()
    {
        float dpi = Screen.dpi;
        if (dpi == 0) dpi = 160f; // Fallback
        
        // Original logic was 0.2f degrees per pixel.
        // We normalize this to degrees per physical inch.
        // Assuming original 0.2f felt good on a ~160 DPI screen (32 degrees per inch).
        dpiRotationSensitivity = 32.0f / dpi;
    }

    void Update()
    {
        var activeTouches = Touch.activeTouches;

        // 2 Fingers to Scale (Pinch) - Allows pulling closer on Z-axis
        if (activeTouches.Count == 2)
        {
            var t1 = activeTouches[0];
            var t2 = activeTouches[1];

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(t1.screenPosition, t2.screenPosition);
                initialScale = transform.localScale;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(t1.screenPosition, t2.screenPosition);
                if (Mathf.Approximately(initialDistance, 0)) return;
                transform.localScale = initialScale * (currentDistance / initialDistance);
            }
        }
        
        // 1 Finger swipe to Rotate
        if (activeTouches.Count == 1 && activeTouches[0].phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = activeTouches[0].delta;
            transform.Rotate(0, -touchDelta.x * dpiRotationSensitivity, 0, Space.World);
        }
        else if (activeTouches.Count == 0 && UnityEngine.InputSystem.Mouse.current != null)
        {
            var mouse = UnityEngine.InputSystem.Mouse.current;
            if (mouse.leftButton.isPressed)
            {
                Vector2 mouseDelta = mouse.delta.ReadValue();
                transform.Rotate(0, -mouseDelta.x * dpiRotationSensitivity, 0, Space.World);
            }
        }
    }
}
