using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConsoleShell
{
    public partial class ConsoleService : MonoBehaviour
    {
        private static ConsoleService Instance;

        [SerializeField] private KeyCode consoleKey = KeyCode.F2;

        [SerializeField, HideInInspector] private ConsoleCommands consoleCommands;
        [SerializeField] private ConsoleVisuals consoleVisuals;
        [SerializeField] private ConsoleHintsVisuals consoleHintsVisuals;
        [SerializeField] private ConsoleHistory consoleHistory;
        [SerializeField] private ConsoleLogger consoleLogger;
        [SerializeField] private ConsoleOutput consoleOutput = new ConsoleOutput();
        
        private HintsSolver hintsSolver = new HintsSolver();
        private ConsoleInput consoleInput = new ConsoleInput();

        private bool cursorVisible;
        private CursorLockMode cursorMode;

        private bool isOpened;

        public event Action<string> OnShowMessage;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            
            consoleLogger.Init();
            
            consoleVisuals.Init(this);
            consoleVisuals.Input.onSubmit.AddListener(OnSubmit);
            consoleVisuals.Input.onValueChanged.AddListener(OnChangeText);
            
            consoleInput.Init(consoleVisuals.Input, consoleHintsVisuals.HintsCount);
            consoleInput.OnUserAction += consoleHistory.CheckInput;
            consoleInput.OnUserAction += OnUserInput;
            
            hintsSolver.Init(consoleInput, consoleCommands);
            consoleHistory.Init(consoleInput);
            consoleHintsVisuals.Init(hintsSolver, consoleInput, consoleVisuals);
            
            consoleOutput.Init();
            
            Application.logMessageReceived -= consoleOutput.OnReceivedUnityMessage;
            Application.logMessageReceived += consoleOutput.OnReceivedUnityMessage;
            
            consoleCommands.ReloadShellCommands();
            
            hintsSolver.HideHints();
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= consoleOutput.OnReceivedUnityMessage;
        }

        public static ConsoleCommands GetCommandsStatic() => Instance.GetCommands();
        public ConsoleCommands GetCommands() => consoleCommands;
        private void OnSubmit(string text) => consoleVisuals.Input.ActivateInputField();

        private void OnChangeText(string text)
        {
            text = text.Replace("\n", "");
            if (text.Trim() == string.Empty)
            {
                consoleHistory.ResetHistory();
                hintsSolver.HideHints();
            }
            else
            {
                hintsSolver.UpdateSolver();
            }
        }
        
        private void OnUserInput(ConsoleInput.ESelectionState state)
        {
            switch (state)
            {
                case ConsoleInput.ESelectionState.None:
                    hintsSolver.UpdateSolver();
                    break;
                case ConsoleInput.ESelectionState.OutOfHints:
                    var targetHistoryItem = consoleHistory.NextItem();
                    if (targetHistoryItem != null)
                    {
                        consoleInput.SetText(targetHistoryItem);
                        hintsSolver.UpdateSolver();
                        hintsSolver.HideHints(true);
                    }
                    break;
                case ConsoleInput.ESelectionState.Up:
                    hintsSolver.UpdateSolver();
                    break;
                case ConsoleInput.ESelectionState.Down:
                    hintsSolver.UpdateSolver();
                    break;
                case ConsoleInput.ESelectionState.Enter:
                    if (!consoleInput.IsText(string.Empty))
                    {
                        if (TrySetSelectedHitText()) return;

                        consoleHistory.AddInHistory(consoleInput.GetText());

                        consoleOutput.LogText(consoleInput.GetText(), string.Empty, ELogType.Log);
                        CalculateText(consoleInput.GetText());
                        consoleInput.SetText("");
                        
                        consoleHistory.ResetHistory();
                    }

                    break;
            }
        }
        

        private void LateUpdate()
        {
            OpenClose();
            if (isOpened)
            {
                consoleInput.Update();
            }
        }


        private bool TrySetSelectedHitText()
        {
            if (consoleInput.SelectedHint > -1)
            {
                if (hintsSolver.CommandsCount != 0)
                {
                    var targetHint = hintsSolver.GetTargetHint();
                    if (targetHint.Contains(consoleInput.GetText()))
                    {
                        consoleInput.SetText(targetHint + " ");
                        consoleInput.ResetIndex();
                        hintsSolver.HideHints();
                        hintsSolver.UpdateSolver();
                        return true;
                    }
                }
            }

            return false;
        }
        
        private void CalculateText(string inputText)
        {
            var items = inputText.Trim().ToLower().Split(' ');

            if (items.Length >= 1)
            {
                var commandsClass = consoleCommands.Commands.Find(x => x.CommandsList.Find(y => y.CommandBase == items[0]) != null);
                if (commandsClass != null)
                {
                    var command = commandsClass.CommandsList.Find(y => y.CommandBase == items[0]);
                    if (command != null)
                    {
                        command.Execute(items.Skip(1).ToArray());
                        return;
                    }
                }

                ConsoleLogger.Log("Command not found", ELogType.CmdException);
            }
        }
        
        private void OpenClose()
        {
            if (Input.GetKeyDown(consoleKey))
            {
                OpenCloseToggle();
            }
        }

        public void OpenCloseToggle()
        {
            isOpened = !isOpened;
            if (isOpened)
            {
                Instance.consoleVisuals.AnimateShow();

                cursorVisible = Cursor.visible;
                cursorMode = Cursor.lockState;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                HideConsole();
            }
        }

        public void CopyTextFromItem(ConsoleLine text)
        {
            var textToCopy = GetTextWithoutFormatting(text.GetLog());
            text.ToggleFullText();
            GUIUtility.systemCopyBuffer = textToCopy;
            OnShowMessage?.Invoke("Message copied");
        }


        public void ToggleErrors()
        {
            consoleOutput.ToggleLogType(ConsoleOutput.EAcceptedLogTypes.Errors);
        }

        public void ToggleWarnings()
        {
            consoleOutput.ToggleLogType(ConsoleOutput.EAcceptedLogTypes.Warnings);
        }

        public void ToggleLogs()
        {
            consoleOutput.ToggleLogType(ConsoleOutput.EAcceptedLogTypes.Logs);
        }
        

        public static string GetTextWithoutFormatting(string text)
        {
            var textToCopy = text;
            if (textToCopy.Length >= 1)
            {
                if (textToCopy.Last() == '\n')
                {
                    textToCopy = textToCopy.Substring(0, textToCopy.Length - 1);
                }
            }
            
            foreach (var loggerDataColor in Instance.consoleLogger.LoggerData.Colors)
            {
                textToCopy = textToCopy.Replace($"<color={loggerDataColor.ColorHex}>", "");
            }

            textToCopy = textToCopy.Replace("</color>", "");

            return textToCopy;
        }

        public static void HideConsole()
        {
            Instance.consoleVisuals.AnimateHide();

            Instance.hintsSolver.HideHints();
            
            Cursor.visible = Instance.cursorVisible;
            Cursor.lockState = Instance.cursorMode;

            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            else
            {
                new GameObject("EventSystem").AddComponent<EventSystem>();
            }

            Instance.isOpened = false;
        }

        public static void ClearText()
        {
            Instance.consoleOutput.ClearOutput();
        }

        public static List<string> GetTextStatic()
        {
            return Instance.consoleOutput.AllMessages; 
        }
    }
}
