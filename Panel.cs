using UnityEngine;
using System;
using System.Collections.Generic;
using DotLiquid;

namespace UIGenerator
{
    public enum EventType
    {
      UnityAction,
      SystemAction
    }

    public class Event
    {
      public string    Name;
      public string    Params;
      public EventType Type;
    }

    [Serializable]
    public class Panel : ILiquidizable
    {
        public string name;
        public Element[] elements;

        public string ClassName {
            get {
                return this.name + "Panel";
            }
        }

        public string PublicEvents
        {
            get {
                List<Event> events = new List<Event>();

                for(int i = 0; i < elements.Length; i++)
                {
                  Element el = elements[i];
                  if (el.EmitsEvent)
                  {
/*
 *                      string eventName = string.Format("UIGenerator.Normalize(el.Name);
 *                      events.Add( new Event {
 *                          Name = eventName,
 *                          Params = 
 *
 *                      } );
 *
 */
                  }
                }
                return "// There is no public events.";
            }
        }

        public string ElementsHandlers {
            get {
                return "// There is no elements handlers.";
            }
        }

        public string FileName {
            get {
                return string.Format("Assets/Bully.UI/Source/Panels/{1}.cs",
                    UIGenerator.s_config.ProjectName, ClassName);
            }
        }

        public Type ComponentType {
            get {
                string typename = string.Format("{0}.{1}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                        UIGenerator.s_config.ProjectNamespace,
                        ClassName);

                return Type.GetType(typename);
            }
        }

        public object ToLiquid()
        {
            //Debug.Log("ToLiquid -> elements : " + ClassName + " ; " + elements.Length);
            return new {
                ClassName = ClassName,
                PublicEvents = "",
                elements = elements
            };
        }
    }
}
