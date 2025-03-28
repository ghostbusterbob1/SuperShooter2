using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConsoleShell.Misc
{
    public class CounterWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text errorsCounter;
        [SerializeField] private TMP_Text warningsCounter;

        private int errors, warnings;
        private ConsoleService console;

        private void Awake()
        {
            Application.logMessageReceived += ApplicationOnlogMessageReceived;
            UpdateCounters();
        }

        private void ApplicationOnlogMessageReceived(string condition, string stacktrace, LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                    errors++;
                    break;
                case LogType.Assert:
                    break;
                case LogType.Warning:
                    warnings++;
                    break;
                case LogType.Log:
                    break;
                case LogType.Exception:
                    errors++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            UpdateCounters();
        }

        private void UpdateCounters()
        {
            errorsCounter.text = errors.ToString();
            warningsCounter.text = warnings.ToString();
        }

        public void OpenConsole()
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (!console)
            {
                console = FindObjectOfType<ConsoleService>();
            }

            if (console)
            {
                console.OpenCloseToggle();
            }
        }
    }
}
