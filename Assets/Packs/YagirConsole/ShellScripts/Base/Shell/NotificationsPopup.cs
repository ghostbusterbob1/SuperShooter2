using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ConsoleShell
{
    public class NotificationsPopup : MonoBehaviour
    {
        [SerializeField] private ConsoleService consoleService;
        [SerializeField] private RectTransform popup;
        [SerializeField] private AnimationCurve animation;
        [SerializeField] private TMP_Text text;
        [SerializeField] private float minValue, maxValue;
        [SerializeField] private float animationSpeed = 1;

        private float animationProgress;
        private void Awake()
        {
            consoleService.OnShowMessage += ConsoleServiceOnOnShowMessage;
            popup.anchoredPosition = new Vector2(minValue, popup.anchoredPosition.y);
        }

        private void ConsoleServiceOnOnShowMessage(string text)
        {
            this.text.text = text;
            popup.anchoredPosition = new Vector2(minValue, popup.anchoredPosition.y);
            animationProgress = 0;
            StopAllCoroutines();
            StartCoroutine(Animation());
        }


        IEnumerator Animation()
        {
            animationProgress = 0;
            var time = animation.keys.Last().time;
            while (animationProgress < time)
            {
                yield return null;
                animationProgress += Time.unscaledDeltaTime * animationSpeed;
                popup.anchoredPosition = new Vector2(Mathf.Lerp(minValue, maxValue, animation.Evaluate(animationProgress)), popup.anchoredPosition.y);
            }
        }
    }
}
