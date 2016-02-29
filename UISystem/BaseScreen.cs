//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80 
//
//  Project: Chronic Angina Challenge
//  Date:    8/20/2015 2:41:39 PM
//  Creator: Dmitrii Stavila <dstavila@bullyentertainment.com>
//
//===----------------------------------------------------------------------===//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using BullyFramework;

namespace UISystem
{
    /// <summary>
    /// BaseScreen class.
    /// </summary>
    public class BaseScreen : MonoBehaviour
    {
        protected PanelsGroup Panels;
        public string ScreenName { get; protected set; }

#region Public events and delegates
        public delegate void OnTriggerRaised_delegate(string trigger);
        public event OnTriggerRaised_delegate OnTriggerRaised;
        public event Action OnOutroDone;
        public event Action OnIntroDone;
#endregion

        //public System.Action OnScreenEnterAction;
        //public System.Action OnScreenExitAction;

#region Screen states
        private ScreenState _state;

        public ScreenState State
        {
            get {
                return _state;
            }

            set {

                if (_state == value)
                    return;

                _state = value;

                switch(_state)
                {
                    case ScreenState.Active:
                        {
                            OnBecomeActive();
                        } break;
                    case ScreenState.Inactive:
                        {
                            OnBecomeInactive();
                        } break;
                    case ScreenState.Intro:
                        {
                            OnBecomeIntro();
                        } break;
                    case ScreenState.Outro:
                        {
                            OnBecomeOutro();
                        } break;
                }
            }
        }

        protected virtual void OnBecomeActive()
        {
        }

        protected virtual void OnBecomeInactive()
        {
        }

        protected virtual void OnBecomeIntro()
        {
        }

        protected virtual void OnBecomeOutro()
        {
        }
#endregion

#region Screen methods for ScreensManager

        public void RequestOutro()
        {
            //TODO: implement it 
            if (OnOutroDone != null)
                OnOutroDone();
            else
                Bully.Log("");
        }

        public void RequestIntro()
        {
            //TODO: implement it 
            if (OnIntroDone != null)
                OnIntroDone();
        }
#endregion

        void Awake()
        {
            BindPanelsGroup();
        }

        private void BindPanelsGroup()
        {
            GameObject panelsContainer = GameObject.FindWithTag("Panels");
            if (panelsContainer == null)
            {
                Debug.LogError("There is no panels container with 'Panels' Tag");
                return;
            }

            string groupName = ScreenName + "Panels";
            var panelsGroup = panelsContainer.transform.FindChild(groupName);
            if (panelsGroup != null)
            {
                Panels = panelsGroup.GetComponent(typeof(PanelsGroup)) as PanelsGroup;
            }
            else
            {
                Debug.LogError("Screen can't find PanelsGroup with name " + groupName);
            }
        }


        // Called on screen enter
        public virtual void OnScreenEnter()
        {
            //Debug.Log("Enter " + gameObject.name, gameObject);
            Panels.gameObject.SetActive(true);
        }

        // Calles on screen exit
        public virtual void OnScreenExit()
        {
            //Debug.Log("Exit " + gameObject.name, gameObject);
            Panels.gameObject.SetActive(false);
        }

        // Calles on screen exit
        public virtual string Name { get { return gameObject.name; } }

        public virtual Camera GetCamera()
        {
            return null;
        }

        public virtual void GotoNextState()
        {
            ScreenManager.Instance.GotoNextScreen();
        }



        // Helpers
        protected T GetPanel<T>() where T : Component
        {
            var p = Panels.gameObject.GetComponentsInChildren<T>(true);

            if (p.Length == 0)
            {
                Debug.LogError("No panel of type " + typeof(T).FullName);
                Debug.Break();
                return null;
            }

            if (p.Length > 1)
            {
                Debug.LogWarning("An extra panel of type " + typeof(T).FullName);
            }

            return p[0];
        }





        //void Update()
        //{
        //    /// @NOTE(Dmitrii): That's just for debug purposes.
        //    if (Input.GetKeyUp(KeyCode.RightArrow))
        //    {
        //        GotoNextState();
        //    }
        //}
    }
}
