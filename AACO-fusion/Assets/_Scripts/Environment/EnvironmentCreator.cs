using UnityEngine;

public class EnvironmentCreator : MonoBehaviour
{
    public static Environment CurrentEnvironment { get; private set; }

    public static void GenerateRandomEnvironment()
    {
        if(CurrentEnvironment != null)
        {
            Destroy(CurrentEnvironment.gameObject);
        }
        CurrentEnvironment = new GameObject("Environment").AddComponent<Environment>();
        
    }
}