using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Data
{
    [Serializable]
    public struct ChapterSprite
    {
        public Sprite sprite;
        public ChapterType chapterType;
    }
    
    public class ChapterSpriteData : MonoBehaviour
    {
        [SerializeField] private List<ChapterSprite> chapterSprites;

        public static ChapterSpriteData Instance;


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


        public Sprite GetSpriteFor(ChapterType chapterType)
        {
            foreach (var chapterSprite in chapterSprites)
            {
                if (chapterSprite.chapterType == chapterType)
                {
                    return chapterSprite.sprite;
                }
            }

            return null;
        }
    }
}