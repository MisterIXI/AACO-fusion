using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RunInfoViewer))]
public class RunInfoViewerEditor : Editor
{
    MazeGenerationAlgorithm mazeGenerationAlgorithm;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RunInfoViewer runInfoViewer = (RunInfoViewer)target;

        // check for playmode
        if (Application.isPlaying == false)
        {
            EditorGUILayout.HelpBox("RunInfoViewer only works in playmode", MessageType.Warning);
            return;
        }
        mazeGenerationAlgorithm = (MazeGenerationAlgorithm)EditorGUILayout.EnumPopup("Maze Generation Algorithm", mazeGenerationAlgorithm);
        if (GUILayout.Button("Roll new Maze"))
        {

            int[,] newMaze = MazeGenerator.GenerateMaze(mazeGenerationAlgorithm);
            runInfoViewer.ReplaceMaze(newMaze);
        }
        if (GUILayout.Button("Start Pathfinding"))
        {
            runInfoViewer.SetRunInfo(new ACORunInfo());
        }
    }
}