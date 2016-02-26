using UnityEngine;
using System;
using System.Collections.Generic;
using DotLiquid;
using System.Reflection;

namespace UIGenerator
{
    public enum EventType
    {
      UnityAction,
      SystemAction
    }

    [Serializable]
    [LiquidType("Declaration")]
    public class Event
    {
      public string    Declaration {
          get {
              if (string.IsNullOrEmpty(parameters))
              {
                  return string.Format("public event Action {0};", name);
              }
              else
              {
                  return string.Format("public event Action<{0}> {1};", parameters, name );
              }
          }
      }

      public string    name;
      public string    parameters;
    }

    [Serializable]
    [LiquidType("Name", "Events", "Buttons", "ClassName", "PublicEvents")]
    public class Panel
    {
        public string name;
        public string Name { get { return name; } }

        public Event[] events;
        public Event[] Events {
            get {
                if (events == null)
                    return new Event[0];
                else
                    return events;
            }
        }

        public Element[] elements;
        public Element[] Elements {
            get {
                return elements;
            }
        }

        public string ClassName {
            get {
                return this.name + "Panel";
            }
        }

        public string PublicEvents
        {
            get {
/*
 *                List<Event> events = new List<Event>();
 *
 *                for(int i = 0; i < elements.Length; i++)
 *                {
 *                  Element el = elements[i];
 *                  if (el.EmitsEvent)
 *                  {
 *                  }
 *                }
 */
                return "// There is no public events.";
            }
        }

        public Button[] Buttons {
            get {
                var list = new List<Button>();

                if (elements != null)
                foreach(var element in elements)
                {
                    Button button = element as Button;
                    if (button == null)
                        continue;

                    list.Add(button);
                }

                return list.ToArray();
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

        /*
         *public object ToLiquid()
         *{
         *    //Debug.Log("ToLiquid -> elements : " + ClassName + " ; " + elements.Length);
         *    return new {
         *        ClassName = ClassName,
         *        PublicEvents = "",
         *        elements = elements
         *    };
         *}
         */
    }
}
