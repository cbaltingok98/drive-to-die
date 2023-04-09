using System;
using System.Collections.Generic;
using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.Enemy.SpecialAbility;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Player;
using _project.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace _project.Scripts.Enemy.Enemies
{
    public struct EnemyInitializeInfo
    {
        public int Level;
        public Transform Target;
        public SkillManager SkillManager;
        public CanvasElementPooler CanvasElementPooler;
        public UnityAction<float, Vector3> DeathAction;
        public UnityAction DeSpawnAction;
        public PlayerController PlayerController;


        public EnemyInitializeInfo(EnemyInitializeInfo info)
        {
            Level = info.Level;
            Target = info.Target;
            SkillManager = info.SkillManager;
            CanvasElementPooler = info.CanvasElementPooler;
            DeathAction = info.DeathAction;
            DeSpawnAction = info.DeSpawnAction;
            PlayerController = info.PlayerController;
        }
        
        public EnemyInitializeInfo(int level, Transform target, SkillManager skillManager, CanvasElementPooler canvasElementPooler, UnityAction<float, Vector3> action, UnityAction deSpawnAction , PlayerController playerController)
        {
            Level = level;
            Target = target;
            SkillManager = skillManager;
            CanvasElementPooler = canvasElementPooler;
            DeathAction = action;
            DeSpawnAction = deSpawnAction;
            PlayerController = playerController;
        }
    } 
    
    [Serializable]
    public struct EnemyBasicInfo
    {
        public float health;
        public float moveSpeed;
        public float attackDamage;
        public float killScore;
    }
    public abstract class EnemyBase : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private SpecialAbilityBase specialAbilityBase;
        [SerializeField] protected List<EnemyBasicInfo> zombieInfos;
        [SerializeField] protected Canvas myCanvas;
        [SerializeField] protected CapsuleCollider capsuleCollider;
        [SerializeField] protected Transform modelParent;
        [SerializeField] protected SpriteRenderer fakeShadow;

        protected SkillManager SkillManager;
        protected CanvasElementPooler CanvasElementPooler;
        protected PlayerController PlayerController;

        protected Rigidbody Rb;
        protected Animator Animator;
        protected Transform TargetTransform;

        protected UnityAction<float, Vector3> DeathAction;
        protected UnityAction DeSpawnAction;

        protected EnemyBasicInfo MyBasicInfo;

        private Vector3 difference;
        protected Vector3 Direction;
        private Vector3 flyDirection;
        private Vector3 flyToTarget;

        protected bool IsActive;
        protected bool Flying;
        protected bool ChosenForClosest;
        protected bool GameEnd;

        private float speed;
        private float deathDistance = 2f;
        private float playerSpeed;
        private float heightOffset;
        private float rotateAmount;

        private float curHealth;

        private int level;


        protected virtual void Awake()
        {
            Rb = GetComponent<Rigidbody>();
        }


        public EnemyType GetEnemyType() => enemyType;

        public virtual void Activate(int level)
        {
            GameEnd = false;
            this.level = (level - 1) >= zombieInfos.Count ? zombieInfos.Count : level;
            MyBasicInfo = zombieInfos[this.level - 1];
            curHealth = MyBasicInfo.health;
            transform.LookAt(TargetTransform);
            modelParent.localEulerAngles = Vector3.zero;
            fakeShadow.enabled = true;
            rotateAmount = 0f;
            IsActive = true;
            Flying = false;
        }

        public void DeActivate()
        {
            IsActive = false;
            Flying = false;
        }
        public virtual void Init(EnemyInitializeInfo initializeInfo) // TODO unnecessary
        {
            // EnemyInitializeInfo = new EnemyInitializeInfo(initializeInfo);
            level = initializeInfo.Level;
            TargetTransform = initializeInfo.Target;
            SkillManager = initializeInfo.SkillManager;
            CanvasElementPooler = initializeInfo.CanvasElementPooler;
            DeathAction = initializeInfo.DeathAction;
            DeSpawnAction = initializeInfo.DeSpawnAction;
            PlayerController = initializeInfo.PlayerController;

            UpdateZombieInfo();
        }

        private void UpdateZombieInfo()
        {
            MyBasicInfo = zombieInfos[level - 1];
        }
        
        private void FixedUpdate()
        {
            if (!IsActive || ChosenForClosest) return;
            if (SkillManager.HasClosestEnemy()) return;
            
            ChosenForClosest = SkillManager.SetClosestEnemy(new ClosestEnemyInfo(this, difference.magnitude));
        }
        
        protected virtual void Update()
        {
            if (IsActive && !GameEnd && TargetTransform)
            {
                GoToTarget();
            }
            else if (!IsActive && Flying)
            {
                Fly();
            }
        }


        protected void GoToTarget()
        {
            difference = TargetTransform.position - transform.position;
            Direction = difference.normalized;
            Direction.y = 0f;

            CheckCollision();

            speed = MyBasicInfo.moveSpeed * Time.deltaTime;
            Rb.position += Direction * speed;

            var rotation = Vector3.RotateTowards(transform.forward, Direction, speed, 0f);
            transform.rotation = Quaternion.LookRotation(rotation);
        }


        protected void Fly()
        {
            var target = flyToTarget;
            target.y += heightOffset;

            heightOffset = Mathf.Lerp(heightOffset, -1f, Time.deltaTime * playerSpeed * 0.2f);

            transform.position = Vector3.Slerp(transform.position, target, Time.deltaTime * playerSpeed * 0.1f);

            modelParent.rotation = Quaternion.AngleAxis(rotateAmount, transform.right);

            rotateAmount -= 9f;

            if (transform.position.y > 0) return;
            
            if (curHealth <= 0)
            {
                Die();
            }
            else
            {
                Flying = false;
                IsActive = true;
                fakeShadow.enabled = true;
                var pos = transform.position;
                pos.y = 0f;
                transform.position = pos; 
                modelParent.DOLocalRotate(Vector3.zero, 0.5f);
            }
        }

        private void CheckCollision()
        {
            var dist = difference.sqrMagnitude;
            if (dist < deathDistance * deathDistance)
            {
                PlayerController.TakeDamage(MyBasicInfo.attackDamage);
                TakeDamage(100f, true);
            }
            else if (dist > 8000f)
            {
                DeSpawn();
            }
        }

        protected virtual void Die()
        {
            if (GameEnd) return;
            
            myCanvas.gameObject.SetActive(false);
            DeathAction?.Invoke(MyBasicInfo.killScore, transform.position);
            if (ChosenForClosest)
            {
               RemovedFromClosestTarget();
            }
            gameObject.SetActive(false);
            IsActive = false;
            Flying = false;
        }


        private void ShowDamageUI(float val)
        {
            myCanvas.gameObject.SetActive(true);
            var uiElement = CanvasElementPooler.SpawnFromPool(UiElementType.DamageTaken, Vector3.zero, Quaternion.identity, true,
                myCanvas.transform);
            uiElement.text = val.ToString("0");
        }

        public virtual void TakeDamage(float damage, bool byHit = false)
        {
            if (!IsActive) return;
            
            curHealth -= damage;

            if (byHit && PlayerController.GetRealSpeed() > 17.5f)
            {
                SoundManager.Instance.Play(SoundType.ZombieHit);
                VibrationManager.MediumVibrate();
                CalculateFlyDirection();
                fakeShadow.enabled = false;
                IsActive = false;
                Flying = true;
                
                return;
            }
            
            if (curHealth <= 0)
            {
                VibrationManager.LightVibrate();
                Die();
                return;
            }
            
            ShowDamageUI(damage);
        }

        public void RemovedFromClosestTarget()
        {
            ChosenForClosest = false;
            SkillManager.RemoveClosestEnemy(true);
        }


        private void CalculateFlyDirection()
        {
            playerSpeed = PlayerController.GetRealSpeed();
            var playerMoveDirection = PlayerController.GetMoveForce();
            var directionBetween = -Direction;
            flyDirection = (playerMoveDirection + directionBetween).normalized;
            flyToTarget = transform.position + flyDirection * (playerSpeed * 0.5f);
            heightOffset = 9f;
        }


        private void DeSpawn()
        {
            DeSpawnAction?.Invoke();
            ResetSelf();
        }


        public void GameOver()
        {
            GameEnd = true;
            
            if (ChosenForClosest)
            {
                SkillManager.RemoveClosestEnemy(true);
            }
            ChosenForClosest = false;
        }


        public void ResetSelf()
        {
            GameEnd = false;
            IsActive = false;
            Flying = false;
            
            if (ChosenForClosest)
            {
                SkillManager.RemoveClosestEnemy(true);
            }
            ChosenForClosest = false;
            
            modelParent.localEulerAngles = Vector3.zero;
            fakeShadow.enabled = true;
            rotateAmount = 0f;
            
            gameObject.SetActive(false);
            transform.position = Vector3.zero;
        }

        // GETTERS - start //
        public EnemyBasicInfo GetEnemyBasicInfo() => MyBasicInfo;
        // GETTERS - end //
    }
}