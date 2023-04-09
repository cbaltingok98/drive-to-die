using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic
{
    public abstract class Skill : MonoBehaviour
    {
        [SerializeField] private Sprite skillSprite;
        [SerializeField] private List<GameObject> armoryModels;
        [SerializeField] private List<GameObject> mainModels;

        [SerializeField] protected SkillType SkillType;
        [SerializeField] protected bool passiveSkill;

        protected string Name;
        protected string Description;
        protected int Level;

        protected bool IsActivated;

        protected float Radius;

        public SkillType GetSkillType() => SkillType;
        public abstract string GetName();
        public abstract string GetDescription();
        public virtual Sprite GetSkillSprite() => skillSprite;
        public virtual float GetRadius() => Radius;
        public int GetCurrentLevel() => Level;

        public abstract int GetMaxLevel();

        public abstract int GetUnlockPrice();

        public bool IsFirstActivation() => IsActivated;
        public abstract bool IsMaxedOutInChapterUpgrade();

        public abstract bool IsSkillLevelMaxedOut();
        public abstract int GetUpgradePrice();
        public virtual void ResetSkill()
        {
            IsActivated = false;
        }
        
        public bool IsPassiveSkill() => passiveSkill;
        
        public abstract string GetSkillInfoForArmory();

        protected abstract void LoadSkillStats();
        
        public virtual void ShowArmoryModel()
        {
            for (var i = 0; i < mainModels.Count; i++)
            {
                mainModels[i].SetActive(false);
            }
            
            for (var i = 0; i < armoryModels.Count; i++)
            {
                armoryModels[i].SetActive(true);
            }

            gameObject.SetActive(true);
        }
        
        public virtual void HideArmoryModel()
        {
            for (var i = 0; i < armoryModels.Count; i++)
            {
                armoryModels[i].SetActive(false);
            }

            for (var i = 0; i < mainModels.Count; i++)
            {
                mainModels[i].SetActive(true);
            }
            
            gameObject.SetActive(false);
        }
        

        public void Disable()
        {
            enabled = false;
        }


        public void Initialize()
        {
            if (IsActivated)
            {
                UpdateSkill();
            }
            else
            {
                IsActivated = true;
                FirstActivation();
            }
        }


        protected virtual void FirstActivation()
        {
            LoadSkillStats();
            enabled = true;
            UpdateSkill();
            ActivationAnimation();
        }


        protected abstract void UpdateSkill();
        protected abstract void UpdateCurrentLevelInfo();


        protected virtual void ActivationAnimation()
        {
            var targetScale = transform.localScale;
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(targetScale, 0.5f).SetDelay(0.5f).SetEase(Ease.OutBack).OnComplete(Activate);
        }


        public virtual void ActivatePassive()
        {
            
        }

        protected virtual void Activate()
        {
        }


        public virtual void SetTarget(ClosestEnemyInfo closestEnemyInfo) // mini gun specific atm
        {
        }


        public virtual void RemoveTarget()
        {
        }
    }
}