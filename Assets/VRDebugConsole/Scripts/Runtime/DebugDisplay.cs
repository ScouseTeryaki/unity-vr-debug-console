using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Lightwing.VRDebugConsole {
    [RequireComponent(typeof(DebugLogs))]
    public class DebugDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text display;
        [SerializeField]
        private DebugDropdown dropdown;
        [SerializeField]
        private Button clearButton;
        [SerializeField]
        private TMP_Text logCounterText;
        [SerializeField]
        private TMP_Text warningCounterText;
        [SerializeField]
        private TMP_Text errorCounterText;

        private DebugLogs debugLogs;

        private LogType currentLogType = LogType.Log;

        private void Start()
        {
            debugLogs = GetComponent<DebugLogs>();

            // UI Events
            dropdown.onValueChanged.AddListener(DropdownChanged);
            clearButton.onClick.AddListener(() => debugLogs.ClearLogByType(currentLogType));
            
            // Debug Log Events
            debugLogs.OnUpdate.AddListener(UpdateDisplay);

            debugLogs.storedLogs.OnAdd.AddListener(() => IncrementTextCounter(logCounterText));
            debugLogs.storedWarnings.OnAdd.AddListener(() => IncrementTextCounter(warningCounterText));
            debugLogs.storedErrors.OnAdd.AddListener(() => IncrementTextCounter(errorCounterText));

            debugLogs.storedLogs.OnClear.AddListener(() => ResetTextCounter(logCounterText));
            debugLogs.storedWarnings.OnClear.AddListener(() => ResetTextCounter(warningCounterText));
            debugLogs.storedErrors.OnClear.AddListener(() => ResetTextCounter(errorCounterText));
        }

        private void IncrementTextCounter(TMP_Text textField)
        {
            int count;
            if (Int32.TryParse(textField.text, out count))
            {   
                count += 1;
                textField.text = count.ToString();
            }
            else
            {
                Debug.LogError("Debug counter failed to parse!");
            }
        }

        private void ResetTextCounter(TMP_Text textField)
        {
            textField.text = "0";
        }

        private void DropdownChanged(int value)
        {
            currentLogType = DropdownValueToLogType(value);
            Logs currentLog = debugLogs.GetLogsFromType(currentLogType);
            DisplayLogs(currentLog);
        }

        private void UpdateDisplay()
        {
            Logs currentLog = debugLogs.GetLogsFromType(currentLogType);
            DisplayLogs(currentLog);
        }

        private void DisplayLogs(Logs logs)
        {
            string displayText = "";
            for (int index = 0; index < logs.logData.Count; index++)
            {
                string log = logs.logData[index];
                string logTime = logs.logTime[index];
                displayText += String.Format("{0}: {1}\n", logTime, log);
            }
            display.text = displayText;
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
                case 3:
                    return LogType.Exception;
                case 4:
                    return LogType.Assert;
                default:
                    return LogType.Log;
            }
        }

    }
}
