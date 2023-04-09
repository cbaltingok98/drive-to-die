using System;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _project.Scripts.Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        [SerializeField] private ParticleSystem particle;

        private UnityAction action;
        protected int Level;

        protected bool IsActive;
        
        public abstract CollectableType GetCollectableType();
        public abstract string GetName();
        public abstract string GetDescription();

        

        public virtual void Initialize(int level, UnityAction action)
        {
            this.action = action;
            Level = level;
            SetLevelInfo();
            ActivationAnimation();
        }


        private void ActivationAnimation()
        {
            var targetScale = Vector3.one;
            transform.localScale = Vector3.zero;
            
            gameObject.SetActive(true);
            particle.Play();

            transform.DOScale(targetScale, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                IsActive = true;
            });
        }


        protected virtual void SetLevelInfo()
        {
            
        }


        protected virtual void DeActivate()
        {
            VibrationManager.SoftVibrate();
            action?.Invoke();
            transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
            SoundManager.Instance.Play(SoundType.MagicCast);
        }


        public virtual void ResetItem()
        {
            Level = 0;
            IsActive = false;
            gameObject.SetActive(false);
        }
    }
}