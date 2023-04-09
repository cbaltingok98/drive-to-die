using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Pools;
using _project.Scripts.Skills.SkillStats;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _project.Scripts.Skills.Skill_Logic.ROCKETLAUNCHER
{
    [Serializable]
    public struct RocketLauncherLevelInfo
    {
        public List<RocketLauncherFeatures> rocketLauncherLevelInfos;
    }

    [Serializable]
    public struct RocketLauncherFeatures
    {
        public int damage;
        public int numOfRockets;
        public float fireRate;
        public float rocketSpeed;
        public float blastRadius;
        public float passiveTime;
        
        public RocketLauncherFeatures(RocketLauncherFeatures info)
        {
            damage = info.damage;
            numOfRockets = info.numOfRockets;
            fireRate = info.fireRate;
            rocketSpeed = info.rocketSpeed;
            blastRadius = info.blastRadius;
            passiveTime = info.passiveTime;
        }
    }

    public class RocketLauncher : Skill
    {
        [SerializeField] private Transform tip;
        
        private RocketLauncherInfoScriptable currentLevelInfoScriptable;
        private RocketLauncherLevelInfo currentLevelInfo;
        
        private RocketLauncherFeatures currentFeatures;
        private RocketLauncherFeatures currentFeaturesTemp;

        private Queue<RocketLauncherTarget> targets;

        private ObjectPooler objectPooler;

        private Transform origin;

        private float passiveCounter;
        private float fireRateCounter;

        private bool isActive;
        private bool isWorking;
        private bool isAllFired;
        public override string GetName() => "Rocket";


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.rocketLauncherLevelInfos == null) return false;
            return Level >= currentLevelInfo.rocketLauncherLevelInfos.Count;
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
                   currentLevelInfoScriptable.rocketLauncherLevels.Count;
        }
        
        public override int GetMaxLevel()
        {
            LoadSkillStats();
            return currentLevelInfoScriptable.rocketLauncherLevels.Count;
        }

        public override string GetDescription()
        {
            return "Rain of fire";
        }


        protected override void FirstActivation()
        {
            Printer.Print(GetName() + " Activated");

            objectPooler = ObjectPooler.Instance;
            origin = LevelManager.Instance.GetPlayerTransform();

            base.FirstActivation();
        }
        
        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<RocketLauncherInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.rocketLauncherLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }


        protected override void UpdateSkill()
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }


        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.rocketLauncherLevelInfos.Count;
            var manipulatedLevel = Level - 1 >= maxLevelCount ? maxLevelCount - 1 : Level - 1;

            currentFeatures = currentLevelInfo.rocketLauncherLevelInfos[manipulatedLevel];
        }


        protected override void Activate()
        {
            isActive = true;
            ActivateSkill();
        }


        private void Update()
        {
            if (!isActive) return;

            if (isWorking)
            {
                fireRateCounter += Time.deltaTime;

                if (fireRateCounter >= currentFeatures.fireRate)
                {
                    fireRateCounter = 0;
                    Fire();
                    isAllFired = targets.Count == 0;
                }

                if (!isAllFired) return;
                DeActivateSkill();
            }
            else
            {
                passiveCounter += Time.deltaTime;

                if (passiveCounter <= currentFeatures.passiveTime) return;
                ActivateSkill();
            }
        }


        private void Fire()
        {
            var target = targets.Dequeue();
            var rocket =
                objectPooler.SpawnFromPool(ObjectType.RocketLauncherBullet, Vector3.zero, true, tip); // tip.rotation
            rocket.DestroyTarget(target, objectPooler.transform, currentFeaturesTemp);
        }

        
        private void SetTargets()
        {
            targets = new Queue<RocketLauncherTarget>();
            var direction = 0f;
            var delay = 0f;
            var delayAdd = 0.15f;

            fireRateCounter = -(currentFeatures.numOfRockets * delayAdd);
            
            for (var i = 0; i < currentFeatures.numOfRockets; i++)
            {
                var newTarget = objectPooler.SpawnTargetFromPool(ObjectType.RocketLauncherTarget, Vector3.zero, true, transform);
                delay += delayAdd;
                direction += Random.Range(28, 38);
                var dist = Random.Range(6f, 8f);
                var vector = Quaternion.Euler(0f, direction, 0f) * transform.forward * dist;
                vector.y = 0.4f;
                newTarget.SetTargetInfo(new TargetInfo(vector, origin ,transform), delay);
                targets.Enqueue(newTarget);
            }

            currentFeaturesTemp = new RocketLauncherFeatures(currentFeatures);
        }


        private void ActivateSkill()
        {
            SetTargets();
            isWorking = true;
            isAllFired = false;
        }


        private void DeActivateSkill()
        {
            isAllFired = false;
            passiveCounter = 0f;
            isWorking = false;
        }


        public override void ResetSkill()
        {
            base.ResetSkill();
            isWorking = false;
            isAllFired = false;

            passiveCounter = 0f;
            fireRateCounter = 0f;

            Level = 0;
            
            gameObject.SetActive(false);
        }


        public override string GetSkillInfoForArmory()
        { 
            return SkillInfoProfileManager.GetSkillProfile<RocketLauncherInfoScriptable>().ToList()[0].GetSkillInfos();
        }
    }
}