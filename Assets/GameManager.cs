using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable(); // Allows mouse to simulate touch in the editor
    }

    void Start()
    {
        Application.targetFrameRate = 45;
    }
}