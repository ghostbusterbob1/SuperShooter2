using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ConsoleShell.BaseCommands
{
    public class ConsoleVisualManagement : ConsoleManagementBase
    {
        public ConsoleVisualManagement()
        {
            ClearCommand();
            HelpCommand();
            SaveCommand();
            QuitCommand();
        }
        
        private void SaveCommand()
        {
            AddCommand("/savelogtofile", new List<Argument>(), delegate(ArgumentsShell shell)
            {
                var folder = Application.persistentDataPath + "/ConsoleLogs";
                var paths = folder + "/log " + DateTime.Now.ToFileTimeUtc() + ".txt";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                File.WriteAllLines(paths, ConsoleService.GetTextStatic());

#if UNITY_EDITOR
                Application.OpenURL(Application.persistentDataPath);
#endif
            });
        }

        private void QuitCommand()
        {
            void ApplicationQuit(ArgumentsShell shell)
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            AddCommand("/quit", new List<Argument>(), ApplicationQuit);
            AddCommand("/q", new List<Argument>(), ApplicationQuit);
        }

        private void HelpCommand()
        {
            var command = AddCommand("/help", new List<Argument>(), null);
            
            command.Action += delegate(ArgumentsShell shell)
            {
                var commands = ConsoleService.GetCommandsStatic().Commands;

                var fullResult = "Commands List:";

                for (int i = 0; i < commands.Count; i++)
                {
                    for (int j = 0; j < commands[i].CommandsList.Count; j++)
                    {
                        if (commands[i].CommandsList[j].CommandBase == command.CommandBase) continue;


                        string str = commands[i].CommandsList[j].CommandBase;

                        for (int k = 0; k < commands[i].CommandsList[j].Arguments.Count; k++)
                        {
                            str += $" <u>({commands[i].CommandsList[j].Arguments[k].Type.ToString()})`{commands[i].CommandsList[j].Arguments[k].ArgumentName}`</u>";
                        }

                        fullResult += "\n    " + str;
                    }
                }

                Debug.Log(fullResult);
            };
        }


        public void ClearCommand()
        {
            AddCommand("/clear", new List<Argument>(), delegate(ArgumentsShell shell) { ConsoleService.ClearText(); });
        }
    }
}
