using _project.Scripts.Berk;
using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.Enums;
using _project.Scripts.Skills;
using UnityEngine;

namespace _project.Scripts.Car
{
    public class CarMarriage : MonoBehaviour
    {
        [Header("PARTS")] 
        [Space(10)] 
        [SerializeField] private Transform bodyParent;
        [SerializeField] private Transform leftFrontTireParent;
        [SerializeField] private Transform rightFrontTireParent;
        [SerializeField] private Transform leftRearTireParent;
        [SerializeField] private Transform rightRearTireParent;

        [Header("REF POINTS")] 
        [Space(10)] 
        [SerializeField] private Transform frontRefPoint;
        [SerializeField] private Transform rearRefPoint;
        [SerializeField] private Transform leftRefPoint;
        [SerializeField] private Transform rightRefPoint;

        private bool isModelActive;

        private CarGarage carGarage;
        private CarController carController;
        private CarProfile activeCarProfile;

        // private SkillSystemHelper _skillSystemHelper;

        private void Awake()
        {
            carController = GetComponent<CarController>();
        }

        // public void MarryMe(CarModels carModelToMarry, SkillSystemHelper skillSystemHelper)
        // {
        //     _carGarage = CarGarage.Instance;
        //     ChangeModel(carModelToMarry, skillSystemHelper);
        // }
        
        public void MarryMe(CarModels carModelToMarry)
        {
            carGarage = CarGarage.Instance;
            ChangeModel(carModelToMarry);
        }

        public void MarryMe(CarProfile carProfile)
        {
            carGarage = CarGarage.Instance;
            ChangeModel(carProfile);
        }

        public void Divorce()
        {
            // take car out of the car system, put it back to garage then destroy the car system
            CloseActiveModel();
        }
        
        private void ChangeModel(CarModels carModelToMarry)
        {
            if (isModelActive)
            {
                carController.DeActivate();
                Divorce();
            }

            // _skillSystemHelper = skillSystemHelper; 
            SetNewModel(carModelToMarry);
            isModelActive = true;
        }
        
        private void ChangeModel(CarProfile carProfile)
        {
            if (isModelActive)
            {
                carController.DeActivate();
                Divorce();
            }
            SetNewModel(carProfile);
            isModelActive = true;
        }

        private void SetNewModel(CarModels carModelToMarry)
        {
            activeCarProfile = carGarage.GetModel(carModelToMarry);
            Parts carParts = activeCarProfile.GetParts();
            // main body
            carParts.mainBody.rotation = bodyParent.parent.rotation;
            
            carParts.mainBody.SetParent(bodyParent);
            carParts.mainBody.localPosition = Vector3.zero;
            // tires
            leftFrontTireParent.localPosition = carParts.leftFrontTire.localPosition;
            carParts.leftFrontTire.parent = leftFrontTireParent;
            
            rightFrontTireParent.localPosition = carParts.rightFrontTire.localPosition;
            carParts.rightFrontTire.parent = rightFrontTireParent;
            
            leftRearTireParent.localPosition = carParts.leftRearTire.localPosition;
            carParts.leftRearTire.parent = leftRearTireParent;
            
            rightRearTireParent.localPosition = carParts.rightRearTire.localPosition;
            carParts.rightRearTire.parent = rightRearTireParent;
            // ref angle points
            var frontLeftLocalPos = leftFrontTireParent.localPosition;
            var frontRightLocalPos = rightFrontTireParent.localPosition;
            var rearLeftLocalPos = leftRearTireParent.localPosition;
            var rearRightLocalPos = rightRearTireParent.localPosition;


            var frontRefAvgPos = new Vector3((frontLeftLocalPos.x + frontRightLocalPos.x) / 2f,
                (frontLeftLocalPos.y + frontRightLocalPos.y) / 2f, (frontLeftLocalPos.z + frontRightLocalPos.z) / 2f);
            
            var rearRefAvgPos = new Vector3((rearLeftLocalPos.x + rearRightLocalPos.x) / 2f,
                (rearLeftLocalPos.y + rearRightLocalPos.y) / 2f, (rearLeftLocalPos.z + rearRightLocalPos.z) / 2f);
            
            var leftRefAvgPos = new Vector3((frontLeftLocalPos.x + rearLeftLocalPos.x) / 2f,
                (frontLeftLocalPos.y + rearLeftLocalPos.y) / 2f, (frontLeftLocalPos.z + rearLeftLocalPos.z) / 2f);
            
            var rightRefAvgPos = new Vector3((frontRightLocalPos.x + rearRightLocalPos.x) / 2f,
                (frontRightLocalPos.y + rearRightLocalPos.y) / 2f, (frontRightLocalPos.z + rearRightLocalPos.z) / 2f);

            frontRefPoint.localPosition = frontRefAvgPos;
            rearRefPoint.localPosition = rearRefAvgPos;
            leftRefPoint.localPosition = leftRefAvgPos;
            rightRefPoint.localPosition = rightRefAvgPos;
            // assign exhaust and tire
            carController.SetExhaustPositions(carParts.leftExhaust, carParts.rightExhaust, carParts.mainBody, activeCarProfile.GetExhaustParticle());
            carController.SetFrontTires(carParts.leftFrontTire, carParts.rightFrontTire);
            
            activeCarProfile.gameObject.SetActive(true);
            
            // skill system placement
            var skillPlacementInfo = activeCarProfile.GetSkillPlacementInfo();
            
            foreach (var placementInfo in skillPlacementInfo)
            {
                SkillSystemHelper.Instance.SetSkillTransform(placementInfo);
            }
        }
        
        private void SetNewModel(CarProfile carProfile)
        {
            activeCarProfile = carProfile;
            Parts carParts = activeCarProfile.GetParts();
            // main body
            carParts.mainBody.rotation = bodyParent.parent.rotation;
            
            carParts.mainBody.SetParent(bodyParent);
            carParts.mainBody.localPosition = Vector3.zero;
            // tires
            leftFrontTireParent.localPosition = carParts.leftFrontTire.localPosition;
            carParts.leftFrontTire.parent = leftFrontTireParent;
            
            rightFrontTireParent.localPosition = carParts.rightFrontTire.localPosition;
            carParts.rightFrontTire.parent = rightFrontTireParent;
            
            leftRearTireParent.localPosition = carParts.leftRearTire.localPosition;
            carParts.leftRearTire.parent = leftRearTireParent;
            
            rightRearTireParent.localPosition = carParts.rightRearTire.localPosition;
            carParts.rightRearTire.parent = rightRearTireParent;
            // ref angle points
            var frontLeftLocalPos = leftFrontTireParent.localPosition;
            var frontRightLocalPos = rightFrontTireParent.localPosition;
            var rearLeftLocalPos = leftRearTireParent.localPosition;
            var rearRightLocalPos = rightRearTireParent.localPosition;


            var frontRefAvgPos = new Vector3((frontLeftLocalPos.x + frontRightLocalPos.x) / 2f,
                (frontLeftLocalPos.y + frontRightLocalPos.y) / 2f, (frontLeftLocalPos.z + frontRightLocalPos.z) / 2f);
            
            var rearRefAvgPos = new Vector3((rearLeftLocalPos.x + rearRightLocalPos.x) / 2f,
                (rearLeftLocalPos.y + rearRightLocalPos.y) / 2f, (rearLeftLocalPos.z + rearRightLocalPos.z) / 2f);
            
            var leftRefAvgPos = new Vector3((frontLeftLocalPos.x + rearLeftLocalPos.x) / 2f,
                (frontLeftLocalPos.y + rearLeftLocalPos.y) / 2f, (frontLeftLocalPos.z + rearLeftLocalPos.z) / 2f);
            
            var rightRefAvgPos = new Vector3((frontRightLocalPos.x + rearRightLocalPos.x) / 2f,
                (frontRightLocalPos.y + rearRightLocalPos.y) / 2f, (frontRightLocalPos.z + rearRightLocalPos.z) / 2f);

            frontRefPoint.localPosition = frontRefAvgPos;
            rearRefPoint.localPosition = rearRefAvgPos;
            leftRefPoint.localPosition = leftRefAvgPos;
            rightRefPoint.localPosition = rightRefAvgPos;
            // assign exhaust and tire
            carController.SetExhaustPositions(carParts.leftExhaust, carParts.rightExhaust, carParts.mainBody, activeCarProfile.GetExhaustParticle());
            carController.SetFrontTires(carParts.leftFrontTire, carParts.rightFrontTire);
            
            activeCarProfile.gameObject.SetActive(true);
        }

        private void DivorceAi()
        {
            
        }

        private void CloseActiveModel()
        {
            if (activeCarProfile == null)
            {
                Printer.Print("Active Car Profile is Null", DesiredColor.Red);
                return;
            }

            activeCarProfile.PutAllPartsBack();
            activeCarProfile.GetParts().mainBody.SetParent(carGarage.transform);
            activeCarProfile.gameObject.SetActive(false);

            activeCarProfile = null;
            isModelActive = false;
            
             SkillSystemHelper.Instance.Reset();
        }
    }
}