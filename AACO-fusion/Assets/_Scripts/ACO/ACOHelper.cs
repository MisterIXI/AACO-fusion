using System.Collections.Generic;
using UnityEngine;

public class ACOHelper
{
    public static List<Vector2Int> GetValidNeighbours(Vector2Int node, int[,] gridInfo, HashSet<Vector2Int> visitedNodes = null)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        // check all 8 directions
        for (int x = -1; x <= 1; x++)
        {
            // check all 8 directions
            for (int y = -1; y <= 1; y++)
            {
                // skip the current node
                if (x == 0 && y == 0)
                {
                    continue;
                }
                // skip visited nodes
                if (visitedNodes != null && visitedNodes.Contains(new Vector2Int(node.x + x, node.y + y)))
                {
                    continue;
                }
                // get the neighbor position
                Vector2Int neighborPos = new Vector2Int(node.x + x, node.y + y);
                // check if the neighbor is within the grid
                if (neighborPos.x >= 0 && neighborPos.x < gridInfo.GetLength(0) && neighborPos.y >= 0 && neighborPos.y < gridInfo.GetLength(1))
                {
                    // check if the neighbor is walkable
                    if (gridInfo[neighborPos.x, neighborPos.y] == 0)
                    {
                        neighbours.Add(neighborPos);
                    }
                }
            }
        }
        return neighbours;
    }
}