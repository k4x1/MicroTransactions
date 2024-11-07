using UnityEngine;
using UnityEditor;

public class PlayerSettingsWindow : EditorWindow
{
    private float accelerationRate = 2.0f;
    private float decelerationRate = 2.0f;
    private float maxSpeed = 10.0f;
    private float maxRate = 50.0f;

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
        float scaleFactor = baseScale / (1.0f + maxRate / adjustmentFactor);

        // Calculate horizontal positions based on the ratio to maxNumber
        float decelerationXPosition = graphRect.x + graphRect.width * (decelerationRate / maxRate);

        // Calculate max speed line position
        float maxSpeedYPosition = graphRect.y + graphRect.height * (1 - maxSpeed / maxRate);

        // Draw lines representing acceleration and deceleration
        Handles.color = Color.green;

        // Calculate acceleration line endpoints
        Vector3 accelerationStart = new Vector3(graphRect.x, graphRect.y + graphRect.height, 0);
        Vector3 accelerationEnd = new Vector3(
            Mathf.Lerp(graphRect.x + graphRect.width, graphRect.x, accelerationRate / maxRate),
            maxSpeedYPosition,
            0
        );

        Handles.DrawLine(accelerationStart, accelerationEnd);

        Handles.color = Color.red;
        Vector3 decelerationStart = new Vector3(graphRect.x + graphRect.width, graphRect.y + graphRect.height, 0);
        Vector3 decelerationEnd = new Vector3(decelerationXPosition, maxSpeedYPosition, 0);
        Handles.DrawLine(decelerationStart, decelerationEnd);

        // Draw max speed line
        Handles.color = Color.blue;
        Handles.DrawLine(
            new Vector3(graphRect.x, maxSpeedYPosition, 0),
            new Vector3(graphRect.x + graphRect.width, maxSpeedYPosition, 0)
        );

        // Add controls to adjust values
        maxRate = EditorGUILayout.FloatField("Max Number", maxRate);
        accelerationRate = EditorGUILayout.Slider("Acceleration Rate", accelerationRate, 0f, maxRate);
        decelerationRate = EditorGUILayout.Slider("Deceleration Rate", decelerationRate, 0f, maxRate);
        maxSpeed = EditorGUILayout.Slider("Max Speed", maxSpeed, 0f, maxRate);
    }


    private void ApplySettingsToPlayer()
    {
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();
        if (playerScript != null)
        {
            playerScript.SetMovementParameters(maxRate, accelerationRate, decelerationRate, maxSpeed);
        }
        else
        {
            Debug.LogWarning("No PlayerScript found in the scene.");
        }
    }
}
