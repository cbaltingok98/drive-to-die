using System;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private Vector3 offset;
        
        private Transform target;

        private bool isActive;

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
            
            gameObject.SetActive(false);
        }

        private void LateUpdate()
        {
            if (!isActive) return;
            
            var desiredPosition = target.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }

        public void SetPlayer(Transform player)
        {
            target = player;
            isActive = true;
        }

        public void ControlInLevelCamera(bool set)
        {
            gameObject.SetActive(set);
        }
    }
}