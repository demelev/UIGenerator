using System;
namespace UIGenerator
{
    [Serializable]
    public class Panel
    {
        public string name;
        public Element[] elements;

        public string ClassName {
            get {
                return this.name + "Panel";
            }
        }

        public string FileName {
            get {
                return string.Format("Assets/Bully.Color/Source/View/Panels/{0}.cs", ClassName);
            }
        }

        public Type ComponentType {
            get {
                return Type.GetType("DWColor." + ClassName + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            }
        }
    }
}
