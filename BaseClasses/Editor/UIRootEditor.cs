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


/// <summary>
/// PanelsGroup class.
/// </summary>

[CustomEditor(typeof(UIRoot))]
public class UIRootEditor : Editor
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

        var t = (UIRoot) target;

        GUILayout.Space(10);

        var old = GUI.color;

        foreach (var p in t.gameObject.GetComponentsInChildren<PanelsGroup>(true)) {

           var go = p.gameObject;

           GUI.color = go.activeSelf ? Color.red : Color.white;

           if (GUILayout.Button( go.name )) {
              go.SetActive( !go.activeSelf );
           }

           GUILayout.BeginVertical(GUILayout.Width(200));


            foreach (var s in go.GetComponentsInChildren<PanelBase>(true)) {

               var g = s.gameObject;

               GUI.color = g.activeSelf ? Color.red : Color.white;

               if (GUILayout.Button( g.name )) {
                  g.SetActive( !g.activeSelf );
               }
            }

           GUILayout.EndVertical();

        }

        GUI.color = old;

    }
    #endregion



}
