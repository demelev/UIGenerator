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

[CustomEditor(typeof(PanelsGroup))]
public class PanelsGroupEditor : Editor
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

        var t = (PanelsGroup) target;

        GUILayout.Space(10);

        var old = GUI.color;

        foreach (var p in t.gameObject.GetComponentsInChildren<PanelBase>(true)) {

           var go = p.gameObject;

           GUI.color = go.activeSelf ? Color.red : Color.white;

           if (GUILayout.Button( go.name )) {
              go.SetActive( !go.activeSelf );
           }
        }

        GUI.color = old;

    }
    #endregion



}
