using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Managers;

namespace _project.Scripts.LevelInfo
{
    public static class LevelObjectManager
    {
        private static bool initialized;

        private static Dictionary<ChapterType, LevelObject> levelObjects;

        private static void Init()
        {
            if (initialized) return;
            initialized = true;

            LevelObjectManager.levelObjects = new Dictionary<ChapterType, LevelObject>();
            var levelObjects = DataManager.GetLevelObjects();

            foreach (var levelObject in levelObjects)
            {
                LevelObjectManager.levelObjects.Add(levelObject.id, levelObject);   
            }
        }


        public static LevelObject GetLevelObjectOfType(ChapterType chapterType)
        {
            Init();
            
            if (!levelObjects.ContainsKey(chapterType)) return null;

            return levelObjects[chapterType];
        }


        public static bool IsComingSoon(ChapterType chapterType)
        {
            Init();
            
            if (!levelObjects.ContainsKey(chapterType)) return true;

            return levelObjects[chapterType].comingSoon;
        }
    }
}