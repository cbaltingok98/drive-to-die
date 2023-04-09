using System;
using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Skills.SkillStats;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.MINIGUN
{
    [Serializable]
    public struct MiniGunLevelInfo
    {
        public List<MiniGunFeatures> miniGunLevelInfo;
    }

    [Serializable]
    public struct MiniGunFeatures
    {
        public float radius;
        public float fireRate;
        public float damage;


        public MiniGunFeatures(MiniGunFeatures newInfo)
        {
            radius = newInfo.radius;
            fireRate = newInfo.fireRate;
            damage = newInfo.damage;
        }
    }

    public class MiniGun : Skill
    {
        [SerializeField] private Transform gunTip;
        [SerializeField] private Transform gunMount;

        private ParticlePooler particlePooler;

        private MiniGunInfoScriptable currentLevelInfoScriptable;
        private MiniGunLevelInfo currentLevelInfo;
        private MiniGunFeatures currentMiniGunFeatures;

        private Vector3 differenceVector;

        private float fireRateCounter;
        private float maxAttackRadius;

        private bool isActive;
        private bool canFire;

        private SoundManager soundManager;
        
        public override string GetName() => "MiniGun";


        public override string GetDescription()
        {
            return "Gun go brrrrr";
        }


        private ClosestEnemyInfo _closestEnemyInfo;

        private Transform _targetTransform;

        public override float GetRadius() => currentMiniGunFeatures.radius;


        public override bool IsMaxedOutInChapterUpgrade()
        {
            if (currentLevelInfo.miniGunLevelInfo == null) return false;
            
            return Level >= currentLevelInfo.miniGunLevelInfo.Count;
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
                   currentLevelInfoScriptable.miniGunLevels.Count;
        }
        
        public override int GetMaxLevel()
        {
            LoadSkillStats();
            return currentLevelInfoScriptable.miniGunLevels.Count;
        }


        protected override void FirstActivation()
        {
            soundManager = SoundManager.Instance;
            particlePooler = ParticlePooler.Instance;

            base.FirstActivation();
        }


        protected override void LoadSkillStats()
        {
            currentLevelInfoScriptable = SkillInfoProfileManager.GetSkillProfile<MiniGunInfoScriptable>().ToList()[0];
            currentLevelInfo = currentLevelInfoScriptable.miniGunLevels[DataManager.Instance.GetSkillLevelFor(SkillType) - 1];
        }


        protected override void UpdateSkill()
        {
            Level += 1;
            UpdateCurrentLevelInfo();
        }


        protected override void UpdateCurrentLevelInfo()
        {
            var maxLevelCount = currentLevelInfo.miniGunLevelInfo.Count;
            var manipulatedLevel = Level - 1 >= maxLevelCount ? maxLevelCount - 1 : Level - 1;

            currentMiniGunFeatures = currentLevelInfo.miniGunLevelInfo[manipulatedLevel];
        }


        protected override void Activate()
        {
            isActive = true;
        }


        private void Update()
        {
            if (!isActive) return;
            if (_closestEnemyInfo.EnemyBase == null) return;

            LookAtTarget();
            Fire();
        }


        public override void SetTarget(ClosestEnemyInfo closestEnemyInfo)
        {
            _closestEnemyInfo = closestEnemyInfo;
            _targetTransform = closestEnemyInfo.EnemyBase.transform;
        }


        public override void RemoveTarget()
        {
            _closestEnemyInfo = new ClosestEnemyInfo();
        }


        private void CheckDistance(float distance)
        {
            if (distance > currentMiniGunFeatures.radius)
            {
                SkillManager.Instance.RemoveClosestEnemy();
                RemoveTarget();
            }
        }


        private void LookAtTarget()
        {
            differenceVector = (_targetTransform.position - transform.position);
            var dir = differenceVector.normalized;
            dir.y = 0;
            CheckDistance(differenceVector.magnitude);

            var dot = Vector3.Dot(dir, gunMount.forward);
            canFire = dot >= 0.8f;

            var rotation = Vector3.RotateTowards(gunMount.forward, dir, 5f * Time.deltaTime, 0f);
            gunMount.rotation = Quaternion.LookRotation(rotation);
        }


        private void Fire()
        {
            if (!canFire) return;

            fireRateCounter += Time.deltaTime;
            if (fireRateCounter < currentMiniGunFeatures.fireRate) return;
            fireRateCounter = 0f;

            var enemy = _closestEnemyInfo.EnemyBase;
            if (enemy)
            {
                enemy.TakeDamage(currentMiniGunFeatures.damage);
            }
            
            SendBulletTrail();
            soundManager.Play(SoundType.MiniGun);
        }


        private void SendBulletTrail()
        {
            var gunTipPos = gunTip.position;
            var direction = differenceVector.normalized;
            var length = differenceVector.magnitude;
            var target = gunTipPos + direction * length;

            var trail = particlePooler.SpawnFromPool(SpawnType.MiniGunBulletTrail, gunTipPos);
            trail.transform.DOMove(target, 0.2f).OnComplete(() =>
            {
                trail.gameObject.SetActive(false);
                //trail.transform.SetParent(_particlePooler.transform);
            });
        }


        public override void ResetSkill()
        {
            base.ResetSkill();
            isActive = false;
            canFire = false;
            
            fireRateCounter = 0f;
            maxAttackRadius = 0f;

            Level = 0;
            RemoveTarget();
            
            gameObject.SetActive(false);
        }
        
        public override string GetSkillInfoForArmory()
        { 
            return SkillInfoProfileManager.GetSkillProfile<MiniGunInfoScriptable>().ToList()[0].GetSkillInfos();
        }
    }
}