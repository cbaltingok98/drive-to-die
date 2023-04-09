using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.Menu.GarageMenu
{
    public class SingleSpecBar : MonoBehaviour
    {
        [SerializeField] private Image innerBar;
        [SerializeField] private Image outerBar;
        [SerializeField] private Animator animator;

        private Tween lightUpTween;
        private Tween turnOffTween;

        public void LightUp()
        {
            ControlAnimator(false);
            lightUpTween?.Kill();
            lightUpTween = innerBar.DOFade(1f, 0.4f);
        }


        public void TurnOff()
        {
            ControlAnimator(false);
            turnOffTween?.Kill();
            turnOffTween = innerBar.DOFade(0f, 0.4f);
        }
        
        public void ControlAnimator(bool control)
        {
            animator.enabled = control;
        }
    }
}