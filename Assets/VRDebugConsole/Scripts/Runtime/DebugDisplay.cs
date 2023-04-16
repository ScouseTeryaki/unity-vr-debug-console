using System;
using System.Collections.Generic;
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

        private List<LogType> currentLogTypes;

        private void Awake()
        {
            currentLogTypes = dropdown.options[0].types;
        }

        private void Start()
        {
            debugLogs = GetComponent<DebugLogs>();

            // UI Events
            dropdown.onValueChanged.AddListener(DropdownChanged);
            clearButton.onClick.AddListener(() => debugLogs.ClearLogsByTypes(currentLogTypes));
            
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
            currentLogTypes = dropdown.options[value].types;
            List<Log> currentLogs = debugLogs.GetLogsFromTypes(currentLogTypes);
            DisplayLogs(currentLogs);
        }

        private void UpdateDisplay()
        {
            List<Log> currentLogs = debugLogs.GetLogsFromTypes(currentLogTypes);
            DisplayLogs(currentLogs);
        }

        private void DisplayLogs(List<Log> logs)
        {
            foreach (Log log in logs)
            {
                string displayText = "";
                for (int index = 0; index < log.logData.Count; index++)
                {
                    string logText = log.logData[index];
                    string logTime = log.logTime[index];
                    displayText += String.Format("{0}: {1}\n", logTime, logText);
                }
                display.text = displayText;
            }
        }
    }
}
