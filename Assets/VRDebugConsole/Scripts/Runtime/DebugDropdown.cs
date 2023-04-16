using System;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

namespace Lightwing.VRDebugConsole {
    public class DebugDropdown : TMP_Dropdown
    {
        [Serializable]
        public new class OptionData
        {
            [SerializeField]
            private DebugOption m_Option;

            /// <summary>
            /// The text associated with the option.
            /// </summary>
            public string text { get { return m_Option.optionName; } }

            /// <summary>
            /// The image associated with the option.
            /// </summary>
            public Sprite image { get { return m_Option.optionImage; } }

            /// <summary>
            /// The log types associated with the option.
            /// </summary>
            public List<LogType> type { get { return m_Option.optionLogTypes; } }

            public DebugOption option { get { return m_Option; } set { m_Option = value; } }

            public OptionData() { }

            public OptionData(DebugOption option)
            {
                this.option = option;
            }
        }

        [Serializable]
        /// <summary>
        /// Class used internally to store the list of options for the dropdown list.
        /// </summary>
        /// <remarks>
        /// The usage of this class is not exposed in the runtime API. It's only relevant for the PropertyDrawer drawing the list of options.
        /// </remarks>
        public new class OptionDataList
        {
            [SerializeField]
            private List<OptionData> m_DebugOptions;

            /// <summary>
            /// The list of options for the dropdown list.
            /// </summary>
            public List<OptionData> options { get { return m_DebugOptions; } set { m_DebugOptions = value; } }

            public OptionDataList()
            {
                options = new List<OptionData>();
            }
        }

        [Space]

        // Items that will be visible when the dropdown is shown.
        // We box this into its own class so we can use a Property Drawer for it.
        [SerializeField]
        private OptionDataList m_DebugOptions = new OptionDataList();

        public new List<OptionData> options
        {
            get { return m_DebugOptions.options; }
            set { m_DebugOptions.options = value; RefreshShownValue(); }
        }
    }
}
