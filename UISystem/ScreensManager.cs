//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Date:    02/26/2016 12:02:37
//  Creator: Emeliov Dmitri <demelev1990@gmail.com>
//
//===----------------------------------------------------------------------===//
using System;


namespace UISystem
{

    public class ScreensManager : Singleton<ScreensManager>
    {
#region protected members
        protected BaseScreen[] _screens;
        protected string[] _triggers;
        protected int[][]  _transitions;

        protected BaseScreen _currentScreen;
        protected BaseScreen _nextScreen;
#endregion

        ScreensManager()
        {
            RegisterSingleton(this);
        }

        void ReadTransitions(string path)
        {
        }

        void OnTriggerRaised(string trigger)
        {
            //TODO: get next screen
            //
        }

        void TransitionTo(BaseScreen target)
        {
            _nextScreen = target;

            _currentScreen.OnOutroDone += OnOutroDone_handler;
            _nextScreen.OnIntroDone += OnIntroDone_handler;

            _currentScreen.RequestOutro();
            _currentScreen.State = ScreenState.Outro;
        }

#region Events handlers

        void OnOutroDone_handler()
        {
            _currentScreen.State = ScreenState.Inactive;

            _nextScreen.RequestIntro();
            _nextScreen.State = ScreenState.Intro;
        }

        void OnIntroDone_handler()
        {
            _currentScreen = _nextScreen;
            _nextScreen = null;

            _currentScreen.State = ScreenState.Active;
        }

#endregion
    }
}
