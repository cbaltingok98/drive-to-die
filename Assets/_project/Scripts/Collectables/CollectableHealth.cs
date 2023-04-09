using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using ETFXPEL;
using UnityEngine;

namespace _project.Scripts.Collectables
{
    [Serializable]
    public struct CollectableHealthLevelInfo
    {
        public float health;
        
        public CollectableHealthLevelInfo(CollectableHealthLevelInfo info)
        {
            health = info.health;
        }
    }
    public class CollectableHealth : Collectable
    {
        [SerializeField] private List<CollectableHealthLevelInfo> collectableHealthLevelInfos;

        private CollectableHealthLevelInfo _curLevelInfo;
        
        public override CollectableType GetCollectableType() => CollectableType.Health;
        
        public override string GetName() => "Pack a punch";


        public override string GetDescription()
        {
            return "Get some heal some";
        }


        protected override void SetLevelInfo()
        {
            _curLevelInfo = collectableHealthLevelInfos[(Level - 1) % collectableHealthLevelInfos.Count];
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!IsActive) return;
            if (!other.CompareTag("Player")) return;
            IsActive = false;
            
            var particleSystem =  ParticlePooler.Instance.SpawnFromPool(SpawnType.CollectableHealParticle, transform.position,
                Quaternion.identity, false);
            particleSystem.Play();
            
            LevelManager.Instance.GetPlayerController().AddHealth(_curLevelInfo.health);

            base.DeActivate();
        }
    }
}