using _project.Scripts.Berk;
using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Player
{
    public class PlayerController : CarController
    {
        public static PlayerController Instance;

        [SerializeField] private HealthBar myHealthBar;

        private float maxHealth;

        private bool gameOver;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private Vector3 myCanvasTargetScale;

        private bool damageBlockActive;


        public float GetMaxHealth() => maxHealth;

        public Transform GetShieldIcon() => myHealthBar.GetShieldIcon();
        
        public override void Init()
        {
            SetupHealthBar();
            gameOver = false;
            myHealthBar.Control(true);
            
            base.Init();
        }


        public void DisableHealthBar()
        {
            myHealthBar.Control(false);
        }


        public void SetupHealthBar()
        {
            maxHealth = car.health;
            myHealthBar.SetMaxHealth(car.health);
        }


        public void WarnPlayerForEndingShield()
        {
            
        }

        public Car.DrivingMechanic.Car GetProfile() => car;


        public void ControlAttackBlocker(bool set)
        {
            damageBlockActive = set;

            myHealthBar.Control(!set);
        }


        public void TakeDamage(float damageTaken)
        {
            if (damageBlockActive) return;

            if (!IsDrifting())
            {
                damageTaken = damageTaken * 0.8f;
            }
            
            car.health -= damageTaken;
            
            UpdateHealthBar(true);
            CheckIfGameOver();
        }


        public void AddHealth(float amount)
        {
            var newHealth = car.health + amount;
            newHealth = newHealth > maxHealth ? maxHealth : newHealth;
            DOTween.To(() => car.health, x => car.health = x, newHealth, 0.2f).OnUpdate(() =>
            {
                UpdateHealthBar(false);
            });
        }


        private void CheckIfGameOver()
        {
            if (car.health > 0 || gameOver) return;
            gameOver = true;

            LevelManager.Instance.LevelCompleted(false);
            CanvasManager.Instance.GetInGameUI().ShowWarningMessage(WarningType.Death);
        }


        private void UpdateHealthBar(bool warnPlayer)
        {
            var val = car.health > 0 ? car.health : 0f;
            myHealthBar.UpdateHealth(val, warnPlayer);
        }
    }
}