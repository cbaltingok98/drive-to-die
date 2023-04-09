using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.UI.Menu;
using _project.Scripts.UI.Menu.GarageMenu;
using _project.Scripts.UI.Menu.ShopMenu;
using TMPro;
using UnityEngine;

namespace _project.Scripts.UI
{
    public class CanvasElementPooler : MonoBehaviour
    {
        public static CanvasElementPooler Instance;
        
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

        [Serializable]
        public class Pool<T> where T : class
        {
            public UiElementType tag;
            public T prefab;
            public int size;
            public Transform parent;
        }
 
        public List<Pool<TextMeshProUGUI>> pools;
        public List<Pool<ScrollViewItemUi>> pools2;
        public List<Pool<SpecItem>> pools3;
        public List<Pool<ScrollViewItemArmoryUi>> pools4;
        public List<Pool<ShopSingleItemUi>> pools5;
        
        private Dictionary<UiElementType, Queue<TextMeshProUGUI>> poolDictionary;
        private Dictionary<UiElementType, Queue<ScrollViewItemUi>> poolDictionary2;
        private Dictionary<UiElementType, Queue<SpecItem>> poolDictionary3;
        private Dictionary<UiElementType, Queue<ScrollViewItemArmoryUi>> poolDictionary4;
        private Dictionary<UiElementType, Queue<ShopSingleItemUi>> poolDictionary5;
    
        public void Init()
        {
            poolDictionary = new Dictionary<UiElementType, Queue<TextMeshProUGUI>>();

            foreach (var pool in pools)
            {
                Queue<TextMeshProUGUI> objectPool = new Queue<TextMeshProUGUI>();
                for (int i = 0; i < pool.size; i++)
                {
                    TextMeshProUGUI obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary.Add(pool.tag, objectPool);
            } 
            
            poolDictionary2 = new Dictionary<UiElementType, Queue<ScrollViewItemUi>>();

            foreach (var pool in pools2)
            {
                Queue<ScrollViewItemUi> objectPool = new Queue<ScrollViewItemUi>();
                for (int i = 0; i < pool.size; i++)
                {
                    ScrollViewItemUi obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary2.Add(pool.tag, objectPool);
            } 
            
            poolDictionary3 = new Dictionary<UiElementType, Queue<SpecItem>>();

            foreach (var pool in pools3)
            {
                Queue<SpecItem> objectPool = new Queue<SpecItem>();
                for (int i = 0; i < pool.size; i++)
                {
                    SpecItem obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary3.Add(pool.tag, objectPool);
            } 
            
            poolDictionary4 = new Dictionary<UiElementType, Queue<ScrollViewItemArmoryUi>>();

            foreach (var pool in pools4)
            {
                Queue<ScrollViewItemArmoryUi> objectPool = new Queue<ScrollViewItemArmoryUi>();
                for (int i = 0; i < pool.size; i++)
                {
                    ScrollViewItemArmoryUi obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary4.Add(pool.tag, objectPool);
            } 
            
            poolDictionary5 = new Dictionary<UiElementType, Queue<ShopSingleItemUi>>();

            foreach (var pool in pools5)
            {
                Queue<ShopSingleItemUi> objectPool = new Queue<ShopSingleItemUi>();
                for (int i = 0; i < pool.size; i++)
                {
                    ShopSingleItemUi obj = Instantiate(pool.prefab);
                
                    if(pool.parent)
                        obj.transform.SetParent(pool.parent);
                
                    obj.gameObject.SetActive(false);
                    objectPool.Enqueue(obj);
                }
                poolDictionary5.Add(pool.tag, objectPool);
            } 
        }

        public TextMeshProUGUI SpawnFromPool(UiElementType tag, Vector3 position, Quaternion rotation, bool inPlace, Transform parent = null)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            TextMeshProUGUI objectToSpawn = poolDictionary[tag].Dequeue();

            if (inPlace)
            {
                objectToSpawn.transform.SetParent(parent);
            }
            
            objectToSpawn.gameObject.SetActive(true);
            objectToSpawn.transform.localPosition = position; 
            objectToSpawn.transform.rotation = rotation;
        
            poolDictionary[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public ScrollViewItemUi SpawnScrollViewItemFromPool(UiElementType tag, Transform parent)
        {
            if (!poolDictionary2.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            ScrollViewItemUi objectToSpawn = poolDictionary2[tag].Dequeue();

    
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.gameObject.SetActive(true);
        
            poolDictionary2[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public SpecItem SpawnScrollViewInfoItemFromPool(UiElementType tag, Transform parent)
        {
            if (!poolDictionary3.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            SpecItem objectToSpawn = poolDictionary3[tag].Dequeue();

    
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.gameObject.SetActive(true);
        
            poolDictionary3[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public ScrollViewItemArmoryUi SpawnScrollViewArmoryInfoItemFromPool(UiElementType tag, Transform parent)
        {
            if (!poolDictionary4.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            ScrollViewItemArmoryUi objectToSpawn = poolDictionary4[tag].Dequeue();

    
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.gameObject.SetActive(true);
        
            poolDictionary4[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }
        
        public ShopSingleItemUi SpawnShopSingleItem(UiElementType tag, Transform parent)
        {
            if (!poolDictionary5.ContainsKey(tag))
            {
                Printer.Print("Pool with tag " + tag + "doesn't exist");
                return null;
            }
            ShopSingleItemUi objectToSpawn = poolDictionary5[tag].Dequeue();

    
            objectToSpawn.transform.SetParent(parent);
            objectToSpawn.gameObject.SetActive(true);
        
            poolDictionary5[tag].Enqueue(objectToSpawn);
        
            return objectToSpawn;
        }


        public void Reset()
        {
            foreach (var dictionary in poolDictionary)
            {
                var queue = poolDictionary[dictionary.Key];

                foreach (var textMeshProUGUI in queue)
                {
                    textMeshProUGUI.transform.SetParent(transform);
                }
            }
        }
    }
}