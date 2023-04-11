using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating("Test", 0.0f, 1.0f);
    }

    private void Test()
    {
        Debug.Log("Test");
        Debug.LogWarning("Warning");
        Debug.LogError("Error");
    }
}
