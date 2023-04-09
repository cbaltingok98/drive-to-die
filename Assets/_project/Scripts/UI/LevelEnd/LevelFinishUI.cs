using _project.Scripts.Models;
using _project.Scripts.Utils;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class LevelFinishUI : MonoBehaviour
    {
        public LevelFailedUI levelFailUI;
        public LevelCompleteUI levelCompleteUI;


        public void LevelCompleted(LevelFinishData levelFinishData)
        {
            levelCompleteUI.Initialize(levelFinishData);
            levelCompleteUI.gameObject.SetActive(true);
        }


        public void HideLevelCompleteUi()
        {
            levelCompleteUI.gameObject.SetActive(false);
        }


        public void LevelFailed(LevelFinishData levelFinishData)
        {
            levelFailUI.Initialize(levelFinishData);
            levelFailUI.gameObject.SetActive(true);
        }


        public void HideLevelFailUi()
        {
            levelFailUI.gameObject.SetActive(false);
        }
    }
}