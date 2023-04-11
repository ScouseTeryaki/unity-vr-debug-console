using System;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating("LogTest", 0.0f, 1.0f);
        InvokeRepeating("WarningTest", 0.0f, 1.0f);
        InvokeRepeating("ErrorTest", 0.0f, 1.0f);
        InvokeRepeating("ExceptionTest", 0.0f, 1.0f);
        InvokeRepeating("AssertTest", 0.0f, 1.0f);
    }

    private void LogTest()
    {
        Debug.Log("Log Test");
    }

    private void WarningTest()
    {
        Debug.LogWarning("Warning Test");
    }

    private void ErrorTest()
    {
        Debug.LogError("Error Test");
    }

    private void ExceptionTest()
    {
        Debug.LogException(new TestException("Exception Test"));
    }

    private void AssertTest()
    {
        Debug.LogAssertion("Assertion Test");
    }
}

[Serializable]
public class TestException : Exception 
{ 
    public TestException(string message)
        : base(message) { }
 }
