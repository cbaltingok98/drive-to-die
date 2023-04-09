using _project.Scripts.Car.DrivingMechanic;
using _project.Scripts.Input;
using _project.Scripts.Mimic;
using UnityEngine;

namespace _project.Scripts.AI
{
    public class AiController : CarController
    {
        private MimicData myMimicData;
        private int maxIndex;

        private Vector3 startPos;
        private Quaternion starRot;
        
        protected override void Start()
        {
            JoystickController = JoystickController.Instance;

            WaitFor = new WaitForSeconds(0.2f);
            SmokeParticleDefaultScale = rightSmokeParticle.transform.localScale;
            
            directionIndicator.gameObject.SetActive(false);

            startPos = transform.position;
            starRot = transform.rotation;
        }

        public void SetMimicData(int gridPos)
        {
            myMimicData = new MimicData(MimicDriving.Load(gridPos));
            maxIndex = myMimicData.joystickDirection.Count;
        }

        public override void Activate()
        {
            IsActive = true;
        }

        protected override void ApplyInput()
        {
            if (BlockInput) return;

            MoveForce += myMimicData.moveForce[myMimicData.moveForceIndex++];// * Time.deltaTime;
            if (myMimicData.moveForceIndex < maxIndex) return;
            
            DeActivate();
            MoveForce = Vector3.zero;
            transform.position = startPos;
            transform.rotation = starRot;

            myMimicData.moveForceIndex = 0;
            myMimicData.joystickIndex = 0;
            Activate();
        }

        protected override void ApplyRotation()
        {
            if (BlockInput) return;
            
            Vector3 eulerAngle = transform.eulerAngles;
            Vector3 eulerAnglePrefab = carModelTransform.eulerAngles;
            
            float targetAngle = myMimicData.joystickDirection[myMimicData.joystickIndex++];

            var angleY = Mathf.SmoothDampAngle(eulerAngle.y, targetAngle, ref TurnSmoothVelocity, car.correctionTime);
            var angleX = Mathf.SmoothDampAngle(eulerAnglePrefab.x, XRotation, ref TurnSmoothVelocity2, car.suspensionSmoothTime);
            var angleZ = Mathf.SmoothDampAngle(eulerAnglePrefab.z, ZRotation, ref TurnSmoothVelocity3, car.suspensionSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angleY, 0f);
            carModelTransform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
        }

        protected override void ControlTireMarksAndSmoke()
        {
            ScaleSmokeParticle();
            
            var dotPro = Vector3.Dot(MoveForce.normalized, transform.forward);
            var speed = MoveForce.magnitude * ForceMagnitudeDisplayMultiplier;

            if (speed > 10f && dotPro <= DriftMax && dotPro >= DriftMin)
            {
                if (TireMarkAndSmoke) return;
                TireMarkAndSmoke = true;
                
                rearLeftTrailRenderer.emitting = true;
                rearRightTrailRenderer.emitting = true;
                
                leftSmokeParticle.transform.localScale = SmokeParticleDefaultScale;
                rightSmokeParticle.transform.localScale = SmokeParticleDefaultScale;

                leftSmokeParticle.Play();
                rightSmokeParticle.Play();
            }
            else
            {
                if (!TireMarkAndSmoke) return;
                TireMarkAndSmoke = false;
                
                rearLeftTrailRenderer.emitting = false;
                rearRightTrailRenderer.emitting = false;
                
                leftSmokeParticle.Stop();
                rightSmokeParticle.Stop();
            }
        }

        protected override void SetDirectionIndicator()
        {
            
        }
    }
}