using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SetInfo))]
public class SetInfoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SetInfo setInfo = (SetInfo)target;

        // Manually display the autoPopulateSpawnLocations toggle
        setInfo.autoPopulateSpawnLocations = EditorGUILayout.Toggle("Auto Populate Spawn Locations", setInfo.autoPopulateSpawnLocations);

        // Conditionally display fields based on autoPopulateSpawnLocations
        if (setInfo.autoPopulateSpawnLocations)
        {
            SerializedProperty autoSpawnLocation = serializedObject.FindProperty("AutoSpawnLocation");
            EditorGUILayout.PropertyField(autoSpawnLocation);
        }
        else
        {
            SerializedProperty spawnLocations = serializedObject.FindProperty("spawnLocations");
            EditorGUILayout.PropertyField(spawnLocations, true);
        }

        // Draw the rest of the fields
        SerializedProperty spawnEverything = serializedObject.FindProperty("spawnEverything");
        EditorGUILayout.PropertyField(spawnEverything);

        SerializedProperty lootLocation = serializedObject.FindProperty("lootLocation");
        EditorGUILayout.PropertyField(lootLocation);

        SerializedProperty lootLocations = serializedObject.FindProperty("lootLocations");
        EditorGUILayout.PropertyField(lootLocations, true);

        SerializedProperty platforms = serializedObject.FindProperty("platforms");
        EditorGUILayout.PropertyField(platforms);

        SerializedProperty availableLoot = serializedObject.FindProperty("availableLoot");
        EditorGUILayout.PropertyField(availableLoot, true);

        SerializedProperty breakables = serializedObject.FindProperty("breakables");
        EditorGUILayout.PropertyField(breakables, true);

        SerializedProperty sets = serializedObject.FindProperty("sets");
        EditorGUILayout.PropertyField(sets);

        SerializedProperty movingSpeed = serializedObject.FindProperty("movingSpeed");
        EditorGUILayout.PropertyField(movingSpeed);

        SerializedProperty weight = serializedObject.FindProperty("weight");
        EditorGUILayout.PropertyField(weight);

        serializedObject.ApplyModifiedProperties();
    }
}
