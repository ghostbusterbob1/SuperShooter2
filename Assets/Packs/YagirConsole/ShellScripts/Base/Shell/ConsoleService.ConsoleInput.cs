using System;
using TMPro;
using UnityEngine;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        public class ConsoleInput
        {
            public enum ESelectionState
            {
                None,
                OutOfHints,
                Up,
                Down,
                Enter
            }
            
            private int selectedHint = -1;
            private TMP_InputField inputField;
            private ESelectionState state;
            private int maxHintsCount;

            public Action<ESelectionState> OnUserAction;

            public int SelectedHint => selectedHint;

            public void Init(TMP_InputField inputField, int maxHintsCount)
            {
                this.maxHintsCount = maxHintsCount;
                this.inputField = inputField;
            }


            public void Update()
            {
                state = ESelectionState.None;
                ChangeSelected();

                if (state != ESelectionState.None)
                {
                    OnUserAction?.Invoke(state);
                }
            }

            private void ChangeSelected()
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectedHint--;
                    
                    if (SelectedHint < -1)
                    {
                        state = ESelectionState.OutOfHints;
                    }
                    else
                    {
                        state = ESelectionState.Up;
                    }
                    
                    if (selectedHint < -1)
                    {
                        selectedHint = -1;
                    }
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectedHint++;
                    state = ESelectionState.Down;
                    if (selectedHint >= maxHintsCount)
                    {
                        selectedHint = 0;
                    }
                }


                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Tab))
                {
                    state = ESelectionState.Enter;
                }
            }


            public void ResetIndex()
            {
                selectedHint = -1;
            }


            public string GetText()
            {
                return inputField.text;
            }

            public bool IsText(string text)
            {
                return GetText().Trim() == text.Trim();
            }

            public bool IsContains(string text)
            {
                return GetText().Contains(text);
            }

            public void SetText(string text)
            {
                inputField.text = text;
                inputField.caretPosition = text.Length;
                
                if (text == String.Empty)
                {
                    ResetIndex();
                }
            }

            public void SetIndex(int i)
            {
                selectedHint = i;
            }
        }
    }
}