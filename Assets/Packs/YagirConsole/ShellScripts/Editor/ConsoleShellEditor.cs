using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ConsoleShell.Editor
{
    [CustomEditor(typeof(ConsoleService))]
    public class ConsoleShellEditor : UnityEditor.Editor
    {
        private bool foldoutIsOpen = false;
        private ConsoleService consoleService;

        private int commandsSelection = 0;
        
        private void OnEnable()
        {
            consoleService = target as ConsoleService;
            foldoutIsOpen = EditorPrefs.GetBool("consoleService_commands", false);
        }

        private void OnDisable()
        {
            EditorPrefs.SetBool("consoleService_commands", foldoutIsOpen);
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foldoutIsOpen = EditorGUILayout.Foldout(foldoutIsOpen, "Console Commands", true);

            if (foldoutIsOpen)
            {
                var subclassTypes = Assembly
                    .GetAssembly(typeof(ConsoleManagementBase))
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(ConsoleManagementBase)));

                List<string> list = new List<string>();
                Dictionary<string, int> counters = new Dictionary<string, int>();
                foreach (var cl in subclassTypes)
                {
                    var name = Path.GetFileNameWithoutExtension(cl.FullName);
                    if (!list.Contains(name))
                    {
                        list.Add(name);
                        counters.Add(name, 1);
                    }
                    else
                    {
                        counters[name]++;
                    }
                }

                var activePaths = consoleService.GetCommands().CommandsFullNamePath;
                foreach (var s in activePaths)
                {
                    if (!list.Contains(s))
                    {
                        list.Add(s);
                    }
                }


                for (int i = 0; i < list.Count; i++)
                {
                    var guiColor = GUI.color;
                    GUILayout.BeginHorizontal();
                    bool isActivePath = activePaths.Contains(list[i]);
                    if (GUILayout.Button(isActivePath ? "-" : "+", GUILayout.MinWidth(20), GUILayout.MaxWidth(20)))
                    {
                        if (isActivePath)
                        {
                            consoleService.GetCommands().RemoveCommandsNames(list[i]);
                            EditorUtility.SetDirty(consoleService);
                        }
                        else
                        {
                            consoleService.GetCommands().AddCommandsNames(list[i]);
                            EditorUtility.SetDirty(consoleService);
                        }
                    }

                    var count = 0;

                    if (counters.ContainsKey(list[i]))
                    {
                        count = counters[list[i]];
                    }
                    
                    GUI.color = activePaths.Contains(list[i]) ? Color.green : Color.red;
                    EditorGUILayout.LabelField(list[i] + $" [{count}]");
                    GUI.color = guiColor;
                    GUILayout.EndHorizontal();
                }
            }
        }
    }
}
