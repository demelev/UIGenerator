using System;
using System.Collections;
using DotLiquid;

namespace UIGenerator
{
    [Serializable]
    public class UIGeneratorConfig : ILiquidizable
    {
      public bool PrefabWithPrefix;
      public string ProjectName;
      public string ProjectNamespace;

      public object ToLiquid()
      {
          return new {
              PrefabWithPrefix = PrefabWithPrefix,
              ProjectName = ProjectName,
              ProjectNamespace = ProjectNamespace
          };
      }
    }
}
