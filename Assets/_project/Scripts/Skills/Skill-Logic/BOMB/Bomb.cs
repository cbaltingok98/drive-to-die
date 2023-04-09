using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Pools;
using _project.Scripts.Skills.SkillStats;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.BOMB
{
    [Serializable]
    public struct BombLevelInfo
    {
        public List<BombFeature> bombLevelInfo;
    }

    [Serializable]
    public struct BombFeature
    {
        public int amount;
        public float damage;
        public float blastRadius;
        public float passiveTime;


        public BombFeature(BombFeature newInfo)
        {
            amount = newInfo.amount;
            damage = newInfo.damage;
            blastRadius = newInfo.blastRadius;
            passiveTime = newInfo.passiveTime;
        }
    }
            
    public class Bomb : Skill
    {
        private BombInfoScriptable currentLevelInfoScriptable;
        private BombLevelInfo currentLevelInfo;
        private BombFeature currentBombFeatures;

        private ObjectPooler objectPooler;

        private WaitForSeconds waitFor;
        private WaitForSeconds waitForBlowUp;

        private bool isActive;
        private bool isWorking;

        private float passiveTimeCounter;
        
        
        public override string GetName() => "Bomb";
        public override string GetDescription() => "Booom!";


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.bombLevelInfo == null) return false;
            
            return Level >= currentLevelInfo.bombLevelInfo.Count;
        }
        
        public override int GetUnlockPrice()
        {
            LoadSkillStats();

            return currentLevelInfoScriptable.unlockPrice;
        }
        
        
        public override int GetUpgradePrice()
        {
            return currentLevelInfoScriptable.priceList[DataManager.Instance.GetSkillLevelFor(GetSkillType()) - 1];
        }
        
        
        public override bool IsSkillLevelMaxedOut()
        {
            LoadSkillStats();
            
            return DataManager.Instance.GetSkillLevelFor(GetSkillType()) >=
                   currentLevelInfoScriptable.bombLevels.Count;
        }


        public override int GetMaxLevel()
        {
            LoadSkillStats();
            return currentLevelInfoScriptable.bombLevels.Count;
        }


        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<BombInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.bombLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }


        protected override void Activate()
        {
            objectPooler = ObjectPooler.Instance;
            waitFor = new WaitForSeconds(0.25f);
            waitForBlowUp = new WaitForSeconds(1f);
            
            isActive = true;
            isWorking = true;
        }

        protected override void UpdateSkill()
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }


        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.bombLevelInfo.Count;
            var manipulatedLevel = Level - 1 >= maxLevelCount ? maxLevelCount - 1 : Level - 1;

            currentBombFeatures = currentLevelInfo.bombLevelInfo[manipulatedLevel];
        }


        private void Update()
        {
            if (!isActive) return;

            if (isWorking)
            {
                isWorking = false;
                StartCoroutine(SpawnBombs());
            }
            else
            {
                passiveTimeCounter += Time.deltaTime;
                if (passiveTimeCounter < currentBombFeatures.passiveTime) return;
                passiveTimeCounter = 0f;

                isWorking = true;
            }

        }


        private IEnumerator SpawnBombs()
        {
            for (var i = 0; i < currentBombFeatures.amount; i++)
            {
                Spawn();
                yield return waitFor;
            }
        }


        private void Spawn()
        {
            var newBomb =
                objectPooler.SpawnSingleBombFromPool(ObjectType.Bomb, transform.position);
            
            newBomb.Init(waitForBlowUp, currentBombFeatures.damage, currentBombFeatures.blastRadius);
        }


        public override void ResetSkill()
        {
            base.ResetSkill();
            
            Level = 0;
            
            isActive = false;
            isWorking = false;

            passiveTimeCounter = 0f;
            
            gameObject.SetActive(false);
        }
        
        public override string GetSkillInfoForArmory()
        { 
            return SkillInfoProfileManager.GetSkillProfile<BombInfoScriptable>().ToList()[0].GetSkillInfos();
        }
    }
}