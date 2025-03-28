using System.Collections.Generic;
using UnityEngine;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class ConsoleHistory
        {
            [SerializeField] private List<string> commandsHistory = new List<string>(10);
            [SerializeField] private int historyIndex;
            
            private ConsoleInput consoleInput;


            public void Init(ConsoleInput consoleInput1)
            {
                this.consoleInput = consoleInput1;
            }
            
            public void CheckInput(ConsoleInput.ESelectionState input)
            {
                if (input != ConsoleInput.ESelectionState.OutOfHints)
                {
                    ResetHistory();
                }
            }

            public void ResetHistory()
            {
                historyIndex = 0;
            }

            public void AddInHistory(string command)
            {
                if (commandsHistory.Count == 0)
                {
                    commandsHistory.Add(command);
                    return;
                }

                if (commandsHistory[0].Trim() != command.Trim())
                {
                    commandsHistory.Insert(0, command);
                }
            }

            public string NextItem()
            {
                if (commandsHistory.Count != 0)
                {
                    var n = commandsHistory[Mathf.Clamp(historyIndex, 0, commandsHistory.Count - 1)];
                    historyIndex++;
                    return n;
                }

                return null;
            }
        }
    }
}