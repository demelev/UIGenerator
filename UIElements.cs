using UnityEngine;
using UnityEditor;
using System;
using DotLiquid;
using System.Reflection;
using UnityButton = UnityEngine.UI.Button;

namespace UIGenerator
{
    [Serializable]
    [LiquidType("Name", "type", "Declaration")]
    public class Element
    {
        public string name;
        public string Name { get { return name; } }

        virtual public bool EmitsEvent {
            get {
                return false;
            }
        }

        public virtual void Visit(Panel panelDescr, PanelBase panelUI) 
        {
        }

        public virtual string Declaration() 
        {
            return "Element declaration";
        }
/*
 *
 *        public virtual object ToLiquid()
 *        {
 *            return new {
 *                name = name
 *            };
 *        }
 */
    }

    [Serializable]
    [LiquidType("Name", "text", "emits", "Declaration")]
    public class Button : Element
    {
        public string text;
        public string[] emits;

        override public bool EmitsEvent {
            get {
                return emits != null;
            }
        }

        public override string Declaration() 
        {
            return "Button " + NamesFilter.PrivateMember(name, "Button") + ";";
        }

        static UnityButton CreateButton( string name )
        {
            var button = GameObject.Instantiate(PrefabsHelper.Prefabs["Button"]);
            button.name = name;
            return button.GetComponent<UnityButton>();
        }

        override public void Visit(Panel panelDescr, PanelBase panelUI) 
        {
            var fields = panelDescr.ComponentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            string button_name = NamesFilter.PrivateMember(this.Name, "Button");

            var button = CreateButton(button_name);
            button.transform.SetParent(panelUI.transform);

            foreach(var field in fields)
            {
                if (field.Name == button_name)
                {
                    field.SetValue(panelUI, button);
                }
            }
        }
    }

    [Serializable]
    [LiquidType("name", "text", "type")]
    public class Text : Element
    {
        public string text;
    }

    [Serializable]
    [LiquidType("horizontal", "type")]
    public class List : Element
    {
        public bool horizontal;
        public string list_item_name;
    }
}
