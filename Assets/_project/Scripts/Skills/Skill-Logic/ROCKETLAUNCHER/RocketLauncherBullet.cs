using System.Collections.Generic;
using _project.Scripts.Enemy.Enemies;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.ROCKETLAUNCHER
{
    public class RocketLauncherBullet : MonoBehaviour
    {
        [SerializeField] private ParticleSystem trailParticle;
        [SerializeField] private SphereCollider sphereCollider;
        
        private RocketLauncherTarget myTarget;

        private List<EnemyBase> enemyToBlast;

        private Transform parentToGoBack;

        private float flySpeed;
        private float offset = 10f;
        private float offsetTarget;
        private float curveSpeed = 5f;
        private float curveProgress;
        private float rocketProgress;

        private float damage;

        private bool isActive;
        public void DestroyTarget(RocketLauncherTarget targetToDestroy, Transform parent,  RocketLauncherFeatures levelInfo)
        {
            enemyToBlast = new List<EnemyBase>();
            SetColliderRadius(levelInfo.blastRadius);
            damage = levelInfo.damage;
            curveSpeed = levelInfo.rocketSpeed + 1;
            myTarget = targetToDestroy;
            parentToGoBack = parent;
            flySpeed = levelInfo.rocketSpeed;
            
            transform.SetParent(targetToDestroy.transform);

            curveProgress = 0f;
            rocketProgress = 0f;

            offsetTarget = -2f;
            offset = 10f;
            
            trailParticle.Play();
            isActive = true;
            
            myTarget.DeAttach();
        }


        private void SetColliderRadius(float radius)
        {
            sphereCollider.radius = radius;
        }


        public void Blast()
        {
            isActive = false;
            myTarget.DeActivate();
            gameObject.SetActive(false);
            transform.SetParent(parentToGoBack);

            for (var i = 0; i < enemyToBlast.Count; i++)
            {
                enemyToBlast[i].TakeDamage(damage);
            }
            
            SoundManager.Instance.Play(SoundType.Explosion);
        }


        private void Update()
        {
            if (!isActive) return;

            var target = myTarget.transform.position;
            target.y += offset;
            
            offset = Mathf.Lerp(offset, offsetTarget, Time.deltaTime * curveSpeed);

            transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime * flySpeed);

            transform.LookAt(target);

            var dist = (transform.position - myTarget.transform.position).sqrMagnitude;

            if (dist * dist > 0.8f * 0.8f) return;
            
            Blast();
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
            isActive = false;
            curveProgress = 0f;
            rocketProgress = 0f;

            offsetTarget = -2f;
            offset = 10f;
            
            SetColliderRadius(0);
            trailParticle.Stop();
            transform.SetParent(parentToGoBack);
            gameObject.SetActive(false);
        }
    }
}