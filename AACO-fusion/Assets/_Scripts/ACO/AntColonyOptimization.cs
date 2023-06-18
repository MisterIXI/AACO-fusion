using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A class that uses Ant colony optimization to find the shortest path between two points on a grid.
/// </summary>
public class AntColonyOptimization : MonoBehaviour
{
    [field: SerializeField] public AACO_Settings Settings { get; private set; }
    public float Progress { get; private set; } = 0f;
    public bool IsRunning { get; private set; } = false;
    private bool _initialized = false;
    private float[,] _pheromoneMatrix;
    private int[,] _gridInformation;
    private int _gridWidth;
    private int _gridHeight;
    [field: SerializeField] private Vector2Int _startPoint;
    [field: SerializeField] private Vector2Int _goalPoint;
    private int _numberOfAnts;
    public static int[,] GridInformation { get { return Instance._gridInformation; } }
    public static AntColonyOptimization Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        if (Settings == null)
        {
            Debug.LogError("AntColonyOptimization: Settings not set!");
            Destroy(gameObject);
            return;
        }
        InitializeACO();
    }
    public void InitializeACO()
    {
        // init the grid
        if (Settings.StartGrid == null || Settings.StartGrid.GetLength(0) == 0 || Settings.StartGrid.GetLength(1) == 0)
        {
            Debug.Log("AntColonyOptimization: Generating maze...");
            // _gridInformation = MazeGenerator.GenerateMaze(_settings.MazeWidth, _settings.MazeHeight);
            _gridInformation = MazeGenerator.PerlinMaze(Settings.MazeWidth, Settings.MazeHeight, Settings.WallThreshold, Settings.PerlinOffset);
        }
        else
        {
            _gridInformation = Settings.StartGrid.Clone() as int[,];
        }
        // init the pheromone grid
        _pheromoneMatrix = new float[_gridInformation.GetLength(0), _gridInformation.GetLength(1)];
        // init the grid width and height
        _gridWidth = _gridInformation.GetLength(0);
        _gridHeight = _gridInformation.GetLength(1);
    }

    public void TestPath()
    {
        FindPathNonBlocking(RunInfoViewer.Instance.SetRunInfo, _startPoint, _goalPoint, false);
    }
    public async void FindPathNonBlocking(System.Action<ACORunInfo> callback, Vector2Int startPoint, Vector2Int goalPoint, bool runSingleThreaded = true)
    {
        // if startPoint and goalPoint are (0,0), set random ones
        if (startPoint == Vector2Int.zero && goalPoint == Vector2Int.zero)
        {
            while (startPoint == Vector2Int.zero && _gridInformation[startPoint.x, startPoint.y] == 1)
            {
                startPoint = new Vector2Int(Random.Range(0, _gridWidth), Random.Range(0, _gridHeight));
            }
            while (goalPoint == Vector2Int.zero && _gridInformation[goalPoint.x, goalPoint.y] == 1)
            {
                goalPoint = new Vector2Int(Random.Range(0, _gridWidth), Random.Range(0, _gridHeight));
            }
        }
        ACORunInfo runInfo = await Task.Run(() => FindPath(startPoint, goalPoint, runSingleThreaded));
        callback?.Invoke(runInfo);
    }


    public ACORunInfo FindPath(Vector2Int startPoint, Vector2Int goalPoint, bool runSingleThreaded = true)
    {

        // init RunInfo
        ACORunInfo runInfo = new ACORunInfo();
        runInfo.StartPoint = startPoint;
        runInfo.GoalPoint = goalPoint;
        runInfo.GridInformation = _gridInformation.Clone() as int[,];
        runInfo.PheromoneMatrices = new List<float[,]>();
        runInfo.Paths = new List<List<Vector2Int>>();
        // prepare for pathing
        _pheromoneMatrix = new float[_gridInformation.GetLength(0), _gridInformation.GetLength(1)];
        List<List<AntAgent>> ants = new List<List<AntAgent>>();
        for (int i = 0; i < Settings.NumberOfIterations; i++)
        {
            ants.Add(new List<AntAgent>());
        }
        List<Vector2Int> path = new List<Vector2Int>();
        // run the pathing
        IsRunning = true;
        Progress = 0f;
        float progressStep = 1f / Settings.NumberOfIterations;

        if (runSingleThreaded)
        {
            // run singlethreaded
            for (int i = 0; i < Settings.NumberOfIterations; i++)
            {
                // init the ants
                for (int j = 0; j < Settings.NumberOfAntsPerIteration; j++)
                {
                    ants[i].Add(new AntAgent(startPoint, goalPoint, _gridInformation, _pheromoneMatrix, Settings));
                }
                // run the ants
                for (int j = 0; j < Settings.NumberOfAntsPerIteration; j++)
                {
                    ants[i][j].Run();
                    if (ants[i][j].FoundPath)
                        UpdatePheromone(ants[i][j]);
                }
                // decay the pheromones
                PheromoneDecay();
                // update the progress
                Progress += progressStep;
                runInfo.PheromoneMatrices.Add(_pheromoneMatrix.Clone() as float[,]);
                runInfo.Paths.Add(ConstructPath(_pheromoneMatrix));
            }

        }
        else
        {
            // run multithreaded
        }
        // return the path
        runInfo.Ants = ants;
        return runInfo;
    }
    private List<Vector2Int> ConstructPath(float[,] pheromones)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int? currentPoint = _startPoint;
        while (currentPoint != _goalPoint)
        {
            path.Add(currentPoint.Value);
            var neighbours = ACOHelper.GetValidNeighbours(currentPoint.Value, _gridInformation);
            if (neighbours.Count == 0 || !neighbours.Any(x => pheromones[x.x, x.y] != 0f))
            {
                Debug.LogWarning("AntColonyOptimization: No valid neighbours found!");
                break;
            }
            currentPoint = neighbours.OrderByDescending(x => pheromones[x.x, x.y]).FirstOrDefault();
        }
        if (currentPoint == _goalPoint)
            path.Add(_goalPoint);
        else
            return null;
        return path;
    }
    private void UpdatePheromone(AntAgent ant)
    {
        // update the pheromone matrix
        for (int i = 0; i < ant.Path.Count; i++)
        {
            _pheromoneMatrix[ant.Path[i].x, ant.Path[i].y] += Settings.PheromoneStrength;
        }
    }

    private void PheromoneDecay()
    {
        // decay the pheromone matrix
        for (int i = 0; i < _pheromoneMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < _pheromoneMatrix.GetLength(1); j++)
            {
                _pheromoneMatrix[i, j] *= Settings.PheromoneDecay;
            }
        }
    }


    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

}

