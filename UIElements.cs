using System;
using DotLiquid;

namespace UIGenerator
{
    [Serializable]
    [LiquidType("name")]
    public class Element
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
    [LiquidType("name", "text", "emits", "type")]
    public class Button : Element
    {
        public string text;
        public string[] emits;
        public string type = "Button";

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
    [LiquidType("name", "text", "type")]
    public class Text : Element
    {
        public string type = "Text";
        public string text;
    }

    [Serializable]
    [LiquidType("name", "horizontal", "type")]
    public class List : Element
    {
        public string type = "List";
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
