//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Project: VirtualWindow
//  Date:    11/26/2015 13:11:18
//  Creator: Mihailenco Eugene <emihailenco@bullyentertainment.com>
//
//===----------------------------------------------------------------------===//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;



namespace UISystem
{
    /// <summary>
    /// ScreenManager class.
    /// </summary>
    public class ScreenManager : PersistentSingleton<ScreenManager>
    {

#region Public serialized variables
        public List<BaseScreen> Screens;
        public BaseScreen Current { get { return Screens[_currentScreen]; }}
#endregion



#region Private variables
        private int _currentScreen;
#endregion



#region Public methods and properties

        [ContextMenu("Next Screen")]
        public void GotoNextScreen()
        {
            GotoScreen ( _currentScreen + 1 );
        }

        [ContextMenu("Prev Screen")]
        public void GotoPrewScreen()
        {
            GotoScreen ( _currentScreen - 1 );
        }

        public void GotoFirstScreen()
        {
            GotoScreen (0);
        }

        public void GotoFinalScreen()
        {
            GotoScreen (Screens.Count - 1);
        }
#endregion



#region Monobehavior methods

#if UNITY_EDITOR
        public override void Awake()
        {
            base.Awake();
            if (Screens.Count == 0)
            {
                Debug.LogError("No screens in manager", gameObject);
                Debug.Break();
                return;
            }
        }
#endif

        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), Current.Name);
        }

        void Start()
        {
            _currentScreen = 0;
            Current.OnScreenEnter();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Current.GotoNextState();
            }
        }


        void GotoScreen(int targ) {

            if (targ < 0 || targ >= Screens.Count)
                return;

            //var n = Current.Name;

            Current.OnScreenExit();
            _currentScreen = targ;
            Current.OnScreenEnter();

            //Debug.Log(string.Format("{0} -> {1}", n, Current.Name), gameObject);
        }

#endregion


    }
}
