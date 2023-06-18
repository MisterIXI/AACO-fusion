using UnityEngine;

public class Environment : MonoBehaviour
{
    public static int[,] WallInfo { get; private set; }
    public void UpdateMesh(int[,] wallInfo)
    {
        WallInfo = wallInfo;
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        
    }
}