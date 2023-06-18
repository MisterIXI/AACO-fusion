using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AACO_Settings))]
public class AACO_SettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AACO_Settings settings = (AACO_Settings)target;
        GUILayout.Space(20);
        GUILayout.Label("Grid Storage", EditorStyles.boldLabel);
        if (settings.GridStorage.Count > 0)
        {
            foreach (var item in settings.GridStorage)
            {
                GUILayout.Label("Grid: " + item.GetLength(0) + "x" + item.GetLength(1));
            }
        }
        else
        {
            GUILayout.Label("No grids stored");
        }
        GUILayout.Space(10);
        GUILayout.Label("Start Grid", EditorStyles.boldLabel);
        if (settings.StartGrid != null)
        {
            GUILayout.Label("Grid: " + settings.StartGrid.GetLength(0) + "x" + settings.StartGrid.GetLength(1));
        }
        else
        {
            GUILayout.Label("No grid set");
        }
        
    }
}