using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Player;
using _project.Scripts.Skills.SkillStats;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.FUEL_GEN
{
    [Serializable]
    public struct FuelGeneratorLevelInfo
    {
        public List<FuelGeneratorFeature> fuelGeneratorLevelInfo;
    }

    [Serializable]
    public struct FuelGeneratorFeature
    {
        public float percent;
        public int forEveryKill;


        public FuelGeneratorFeature(FuelGeneratorFeature newInfo)
        {
            percent = newInfo.percent;
            forEveryKill = newInfo.forEveryKill;
        }
    }
    public class FuelGenerator : Skill
    {
        [SerializeField] private ParticleSystem particle;
        
        private FuelGenInfoScriptable currentLevelInfoScriptable;
        private FuelGeneratorLevelInfo currentLevelInfo;
        private FuelGeneratorFeature generatorFeatures;

        private FuelManager fuelManager;

        private int counter;
        
        public override string GetName()
        {
            return "Petrol";
        }


        public override string GetDescription()
        {
            return "Generates fuel";
        }


        protected override void FirstActivation()
        {
            fuelManager = FuelManager.Instance;
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
            return currentLevelInfoScriptable.fuelGeneratorLevels.Count;
        }


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.fuelGeneratorLevelInfo == null) return false;
            
            return Level >= currentLevelInfo.fuelGeneratorLevelInfo.Count;
        }


        public override bool IsSkillLevelMaxedOut()
        {
            LoadSkillStats();
            
            return DataManager.Instance.GetSkillLevelFor(GetSkillType()) >=
                   currentLevelInfoScriptable.fuelGeneratorLevels.Count;
        }


        public override int GetUpgradePrice()
        {
            return currentLevelInfoScriptable.priceList[DataManager.Instance.GetSkillLevelFor(GetSkillType()) - 1];
        }


        public override string GetSkillInfoForArmory()
        {
            return SkillInfoProfileManager.GetSkillProfile<FuelGenInfoScriptable>().ToList()[0].GetSkillInfos();
        }


        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<FuelGenInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.fuelGeneratorLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }


        protected override void UpdateSkill()
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }

        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.fuelGeneratorLevelInfo.Count;
            var manipulatedLevel = Level - 1 >=  maxLevelCount ? maxLevelCount - 1 : Level - 1;
                
            generatorFeatures = currentLevelInfo.fuelGeneratorLevelInfo[manipulatedLevel];
        }


        private void SetCounter()
        {
            counter = generatorFeatures.forEveryKill / 50;
        }


        public override void ActivatePassive()
        {
            counter -= 1;

            if (counter != 0) return;

            var maxFuel = fuelManager.GetMaxFuel();
            var addAmount = maxFuel * generatorFeatures.percent / 100f;
            
            fuelManager.AddFuel(addAmount);
            particle.Play();

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