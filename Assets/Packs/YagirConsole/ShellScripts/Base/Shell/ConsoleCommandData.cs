using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ConsoleShell
{
    public class ConsoleCommandData
    {
        private string commandBase;
        private List<Argument> arguments = new List<Argument>();
        public Action<ArgumentsShell> Action;


        public List<Argument> Arguments => arguments;

        public string CommandBase => commandBase;
        

        public ConsoleCommandData(string commandBase, List<Argument> arguments)
        {
            this.commandBase = commandBase;
            this.arguments = arguments;
        }

        public void Execute(string[] arguments)
        {
            var isValid = false;
            var args = StringToArguments(arguments, out isValid);

            if (isValid)
            {
                var shell = new ArgumentsShell(args);
                try
                {
                    if (Action != null)
                    {
                        Action?.Invoke(shell);
                    }
                    else
                    {
                        ConsoleLogger.Log("Command without action", ELogType.CmdException);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        
        private List<Argument> StringToArguments(string[] arguments, out bool isCan)
        {
            var args = new List<Argument>();

            isCan = true;
            for (int i = 0; i < arguments.Length; i++)
            {
                args.Add(new Argument(arguments[i]));
            }

            if (args.Count <= this.arguments.Count)
            {

                for (int i = 0; i < this.arguments.Count; i++)
                {
                    if (i < args.Count)
                    {
                        if (this.arguments[i] != args[i])
                        {
                            isCan = false;
                            ConsoleLogger.Log("Argument types do not match", ELogType.CmdException);
                            break;
                        }
                        else
                        {
                            args[i].SetName(this.arguments[i].ArgumentName);
                        }
                    }
                    else
                    {
                        if (this.arguments[i].IsRecuired)
                        {
                            isCan = false;
                            ConsoleLogger.Log("Invalid number of arguments", ELogType.CmdException);
                            break;
                        }
                        else
                        {
                            args.Add(new Argument(this.arguments[i].ArgumentName, this.arguments[i].Type, this.arguments[i].IsRecuired, this.arguments[i].NumberValue, this.arguments[i].StringValue, this.arguments[i].LogicValue));
                        }
                    }
                }

                if (isCan)
                {
                    return args;
                }
            }
            else
            {
                ConsoleLogger.Log("Invalid number of arguments", ELogType.CmdException);
            }
            isCan = false;
            return args;
        }
    }
}