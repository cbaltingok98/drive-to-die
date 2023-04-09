using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Collectables;
using _project.Scripts.Enemy.Enemies;
using _project.Scripts.Enemy.EnemyItems;
using _project.Scripts.Enums;
using _project.Scripts.Platform;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Skills.Skill_Logic.BOMB;
using _project.Scripts.Skills.Skill_Logic.ROCKETLAUNCHER;
using UnityEngine;

namespace _project.Scripts.Pools
{
    public class ObjectPooler : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public CollectableType tag;
            public Collectable prefab;
            public int size;
            public Transform parent;
        }

        [System.Serializable]
        public class Pool2<T> where T : class
        {
            public ObjectType tag;
            public T prefab;
            public int size;
            public Transform parent;
        }
        #region Singleton
        public static ObjectPooler Instance;
    
        private void Awake()
        {
            Instance = this;
        }
        #endregion
 
        public List<Pool2<RocketLauncherBullet>> pools;
        public List<Pool2<RocketLauncherTarget>> pools2;
        public List<Pool> pools3;
        public List<Pool2<SingleBomb>> pools4;
        public List<Pool2<PlatformSpawner>> pools5;
        public List<Pool2<AttackerBullet>> pools6;
        public List<Pool2<AudioSource>> pools7;
        
        private Dictionary<ObjectType, Queue<RocketLauncherBullet>> poolDictionary;
        private Dictionary<ObjectType, Queue<RocketLauncherTarget>> poolDictionary2;
        private Dictionary<CollectableType, Queue<Collectable>> poolDictionary3;
        private Dictionary<ObjectType, Queue<SingleBomb>> poolDictionary4;
        private Dictionary<ObjectType, Queue<PlatformSpawner>> poolDictionary5;
        private Dictionary<ObjectType, Queue<AttackerBullet>> poolDictionary6;
        private Dictionary<ObjectType, Queue<AudioSource>> poolDictionary7;
    
        public void Init()
        {
            poolDictionary = new Dictionary<ObjectType, Queue<RocketLauncherBullet>>();

            foreach (var pool in pools)
            {
                Queue<RocketLauncherBullet> objectPool = new Queue<RocketLauncherBullet>();
                for (int i = 0; i < pool.size; i++)
                {
                    RocketLauncherBullet obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            } 
            
            poolDictionary2 = new Dictionary<ObjectType, Queue<RocketLauncherTarget>>();

            foreach (var pool in pools2)
            {
                Queue<RocketLauncherTarget> objectPool = new Queue<RocketLauncherTarget>();
                for (int i = 0; i < pool.size; i++)
                {
                    RocketLauncherTarget obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary2.Add(pool.tag, objectPool);
            } 
            
            poolDictionary3 = new Dictionary<CollectableType, Queue<Collectable>>();

            foreach (var pool in pools3)
            {
                Queue<Collectable> objectPool = new Queue<Collectable>();
                for (int i = 0; i < pool.size; i++)
                {
                    Collectable obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary3.Add(pool.tag, objectPool);
            } 
            
            poolDictionary4 = new Dictionary<ObjectType, Queue<SingleBomb>>();

            foreach (var pool in pools4)
            {
                Queue<SingleBomb> objectPool = new Queue<SingleBomb>();
                for (int i = 0; i < pool.size; i++)
                {
                    SingleBomb obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary4.Add(pool.tag, objectPool);
            } 
            
            poolDictionary5 = new Dictionary<ObjectType, Queue<PlatformSpawner>>();

            foreach (var pool in pools5)
            {
                Queue<PlatformSpawner> objectPool = new Queue<PlatformSpawner>();
                for (int i = 0; i < pool.size; i++)
                {
                    PlatformSpawner obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary5.Add(pool.tag, objectPool);
            } 
            
            poolDictionary6 = new Dictionary<ObjectType, Queue<AttackerBullet>>();

            foreach (var pool in pools6)
            {
                Queue<AttackerBullet> objectPool = new Queue<AttackerBullet>();
                for (int i = 0; i < pool.size; i++)
                {
                    AttackerBullet obj = Instantiate(pool.prefab);
                    
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary6.Add(pool.tag, objectPool);
            }
            
            poolDictionary7 = new Dictionary<ObjectType, Queue<AudioSource>>();

            foreach (var pool in pools7)
            {
                Queue<AudioSource> objectPool = new Queue<AudioSource>();
                for (int i = 0; i < pool.size; i++)
                {
                    AudioSource obj = Instantiate(pool.prefab);
                    
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary7.Add(pool.tag, objectPool);
            }
        }

        public RocketLauncherBullet SpawnFromPool(ObjectType tag, Vector3 position, bool inPlace, Transform parent = null)
        {
            RocketLauncherBullet objectToSpawn = poolDictionary[tag].Dequeue();

            if (inPlace)
            {
                objectToSpawn.transform.SetParent(parent);
            }
            
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.localPosition = position;

            poolDictionary[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public RocketLauncherTarget SpawnTargetFromPool(ObjectType tag, Vector3 position, bool inPlace, Transform parent = null)
        {
            RocketLauncherTarget objectToSpawn = poolDictionary2[tag].Dequeue();
            
            objectToSpawn.transform.SetParent(parent);
            
            objectToSpawn.transform.localPosition = position;

            poolDictionary2[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public Collectable SpawnCollectableFromPool(CollectableType tag, Vector3 position)
        {
            if (!poolDictionary3.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            Collectable objectToSpawn = poolDictionary3[tag].Dequeue();
            
            objectToSpawn.transform.localPosition = position;
        
            poolDictionary3[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        
        public SingleBomb SpawnSingleBombFromPool(ObjectType tag, Vector3 position)
        {
            if (!poolDictionary4.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            SingleBomb objectToSpawn = poolDictionary4[tag].Dequeue();

            position.y += 0.5f;
            objectToSpawn.transform.localPosition = position;

            poolDictionary4[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public PlatformSpawner SpawnPlatformSpawnerFromPool(ObjectType tag, Vector3 position, Quaternion rotation, bool inPlace, Transform parent = null)
        {
            if (!poolDictionary5.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            
            PlatformSpawner objectToSpawn = poolDictionary5[tag].Dequeue();

            if (inPlace)
            {
                objectToSpawn.transform.SetParent(parent);
            }
            
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.localPosition = position; 
            objectToSpawn.transform.rotation = rotation;
        
            poolDictionary5[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }


        public AttackerBullet SpawnAttackerBullet(ObjectType tag, Vector3 position)
        {
            if (!poolDictionary6.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            
            AttackerBullet objectToSpawn = poolDictionary6[tag].Dequeue();

            
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.localPosition = position; 
        
            poolDictionary6[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public void SpawnAudioSource(ObjectType tag, Sound clip)
        {
            AudioSource objectToSpawn = poolDictionary7[tag].Dequeue();
            poolDictionary7[tag].Enqueue(objectToSpawn);
            
            objectToSpawn.gameObject.SetActive(true);

            var rand = Random.Range(0f, 0.2f);
        
            objectToSpawn.Stop();
            objectToSpawn.volume = clip.Volume;
            objectToSpawn.pitch = clip.Pitch - rand;
            objectToSpawn.PlayOneShot(clip.Clip);
        }

        public void Reset()
        {
            foreach (var dictionary in poolDictionary)
            {
                var queue = poolDictionary[dictionary.Key];
                foreach (var rocketLauncherBullet in queue)
                {
                    rocketLauncherBullet.ResetItem();
                    rocketLauncherBullet.transform.SetParent(transform);
                }
            }
            
            foreach (var dictionary in poolDictionary2)
            {
                var queue = poolDictionary2[dictionary.Key];
                foreach (var rocketLauncherTarget in queue)
                {
                    rocketLauncherTarget.ResetItem();
                    rocketLauncherTarget.transform.SetParent(transform);
                }
            }
            
            foreach (var dictionary in poolDictionary3)
            {
                var queue = poolDictionary3[dictionary.Key];
                foreach (var collectable in queue)
                {
                    collectable.ResetItem();
                }
            }
            
            foreach (var dictionary in poolDictionary4)
            {
                var queue = poolDictionary4[dictionary.Key];
                foreach (var singleBomb in queue)
                {
                    singleBomb.ResetItem();
                }
            }
            
            foreach (var dictionary in poolDictionary5)
            {
                var queue = poolDictionary5[dictionary.Key];
                foreach (var platformSpawner in queue)
                {
                    platformSpawner.transform.SetParent(transform);
                }
            }

            foreach (var dictionary in poolDictionary6)
            {
                var queue = poolDictionary6[dictionary.Key];
                foreach (var attackerBullet in queue)
                {
                    attackerBullet.Deactivate();
                }
            }
        }
    }
}