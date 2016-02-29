//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80 
//
//  Project: Chronic Angina Challenge
//  Date:    08/13/2015 09:35:45
//  Creator: Mihailenco Eugene <emihailenco@bullyentertainment.com>
//
//===----------------------------------------------------------------------===//

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;
using UISystem;


/// <summary>
/// 
/// </summary>

[CustomEditor(typeof(ScreenManager))]
public class ScreenManagerEditor : Editor
{
    #region Public serialized variables
    #endregion



    #region Private variables
    #endregion



    #region Public methods and properties
    #endregion



    #region Monobehavior methods
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var go = (ScreenManager)target;


        if (GUILayout.Button("Generate List")) {

           go.Screens.Clear();

           foreach (var t in go.gameObject.GetComponentsInChildren<BaseScreen>()) {
              go.Screens.Add(t);
           }

        }

        

    }
    #endregion



}
