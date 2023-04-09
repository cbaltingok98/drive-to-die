using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Player;
using _project.Scripts.Pools;
using UnityEngine;

namespace _project.Scripts.Platform
{
    [Serializable]
    public struct PlatformSettings
    {
        public bool Front;
        public bool Rear;
        public bool Left;
        public bool Right;
        public bool Self;

        public PlatformSettings(bool front, bool rear, bool left, bool right, bool self)
        {
            Front = front;
            Rear = rear;
            Left = left;
            Right = right;
            Self = self;
        }
    }

    public enum PlatformType
    {
        Front = 0,
        Rear = 1,
        Right = 2,
        Left = 3
    }

    public class PlatformSpawner : MonoBehaviour
    {
        [SerializeField] private float distanceToEnable;
        [SerializeField] private float distanceToDisable;
        [SerializeField] private Transform front;
        [SerializeField] private Transform rear;
        [SerializeField] private Transform left;
        [SerializeField] private Transform right;
        [SerializeField] private Transform self;
        [SerializeField] private PlatformSpawner prefab;
        [SerializeField] private Material roadMaterial;

        private Transform _playerTransform;
        private PlayerController _playerController;

        public PlatformSettings _platformSettings;

        public PlatformSpawner _frontPlatform; // TODO private
        public PlatformSpawner _rearPlatform;
        public PlatformSpawner _leftPlatform;
        public PlatformSpawner _rightPlatform;

        private bool _isActive;
        private bool _platformsSpawned;

        private float _updateRate = 1f;
        private float _counter;

        public float magnitude;

        public PlatformSpawner GetFrontPlatform() => _frontPlatform;
        public PlatformSpawner GetRearPlatform() => _rearPlatform;
        public PlatformSpawner GetLeftPlatform() => _leftPlatform;
        public PlatformSpawner GetRightPlatform() => _rightPlatform;


        public void SetPlatform(PlatformSpawner newPlatform, PlatformType platformType)
        {
            switch (platformType)
            {
             case  PlatformType.Front:
                 _frontPlatform = newPlatform;
                 break;
             case PlatformType.Rear:
                 _rearPlatform = newPlatform;
                 break;
             case PlatformType.Left:
                 _leftPlatform = newPlatform;
                 break;
             case PlatformType.Right:
                 _rightPlatform = newPlatform;
                 break;
            }
        }


        public void Init()
        {
            _playerController = PlayerController.Instance;
            _playerTransform = _playerController.transform;

            _isActive = true;
        }


        public void ApplySettings(PlatformSettings newSettings)
        {
            _platformSettings = newSettings;
        }
        

        public PlatformSettings GetPlatformSettings()
        {
            return _platformSettings;
        }


        private void Update()
        {
            if (!_isActive) return;

            transform.position = _playerTransform.position;
            var direction = new Vector2(-_playerTransform.position.x, -_playerTransform.position.z);
            var speed = _playerController.GetRealSpeed() * Time.deltaTime;

            //direction *= 1 * 0.03f;

            roadMaterial.mainTextureOffset = direction;

            // _counter += Time.deltaTime;
            // if (_counter < _updateRate) return;
            // _counter = 0f;
            //
            // CheckDistance();
        }


        private void CheckDistance()
        {
            magnitude = (transform.localPosition - _playerTransform.localPosition).magnitude;
            
            if (magnitude >= distanceToDisable)
            {
                _platformsSpawned = false;
                DisablePlatforms();
                Destroy(gameObject);
            }
            else if (gameObject.activeSelf && !_platformsSpawned && magnitude <= distanceToEnable)
            {
                EnablePlatforms();
                _platformsSpawned = true;
            }
        }


        private void EnablePlatforms()
        {
            var flag = false;
            if (!_platformSettings.Self)
            {
                _platformSettings.Self = true;
                self.gameObject.SetActive(true);
                flag = true;
            }
            
            if (!_platformSettings.Front && CanISpawnFront())
            {
                _platformSettings.Front = true;
                var newSettings = new PlatformSettings(false, true, false, false, false);
                _frontPlatform = SetNewPlatform(newSettings, front, PlatformType.Rear);
                flag = true;
            }
            
            if (!_platformSettings.Rear && CanISpawnRear())
            {
                _platformSettings.Rear = true;
                var newSettings = new PlatformSettings(true, false, false, false, false);
                _rearPlatform = SetNewPlatform(newSettings, rear, PlatformType.Front);
                flag = true;
            }
            
            if (!_platformSettings.Left && CanISpawnLeft())
            {
                _platformSettings.Left = true;
                var newSettings = new PlatformSettings(false, false, false, true, false);
                _leftPlatform = SetNewPlatform(newSettings, left, PlatformType.Right);
                flag = true;
            }
            
            if (!_platformSettings.Right && CanISpawnRight())
            {
                _platformSettings.Right = true;
                var newSettings = new PlatformSettings(false, false, true, false, false);
                _rightPlatform = SetNewPlatform(newSettings, right, PlatformType.Left);
                flag = true;
            }

            if (flag)
            {
                _platformsSpawned = true;
            }
        }


        private PlatformSpawner SetNewPlatform(PlatformSettings settings, Transform spawnPos, PlatformType platformType)
        {
            var platform = SpawnPlatformAt(spawnPos);
            platform.ApplySettings(settings);
            platform.SetPlatform(this, platformType);
            platform.Init();

            return platform;
        }


        private bool CanISpawnRight()
        {
            var res1 = _frontPlatform && _frontPlatform.GetRightPlatform() && _frontPlatform.GetRightPlatform().GetRearPlatform();
            var res2 = _rearPlatform && _rearPlatform.GetRightPlatform() && _rearPlatform.GetRightPlatform().GetFrontPlatform();

            return !(res1 || res2);
        }


        private bool CanISpawnLeft()
        {
            var res1 = _frontPlatform && _frontPlatform.GetLeftPlatform() && _frontPlatform.GetLeftPlatform().GetRearPlatform();
            var res2 = _rearPlatform && _rearPlatform.GetLeftPlatform() && _rearPlatform.GetLeftPlatform().GetFrontPlatform();

            return !(res1 || res2);
        }


        private bool CanISpawnFront()
        {
            var res1 = _leftPlatform && _leftPlatform.GetFrontPlatform() && _leftPlatform.GetFrontPlatform().GetRightPlatform();
            var res2 = _rightPlatform && _rightPlatform.GetFrontPlatform() && _rightPlatform.GetFrontPlatform().GetLeftPlatform();

            return !(res1 || res2);
        }


        private bool CanISpawnRear()
        {
            var res1 = _leftPlatform && _leftPlatform.GetRearPlatform() && _leftPlatform.GetRearPlatform().GetRightPlatform();
            var res2 = _rightPlatform && _rightPlatform.GetRearPlatform() && _rightPlatform.GetRearPlatform().GetLeftPlatform();

            return !(res1 || res2);
        }


        private PlatformSpawner SpawnPlatformAt(Transform targetTransform)
        {
            var newPlatform = Instantiate(prefab, targetTransform.position, Quaternion.identity, transform.parent);
            // var newPlatform = ObjectPooler.Instance.SpawnPlatformSpawnerFromPool(ObjectType.PlatformSpawner, targetTransform.position, Quaternion.identity, true, transform.parent);

            return newPlatform;
        }


        private void DisablePlatforms()
        {

            if (_platformSettings.Front && _frontPlatform)
            {
                var settings = _frontPlatform.GetPlatformSettings();
                settings.Rear = false;
                _frontPlatform.ApplySettings(settings);
                _frontPlatform.SetPlatform(null, PlatformType.Rear);
                _frontPlatform = null;
                _platformSettings.Front = false;
            }
            
            if (_platformSettings.Rear && _rearPlatform)
            {
                var settings = _rearPlatform.GetPlatformSettings();
                settings.Front = false;
                _rearPlatform.ApplySettings(settings);
                _rearPlatform.SetPlatform(null, PlatformType.Front);
                _rearPlatform = null;
                _platformSettings.Rear = false;
            }
            
            if (_platformSettings.Left && _leftPlatform)
            {
                var settings = _leftPlatform.GetPlatformSettings();
                settings.Right = false;
                _leftPlatform.ApplySettings(settings);
                _leftPlatform.SetPlatform(null, PlatformType.Right);
                _leftPlatform = null;
                _platformSettings.Left = false;
            }
            
            if (_platformSettings.Right && _rightPlatform)
            {
                var settings = _rightPlatform.GetPlatformSettings();
                settings.Left = false;
                _rightPlatform.ApplySettings(settings);
                _rightPlatform.SetPlatform(null, PlatformType.Left);
                _rightPlatform = null;
                _platformSettings.Right = false;
            }

            
            _platformSettings.Self = false;
            _isActive = false;
            gameObject.SetActive(false);
            
        }
        
    }
}