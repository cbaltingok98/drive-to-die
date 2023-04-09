using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Pools;
using UnityEngine;

namespace _project.Scripts.Enemy.Enemies
{
    [Serializable]
    public class AttackerInfo
    {
        public float fireRate;
        public float damage;
        public float speed;
        public int amount;
    }
    
    public class Attacker : EnemyBase
    {
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private List<AttackerInfo> attackerInfo;

        private ObjectPooler objectPooler;
        private AttackerInfo currentInfo;

        private WaitForSeconds waitFor;

        private Coroutine fireCoroutine;
        
        private float rateCounter;



        public override void Init(EnemyInitializeInfo initializeInfo) // TODO unnecessary
        {
            objectPooler = ObjectPooler.Instance;
            waitFor = new WaitForSeconds(0.15f);
            base.Init(initializeInfo);
        }


        public override void Activate(int level)
        {
            currentInfo = attackerInfo[level - 1 >= attackerInfo.Count ? attackerInfo.Count - 1 : level - 1];
            base.Activate(level);
        }
        
        protected override void Update()
        {
            if (!GameEnd && IsActive && TargetTransform)
            {
                GoToTarget();
                CheckFire();
            }
            else if (!IsActive && Flying)
            {
                Fly();
            }
        }


        private void CheckFire()
        {
            rateCounter += Time.deltaTime;
            if (rateCounter < currentInfo.fireRate) return;
            rateCounter = 0f;

            if (!gameObject.activeSelf) return;
            fireCoroutine = StartCoroutine(Fire());
        }
        
        private IEnumerator Fire()
        {
            for (var i = 0; i < currentInfo.amount; i++)
            {
                Spawn();
                yield return waitFor;
            }
        }


        protected override void Die()
        {
            if (fireCoroutine != null)
            {
                StopCoroutine(fireCoroutine);
            }
            base.Die();
        }
        
        private void Spawn()
        {
            var newBullet =
                objectPooler.SpawnAttackerBullet(ObjectType.AttackerBullet, bulletSpawnPoint.position);
            
            
            newBullet.Init(Direction, currentInfo.damage, currentInfo.speed, PlayerController);
        }
    }
}