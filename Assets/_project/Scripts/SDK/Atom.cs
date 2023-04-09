// using GameAnalyticsSDK;
using UnityEngine;

namespace _project.Scripts.SDK
{
    public class Atom : MonoBehaviour//, IGameAnalyticsATTListener
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
                
            //InitializeGameAnalytics();
        }

        // private void InitializeGameAnalytics()
        // {
        //     if(Application.platform == RuntimePlatform.IPhonePlayer)
        //     {
        //         GameAnalytics.RequestTrackingAuthorization(this);
        //     }
        //     else
        //     {
        //         GameAnalytics.Initialize();
        //     }
        // }
        //
        // public void GameAnalyticsATTListenerNotDetermined()
        // {
        //     GameAnalytics.Initialize();
        // }
        //
        // public void GameAnalyticsATTListenerRestricted()
        // {
        //     GameAnalytics.Initialize();
        // }
        //
        // public void GameAnalyticsATTListenerDenied()
        // {
        //     GameAnalytics.Initialize();
        // }
        //
        // public void GameAnalyticsATTListenerAuthorized()
        // {
        //     GameAnalytics.Initialize();
        // }
    }
}