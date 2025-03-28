using System;
using System.Collections.Generic;

namespace ConsoleShell
{
    public class ConsoleManagementBase : ICommandExecutable
    {
        public List<ConsoleCommandData> CommandsList => commands;
        
        protected List<ConsoleCommandData> commands = new List<ConsoleCommandData>();


        public ConsoleCommandData AddCommand(string commandName, List<Argument> arguments, Action<ArgumentsShell> action)
        {
            var command = new ConsoleCommandData(commandName, arguments);
            command.Action += action;
            commands.Add(command);

            return command;
        }
    }
}