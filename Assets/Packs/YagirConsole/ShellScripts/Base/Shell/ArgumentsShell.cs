using System.Collections.Generic;
using UnityEngine;

namespace ConsoleShell
{
    public class ArgumentsShell
    {
        private List<Argument> arguments;


        public ArgumentsShell(List<Argument> arguments)
        {
            this.arguments = arguments;
        }


        public bool GetBool(string name)
        {
            var val = FindByName(name);
            if (val == null) return false;
            return val.LogicValue;
        }

        public float GetNumber(string name)
        {
            var val = FindByName(name);
            if (val == null) return -1f;
            return val.NumberValue;
        }

        public float GetInteger(string name)
        {
            return Mathf.RoundToInt(GetNumber(name));
        }

        public string GetString(string name)
        {
            var val = FindByName(name);
            if (val == null) return null;
            return val.StringValue;
        }

        private Argument FindByName(string name) => arguments.Find(x => x.ArgumentName == name);
    }
}