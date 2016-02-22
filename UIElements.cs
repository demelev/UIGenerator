using System;

namespace UIGenerator
{
    [Serializable]
    public class Element
    {
        public string name;
        public virtual void Visit(PanelBase panel) 
        {
        }
    }

    [Serializable]
    public class Button : Element
    {
        public string text;
    }

    [Serializable]
    public class Text : Element
    {
        public string text;
    }
}
