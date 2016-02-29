//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Date:    02/25/2016 20:02:41
//  Creator: Emeliov Dmitri <demelev1990@gmail.com>
//
//===----------------------------------------------------------------------===//

using UnityEngine;
using UnityEngine.UI;
using UISystem;

namespace UIGenerator
{
    internal class MainSceneStructure
    {
        public UISystem.ScreenManager ScreenManager;
        public GameObject Canvas;
        public GameObject Panels;
        public GameObject Backgrounds;
        public GameObject Managers;

        public MainSceneStructure()
        {
            ScreenManager = GameObject.FindObjectOfType<ScreenManager>();
            if (ScreenManager == null)
            {
                var go = new GameObject("ScreenManager");
                ScreenManager = go.AddComponent(typeof(ScreenManager)) as ScreenManager;
            }

            Canvas = GameObject.FindWithTag("Canvas");
            if (Canvas == null)
            {
                Canvas = GameObject.Instantiate(PrefabsHelper.Prefabs["Canvas"]);
                Canvas.name = "Canvas";
                Canvas.tag = "Canvas";
            }

            Panels = GameObject.FindWithTag("Panels");
            if (Panels == null)
            {
                Panels = new GameObject("Panels");
                Panels.AddComponent<RectTransform>();
                Panels.transform.SetParent(Canvas.transform);
                Panels.tag = "Panels";
            }
        }
    }
}
