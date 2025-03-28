using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConsoleShell
{
    public partial class ConsoleService
    {
        [System.Serializable]
        public class ConsoleVisuals
        {
            [SerializeField] private Canvas canvas;

            [SerializeField] private TMP_InputField input;

            [Space] [SerializeField] private Color consoleColor;
            [SerializeField] private Color selectedColor;
            [SerializeField] private Color selectedColorText;
            [SerializeField] private List<Image> backgrounds;


            private RectTransform rectTransform;
            private ConsoleService consoleService;

            public TMP_InputField Input => input;

            public Color SelectedColor => selectedColor;

            public Color SelectedColorText => selectedColorText;

            public void Init(ConsoleService consoleService)
            {
                this.consoleService = consoleService;
                rectTransform = canvas.GetComponent<RectTransform>();
                canvas.enabled = false;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);


                for (int i = 0; i < backgrounds.Count; i++)
                {
                    backgrounds[i].color = consoleColor;
                }

                AnimateHide();
            }


            public void AnimateHide()
            {
                consoleService.StopAllCoroutines();
                consoleService.StartCoroutine(LerpSize(new Vector2(rectTransform.sizeDelta.x, 0), delegate
                {
                    canvas.enabled = false;
                }));
            }

            public void AnimateShow()
            {
                canvas.enabled = true;
                
                consoleService.StopAllCoroutines();
                consoleService.StartCoroutine(LerpSize(new Vector2(rectTransform.sizeDelta.x, 800), null));

                input.Select();
            }


            IEnumerator LerpSize(Vector2 targetSize, Action onEnd)
            {
                float time = 0;
                float targetTime = 0.2f;

                Vector2 startScale = rectTransform.sizeDelta;

                while (time <= targetTime)
                {
                    yield return null;

                    rectTransform.sizeDelta = Vector2.Lerp(startScale, targetSize, time / targetTime);
                    time += Time.unscaledDeltaTime;
                }

                rectTransform.sizeDelta = targetSize;
                
                onEnd?.Invoke();
            }
        }
    }
}