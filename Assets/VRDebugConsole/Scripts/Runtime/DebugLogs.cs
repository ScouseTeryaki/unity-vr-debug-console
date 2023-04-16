using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Lightwing.VRDebugConsole {
    public class DebugLogs : MonoBehaviour
    {
        public Log storedLogs {get; set;} = new Log();
        public Log storedWarnings {get; set;} = new Log();
        public Log storedErrors { get; set; } = new Log();
        public Log storedExceptions { get; set; } = new Log();
        public Log storedAsserts { get; set; } = new Log();

        public UnityEvent OnUpdate = new UnityEvent();

        public DebugLogs()
        {
            storedLogs.OnAdd.AddListener(OnLogUpdate);
            storedWarnings.OnAdd.AddListener(OnLogUpdate);
            storedErrors.OnAdd.AddListener(OnLogUpdate);
            storedExceptions.OnAdd.AddListener(OnLogUpdate);
            storedAsserts.OnAdd.AddListener(OnLogUpdate);

            storedLogs.OnClear.AddListener(OnLogUpdate);
            storedWarnings.OnClear.AddListener(OnLogUpdate);
            storedErrors.OnClear.AddListener(OnLogUpdate);
            storedExceptions.OnClear.AddListener(OnLogUpdate);
            storedAsserts.OnClear.AddListener(OnLogUpdate);
        }

        private void OnEnable() 
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable() 
        {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            Log log = GetLogFromType(type);
            log.Add(logString, stackTrace);
        }

        protected virtual void OnLogUpdate()
        {
            if (OnUpdate != null)
            {
                OnUpdate.Invoke();
            }
        }
        public void ClearLogsByTypes(List<LogType> logType)
        {
            List<Log> logs = GetLogsFromTypes(logType);
            foreach (Log log in logs)
            {
                log.Clear();
            }
        }

        public List<Log> GetLogsFromTypes(List<LogType> logTypes)
        {
            List<Log> log = new List<Log>();
            try 
            {
                foreach (LogType type in logTypes)
                {
                    log.Add(GetLogFromType(type));
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogError("Debug option has no selected log types!");
            }
            return log;
        }

        public Log GetLogFromType(LogType type)
        {
            switch (type)
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

    public class Log 
    {
        public List<string> logData { get; private set; } = new List<string>();
        public List<string> logKey { get; private set; } = new List<string>();
        public List<string> logTime { get; private set; } = new List<string>();
        public List<string> stackTraces { get; private set; } = new List<string>();

        public UnityEvent OnAdd = new UnityEvent();
        public UnityEvent OnClear = new UnityEvent();

        public void Add(string log, string stackTrace, string key = "")
        {
            if (logData.Count != 0)
            {
                RemoveRepeatedLog(log);
                RemoveRepeatedKey(key);
            }

            logData.Add(log);
            logKey.Add(key);
            stackTraces.Add(stackTrace);

            string time = DateTime.Now.ToString("HH:mm:ss");
            logTime.Add(time);

            OnAdd.Invoke();
        }

        public void Clear()
        {
            logData.Clear();
            logTime.Clear();
            stackTraces.Clear();
            OnClear.Invoke();
        }

        private void RemoveRepeatedKey(string log)
        {
            string key = log.Split(":")[0];
            for (int index = 0; index < logKey.Count; index++)
            {
                if (logKey[index] == key)
                {
                    RemoveLogAtIndex(index);
                }
            }
        }

        private void RemoveRepeatedLog(string log)
        {
            for (int index = 0; index < logData.Count; index++)
            {
                if (logData[index] == log)
                {
                    RemoveLogAtIndex(index);
                }
            }
        }

        private void RemoveLogAtIndex(int index)
        {
            logData.RemoveAt(index);
            logKey.RemoveAt(index);
            logTime.RemoveAt(index);
            stackTraces.RemoveAt(index);
        }
    }
}
