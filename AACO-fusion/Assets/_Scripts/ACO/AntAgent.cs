using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AntAgent
{
    private Vector2Int _startPoint;
    private Vector2Int _goalPoint;
    private int[,] _gridInformation;
    private float[,] _pheromoneMatrix;
    private float _pheromoneMax;
    private float _StartEndDistance;
    private AACO_Settings _settings;
    private int _gridWidth;
    private int _gridHeight;
    public HashSet<Vector2Int> VisitedNodes { get; private set; } = new HashSet<Vector2Int>();
    public List<Vector2Int> Path { get; private set; } = new List<Vector2Int>();
    public bool FoundPath = false;

    public AntAgent(Vector2Int startPoint, Vector2Int endPoint, int[,] gridInformation, float[,] pheromoneMatrix, AACO_Settings settings)
    {
        _startPoint = startPoint;
        _goalPoint = endPoint;
        _gridInformation = gridInformation;
        _pheromoneMatrix = pheromoneMatrix;
        _settings = settings;
        _gridWidth = _gridInformation.GetLength(0);
        _gridHeight = _gridInformation.GetLength(1);
        _pheromoneMax = _pheromoneMatrix.Cast<float>().Max();
        _StartEndDistance = Vector2Int.Distance(_startPoint, _goalPoint);
    }

    public void Run()
    {
        // reset the visited nodes and path
        VisitedNodes.Clear();
        Path.Clear();
        // add the start node to the visited nodes
        VisitedNodes.Add(_startPoint);
        // add the start node to the path
        Path.Add(_startPoint);
        // get the next node
        Vector2Int? currentNode = _startPoint;
        while (currentNode != _goalPoint)
        {
            currentNode = GetNextNode(currentNode.Value);
            if (currentNode == null)
                break;
            VisitedNodes.Add(currentNode.Value);
            Path.Add(currentNode.Value);
        }
        if (currentNode == _goalPoint)
            FoundPath = true;
        else
            FoundPath = false;
    }

    private Vector2Int? GetNextNode(Vector2Int currentNode)
    {
        // get the neighbors of the current node
        List<Vector2Int> neighbors = ACOHelper.GetValidNeighbours(currentNode, _gridInformation, VisitedNodes);
        // if there are no neighbors, return the current node
        if (neighbors.Count == 0)
        {
            throw new Exception("Nowhere to go! No Path found!");
        }
        // if there is only one neighbor, return that neighbor
        if (neighbors.Count == 1)
        {
            return neighbors[0];
        }
        // calculate the probability of each neighbor
        float[,] neighbourData = new float[neighbors.Count, 3];
        float maxDistance = 0;
        float maxPheromone = 0;
        for (int i = 0; i < neighbors.Count; i++)
        {
            // calulate the distance between the neighbor and the goal
            neighbourData[i, 1] = Vector2Int.Distance(neighbors[i], _goalPoint);
            if (neighbourData[i, 1] > maxDistance)
                maxDistance = neighbourData[i, 1];
            // calculate the pheromone level of the neighbor
            neighbourData[i, 2] = _pheromoneMatrix[neighbors[i].x, neighbors[i].y];
            if (neighbourData[i, 2] > maxPheromone)
                maxPheromone = neighbourData[i, 2];
        }
        // normalize the distance and pheromone data
        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbourData[i, 1] = maxDistance - neighbourData[i, 1];
            neighbourData[i, 1] /= maxDistance;
            neighbourData[i, 2] /= maxPheromone;
        }
        // calculate the probability of each neighbor
        for (int i = 0; i < neighbors.Count; i++)
        {
            neighbourData[i, 0] = (1 - _settings.PheromoneStrength) * neighbourData[i, 1] + _settings.PheromoneStrength * neighbourData[i, 2];
        }
        // roll a random neighbour weighted by the probability
        float randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
        float sum = 0;
        Vector2Int nextNode = new Vector2Int(-1, -1);
        for (int i = 0; i < neighbors.Count; i++)
        {
            sum += neighbourData[i, 0];
            if (randomValue <= sum)
            {
                nextNode = neighbors[i];
                break;
            }
        }
        return nextNode;
    }



}