using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using UnityEngine;

namespace _project.Scripts.Collectables
{
    [Serializable]
    public struct CollectableFuelLevelInfo
    {
        public float fuelAmount;


        public CollectableFuelLevelInfo(CollectableFuelLevelInfo info)
        {
            fuelAmount = info.fuelAmount;
        }
    }
    public class CollectableFuel : Collectable
    {
        [SerializeField] private List<CollectableFuelLevelInfo> collectableFuelLevelInfos;

        private CollectableFuelLevelInfo _curLevelInfo;
        
        public override CollectableType GetCollectableType() => CollectableType.Fuel;


        public override string GetName() => "Juice of power";


        public override string GetDescription()
        {
            return "Fuel my heart";
        }
        
        protected override void SetLevelInfo()
        {
            _curLevelInfo = collectableFuelLevelInfos[(Level - 1) % collectableFuelLevelInfos.Count];
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!IsActive) return;
            if (!other.CompareTag("Player")) return;
            IsActive = false;

            var particleSystem =  ParticlePooler.Instance.SpawnFromPool(SpawnType.CollectableFuelParticle, transform.position,
                Quaternion.identity, false);
            particleSystem.Play();
            
            FuelManager.Instance.AddFuel(_curLevelInfo.fuelAmount);
            
            base.DeActivate();
        }
    }
}