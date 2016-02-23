using System;

namespace UIGenerator
{
    [Serializable]
    public class Element
    {
        public string Name;
        virtual public bool EmitsEvent {
            get {
                return false;
            }
        }

        public virtual void Visit(PanelBase panel) 
        {
        }
    }

    [Serializable]
    public class Button : Element
    {
        public string text;
        public string[] emits;

        override public bool EmitsEvent {
            get {
                return emits != null;
            }
        }
    }

    [Serializable]
    public class Text : Element
    {
        public string text;
    }

    [Serializable]
    public class List : Element
    {
        public bool horizontal;
        public string list_item_name;
    }
}
