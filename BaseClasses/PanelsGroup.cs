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
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Rand = UnityEngine.Random;
using Obj = UnityEngine.Object;


/// <summary>
/// PanelsGroup class.
/// </summary>
public class PanelsGroup : MonoBehaviour
{
    #region Public serialized variables
    #endregion



    #region Private variables
    #endregion



    #region Public methods and properties
    #endregion



    #region Monobehavior methods
    void Awake() {
       // Hide all panels on awake
       foreach (Transform t in transform) {
          t.gameObject.SetActive(false);
       }
    }
    #endregion



}
