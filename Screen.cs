using UnityEngine;
using System;
using DotLiquid;

namespace UIGenerator
{
  [Serializable]
  [LiquidType("Name", "Panles", "ClassName", "GetPanelsDeclaration")]
  public class Screen
  {
    public string Name;
    public Panel[] Panels;

    public string ClassName {
      get {
        return this.Name + "Screen";
      }
    }

    public string FileName {
      get {
        //TODO: Make paths universal.
        return string.Format("Assets/Bully.{0}/Source/Screens/{1}.cs",
            UIGenerator.s_config.ProjectName, ClassName);
      }
    }

    public string PanelsGroupName {
      get {
        return Name + "Panels";
      }
    }

    public Type ComponentType {
      get {
        //TODO: Make Namespace universal.
        string temp = "{0}.{1}, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        string typename = string.Format(temp, UIGenerator.s_config.ProjectNamespace,
                                              ClassName);
        return Type.GetType(typename);
      }
    }


    static public string MakeFieldName(string name)
    {
      return Char.ToLower(name[0]) + name.Substring(1);
    }

    public string[] GetPanelsDeclaration()
    {
      string[] fields = new string[Panels.Length];
      int index = 0;

      foreach (var panel in Panels)
      {
        string field_name_downcase = NamesFilter.PrivateMember(panel.ClassName, ""); 
        string field_decl = string.Format("{0} _{1};",
            panel.ClassName, field_name_downcase);

        fields[index++] = field_decl;
      }

      return fields;
    }

    /*
     *public object ToLiquid()
     *{
     *    return new {
     *        Name = Name,
     *        ClassName = ClassName,
     *        PanelsDeclaration = GetPanelsDeclaration()
     *    };
     *}
     */
  }
}
