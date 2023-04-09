using System.Collections.Generic;
using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Enemy.Enemies
{
    public class Minion : EnemyBase
    {
        [SerializeField] private List<Transform> skins;
        protected override void Awake()
        {
            Rb = GetComponent<Rigidbody>();

            var pick = Random.Range(0, skins.Count);
            var pickedSkin = skins[pick];
            pickedSkin.gameObject.SetActive(true);
            Animator = pickedSkin.GetComponent<Animator>();
        }
    }
}