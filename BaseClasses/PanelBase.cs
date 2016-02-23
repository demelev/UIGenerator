//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80 
//
//  Project: Chronic Angina Challenge
//  Date:    08/11/2015 11:18:07
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
/// PanelBase class.
/// </summary>
public class PanelBase : MonoBehaviour
{
    #region Public serialized variables
    #endregion



    #region Private variables
    #endregion



    #region Public methods and properties

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion



    #region Monobehavior methods
    #endregion




}
