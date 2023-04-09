using System.Collections;
using _project.Scripts.Berk;
using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("UI")] [Space(10)] [SerializeField]
        private Slider slider;

        [SerializeField] private Gradient gradient;
        [SerializeField] private Image fill;
        [SerializeField] private Transform shieldImage;

        private Vector3 targetScale;
        private Vector3 targetScale2;

        private WaitForSeconds waitFor;

        private bool warningColor;


        public Transform GetShieldIcon() => shieldImage;

        public void SetMaxHealth(float maxHealth)
        {
            slider.maxValue = maxHealth;
            gradient.Evaluate(1f);
            UpdateHealth(maxHealth, false);

            targetScale = Vector3.one;
            targetScale2 = new Vector3(1f, 1f, 1f);

            waitFor = new WaitForSeconds(0.2f);

            DeActivate();
        }


        public void UpdateHealth(float val, bool warnPlayer)
        {
            slider.value = val;

            if (!warnPlayer && !warningColor)
            {
                fill.color = gradient.Evaluate(slider.normalizedValue);   
            }
            else if (!warningColor)
            {
                StartCoroutine(WarnDamageTaken());   
            }
        }

        private IEnumerator WarnDamageTaken()
        {
            warningColor = true;
            
            fill.color = gradient.Evaluate(0f);
            yield return waitFor;
            fill.color = gradient.Evaluate(slider.normalizedValue);
            
            warningColor = false;
        }

        public void Control(bool set)
        {
            if (set)
            {
                DeActivate();
                //ShowWarning(false);
            }
            else
            {
                Activate();
                //ShowWarning(true);
            }
        }


        private void ShowWarning(bool enable)
        {
            if (DataManager.Instance.IsPlayedBefore()) return;
            
            CanvasManager.Instance.GetInGameUI().ShowWarningMessage(enable ? WarningType.ShieldOn : WarningType.ShieldOff);
        }
        
        private void Activate()
        {
            gameObject.SetActive(true);
            shieldImage.DOScale(targetScale2, 0.3f).SetEase(Ease.OutBack);
            // transform.DOScale(_targetScale, 0.4f).SetEase(Ease.OutBack);
        }

        private void DeActivate()
        {
            shieldImage.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                // gameObject.SetActive(false);
            });
        }
    }
}