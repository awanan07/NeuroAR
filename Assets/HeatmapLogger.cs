using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class HeatmapLogger : MonoBehaviour
{
    private string filePath;
    private static bool isWriting = false;

    void Start()
    {
        filePath = Application.persistentDataPath + "/heatmap_data.csv";
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "Norm_X,Norm_Y,Time\n");
    }

    void Update()
    {
        if (Touch.activeTouches.Count > 0)
        {
            var activeTouch = Touch.activeTouches[0];
            if (activeTouch.phase == TouchPhase.Began)
            {
                // Mathematically normalize coordinates (0.0 to 1.0) so data matches across all phone sizes
                Vector2 touchPos = activeTouch.screenPosition;
                float normalizedX = touchPos.x / Screen.width;
                float normalizedY = touchPos.y / Screen.height;

                string data = $"{normalizedX},{normalizedY},{Time.time}\n";
                WriteDataAsync(data);
            }
        }
    }

    private async void WriteDataAsync(string data)
    {
        // Simple async lock to avoid concurrent file access conflicts without blocking main thread
        while (isWriting) await Task.Yield();
        
        isWriting = true;
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                await writer.WriteAsync(data);
            }
        }
        finally
        {
            isWriting = false;
        }
    }
}