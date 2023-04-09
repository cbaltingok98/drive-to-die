using System;
using System.Collections;
using System.Collections.Generic;
using _project.Scripts.Enemy.Enemies;
using _project.Scripts.Enums;
using _project.Scripts.Models;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.BOMB
{
    public class SingleBomb : MonoBehaviour
    {
        [SerializeField] private SphereCollider sphereCollider;
        
        private List<EnemyBase> enemyToBlast;
        private WaitForSeconds waitFor;

        private float damage;
        public void Init(WaitForSeconds waitfor, float damage, float radius)
        {
            enemyToBlast = new List<EnemyBase>();
            waitFor = waitfor;
            this.damage = damage;
            
            SetColliderRadius(radius);

            var targetScale = Vector3.one * 2f;
            transform.localScale = Vector3.zero;
            
            gameObject.SetActive(true);

            transform.DOScale(targetScale, 0.4f).SetEase(Ease.OutBack);
            
            StartCoroutine(LateBlowUp());
        }


        private void SetColliderRadius(float value)
        {
            sphereCollider.radius = value;
        }


        private IEnumerator LateBlowUp()
        {
            yield return waitFor;
            BlowUp();
        }


        private void BlowUp()
        {
            for (var i = 0; i < enemyToBlast.Count; i++){
                enemyToBlast[i].TakeDamage(damage);
            }
            
            var pr = ParticlePooler.Instance.SpawnFromPool(SpawnType.RocketLauncherBlast, transform.position,
                Quaternion.identity, false);
            pr.Play();
            SoundManager.Instance.Play(SoundType.Explosion);
            gameObject.SetActive(false);
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;

            var enemy = other.GetComponent<EnemyBase>();

            if (enemyToBlast.Contains(enemy)) return;
            
            enemyToBlast.Add(enemy);
        }


        public void ResetItem()
        {
            
        }
    }
}