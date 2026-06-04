using System.IO;
using UnityEngine;

public class HeatmapLogger : MonoBehaviour
{
    private string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/heatmap_data.csv";
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "Norm_X,Norm_Y,Time\n");
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Mathematically normalize coordinates (0.0 to 1.0) so data matches across all phone sizes
            Vector2 touchPos = Input.GetTouch(0).position;
            float normalizedX = touchPos.x / Screen.width;
            float normalizedY = touchPos.y / Screen.height;

            string data = $"{normalizedX},{normalizedY},{Time.time}\n";
            File.AppendAllText(filePath, data);
        }
    }
}