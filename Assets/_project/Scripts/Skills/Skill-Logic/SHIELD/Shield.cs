using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Player;
using _project.Scripts.Skills.SkillStats;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.SHIELD
{
    [Serializable]
    public struct ShieldLevelInfo
    {
        public List<ShieldFeatures> shieldLevelInfo;
    }

    [Serializable]
    public struct ShieldFeatures
    {
        public float activeTime;
        public float passiveTime;
    }
    public class Shield : Skill
    {
        [SerializeField] private ParticleSystem myParticle;
        
        private ShieldInfoScriptable currentLevelInfoScriptable;
        private ShieldLevelInfo currentLevelInfo;
        private ShieldFeatures shieldFeatures;
        
        private PlayerController playerController;

        private Transform myParticleTransform;
        private Transform shieldIconTransform;

        private Vector3 particleTargetScale;
        private Vector3 particleWarningScale;
        
        private Vector3 shieldIconTargetScale;
        private Vector3 shieldIconWarning;
        
        
        private bool isActive;
        private bool isWorking;
        private bool warningStarted;
        private bool particleSet;

        private float activeCounter;
        private float passiveCounter;
        private float warnTime = 3f;

        private Coroutine myCoroutine;
        private WaitForSeconds waitFor;


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.shieldLevelInfo == null) return false;
            
            return Level >= currentLevelInfo.shieldLevelInfo.Count;
        }


        public override int GetUnlockPrice()
        {
            LoadSkillStats();

            return currentLevelInfoScriptable.unlockPrice;
        }


        public override bool IsSkillLevelMaxedOut()
        {
            LoadSkillStats();
            
            return DataManager.Instance.GetSkillLevelFor(GetSkillType()) >=
                   currentLevelInfoScriptable.shieldLevels.Count;
        }
        
        public override int GetMaxLevel()
        {
            LoadSkillStats();
            return currentLevelInfoScriptable.shieldLevels.Count;
        }
        
        public override string GetName() => "Shield";
        public override string GetDescription()
        {
            return "Block any damage";
        }


        public override int GetUpgradePrice()
        {
            return currentLevelInfoScriptable.priceList[DataManager.Instance.GetSkillLevelFor(GetSkillType()) - 1];
        }

        protected override void FirstActivation()
        {
            SetParticleInfo();
            
            waitFor = new WaitForSeconds(0.15f);
            playerController = PlayerController.Instance;

            shieldIconTransform = playerController.GetShieldIcon();
            
            base.FirstActivation();
        }


        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<ShieldInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.shieldLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }

        private void SetParticleInfo()
        {
            if (!particleSet)
            {
                particleSet = true;
                myParticleTransform = myParticle.transform;
                particleTargetScale = myParticleTransform.localScale;
                particleWarningScale = particleTargetScale;
                particleWarningScale.y = 0f;
            }

            shieldIconTargetScale = Vector3.one;
            shieldIconWarning = shieldIconTargetScale;
            shieldIconWarning.y = 0f;
           
            myParticleTransform.localScale = Vector3.zero;
        }

        protected override void UpdateSkill() // in chapter update by earning stars
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }


        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.shieldLevelInfo.Count;
            var manipulatedLevel = Level - 1 >=  maxLevelCount ? maxLevelCount - 1 : Level - 1;

           shieldFeatures = currentLevelInfo.shieldLevelInfo[manipulatedLevel];
        }


        private void Update()
        {
            if (!isActive) return;

            if (isWorking)
            {
                activeCounter += Time.deltaTime;
                CheckForWarning();
                
                if (activeCounter <= shieldFeatures.activeTime) return;
                DeActivateSkill();
            }
            else
            {
                passiveCounter += Time.deltaTime;
                if (passiveCounter <= shieldFeatures.passiveTime) return;
                Activate();
            }
        }


        private void CheckForWarning()
        {
            if (warningStarted) return;
            
            var diff = shieldFeatures.activeTime - activeCounter;
            if (diff > warnTime) return;
            warningStarted = true;

            myCoroutine = StartCoroutine(WarnPlayer());
            PlayerController.Instance.WarnPlayerForEndingShield();
        }

        private IEnumerator WarnPlayer()
        {
            while (isWorking)
            {
                yield return waitFor;
                myParticleTransform.localScale = particleWarningScale;
                shieldIconTransform.localScale = shieldIconWarning;
                yield return waitFor;
                myParticleTransform.localScale = particleTargetScale;
                shieldIconTransform.localScale = shieldIconTargetScale;
            }
        }

        protected override void Activate()
        {
            passiveCounter = 0f;
            isWorking = true;
            playerController.ControlAttackBlocker(true);
            myParticle.gameObject.SetActive(true);
            myParticleTransform.DOScale(particleTargetScale, 0.4f).SetDelay(0.1f).SetEase(Ease.OutBack);
            warningStarted = false;
            SoundManager.Instance.Play(SoundType.Shield);
        }
        
        private void DeActivateSkill()
        {
            activeCounter = 0f;
            isWorking = false;
            
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            myParticleTransform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
            {
                playerController.ControlAttackBlocker(false);
                myParticle.gameObject.SetActive(false);
            });
        }
        
        protected override void ActivationAnimation()
        {
            gameObject.SetActive(true);
            isWorking = true;
            isActive = true;
            Activate();
        }


        public override void ResetSkill()
        {
            base.ResetSkill();
            isActive = false;
            isWorking = false;
            Level = 0;
            
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
            
            myParticle.gameObject.SetActive(false);

            warningStarted = false;
            passiveCounter = 0f;
            activeCounter = 0f;

            if (playerController)
            {
                playerController.ControlAttackBlocker(false);   
            }
            gameObject.SetActive(false);
        }


        public override string GetSkillInfoForArmory()
        { 
            return SkillInfoProfileManager.GetSkillProfile<ShieldInfoScriptable>().ToList()[0].GetSkillInfos();
        }
    }
}