//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Date:    02/25/2016 20:02:29
//  Creator: Emeliov Dmitri <demelev1990@gmail.com>
//
//===----------------------------------------------------------------------===//

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace UIGenerator
{
    public static class PrefabsHelper
    {
        public const string GeneratorRoot = "Assets/Bully.Core/Dependencies/UIGenerator";
        public const string PrefabsPath = "Assets/Bully.Core/Dependencies/UIGenerator/Prefabs";

        static public Dictionary<string, GameObject> Prefabs ;

        static public string GetPrefabPath(string name)
        {
            return Path.Combine(PrefabsPath, name + ".prefab");
        }

        static private GameObject LoadAsset(string name)
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>(GetPrefabPath(name));
        }

        static internal GameObject CreateObject(string prefabName, string name)
        {
            var prefab = Prefabs[prefabName];

            if (prefab == null)
            {
                Debug.LogError(string.Format("There is no prefab '{0}'.", name));
                return null;
            }
            
            var obj = GameObject.Instantiate(prefab);
            obj.name = name;
            return obj;
        }

        static public void LoadPrefabs()
        {
            Prefabs = new Dictionary<string, GameObject>();

            string[] list = new string[] {
                "Button", "Canvas", "Scroll View", "Text"
            };

            for(int i = 0; i < list.Length; i++)
            {
                Prefabs.Add(list[i], LoadAsset( list[i] ));
            }
        }

        static public void UnloadPrefabs()
        {
            Prefabs.Clear();
        }
    }
}
