using _project.Scripts.Enums;
using _project.Scripts.Models;
using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.Skills.Skill_Logic.ROCKETLAUNCHER
{
    public struct TargetInfo
    {
        public Vector3 Vector;
        public Transform Origin;
        public Transform Parent;

        public TargetInfo(Vector3 v, Transform o, Transform p)
        {
            Vector = v;
            Origin = o;
            Parent = p;
        }
    }
    
    public class RocketLauncherTarget : MonoBehaviour
    {
        private TargetInfo myInfo;

        private bool isActive;

        private Tween myTween;


        public void SetTargetInfo(TargetInfo newInfo, float delay)
        {
            myInfo = newInfo;
            transform.position = myInfo.Origin.position + myInfo.Vector;
            transform.SetParent(null);
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            isActive = true;
            myTween = transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack).SetDelay(delay);
        }


        public void DeActivate()
        {
            isActive = false;
            gameObject.SetActive(false);
            transform.SetParent(myInfo.Origin);
            ParticlePooler.Instance.SpawnFromPool(SpawnType.RocketLauncherBlast, transform.position, Quaternion.identity, false).Play();
            
        }


        public void DeAttach()
        {
            isActive = false;
            transform.SetParent(null);
        }


        private void Update()
        {
            if (!isActive) return;
            
            transform.position = myInfo.Origin.position + myInfo.Vector;
        }


        public void ResetItem()
        {
            isActive = false;

            myTween?.Kill();
            
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
        }
    }
}