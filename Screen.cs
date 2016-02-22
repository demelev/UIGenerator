using System;

namespace UIGenerator
{
  [Serializable]
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
        return string.Format("Assets/Bully.Color/Source/Screens/{0}.cs", ClassName);
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
        return Type.GetType("DWColor." + ClassName + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
      }
    }


    static public string MakeFieldName(string name)
    {
      return Char.ToLower(name[0]) + name.Substring(1);
    }

    public string GetPanelsDeclaration()
    {
      string text = "";

      foreach (var panel in Panels)
      {
        Type panel_type = panel.ComponentType;

        string field_name_downcase = MakeFieldName(panel_type.Name);
        string field_decl = string.Format("\t[SerializeField]\n\t{0} _{1};\n\n",
            panel.ClassName, field_name_downcase);

        text += field_decl;
      }

      return text;
    }
  }
}
