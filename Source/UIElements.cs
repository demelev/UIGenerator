using UnityEngine;
using UnityEditor;
using System;
using DotLiquid;
using System.Reflection;
using UnityButton = UnityEngine.UI.Button;
using BullyInternal;

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

        internal virtual void Visit(Panel panelDescr, PanelBase panelUI) 
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
            return "Button " + NamesFormatter.PrivateMember(name, "Button") + ";";
        }

        static UnityButton CreateButton( string name )
        {
            var button = GameObject.Instantiate(PrefabsHelper.Prefabs["Button"]);
            button.name = name;
            return button.GetComponent<UnityButton>();
        }

        override internal void Visit(Panel panelDescr, PanelBase panelUI) 
        {
            var fields = panelDescr.ComponentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            string button_name = NamesFormatter.ObjectName(this.Name, "Button");

            var button = PrefabsHelper.CreateObject("Button", button_name);
            button.transform.SetParent(panelUI.transform);

            UnityButton ui_button = button.GetComponent<UnityButton>();

            string field_name = NamesFormatter.PrivateMember(this.Name, "Button");
            foreach(var field in fields)
            {
                if (field.Name == field_name)
                {
                    field.SetValue(panelUI, ui_button);
                }
            }
        }
    }

    [Serializable]
    [LiquidType("Name", "text")]
    public class Text : Element
    {
        public string text;

        override internal void Visit(Panel panelDescr, PanelBase panelUI) 
        {
            string text_name = NamesFormatter.ObjectName(this.Name, "Label");
            var text_object = PrefabsHelper.CreateObject("Text", text_name);
            var c_text = text_object.GetComponent<UnityEngine.UI.Text>();
            c_text.text = text;
            text_object.transform.SetParent(panelUI.transform);
        }
    }

    [Serializable]
    [LiquidType("horizontal")]
    public class List : Element
    {
        //TODO: User horizontal and create item prefab.
        public bool horizontal;
        public string list_item_name;

        override internal void Visit(Panel panelDescr, PanelBase panelUI) 
        {
            string list_name = NamesFormatter.ObjectName(this.Name, "List");

            var list = PrefabsHelper.CreateObject("Scroll View", list_name);
            list.transform.SetParent(panelUI.transform);
            list.transform.ResetTransform();

            /*
             *var fields = panelDescr.ComponentType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
             *string field_name = NamesFormatter.PrivateMember(this.Name, "List");
             *foreach(var field in fields)
             *{
             *    if (field.Name == field_name)
             *    {
             *        field.SetValue(panelUI, list);
             *    }
             *}
             */
        }
    }
}
