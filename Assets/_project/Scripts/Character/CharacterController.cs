using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Character
{
    [Serializable]
    public class CharacterInfo
    {
        public CharacterType characterType;
        public Transform model;
        public Transform speechBubble;
    }
    public class CharacterController : MonoBehaviour
    {
        public static CharacterController Instance;
        
        [SerializeField] private List<CharacterInfo> characterInfos;


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


        public Transform GetCharacter(CharacterType character)
        {
            for (var i = 0; i < characterInfos.Count; i++)
            {
                if (characterInfos[i].characterType == character)
                {
                    characterInfos[i].speechBubble.gameObject.SetActive(!DataManager.Instance.IsPlayedBefore());
                    
                    return characterInfos[i].model;
                }
            }

            return null;
        }
    }
}