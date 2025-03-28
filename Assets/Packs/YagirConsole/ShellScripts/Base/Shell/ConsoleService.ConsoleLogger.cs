using System;
using System.Linq;
using UnityEngine;

namespace ConsoleShell
{
    [System.Serializable]
    public class ConsoleLogger
    {
        private static ConsoleLogger instance;
        public static ConsoleLogger Instance => instance;

        public ConsoleLoggerData LoggerData => loggerData;

        [SerializeField] private ConsoleLoggerData loggerData;

        public void Init()
        {
            instance = this;
        }

        public static string GetLog(string message, ELogType type)
        {
            if (message.Length == 0) return "Empty String";
            if (message.Last() == notFormattingFlag) return message.Replace(notFormattingFlag.ToString(), "");

            var color = Instance.LoggerData.GetHex(type);
            var spaces = Instance.LoggerData.MaxLogNameLength - type.ToString().Length;
            var str = $"<color={color}>[{type.ToString()}]";
            for (int i = 0; i < spaces; i++)
            {
                str += " ";
            }

            str += $" {message}</color>\n";

            return str;
        }

        private const char notFormattingFlag = '	';
        
        public static void Log(string message, ELogType type)
        {
            switch (type)
            {
                case ELogType.Error:
                    Debug.LogError(message);
                    break;
                case ELogType.Assert:
                    Debug.LogAssertion(message);
                    break;
                case ELogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case ELogType.Log:
                    Debug.Log(message);
                    break;
                case ELogType.Exception:
                    Debug.LogException(new Exception(message));
                    break;
                case ELogType.CmdException:
                    Debug.Log(GetLog(message, type) + notFormattingFlag);
                    break;
                case ELogType.CmdSuccess:
                    Debug.Log(GetLog(message, type) + notFormattingFlag);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}