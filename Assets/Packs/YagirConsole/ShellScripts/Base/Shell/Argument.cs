namespace ConsoleShell
{
    public class Argument
    {
        private string argumentName;
        private EArgumentType type;

        private float numberValue = 0;
        private string stringValue = "";
        private bool logicValue = false;
        
        private bool isRecuired;

        public bool LogicValue => logicValue;

        public string StringValue => stringValue;

        public float NumberValue => numberValue;

        public EArgumentType Type => type;
        public string ArgumentName => argumentName;

        public bool IsRecuired => isRecuired;

        public Argument(string argumentName, EArgumentType type, bool isRecuired = true, float defaultNumber = 0f, string defaultString = "", bool defaultBool = false)
        {
            numberValue = defaultNumber;
            stringValue = defaultString;
            logicValue = defaultBool;

            this.isRecuired = isRecuired; 
            
            this.argumentName = argumentName;
            this.type = type;
        }


        public Argument(string value)
        {
            stringValue = value;
            if (float.TryParse(value, out numberValue))
            {
                type = EArgumentType.Number;
                return;
            }

            if (bool.TryParse(value, out logicValue))
            {
                type = EArgumentType.Bool;
                return;
            }

            type = EArgumentType.String;
        }


        public static bool operator ==(Argument obj1, Argument obj2)
        {
            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            if (obj1.type != obj2.type)
            {
                if (obj1.type == EArgumentType.String && obj2.type != EArgumentType.String)
                {
                    obj2.type = EArgumentType.String;
                    return true;
                }
            }
            
            return obj1.type == obj2.type;
        }

        public static bool operator !=(Argument obj1, Argument obj2)
        {
            return !(obj1 == obj2);
        }
            
        public override string ToString()
        {
            return $"Argument `{ArgumentName}`: " + this.type + " = " + 
                   (
                       (this.type == EArgumentType.Bool ? this.LogicValue.ToString() : 
                           (this.type == EArgumentType.Number ? this.numberValue.ToString() : this.stringValue))
                   );
        }

        public void SetName(string s)
        {
            argumentName = s;
        }
    }
}