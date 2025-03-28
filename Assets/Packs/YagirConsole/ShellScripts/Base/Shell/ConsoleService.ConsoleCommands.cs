using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class ConsoleCommands
        {
            [SerializeField] private List<string> commandsFullNamePath = new List<string>(10);

            private List<ICommandExecutable> commands = new List<ICommandExecutable>(100);
            public List<ICommandExecutable> Commands => commands;

            public List<string> CommandsFullNamePath => commandsFullNamePath;

            public void ReloadShellCommands()
            {

                Commands.Clear();
                var types = Assembly.GetExecutingAssembly().GetTypes();
                for (int i = 0; i < CommandsFullNamePath.Count; i++)
                {
                    var id = i;
                    var q = from t in types
                        where t.IsClass && t.Namespace == CommandsFullNamePath[id] && t.GetInterface(nameof(ICommandExecutable)) == typeof(ICommandExecutable)
                        select t;


                    foreach (var type in q)
                    {
                        var item = (ICommandExecutable) Activator.CreateInstance(type);
                        if (item != null)
                        {
                            Commands.Add(item);
                        }
                    }
                }
            }


            public void AddCommandsNames(string name)
            {
                if (!CommandsFullNamePath.Contains(name))
                {
                    CommandsFullNamePath.Add(name);
                }
            }

            public void RemoveCommandsNames(string name)
            {
                CommandsFullNamePath.Remove(name);
            }
        }
    }
}