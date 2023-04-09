using System;
using System.Collections.Generic;
using _project.Scripts.Enums;
using _project.Scripts.Skills;
using UnityEngine;

namespace _project.Scripts.Car
{
    public class CarProfile : MonoBehaviour
    {
        [SerializeField] private CarModels carModel;
        [SerializeField] private Sprite shopSprite;
        [SerializeField] private Sprite garageSprite;
        [SerializeField] private int unlockPrice;
        [SerializeField] private Parts parts;
        [SerializeField] private List<SkillPlacementInfo> skillPlacementInfo;
        [SerializeField] private DrivingProfileType drivingProfileType;
        [SerializeField] private SpawnType exhaustParticle;
        [SerializeField] private SpriteRenderer fakeShadow;

        private Vector3 leftFrontTire;
        private Vector3 rightFrontTire;
        private Vector3 rightRearTire;
        private Vector3 leftRearTire;
        private Vector3 mainBody;


        private void Start()
        {
            SavePositions();
        }

        public CarModels GetModel() => carModel;
        
        public Parts GetParts()
        {
            
            return parts;
        }
        public List<SkillPlacementInfo> GetSkillPlacementInfo() => skillPlacementInfo;
        public DrivingProfileType GetDrivingProfileType() => drivingProfileType;
        public SpawnType GetExhaustParticle() => exhaustParticle;

        public Sprite GetShopSprite() => shopSprite;
        public Sprite GetGarageSprite() => garageSprite;

        public int GetUnlockPrice() => unlockPrice;

        public void ControlFakeShadow(bool set)
        {
            if (fakeShadow == null) return;
            
            fakeShadow.gameObject.SetActive(set);
        }

        public void SavePositions()
        {
            leftRearTire = parts.leftRearTire.localPosition;
            rightRearTire = parts.rightRearTire.localPosition;
            leftFrontTire = parts.leftFrontTire.localPosition;
            rightFrontTire = parts.rightFrontTire.localPosition;
        }

        public void PutAllPartsBack()
        {
            Quaternion targetRot = new Quaternion(0, 0, 0, 0);
            
            parts.leftFrontTire.SetParent(parts.mainBody);
            parts.leftFrontTire.localRotation = targetRot;
            parts.leftFrontTire.localPosition = leftFrontTire;
            
            parts.rightFrontTire.SetParent(parts.mainBody);
            parts.rightFrontTire.localRotation = targetRot;
            parts.rightFrontTire.localPosition = rightFrontTire;
            
            parts.leftRearTire.SetParent(parts.mainBody);
            parts.leftRearTire.localRotation = targetRot;
            parts.leftRearTire.localPosition = leftRearTire;
            
            parts.rightRearTire.SetParent(parts.mainBody);
            parts.rightRearTire.localRotation = targetRot;
            parts.rightRearTire.localPosition = rightRearTire;
        }
    }
}