using System;
using System.Collections;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.Models;
using _project.Scripts.Pools;
using UnityEngine;

namespace _project.Scripts.Collectables
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private float newSpawnTime;
        [SerializeField] private CollectableType collectableType;
        [SerializeField] private Transform spawnPos;

        private ObjectPooler objectPooler;

        private Collectable myCollectable;

        private WaitForSeconds waitFor;
        private Coroutine myCoroutine;

        private void Start()
        {
            LevelManager.Instance.AddCollectableSpawner(this);
            
            objectPooler = ObjectPooler.Instance;

            waitFor = new WaitForSeconds(newSpawnTime);
            
            myCollectable =
                objectPooler.SpawnCollectableFromPool(collectableType, spawnPos.position);
            ActivateCollectable();
        }


        private void ActivateCollectable()
        {
            myCollectable.Initialize(1, ActivateNewCollectable);
        }


        private void ActivateNewCollectable()
        {
            myCoroutine = StartCoroutine(ActivationWait());
        }


        private IEnumerator ActivationWait()
        {
            yield return waitFor;
            ActivateCollectable();
        }


        public void Reset()
        {
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }
        }
    }
}