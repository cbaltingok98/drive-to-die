using _project.Scripts.Berk;
using _project.Scripts.Enemy;
using _project.Scripts.Mimic;
using _project.Scripts.Score;
using _project.Scripts.Utils;
using UnityEngine;

namespace _project.Scripts.MyDebug
{
    public class MyDebugController : MonoBehaviour // TODO disable in scene
    {
        private void Update()
        {
            // if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            // {
            //     MimicDriving.Save();
            // }
            // if (UnityEngine.Input.GetKeyDown(KeyCode.L))
            // {
            //     var curStar = ScoreManager.Instance.GetStarAmount();
            //     Printer.Print("Cut star : " + curStar);
            // }

            // if (UnityEngine.Input.GetKeyDown(KeyCode.K))
            // {
            //     Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            // }
            //
            if (UnityEngine.Input.GetKeyDown(KeyCode.U))
            {
                ScoreManager.Instance.DebugTest();
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.S))
            {
                EnemySpawnSystem.Instance.GameOver();
            }
        }

        [ContextMenu("Continue Playing")]
        private void ContinuePlaying()
        {
            Time.timeScale = 1f;
        }
    }
}