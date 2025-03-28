using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class ConsoleHintsVisuals
        {
            [SerializeField] private GameObject hintsHolder;
            [SerializeField] private TMP_Text commandHint;
            [SerializeField] private List<ConsoleHintItem> hintsList;
            
            
            
            private ConsoleInput consoleInput;
            private HintsSolver hintsSolver;
            public int HintsCount => hintsList.Count;

            public void Init(HintsSolver hintsSolver, ConsoleInput consoleInput, ConsoleVisuals consoleVisuals)
            {
                this.hintsSolver = hintsSolver;
                this.consoleInput = consoleInput;


                for (int i = 0; i < hintsList.Count; i++)
                {
                    hintsList[i].Init(consoleVisuals.SelectedColor, consoleVisuals.SelectedColorText);
                    hintsList[i].transform.SetSiblingIndex(i);
                }
                
                hintsSolver.OnRecalculatePlaceholder += UpdatePlaceholderVisuals;
                hintsSolver.OnHideHints += HideHintsVisuals;
                hintsSolver.OnShowHints += ShowHintsVisuals;
                hintsSolver.UpdateSelectedVisuals += UpdateSelectedHintsVisuals;
            }
            
            
            private void ShowHintsVisuals()
            {
                hintsHolder.gameObject.SetActive(true);

                if (hintsSolver.CommandsCount == 1)
                {
                    if (consoleInput.GetText().Trim().Contains(hintsList[0].GetText()) || consoleInput.GetText().Trim().Split(' ').Length > 1)
                    {
                        for (int i = 0; i < hintsList.Count; i++)
                        {
                            hintsList[i].gameObject.SetActive(false);
                        }
                        return;
                    }
                }
                
                for (int i = 0; i < hintsList.Count; i++)
                {
                    if (i < hintsSolver.CommandsCount)
                    {
                        hintsList[i].SetText(hintsSolver.GetCommandText(i));
                        hintsList[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        hintsList[i].gameObject.SetActive(false);
                    }
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(hintsHolder.GetComponent<RectTransform>());
            }

            private void HideHintsVisuals()
            {
                hintsHolder.gameObject.SetActive(false);
                for (int i = 0; i < hintsList.Count; i++)
                {
                    hintsList[i].Deselect();
                }
            }
            private void UpdatePlaceholderVisuals(string text)
            {
                commandHint.text = text;
            }
            
            private void UpdateSelectedHintsVisuals()
            {
                for (int i = 0; i < hintsList.Count; i++)
                {
                    if (i == consoleInput.SelectedHint)
                    {
                        hintsList[i].Select();
                    }
                    else
                    {
                        hintsList[i].Deselect();
                    }
                }
            }


        }
    }
}