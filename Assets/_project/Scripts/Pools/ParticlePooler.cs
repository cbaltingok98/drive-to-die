using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Models
{
    public class ParticlePooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public SpawnType tag;
            public GameObject prefab;
            public int size;
            public Transform parent;
        }
        #region Singleton
        public static ParticlePooler Instance;
    
        private void Awake()
        {
            Instance = this;
        }
        #endregion
 
        public List<Pool> pools;
        private Dictionary<SpawnType, Queue<ParticleSystem>> poolDictionary;
    
        public void Init()
        {
            poolDictionary = new Dictionary<SpawnType, Queue<ParticleSystem>>();

            foreach (Pool pool in pools)
            {
                Queue<ParticleSystem> objectPool = new Queue<ParticleSystem>();
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.parent = pool.parent;
                
                    obj.SetActive(false);
                    objectPool.Enqueue(obj.GetComponent<ParticleSystem>());
                }
                poolDictionary.Add(pool.tag, objectPool);
            } 
        }

        public ParticleSystem SpawnFromPool(SpawnType tag, Vector3 position, Quaternion rotation, bool inPlace, Transform parent = null)
        {
            ParticleSystem objectToSpawn = poolDictionary[tag].Dequeue();

            if (inPlace)
            {
                objectToSpawn.transform.SetParent(parent);
            }
            
            objectToSpawn.gameObject.SetActive(true);
            // objectToSpawn.transform.SetLocalPositionAndRotation(position, rotation);
            position.y = 1f;
            objectToSpawn.transform.localPosition = position; 
            // objectToSpawn.transform.localRotation = rotation;
        
            poolDictionary[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public ParticleSystem SpawnFromPool(SpawnType tag, Vector3 position)
        {
            ParticleSystem objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.localPosition = position;
        
            poolDictionary[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }


        public void Reset()
        {
            foreach (var dictionary in poolDictionary)
            {
                var queue = poolDictionary[dictionary.Key];
                foreach (var particleSystem in queue)
                {
                    particleSystem.Stop();
                    particleSystem.gameObject.SetActive(false);
                    particleSystem.transform.SetParent(transform);
                }
            }
        }
    }
}