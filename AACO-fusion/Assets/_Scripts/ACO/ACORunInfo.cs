
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ACORunInfo
{
    public List<List<AntAgent>> Ants;
    public List<float[,]> PheromoneMatrices;
    public int[,] GridInformation;
    public Vector2Int StartPoint;
    public Vector2Int GoalPoint;
    public List<List<Vector2Int>> Paths;
}
