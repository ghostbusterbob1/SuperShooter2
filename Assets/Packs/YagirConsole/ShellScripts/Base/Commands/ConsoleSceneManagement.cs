using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConsoleShell.BaseCommands
{
    public class ConsoleSceneManagement : ConsoleManagementBase
    {
        public ConsoleSceneManagement()
        {
            LoadSceneCommand();
            UnloadSceneCommand();
            ListScenesCommand();
        }

        public void LoadSceneCommand()
        {
            AddCommand("/sceneload", new List<Argument>()
                {
                    new Argument("sceneName", EArgumentType.String),
                    new Argument("async", EArgumentType.Bool, false),
                    new Argument("addative", EArgumentType.Bool, false),
                },
                delegate(ArgumentsShell shell)
                {
                    string sceneName = shell.GetString("sceneName");
                    bool addative = shell.GetBool("addative");
                    bool async = shell.GetBool("async");
                    
                    ConsoleService.HideConsole();
                    if (async)
                    {
                        SceneManager.LoadScene(sceneName, addative ? LoadSceneMode.Additive : LoadSceneMode.Single);
                    }
                    else
                    {
                        SceneManager.LoadSceneAsync(sceneName, addative ? LoadSceneMode.Additive : LoadSceneMode.Single);
                    }
                }
            );
        }

        public void UnloadSceneCommand()
        {
            AddCommand("/sceneunload", new List<Argument>()
                {
                    new Argument("sceneName", EArgumentType.String)
                },
                delegate(ArgumentsShell shell)
                {
                    SceneManager.UnloadSceneAsync(shell.GetString("sceneName"));

                });
        }

        public void ListScenesCommand()
        {
            AddCommand("/scenelist", new List<Argument>(), delegate(ArgumentsShell shell)
            {
                List<string> scenes = new List<string>();

                for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                {
                    try
                    {
                        scenes.Add(SceneManager.GetSceneAt(i).name);
                    }
                    catch (Exception e)
                    {
                        
                        scenes.Add("Unknown scene - reload scenes list");
                    }
                }


                for (int i = 0; i < scenes.Count; i++)
                {
                    Debug.Log(" > `" + scenes[i] + "`");
                }
            });
        }
    }
}
