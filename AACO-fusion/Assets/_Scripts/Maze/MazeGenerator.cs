using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates a maze using the Recursive Backtracking algorithm.
/// </summary>
public class MazeGenerator : MonoBehaviour
{

    public static MazeGenerator instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public static int[,] GenerateMaze(int width, int height, Vector2Int? startPoint = null, Vector2Int? goalPoint = null)
    {
        if (width < 4 || height < 4)
        {
            Debug.LogError("MazeGenerator: Maze dimensions must be at least 4x4");
            return null;
        }
        int[,] maze = new int[width, height];

        // Fill the maze with walls
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                maze[x, y] = 1;
            }
        }

        // Set the starting point to a random point if null
        Vector2Int start;
        if (startPoint == null)
        {
            do
            {
                start = new Vector2Int(Random.Range(0, maze.GetLength(0)), Random.Range(0, maze.GetLength(1)));
            } while (goalPoint != null && start == goalPoint.Value);
        }
        else
        {
            start = startPoint.Value;
        }
        maze[start.x, start.y] = 0;

        // Set the goal point
        Vector2Int goal;
        if (goalPoint == null)
        {
            do
            {
                goal = new Vector2Int(Random.Range(0, maze.GetLength(0)), Random.Range(0, maze.GetLength(1)));
            } while (start == goal);
        }
        else
        {
            goal = goalPoint.Value;
        }

        return maze;
    }

    private static void GenerateMazeRecursiveBacktracking(int[,] maze, Vector2Int start, Vector2Int goal, Vector2Int currentCell)
    {
        // Set the current cell as visited
        maze[currentCell.x, currentCell.y] = 1;

        // Get a random unvisited neighbor of the current cell
        List<Vector2Int> unvisitedNeighbors = GetUnvisitedNeighbors(maze, currentCell);
        if (unvisitedNeighbors.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, unvisitedNeighbors.Count);
            Vector2Int randomNeighbor = unvisitedNeighbors[randomIndex];

            // Remove the wall between the current cell and the chosen neighbor
            RemoveWall(maze, currentCell, randomNeighbor);

            // Recursively call the method with the chosen neighbor as the new current cell
            GenerateMazeRecursiveBacktracking(maze, start, goal, randomNeighbor);
        }
    }

    private static List<Vector2Int> GetUnvisitedNeighbors(int[,] maze, Vector2Int cell)
    {
        List<Vector2Int> unvisitedNeighbors = new List<Vector2Int>();

        // Check the neighbor to the left
        if (cell.x > 1 && maze[cell.x - 2, cell.y] == 0)
        {
            unvisitedNeighbors.Add(new Vector2Int(cell.x - 2, cell.y));
        }

        // Check the neighbor to the right
        if (cell.x < maze.GetLength(0) - 2 && maze[cell.x + 2, cell.y] == 0)
        {
            unvisitedNeighbors.Add(new Vector2Int(cell.x + 2, cell.y));
        }

        // Check the neighbor above
        if (cell.y > 1 && maze[cell.x, cell.y - 2] == 0)
        {
            unvisitedNeighbors.Add(new Vector2Int(cell.x, cell.y - 2));
        }

        // Check the neighbor below
        if (cell.y < maze.GetLength(1) - 2 && maze[cell.x, cell.y + 2] == 0)
        {
            unvisitedNeighbors.Add(new Vector2Int(cell.x, cell.y + 2));
        }

        return unvisitedNeighbors;
    }

    private static void RemoveWall(int[,] maze, Vector2Int cell1, Vector2Int cell2)
    {
        // Determine the direction of the wall to remove
        Vector2Int direction = cell2 - cell1;
        direction = new Vector2Int(direction.x / 2, direction.y / 2);

        // Remove the wall
        maze[cell1.x + direction.x, cell1.y + direction.y] = 0;
    }
}
