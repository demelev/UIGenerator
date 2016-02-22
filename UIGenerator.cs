using UnityEngine;

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
    public class UIGenerator
    {
#region Private members
        private IFileSystem _fileSystem;
        private Context _context;
        private Dictionary<string, Template> _templates = new Dictionary<string, Template>();

        // Cache template request.
        private string _lastTemplateKey;
        private Template _lastTemplate;

        private UIGeneratorConfig _config;
        private NameGenerator _nameGenerator;
#endregion


#region Support methods

#region Private methods

        private void CheckFileSystem()
        {
            Directory.CreateDirectory("Assets/Bully.Color/Source/View/Panels");
            Directory.CreateDirectory("Assets/Bully.Color/Source/Screens");
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

            _fileSystem = new LocalFileSystem(dir);
            _context = new Context();
            Template.FileSystem = _fileSystem;
            CheckFileSystem();

            _config = new UIGeneratorConfig() {
                prefabWithPrefix = true
            };

            _nameGenerator = new NameGenerator(_config);
        }

        public void GenerateUI( UIDesciption uiDescription )
        {
            GenerateScripts(uiDescription);

            /*
             *GameObject dummy = new GameObject("dummy_");
             *var cor = dummy.AddComponent(typeof(Coroutiner)) as Coroutiner;
             *cor.DoAfterCompilation(() => {
             *    CreateUnityObjects(uiDescription);
             *});
             */
        }

        public void GenerateUIFromFile(string path)
        {
            string template = File.ReadAllText(path);
            // Determine file type.
            // var uiDesc = GenerateUIDescription(template, fileType);
            // GenerateUI(uiDesc);
        }

#endregion

#region Generator private methods

        private void GenerateScripts(UIDesciption uiDesc)
        {
            Template screen_template = GetTemplate("'screen'");
            foreach(var screen in uiDesc.Screens)
            {
                string filename = screen.FileName;
    
                if (File.Exists(filename))
                    continue;
    
                var stream = File.CreateText(filename);
                string result = screen_template.Render(Hash.FromAnonymousObject( new  {screen = screen}));
                stream.Write(result);
                stream.Close();
    
                AssetDatabase.ImportAsset(filename);
    
                GeneratePanelsScripts(screen);
            }
        }

        private void GeneratePanelsScripts(Screen screen)
        {
            Template panel_template = GetTemplate("panel");
            foreach (var panel in screen.Panels)
            {
                string className = panel.ClassName;
                string filename =  panel.FileName;

                var stream = File.CreateText(filename);
                string result = panel_template.Render(Hash.FromAnonymousObject( new {panel = panel}));
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
            if (config.prefabWithPrefix)
            {
                return "P_" + name;
            }
            else
                return name;
        }

    }

    public class UIGeneratorConfig
    {
        public bool prefabWithPrefix;
    }

    [Serializable]
    public class UIDesciption
    {
        public Screen[] Screens;
    }

    [Serializable]
    public class Panel
    {
        public string name;

        public string ClassName {
            get {
                return this.name + "Panel";
            }
        }

        public string FileName {
            get {
                return string.Format("Assets/Bully.Color/Source/View/Panels/{0}.cs", ClassName);
            }
        }

        public Type ComponentType {
            get {
                return Type.GetType("DWColor." + ClassName + ", Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            }
        }
    }

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

    public class ColorSupportClass 
    {
#region Panels Generator

        [MenuItem("Tools/Generators/PanelsGenerator")]
        static void OpenPanelsGenerator()
        {
            Screen[] screens = new Screen[]
            {
                new Screen { Name = "ColorRoomIPSelection",  Panels = new Panel[] {
                    new Panel { name = "ColorRoomList" }
                }},

                new Screen { Name = "CharacterIntro", Panels = new Panel[] {
                    new Panel { name = "ColorRoomSelectionConfirm" }
                }},

                new Screen { Name = "Drawing", Panels = new Panel[] {
                    new Panel { name = "CharacterSelection" },
                    new Panel { name = "BackgroundSelection" },
                    new Panel { name = "ToolSelection" },
                    new Panel { name = "ColorSelection" },
                    new Panel { name = "PropSelection" },
                    new Panel { name = "FrameSelection" },
                    new Panel { name = "NameSelection" }
                }},

                new Screen { Name = "BringToLife", Panels = new Panel[] {
                    new Panel { name = "BringToLifeUI" }
                }},

                new Screen { Name = "SaveAndShare", Panels = new Panel[] {
                    new Panel { name = "SaveAndShareUI" }
                }},
            };

            UIDesciption uiDescription = new UIDesciption();
            uiDescription.Screens = screens;

            UIGenerator generator = new UIGenerator("Assets/Bully.Core/Dependencies/UIGenerator/Templates");
            generator.GenerateUI(uiDescription);
        }

        [MenuItem("Tools/Generators/Check Yaml parser")]
        static void CheckYamlParser()
        {
            var serializer = new YamlSerializer();
            string yaml = serializer.Serialize(p);
            Point restored = serializer.Deserialize(yaml)[0] as Point;
            Debug.Log("Yaml : " + yaml);
            Debug.Log("Restores : " + restored);
        }


#endregion
    }
}
