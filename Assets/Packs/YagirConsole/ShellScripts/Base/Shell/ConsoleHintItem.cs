using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConsoleShell
{
    public class ConsoleHintItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Image background;


        private Color defaultTextColor, defaultBackgroundColor;
        private Color activeTextColor, activeBackgroundColor;
        private bool selected;
        
        public void Init(Color consoleVisualsSelectedColor, Color consoleVisualsSelectedColorText)
        {
            defaultTextColor = text.color;
            defaultBackgroundColor = background.color;

            activeTextColor = consoleVisualsSelectedColorText;
            activeBackgroundColor = consoleVisualsSelectedColor;
        }
        

        public void Select()
        {
            if (!selected)
            {
                StopAllCoroutines();
                StartCoroutine(LerpColor(activeTextColor, activeBackgroundColor));
                selected = true;
            }
        }

        IEnumerator LerpColor(Color textColor, Color backgroundColor)
        {
            float time = 0;
            float targetTime = 0.2f;

            Color startText = text.color;
            Color startBackground = background.color;

            while (time <= targetTime)
            {
                yield return null;

                text.color = Color.Lerp(startText, textColor, time / targetTime);
                background.color = Color.Lerp(startBackground, backgroundColor, time / targetTime);


                time += Time.unscaledDeltaTime;
            }

            text.color = textColor;
            background.color = backgroundColor;
        }

        public void Deselect()
        {
            if (selected)
            {
                StopAllCoroutines();
                if (gameObject.active)
                {
                    StartCoroutine(LerpColor(defaultTextColor, defaultBackgroundColor));
                }
                else
                {
                    text.color = defaultTextColor;
                    background.color = defaultBackgroundColor;
                }

                selected = false;
            }
        }

        public void SetText(string text)
        {
            this.text.text = text;
        }

        public string GetText()
        {
            return text.text;
        }
    }
}
