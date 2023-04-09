using _project.Scripts.UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace _project.Scripts.Managers
{
    public class PlayTimeCounterManager : MonoBehaviour
    {
        public static PlayTimeCounterManager Instance;

        private InGameUI _inGameUi;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public float SecondsPlayed
        {
            get => _seconds;
            private set => _seconds = value;
        }

        private float _seconds;


        public void ControlCounter(bool count)
        {
            if (count)
            {
                _inGameUi = CanvasManager.Instance.GetInGameUI();
                SecondsPlayed = 0f;
            }
            gameObject.SetActive(count);
        }
        
        private void Update()
        {
            SecondsPlayed += Time.deltaTime;

            if (((int)SecondsPlayed / 1) % 1 == 0)
            {
                UpdateUi();
            }
        }


        private void UpdateUi()
        {
            _inGameUi.UpdateTimeText(SecondsPlayed);
        }
    }
}