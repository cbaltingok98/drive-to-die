using System;
using _project.Scripts.Enums;
using UnityEngine;

namespace _project.Scripts.UI.Interface
{
    [Serializable]
    public abstract class MenuUiElement : MonoBehaviour
    {
        [SerializeField] protected BottomMenuType menuType;
        
        public abstract BottomMenuType GetMenuType();
        public abstract void Enable();
        public abstract void Disable();
        public abstract void UpdateInfo();
    }
}