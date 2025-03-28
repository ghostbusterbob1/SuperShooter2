using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ConsoleShell
{
    public class ConsoleLine : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private TMP_Text counter;
        [SerializeField] private string originalMessage;
        [SerializeField] private string originalStacktrace;
        [SerializeField] private ELogType type;

        private int repeatCount;

        public ELogType Type => type;

        public void UpdateTextCounter()
        {
            repeatCount++;
            if (repeatCount >= 99)
            {
                repeatCount = 99;
            }

            counter.text = $"[x{repeatCount}]";
        }

        public void SetText(string content, string stacktrace, ELogType type)
        {
            isShowed = false;
            repeatCount = 0;
            originalMessage = content;
            originalStacktrace = stacktrace;
            counter.text = String.Empty;
            ShortText(stacktrace, type);

            this.type = type;
        }

        private void ShortText(string stacktrace, ELogType type)
        {
            var tmp_text = originalMessage;


            if (!string.IsNullOrEmpty(stacktrace))
            {
                if (stacktrace.Length > 100)
                {
                    stacktrace = stacktrace.Substring(0, 200) + "...";
                }

                tmp_text += "\n" + stacktrace;
            }

            text.text = ConsoleLogger.GetLog(tmp_text, type);
        }


        public void SetActive(bool state)
        {
            text.enabled = state;
            counter.enabled = state;
        }

        public bool IsText(string message)
        {
            return originalMessage == message;
        }

        public bool IsStack(string stacktrace)
        {
            return originalStacktrace == stacktrace;
        }

        public string GetLog()
        {
            return originalMessage + (originalStacktrace.Length > 0 ? "\n" + originalStacktrace : "");
        }

        private bool isShowed = false;
        public void ToggleFullText()
        {
            isShowed = !isShowed;

            if (isShowed)
            {
                text.text = ConsoleLogger.GetLog(GetLog(), type);
            }
            else
            {
                ShortText(originalStacktrace, type);
            }
        }
    }
}
