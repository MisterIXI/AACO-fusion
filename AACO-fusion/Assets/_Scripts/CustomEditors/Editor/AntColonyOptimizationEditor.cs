using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AntColonyOptimization))]
public class AntColonyOptimizationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        AntColonyOptimization aco = (AntColonyOptimization)target;
        if(GUILayout.Button("Start Pathfinding"))
        {
            aco.TestPath();
        }
    }
}