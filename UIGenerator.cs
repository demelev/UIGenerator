using UnityEngine;
using UnityEditor.Callbacks;

#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DotLiquid;
using DotLiquid.FileSystems;
using System.Yaml.Serialization;

namespace UIGenerator
{
    public enum FileType
    {
        YAML
    }

    public class UIGenerator
    {
        static public bool ScriptsWasCreated
        {
            get {
                return EditorPrefs.GetBool("ScriptsWasCreated", false);
            }

            set {
                EditorPrefs.SetBool("ScriptsWasCreated", value);
            }
        }

        static public UIDesciption ActiveDescription;

#region Private members
        private IFileSystem _fileSystem;
        private Context _context;
        private Dictionary<string, Template> _templates = new Dictionary<string, Template>();

        // Cache template request.
        private string _lastTemplateKey;
        private Template _lastTemplate;

        static internal UIGeneratorConfig s_config;
        private NameGenerator _nameGenerator;
#endregion


#region Support methods

#region Private methods

        private void CheckFileSystem()
        {
            Directory.CreateDirectory("Assets/Bully.UI/Source/Panels");
            Directory.CreateDirectory(string.Format("Assets/Bully.{0}/Source/Screens", s_config.ProjectName));
            Directory.CreateDirectory("Assets/Bully.UI/Prefabs/Panels");
        }

        protected Template GetTemplate(string name)
        {
            if (_lastTemplateKey == name)
                return _lastTemplate;

            if (_templates.ContainsKey(name))
            {
                _lastTemplateKey = name;
                _lastTemplate = _templates[name];
            }
            else
            {
                Template temp = Template.Parse(_fileSystem.ReadTemplateFile(_context, name));
                _lastTemplate = _templates[name] = temp;
                _lastTemplateKey = name;
            }

            return _lastTemplate;
        }

#endregion

#endregion

#region Generator public methods
        public UIGenerator(string templatesDir)
        {
            string dir = System.IO.Directory.GetCurrentDirectory();
            dir = Path.Combine(dir, templatesDir);
            Debug.Log("Create filesystem from : " + dir);

            _fileSystem = new LocalFileSystem(dir);
            _context = new Context();
            Template.FileSystem = _fileSystem;

            s_config = new UIGeneratorConfig() {
                PrefabWithPrefix = true
            };

            _nameGenerator = new NameGenerator(s_config);
        }
        
        public UIGeneratorConfig Config {
            set {
                if (s_config != value)
                {
                    s_config = value;
                    _nameGenerator.config = value;
                }
            }
        }

        public void GenerateUIFromFile(string path)
        {
            string template = File.ReadAllText(path).Replace("!UI", "!UIGenerator");
            //TODO: Determine file type.
            var uiDesc = GenerateUIDescription(template, FileType.YAML);
            GenerateUI(uiDesc);
        }

        public void GenerateUI( UIDesciption uiDesc )
        {
            if (!UIGenerator.ScriptsWasCreated)
            {
                CheckFileSystem();
                GenerateScripts(uiDesc);
            }
            else
            {
                CreateUnityObjects(uiDesc);
            }
        }

#endregion

#region Generator private methods


        private UIDesciption GenerateUIDescription(string data, FileType type)
        {
            UIDesciption descr = null;

            switch(type)
            {
                case FileType.YAML: {
                    YamlSerializer sr = new YamlSerializer();
                    var docs = sr.Deserialize(data);
                    Config = docs[0] as UIGeneratorConfig;
                    descr = docs[1] as UIDesciption;
                } break;
            }

            return descr;
        }

        private void GenerateScripts(UIDesciption uiDesc)
        {
            CheckFileSystem();

            Template screen_template = GetTemplate("'screen'");
            foreach(var screen in uiDesc.Screens)
            {
                string filename = screen.FileName;
                /*
                 *Debug.Log("Create screen script: " + filename);
                 *continue;
                 */
    
                if (File.Exists(filename))
                    continue;
    
                var stream = File.CreateText(filename);

                string result = screen_template.Render(
                    Hash.FromAnonymousObject( new  {screen = screen, config = s_config})
                );

                stream.Write(result);
                stream.Close();
    
                GeneratePanelsScripts(screen);
                AssetDatabase.ImportAsset(filename);
            }

            UIGenerator.ScriptsWasCreated = true;
        }

        private void GeneratePanelsScripts(Screen screen)
        {
            Template panel_template = GetTemplate("'panel'");
            foreach (var panel in screen.Panels)
            {
                string className = panel.ClassName;
                string filename =  panel.FileName;
                /*
                 *Debug.Log("Create panel script: " + filename);
                 *continue;
                 */

                var stream = File.CreateText(filename);
                string result = panel_template.Render(
                    Hash.FromAnonymousObject( new {panel = panel, config = s_config})
                );

                stream.Write(result);
                stream.Close();
                AssetDatabase.ImportAsset(filename);
            }
        }

        private void CreateUnityObjects(UIDesciption uiDesc)
        {
            GameObject Screens = new GameObject("Screens");
            GameObject Panels = new GameObject("Panels");

            foreach (var screen in uiDesc.Screens)
            {
                GameObject sc = new GameObject(screen.ClassName);

                GameObject screen_panels_group = new GameObject(screen.PanelsGroupName);
                PanelsGroup panels_group_comp = screen_panels_group.AddComponent(typeof(PanelsGroup)) as PanelsGroup;
                screen_panels_group.transform.SetParent(Panels.transform);

                Type screen_type = screen.ComponentType;

                var screen_comp = sc.AddComponent(screen_type);
                sc.transform.SetParent(Screens.transform);

                var panels_field = typeof(BaseScreen).GetField("Panels");
                panels_field.SetValue(screen_comp, panels_group_comp);

                foreach (var panel in screen.Panels)
                {
                    GameObject pobj = new GameObject( _nameGenerator.PrefabName(panel.ClassName) );
                    var panel_type = panel.ComponentType;
                    var panel_comp = pobj.AddComponent(panel_type);
                    pobj.transform.SetParent(screen_panels_group.transform);

                    string field_name = "_" + Screen.MakeFieldName(panel_type.Name);
                    var fields = screen_type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                    //var field = screen_type.GetField(field_name, BindingFlags.NonPublic | BindingFlags.Instance);
                    foreach(var f in fields)
                    {
                        if(f.Name == field_name)
                            f.SetValue(screen_comp, panel_comp);
                    }

                    PrefabUtility.CreatePrefab("Assets/Bully.UI/Prefabs/Panels/" + pobj.name + ".prefab", pobj, ReplacePrefabOptions.ConnectToPrefab);
                }
            }
        }
#endregion
    }

    public class NameGenerator
    {
        public UIGeneratorConfig config;

        public NameGenerator(UIGeneratorConfig config)
        {
            this.config = config;
        }

        public string PrefabName(string name)
        {
            if (config.PrefabWithPrefix)
            {
                return "P_" + name;
            }
            else
                return name;
        }

    }


    [Serializable]
    public class UIDesciption
    {
        public Screen[] Screens;
        public Panel[] SharedPanels;
    }

    public class ColorSupportClass 
    {
#region Panels Generator

        [MenuItem("Tools/Generators/Generate UI scripts")]
        static void GenerateUIScripts()
        {
            UIGenerator.ScriptsWasCreated = false;
            //UIGenerator generator = new UIGenerator("Assets\\Bully.Core\\Dependencies\\UIGenerator\\Templates");
            UIGenerator generator = new UIGenerator("Assets/Bully.Core/Dependencies/UIGenerator/Templates");
            generator.GenerateUIFromFile("Assets/ui_description.yaml");
        }

        //[MenuItem("Tools/Generators/Generate Unity Objects")]
        [DidReloadScripts]
        static void GenerateUnityObjects()
        {
            Debug.Log("After scripts are loaded");
            // Check this flag in order to don't execute this method
            if (UIGenerator.ScriptsWasCreated)
            {
                //UIGenerator generator = new UIGenerator("Assets\\Bully.Core\\Dependencies\\UIGenerator\\Templates");
                UIGenerator generator = new UIGenerator("Assets/Bully.Core/Dependencies/UIGenerator/Templates");
                generator.GenerateUIFromFile("Assets/ui_description.yaml");
                UIGenerator.ScriptsWasCreated = false;
            }
        }
#endregion
    }
}
