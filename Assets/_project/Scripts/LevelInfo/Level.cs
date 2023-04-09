using System;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.LevelInfo
{
    [Serializable]
    public class Level
    {
        public ChapterType id;
        public GameObject levelPrefab;
        public string chapterName;
        public int chapterTarget;

        public Level(LevelObject levelObject)
        {
            id = levelObject.id;
            levelPrefab = levelObject.levelPrefab;
            chapterName = levelObject.chapterName;
            chapterTarget = levelObject.chapterTarget;
        }
    }
}