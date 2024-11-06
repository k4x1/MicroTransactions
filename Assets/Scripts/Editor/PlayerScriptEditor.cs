/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerScript))]
public class PlayerScriptEditor : Editor
{
    SerializedProperty inputMode;
    SerializedProperty movementSpeed;
    SerializedProperty accelerationRate;
    SerializedProperty decelerationRate;
    SerializedProperty maxSpeed;

    void OnEnable()
    {
        inputMode = serializedObject.FindProperty("inputMode");
        movementSpeed = serializedObject.FindProperty("movementSpeed");
        accelerationRate = serializedObject.FindProperty("accelerationRate");
        decelerationRate = serializedObject.FindProperty("decelerationRate");
        maxSpeed = serializedObject.FindProperty("maxSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        EditorGUILayout.Slider(movementSpeed, 0f, 20f, new GUIContent("Movement Speed"));
        EditorGUILayout.Slider(accelerationRate, 0f, 10f, new GUIContent("Acceleration Rate"));
        EditorGUILayout.Slider(decelerationRate, 0f, 10f, new GUIContent("Deceleration Rate"));
        EditorGUILayout.Slider(maxSpeed, 0f, 20f, new GUIContent("Max Speed"));

        serializedObject.ApplyModifiedProperties();
    }
}
*/