using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class HintsSolver
        {
            private List<string> normalCommands = new List<string>(10);
            private List<List<Argument>> arguments = new List<List<Argument>>(10);
            
            
            private ConsoleInput consoleInput;
            private ConsoleCommands consoleCommands;

            public int CommandsCount => normalCommands.Count;
            
            private List<ICommandExecutable> commands => consoleCommands.Commands;


            public Action<string> OnRecalculatePlaceholder;
            public Action OnHideHints;
            public Action OnShowHints;
            public Action UpdateSelectedVisuals;
            
            public void Init(ConsoleInput consoleInput, ConsoleCommands consoleCommands)
            {
                this.consoleCommands = consoleCommands;
                this.consoleInput = consoleInput;
            }

        

            public void UpdateSolver()
            {
                UpdateCommandsList();
                var isSecondWordNotStarted = consoleInput.GetText().Split(' ').Length == 1;
                if (normalCommands.Count != 0 && !consoleInput.IsText(string.Empty) && isSecondWordNotStarted)
                {
                    ShowHints();
                    UpdateSelectedVisuals?.Invoke();
                }
                else
                {
                    HideHints(!isSecondWordNotStarted);
                }
            }

            public void HideHints(bool hintsOnly = false)
            {
                if (!hintsOnly)
                {
                    OnRecalculatePlaceholder?.Invoke("");
                }

                OnHideHints?.Invoke();
            }

            private void ShowHints()
            {
                if (normalCommands.Count != 0)
                {
                    if (consoleInput.SelectedHint >= normalCommands.Count)
                    {
                        consoleInput.SetIndex(normalCommands.Count - 1);
                    }
                }

                OnShowHints?.Invoke();
            }

            private void SetSelectedHitPlaceholder()
            {
                var hintIndex = consoleInput.SelectedHint == -1 ? 0 : consoleInput.SelectedHint;

                var placeholder = normalCommands[hintIndex] + " ";
                
                
                for (int i = 0; i < arguments[hintIndex].Count; i++)
                {
                    placeholder += arguments[hintIndex][i].ArgumentName + " ";
                }

                OnRecalculatePlaceholder?.Invoke(placeholder);
            }

            public string GetTargetHint()
            {
                return normalCommands[consoleInput.SelectedHint];
            }
            
            public void SetArgumentsHint(int wordsCount)
            {
                string commandPlaceholder = "";
                
                var commandText = consoleInput.GetText().Trim().ToLower().Split(' ')[0];
                
                var commandClass = commands.Find(x => x.CommandsList.Find(y => y.CommandBase == commandText) != null);
                if (commandClass != null)
                {
                    var command = commandClass.CommandsList.Find(x => x.CommandBase == commandText);
                    if (command.Arguments.Count != 0)
                    {
                        commandPlaceholder = consoleInput.GetText() + " ";
                        commandPlaceholder = commandPlaceholder.Replace("  ", " ");
                    
                        for (int i = wordsCount - 1; i < command.Arguments.Count; i++)
                        {
                            if (i < command.Arguments.Count)
                            {
                                commandPlaceholder += command.Arguments[i].ArgumentName + " ";
                            }
                        }
                    }
                }
                OnRecalculatePlaceholder.Invoke(commandPlaceholder);
            }



            public void UpdateCommandsList()
            {
                var text = consoleInput.GetText().Replace("  ", " ");
                consoleInput.SetText(text);

                var wordsCount = text.Trim().Split(' ').Length;


                if (wordsCount == 0 || wordsCount == 1)
                {
                    CalculateCommandsAndArguments();
                }

                
                if (wordsCount != 0)
                {
                    SetArgumentsHint(wordsCount);
                }
            }

            public void CalculateCommandsAndArguments()
            {
                normalCommands.Clear();
                arguments.Clear();

                var text = consoleInput.GetText().Trim();
                if (text.Length != 0 && text.First() == '/')
                {
                    foreach (var scripts in commands)
                    {
                        foreach (var command in scripts.CommandsList)
                        {
                            if (text.Length <= command.CommandBase.Length)
                            {
                                if (command.CommandBase.Contains(text))
                                {
                                    normalCommands.Add(command.CommandBase);
                                    arguments.Add(command.Arguments);
                                }
                            }
                        }
                    }
                }
            }

            public string GetCommandText(int i)
            {
                return normalCommands[i];
            }
        }
    }
}