using UnityEngine;
using UnityEditor;

public class PlayerSettingsWindow : EditorWindow
{
    private float accelerationRate = 2.0f;
    private float decelerationRate = 2.0f;
    private float maxSpeed = 10.0f;
    private float maxNumber = 50.0f;

    [MenuItem("Window/Player Settings")]
    public static void ShowWindow()
    {
        GetWindow<PlayerSettingsWindow>("Player Settings");
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Movement Settings", EditorStyles.boldLabel);

        // Draw the graph
        DrawGraph();

        // Update PlayerScript with new values
        if (GUILayout.Button("Apply to Player"))
        {
            ApplySettingsToPlayer();
        }
    }

    private void DrawGraph()
    {
        Rect graphRect = GUILayoutUtility.GetRect(300, 300);
        EditorGUI.DrawRect(graphRect, Color.gray);

        // Calculate scaling factor for the graph
        float baseScale = 5.0f;
        float adjustmentFactor = 10.0f;
        float scaleFactor = baseScale / (1.0f + maxNumber / adjustmentFactor);

        // Calculate horizontal positions based on the ratio to maxNumber
        float accelerationXPosition = graphRect.x + graphRect.width * (accelerationRate / maxNumber);
        float decelerationXPosition = graphRect.x + graphRect.width * (decelerationRate / maxNumber);

        // Draw lines representing acceleration and deceleration
        Handles.color = Color.green;
        Handles.DrawLine(
            new Vector3(graphRect.x, graphRect.y + graphRect.height, 0),
            new Vector3(accelerationXPosition, graphRect.y, 0)
        );

        Handles.color = Color.red;
        Handles.DrawLine(
            new Vector3(graphRect.x + graphRect.width, graphRect.y + graphRect.height, 0),
            new Vector3(decelerationXPosition, graphRect.y, 0)
        );

        // Draw max speed line
        Handles.color = Color.blue;
        float maxSpeedYPosition = graphRect.y + graphRect.height * (1 - maxSpeed / maxNumber);
        Handles.DrawLine(
            new Vector3(graphRect.x, maxSpeedYPosition, 0),
            new Vector3(graphRect.x + graphRect.width, maxSpeedYPosition, 0)
        );

        // Add controls to adjust values
        maxNumber = EditorGUILayout.FloatField("Max Number", maxNumber);
        accelerationRate = EditorGUILayout.Slider("Acceleration Rate", accelerationRate, 0f, maxNumber);
        decelerationRate = EditorGUILayout.Slider("Deceleration Rate", decelerationRate, 0f, maxNumber);
        maxSpeed = EditorGUILayout.Slider("Max Speed", maxSpeed, 0f, maxNumber);
    }

    private void ApplySettingsToPlayer()
    {
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.SetMovementParameters(accelerationRate, decelerationRate, maxSpeed);
        }
        else
        {
            Debug.LogWarning("No PlayerScript found in the scene.");
        }
    }
}
