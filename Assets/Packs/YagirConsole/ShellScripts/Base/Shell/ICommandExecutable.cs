using System.Collections.Generic;

namespace ConsoleShell
{
    public interface ICommandExecutable
    {
        public List<ConsoleCommandData> CommandsList { get; }
    }
}