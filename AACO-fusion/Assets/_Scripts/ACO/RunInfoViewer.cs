using UnityEngine;

public class RunInfoViewer : MonoBehaviour
{
    [field: SerializeField] public ACORunInfo RunInfo { get; private set; }
    private ACORunInfo _runInfo;
    public bool HasRunInfo => _runInfo != null;
    private GameObject _mazeObject;
    private GameObject _antObject;
    private GameObject _lineObject;
    public static RunInfoViewer Instance { get; private set; }

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
        _mazeObject = new GameObject("Maze");
        _antObject = new GameObject("Ants");
        _lineObject = new GameObject("Lines");

        _mazeObject.transform.parent = transform;
        _antObject.transform.parent = transform;
        _lineObject.transform.parent = transform;
    }

    public void SetRunInfo(ACORunInfo runInfo)
    {
        Debug.Log("RunInfoViewer: InitObjects");
        _runInfo = runInfo;
        ReplaceMaze(_runInfo.GridInformation);
    }

    public void ReplaceMaze(int[,] gridInfo)
    {
        //RunInfo Has Changed
        Destroy(_mazeObject);
        _mazeObject = new GameObject("Maze");
        _mazeObject.transform.parent = transform;
        // create quads for the maze
        for (int x = 0; x < gridInfo.GetLength(0); x++)
        {
            for (int y = 0; y < gridInfo.GetLength(1); y++)
            {
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.name = $"({x},{y})";
                quad.transform.position = new Vector3(x, y, 0);
                quad.transform.parent = _mazeObject.transform;
                if (gridInfo[x, y] == 1)
                {
                    quad.GetComponent<Renderer>().material.color = Color.black;
                }
            }
        }
    }


    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        if (RunInfo != null && _runInfo != RunInfo)
        {
            // SetRunInfo(RunInfo);
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