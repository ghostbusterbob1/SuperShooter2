using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class ConsoleOutput
        {
            [Flags]
            public enum EAcceptedLogTypes
            {
                Errors = 8,
                Warnings = 16,
                Logs = 32
            }
            
            
            [SerializeField] private TextsPool copyTextsPool;
            private EAcceptedLogTypes acceptedLogTypes;
            private List<string> allMessages = new List<string>(100);
            private string lastMessage;
            private string lastTrace;
            private ConsoleLine lastTextInstance;

            public List<string> AllMessages => allMessages;

            public EAcceptedLogTypes AcceptedLogTypes => acceptedLogTypes;


            public void Init()
            {
                acceptedLogTypes = EAcceptedLogTypes.Errors | EAcceptedLogTypes.Logs | EAcceptedLogTypes.Warnings;
                copyTextsPool.Init();

            }

            public void OnReceivedUnityMessage(string condition, string stacktrace, LogType type)
            {
                var eLogType = (ELogType) (int) type;
                LogText(condition, stacktrace, eLogType);
            }

            public bool IsHaveStackTrace(ELogType type) => (type is ELogType.Error or ELogType.Exception or ELogType.Assert or ELogType.Warning);


            public void LogText(string message, string stacktrace, ELogType type)
            {
                if (message != lastMessage || stacktrace != lastTrace)
                {
                    var addTrace = stacktrace.Length > 0 && IsHaveStackTrace(type);

                    AllMessages.Add(message + (addTrace ? "\n" + stacktrace : ""));

                    lastMessage = message;
                    lastTrace = stacktrace;
                    
                    if (addTrace)
                    {
                        for (int i = copyTextsPool.Spawned.Count - 1; i >= 0; i--)
                        {
                            if (copyTextsPool.Spawned[i].Type != type)
                            {
                                break;
                            }

                            if (copyTextsPool.Spawned[i].IsText(message) && copyTextsPool.Spawned[i].IsStack(stacktrace))
                            {
                                copyTextsPool.Spawned[i].UpdateTextCounter();
                                return;
                            }
                        }
                    }

                    var textInstance = copyTextsPool.Get();
                    textInstance.SetText(message, addTrace ? stacktrace : String.Empty, type);
                    lastTextInstance = textInstance;
                    
                    textInstance.gameObject.SetActive(true);
                    textInstance.name = type.ToString();
                    textInstance.GetComponent<ContentSizeFitter>().SetLayoutVertical();

                    textInstance.SetActive(IsCanLogThisType(type));
                }
                else
                {
                    lastTextInstance.UpdateTextCounter();
                }

                string GetRepeatCount(int val)
                {
                    return $" <color=#FFFFFF50>[x{val}]\n";
                }
            }


            public void ClearOutput()
            {
                copyTextsPool.ClearAllSpawnedButtons();
            }


            private bool IsCanLogThisType(ELogType type)
            {
                if (type == ELogType.Error || type == ELogType.Exception || type == ELogType.CmdException)
                {
                    return AcceptedLogTypes.HasFlag(EAcceptedLogTypes.Errors);
                }

                if (type == ELogType.Assert || type == ELogType.Warning)
                {
                    return AcceptedLogTypes.HasFlag(EAcceptedLogTypes.Warnings);
                }

                if (type == ELogType.CmdSuccess || type == ELogType.Log)
                {
                    return acceptedLogTypes.HasFlag(EAcceptedLogTypes.Logs);
                }

                return false;
            }

            public void ToggleLogType(EAcceptedLogTypes type)
            {
                if (acceptedLogTypes.HasFlag(type))
                {
                    acceptedLogTypes &= ~type;
                }
                else
                {
                    acceptedLogTypes |= type;
                }

                UpdateListByLoggedTypes();
            }

            private void UpdateListByLoggedTypes()
            {
                for (var i = 0; i < copyTextsPool.Spawned.Count; i++)
                {
                    copyTextsPool.Spawned[i].SetActive(IsCanLogThisType(copyTextsPool.Spawned[i].Type));
                }
            }
        }
    }
}