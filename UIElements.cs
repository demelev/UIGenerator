using System;
using DotLiquid;

namespace UIGenerator
{
    [Serializable]
    public class Element: ILiquidizable
    {
        public string name;
        virtual public bool EmitsEvent {
            get {
                return false;
            }
        }

        public virtual void Visit(PanelBase panel) 
        {
        }

        public virtual object ToLiquid()
        {
            return new {
                name = name
            };
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

        override public object ToLiquid()
        {
            return Hash.FromAnonymousObject(new {
                type = "Button",
                name = name,
                text = text
            });
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

        override public object ToLiquid()
        {
            return new {
                type = "List",
                name = name
            };
        }
    }
}
