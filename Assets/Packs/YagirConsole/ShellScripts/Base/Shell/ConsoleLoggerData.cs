using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ConsoleShell
{
    [CreateAssetMenu(menuName = "Scriptable/ConsoleLogger", fileName = "ConsoleLogger", order = 0)]
    public class ConsoleLoggerData : ScriptableObject
    {
        [System.Serializable]
        public class LogColors
        {
            [SerializeField] private ELogType type;
            [SerializeField] private Color color;
            [SerializeField] private string colorHex;

            public string ColorHex => colorHex;

            public ELogType Type => type;

            public void Init()
            {
                colorHex = "#" + ColorUtility.ToHtmlStringRGB(color) + ((int)(color.a * 255f)).ToString("X");
            }
        }
        [SerializeField] private List<LogColors> colors;
        [SerializeField] private float maxLogNameLength;

        public float MaxLogNameLength => maxLogNameLength;

        public List<LogColors> Colors => colors;


        private void OnValidate()
        {
            for (int i = 0; i < Colors.Count; i++)
            {
                Colors[i].Init();
            }
            maxLogNameLength = Enum.GetNames(typeof(ELogType)).Max(x => x.Length) + 5;
        }


        public string GetHex(ELogType type)
        {
            return Colors.Find(x => x.Type == type).ColorHex;
        }
    } 
}