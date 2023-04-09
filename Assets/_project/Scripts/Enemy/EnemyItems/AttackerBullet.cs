using System;
using _project.Scripts.Player;
using UnityEngine;

namespace _project.Scripts.Enemy.EnemyItems
{
    public class AttackerBullet : MonoBehaviour
    {
        private bool active;

        private PlayerController playerController;
        
        private Transform target;
        
        private Vector3 dir;
        private float damage;
        private float speed;

        private float hitDistance = 3f;
        private float aliveTime = 5f;
        private float timer;

        public void Init(Vector3 direction, float damage, float speed, PlayerController playerController)
        {
            this.playerController = playerController;
            target = playerController.transform;
            timer = aliveTime;
            dir = direction;
            this.damage = damage;
            this.speed = speed;

            active = true;
        }


        private void Update()
        {
            if (!active) return;

            //_dir = (_target.position - transform.position).normalized;

            transform.position += dir * (speed * Time.deltaTime);
            CheckForHit();

            timer -= Time.deltaTime;

            if (timer > 0f) return;
            Deactivate();    
        }


        private void CheckForHit()
        {
            var distance = (target.position - transform.position).sqrMagnitude;
            if (distance > (hitDistance * hitDistance)) return;
            
            playerController.TakeDamage(damage);
            Deactivate();
        }


        public void Deactivate()
        {
            active = false;
            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }
    }
}