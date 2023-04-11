using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class DebugLogs
{
    public DebugLog storedLogs {get; set;} = new DebugLog();
    public DebugLog storedWarnings {get; set;} = new DebugLog();
    public DebugLog storedErrors { get; set; } = new DebugLog();
    public DebugLog storedExceptions { get; set; } = new DebugLog();
    public DebugLog storedAsserts { get; set; } = new DebugLog();

    public UnityEvent OnLog = new UnityEvent();

    public DebugLogs()
    {
        storedLogs.OnAdd.AddListener(OnLogRecieved);
        storedWarnings.OnAdd.AddListener(OnLogRecieved);
        storedErrors.OnAdd.AddListener(OnLogRecieved);
        storedExceptions.OnAdd.AddListener(OnLogRecieved);
        storedAsserts.OnAdd.AddListener(OnLogRecieved);
    }

    protected virtual void OnLogRecieved()
    {
        if (OnLog != null)
        {
            OnLog.Invoke();
        }
    }
    public void ClearLogByType(LogType logType)
    {
        DebugLog logs = GetLogsFromType(logType);
        logs.Clear();
    }

    public DebugLog GetLogsFromType(LogType logType)
    {
        switch (logType)
        {
            case LogType.Log:
                return storedLogs;
            case LogType.Warning:
                return storedWarnings;
            case LogType.Error:
                return storedErrors;
            case LogType.Exception:
                return storedExceptions;
            case LogType.Assert:
                return storedAsserts;
            default:
                return storedLogs;
        }
    }
}

public class DebugLog : List<string>
{
    public UnityEvent OnAdd = new UnityEvent();

    public new void Add(string item)
    {
        string log = GetLogData(item);

        // Check if log already exists
        int index = base.FindIndex(i => { 
                string logData = GetLogData(i);
                return(log == logData);
            });

        string time = DateTime.Now.ToString("HH:mm:ss");
        string newLog = String.Format("{0}: {1}", time, item);

        if (index != -1)
            base.RemoveAt(index);
            
        base.Add(item);

        OnAdd.Invoke();
    }

    public new void Clear()
    {
        base.Clear();
        OnAdd.Invoke();
    }

    private string GetLogData(string log)
    {
        string[] splitString = log.Split(char.Parse(":"));
        string logData = splitString[3];
        logData = Regex.Replace(logData, @"\s+", "");
        return logData;
    }
}
