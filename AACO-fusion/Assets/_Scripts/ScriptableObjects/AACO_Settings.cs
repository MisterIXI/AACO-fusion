using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "AACO_Settings", menuName = "AACO-fusion/AACO_Settings", order = 0)]
public class AACO_Settings : ScriptableObject
{
    [field: Header("Environment Settings")]
    [field: SerializeField] [field: Range(4,1000)] public int MazeWidth = 160;
    [field: SerializeField] [field: Range(4,1000)] public int MazeHeight = 100;
    [field: SerializeField] public int[,] StartGrid = null;
    [field: SerializeField] public List<int[,]> GridStorage = new List<int[,]>();
    [field: SerializeField] [field: Range(0.0f,1.0f)] public float WallThreshold = 0.5f;
    [field: SerializeField] public float PerlinOffset = 0.0f;
    [field: SerializeField] public Vector2Int StartPoint = new Vector2Int(0,0);
    [field: SerializeField] public Vector2Int GoalPoint = new Vector2Int(0,0);

    [field: Header("Algorithm Settings")]
    [field: SerializeField] [field: Range(1,1000)] public int NumberOfAntsPerIteration = 100;
    [field: SerializeField] [field: Range(1,100)] public int NumberOfIterations = 10;
    [field: SerializeField] [field: Range(0.0f,1.0f)] public float PheromoneDecay = 0.5f;
    [field: SerializeField] [field: Range(0.0f,1.0f)] public float PheromoneStrength = 0.5f;


}