using System.Collections.Generic;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.Managers
{
    public static class SkillInfoProfileManager
    { 
        public static IEnumerable<T> GetSkillProfile<T>() where T : Object
        {
            return DataManager.GetSkillInfo<T>();
        }
    }
}