using System.Collections.Generic;
using System.Linq;
using _project.Scripts.Enums;
using _project.Scripts.Managers;
using _project.Scripts.UI;
using UnityEngine;

namespace _project.Scripts.Themes
{
    public static class ThemeImageManager
    {
        private static List<ThemeImage> themeImages;

        private static bool isInitialized;

        private static void InitializeList()
        {
            if (isInitialized) return;
            
            themeImages = new List<ThemeImage>();
            themeImages = DataManager.GetThemeImages().ToList();
            isInitialized = true;
        }

        public static Sprite GetThemeImage(LevelThemes levelTheme)
        {
            InitializeList();
            return themeImages.First(image => image.imageTheme == levelTheme).image;
        }
    }
}