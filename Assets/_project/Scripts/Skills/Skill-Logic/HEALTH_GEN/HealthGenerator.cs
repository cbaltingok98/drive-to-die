using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Player;
using _project.Scripts.Skills.SkillStats;
using _project.Scripts.UI;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.HEALTH_GEN
{
    [Serializable]
    public struct HealthGeneratorLevelInfo
    {
        public List<HealthGeneratorFeature> healthGeneratorLevelInfo;
    }

    [Serializable]
    public struct HealthGeneratorFeature
    {
        public float percent;
        public int forEveryKill;


        public HealthGeneratorFeature(HealthGeneratorFeature newInfo)
        {
            percent = newInfo.percent;
            forEveryKill = newInfo.forEveryKill;
        }
    }
    
    public class HealthGenerator : Skill
    {
        [SerializeField] private ParticleSystem particle; 
        
        private HealthGenInfoScriptable currentLevelInfoScriptable;
        private HealthGeneratorLevelInfo currentLevelInfo;
        private HealthGeneratorFeature generatorFeatures;

        private PlayerController playerController;

        private int counter;
        
        public override string GetName()
        {
            return "Heal Up";
        }


        public override string GetDescription()
        {
            return "Generates health"; 
        }


        protected override void FirstActivation()
        {
            playerController = PlayerController.Instance;
            base.FirstActivation();
            SetCounter();
        }
        
        public override int GetUnlockPrice()
        {
            LoadSkillStats();

            return currentLevelInfoScriptable.unlockPrice;
        }


        public override int GetMaxLevel()
        {
            LoadSkillStats();
            return currentLevelInfoScriptable.healthGeneratorLevels.Count;
        }


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.healthGeneratorLevelInfo == null) return false;
            
            return Level >= currentLevelInfo.healthGeneratorLevelInfo.Count;
        }


        public override bool IsSkillLevelMaxedOut()
        {
            LoadSkillStats();
            
            return DataManager.Instance.GetSkillLevelFor(GetSkillType()) >=
                   currentLevelInfoScriptable.healthGeneratorLevels.Count;
        }


        public override int GetUpgradePrice()
        {
            return currentLevelInfoScriptable.priceList[DataManager.Instance.GetSkillLevelFor(GetSkillType()) - 1];
        }


        public override string GetSkillInfoForArmory()
        {
            return SkillInfoProfileManager.GetSkillProfile<HealthGenInfoScriptable>().ToList()[0].GetSkillInfos();
        }


        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<HealthGenInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.healthGeneratorLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }


        protected override void UpdateSkill()
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }

        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.healthGeneratorLevelInfo.Count;
            var manipulatedLevel = Level - 1 >=  maxLevelCount ? maxLevelCount - 1 : Level - 1;
                
            generatorFeatures = currentLevelInfo.healthGeneratorLevelInfo[manipulatedLevel];
        }


        private void SetCounter()
        {
            counter = generatorFeatures.forEveryKill / 50;
        }


        public override void ActivatePassive()
        {
            counter -= 1;

            if (counter != 0) return;
            
            var maxHealth = playerController.GetMaxHealth();
            var addAmount = maxHealth * generatorFeatures.percent / 100f;
            
            playerController.AddHealth(addAmount);
            particle.Play();
            SoundManager.Instance.Play(SoundType.MagicCast);
            
            SetCounter();
        }
        
        public override void ResetSkill()
        {
            base.ResetSkill();
            
            Level = 0;
            
            gameObject.SetActive(false);
        }
    }
}