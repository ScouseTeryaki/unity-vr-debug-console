using System.Collections.Generic;
using UnityEngine;

namespace Lightwing.VRDebugConsole {
    [CreateAssetMenu(fileName = "Dropdown Option", menuName = "VRDebugConsole/DropdownOption", order = 1)]
    public class DebugOption : ScriptableObject
    {
        public string optionName;
        public Sprite optionImage;
        public List<LogType> optionLogTypes;
    }
}