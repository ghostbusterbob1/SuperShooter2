using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ConsoleShell.BaseCommands
{
    public class ConsoleSpawnManagement : ConsoleManagementBase
    {

        private Camera camera;

        public ConsoleSpawnManagement()
        {
            SpawnCommand();
            SpawnList();
        }

        public Camera GetCamera()
        {
            if (camera == null)
            {
                camera = Camera.main;
                if (camera == null)
                {
                    camera = Object.FindObjectOfType<Camera>();
                }
            }

            return camera;
        }

        public void SpawnList()
        {
            AddCommand("/createprimitivelist", new List<Argument>(), delegate(ArgumentsShell shell)
            {
                var names = Enum.GetNames(typeof(PrimitiveType));
                var str = "Type: ";
                for (int i = 0; i < names.Length; i++)
                {
                    str += names[i].ToLower() + (i != names.Length - 1 ? ", " : "");
                }

                Debug.Log(str);
            });
        }

        public void SpawnCommand()
        {
            AddCommand("/createprimitive", new List<Argument>()
                {
                    new Argument("type", EArgumentType.String)
                },
                delegate(ArgumentsShell shell)
                {
                    var userType = shell.GetString("type");

                    var isTypeExists = false;

                    var names = Enum.GetNames(typeof(PrimitiveType));

                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].ToLower() == userType)
                        {
                            isTypeExists = true;
                            break;
                        }
                    }


                    if (isTypeExists)
                    {
                        var upperFirst = userType[0].ToString().ToUpper();

                        userType = upperFirst + userType.Substring(1, userType.Length - 1);

                        PrimitiveType type = (PrimitiveType) Enum.Parse(typeof(PrimitiveType), userType);
                        var obj = GameObject.CreatePrimitive(type);

                        if (camera != null)
                        {

                            obj.transform.position = camera.transform.position + camera.transform.forward * 5;
                        }
                    }
                    else
                    {
                        ConsoleLogger.Log("Type missing", ELogType.CmdException);
                    }
                });
        }
    }
}
