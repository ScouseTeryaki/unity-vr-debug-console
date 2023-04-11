using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DebugDisplay : MonoBehaviour
{
    [SerializeField]
    private Text display;
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private Button clearButton;

    private DebugLogs debugLogs = new DebugLogs();

    private LogType currentLogType = LogType.Log;

    private void Start()
    {
        dropdown.onValueChanged.AddListener(DropdownChanged);
        debugLogs.OnLog.AddListener(UpdateDisplay);
        
        clearButton.onClick.AddListener(() => debugLogs.ClearLogByType(currentLogType));
    }

    private void OnEnable() 
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable() 
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void DropdownChanged(int value)
    {
        currentLogType = DropdownValueToLogType(value);
        DebugLog currentLog = debugLogs.GetLogsFromType(currentLogType);
        DisplayLogs(currentLog);
    }

    private void UpdateDisplay()
    {
        DebugLog currentLog = debugLogs.GetLogsFromType(currentLogType);
        DisplayLogs(currentLog);
    }

    private void DisplayLogs(DebugLog logs)
    {
        string displayText = "";
        foreach (string log in logs)
        {
            displayText += log + "\n";
        }
        display.text = displayText;
    }

    private void HandleLog(string log, string stackTrace, LogType type)
    {
        DebugLog logs = debugLogs.GetLogsFromType(type);

        string time = DateTime.Now.ToString("HH:mm:ss");
        string newLog = String.Format("{0}: {1}", time, log);

        logs.Add(newLog);
    }

    private LogType DropdownValueToLogType(int value)
    {
        switch (value)
        {
            case 0:
                return LogType.Log;
            case 1:
                return LogType.Warning;
            case 2:
                return LogType.Error;
            default:
                return LogType.Log;
        }
    }

}
