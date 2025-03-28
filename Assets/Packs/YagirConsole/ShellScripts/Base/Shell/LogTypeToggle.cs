using UnityEngine;
using UnityEngine.UI;

namespace Packs.YagirConsole.ShellScripts.Base.Shell
{
    namespace ConsoleShell.Visuals
    {
        public class LogTypeToggle : MonoBehaviour
        {
            [SerializeField] private Graphic[] graphics;

            public void Toggle(bool state)
            {
                for (int i = 0; i < graphics.Length; i++)
                {
                    graphics[i].color = new Color(1,1,1, state ? 1f : 0.25f);
                }
            }
        }
    }
}
