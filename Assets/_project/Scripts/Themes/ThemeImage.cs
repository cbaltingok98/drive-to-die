using _project.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.Themes
{
    [CreateAssetMenu(fileName = "New Theme Image", menuName = "ScriptableObjects/Theme Image", order = 3)]
    public class ThemeImage : ScriptableObject
    {
        public Sprite image;
        public LevelThemes imageTheme;
    }
}