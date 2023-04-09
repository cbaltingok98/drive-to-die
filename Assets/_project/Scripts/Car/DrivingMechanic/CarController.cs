using System.Collections;
using _project.Scripts.Berk;
using _project.Scripts.Enums;
using _project.Scripts.Input;
using _project.Scripts.Managers;
using _project.Scripts.Mimic;
using _project.Scripts.Models;
using _project.Scripts.Score;
using _project.Scripts.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _project.Scripts.Car.DrivingMechanic
{
    public abstract class CarController : MonoBehaviour
    {
        [Header("CAR INFO")]
        [Space(10)]
        [SerializeField] protected Car car;
        [Header("TRANSFORMS")]
        [Space(10)]
        [SerializeField] protected Transform directionIndicator;
        [SerializeField] protected Transform carModelTransform;
        [Header("PARTICLES")]
        [Space(10)]
        [SerializeField] protected ParticleSystem leftSmokeParticle;
        [SerializeField] protected ParticleSystem rightSmokeParticle;
        [SerializeField] protected TrailRenderer rearLeftTrailRenderer;
        [SerializeField] protected TrailRenderer rearRightTrailRenderer;

        protected JoystickController JoystickController;
        private ScoreManager scoreManager;

        private Vector3 directionIndicatorStartPos;
        private Quaternion directionIndicatorStartRot;

        protected Vector3 MoveForce;
        private Vector3 manipulatedDirection;
        protected Vector3 SmokeParticleDefaultScale;

        private Transform leftFrontTire;
        private Transform rightFrontTire;

        private ParticleSystem leftExhaust;
        private ParticleSystem rightExhaust;

        protected WaitForSeconds WaitFor;
        private Coroutine myCoroutine;
        
        protected float TurnSmoothVelocity;
        protected float TurnSmoothVelocity2;
        protected float TurnSmoothVelocity3;
        private float turnSmoothVelocity4;
        protected float ZRotation;
        protected float XRotation;
        private float rideHeight;
        private float driftCounterForSmokeParticle;

        protected const float DriftMax = 0.92f;
        protected const float DriftMin = 0.2f;
        protected const float ForceMagnitudeDisplayMultiplier = 6f;

        protected bool IsActive;
        protected bool TireMarkAndSmoke;
        protected bool BlockInput;
        private bool smokeParticleScale;
        private bool ghostRider;

        protected virtual void Start()
        {
            directionIndicatorStartPos = directionIndicator.localPosition;
            directionIndicatorStartRot = directionIndicator.localRotation;
        }


        public virtual void Init()
        {
            scoreManager = ScoreManager.Instance;
            JoystickController = JoystickController.Instance;

            WaitFor = new WaitForSeconds(0.2f);
            SmokeParticleDefaultScale = rightSmokeParticle.transform.localScale;
            
            scoreManager.SetActive(true);
            
            FuelManager.Instance.Init(car);
            JoystickController.Init();
        }

        public virtual void Activate()
        {
#if UNITY_EDITOR
            MimicDriving.Init();
#endif
            
            IsActive = true;
        }

        public void DeActivate()
        {
            IsActive = false;
            TireMarkAndSmoke = false;
            FuelManager.Instance.SetConsumption(false);
        }


        public void ResetPlayer()
        {
            DeActivate();
            
            MoveForce = Vector3.zero;
            manipulatedDirection = Vector3.zero;

            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
            }

            ZRotation = 0f;
            XRotation = 0f;
            
            driftCounterForSmokeParticle = 0f;
            
            leftSmokeParticle.Stop();
            rightSmokeParticle.Stop();

            leftSmokeParticle.transform.localScale = SmokeParticleDefaultScale;
            rightSmokeParticle.transform.localScale = SmokeParticleDefaultScale;
            
            rearLeftTrailRenderer.emitting = false;
            rearRightTrailRenderer.emitting = false;

            TireMarkAndSmoke = false;
            smokeParticleScale = false;

            leftExhaust.Stop();
            rightExhaust.Stop();

            leftFrontTire.localEulerAngles = Vector3.zero;
            rightFrontTire.localEulerAngles = leftFrontTire.localEulerAngles;
            BlockInput = false;

            directionIndicator.localPosition = directionIndicatorStartPos;
            directionIndicator.localRotation = directionIndicatorStartRot;

            carModelTransform.localPosition = Vector3.zero;
            carModelTransform.localEulerAngles = Vector3.zero;
            
            scoreManager.SetDrifting(false);
        }

        public void SetMimicSystem(bool set)
        {
            ghostRider = set;
        }

        public void SetDrivingProfile(DrivingProfile drivingProfile)
        {
            Car newCar = new Car(drivingProfile.drivingStatsList[drivingProfile.level-1]);
            
            car = newCar;

            GetComponent<SuspensionManager>().SetCar(car); // TODO remove (maybe)

            Printer.Print("New Driving Profile Set [" + drivingProfile.profileType + "]", DesiredColor.Orange);
        }

        public void SetExhaustPositions(Transform left, Transform right, Transform parent, SpawnType exhaustParticle)
        {
            if (leftExhaust != null && rightExhaust != null)
            {
                leftExhaust.transform.SetParent(ParticlePooler.Instance.transform);
                rightExhaust.transform.SetParent(ParticlePooler.Instance.transform);
                
                leftExhaust.gameObject.SetActive(false);
                rightExhaust.gameObject.SetActive(false);
            }
            
            leftExhaust = ParticlePooler.Instance.SpawnFromPool(exhaustParticle, left.localPosition, left.localRotation, true, parent);
            leftExhaust.transform.SetLocalPositionAndRotation(left.localPosition, left.localRotation);
            rightExhaust = ParticlePooler.Instance.SpawnFromPool(exhaustParticle, right.localPosition, right.localRotation, true, parent);
            rightExhaust.transform.SetLocalPositionAndRotation(right.localPosition, right.localRotation);
        }

        public void SetFrontTires(Transform leftTire, Transform rightTire)
        {
            leftFrontTire = leftTire;
            rightFrontTire = rightTire;
        }
    
        private void Update()
        {
            if (!IsActive) return;
            
            ApplyInput();
            ApplyRotation();
            ApplyDrag();
            ApplySpeedLimit();
            UpdatePosition();
            
            SetRideHeight();
            SetTireRotation();
            SetDirectionIndicator();
            ControlTireMarksAndSmoke();
            ControlExhaustBlow();
        }

        protected virtual void ApplyInput()
        {
            if (BlockInput) return;

            var calculatedForce = transform.forward * (car.acceleration * JoystickController.GetVerticalDirection() * Time.deltaTime);
            calculatedForce.y = 0f;
            
            MoveForce += calculatedForce;

#if UNITY_EDITOR
            if (ghostRider)
            {
                MimicDriving.SaveMoveForce(calculatedForce); // save driving TODO remove
            }
#endif
        }

        private void ApplyDrag()
        {
            MoveForce *= car.drag;
        }

        private void ApplySpeedLimit()
        {
            MoveForce = Vector3.ClampMagnitude(MoveForce, car.maxSpeed);
        }

        private void UpdatePosition()
        {
            MoveForce = Vector3.Lerp(MoveForce.normalized, transform.forward, car.traction * Time.deltaTime) * MoveForce.magnitude;
            transform.position += MoveForce * Time.deltaTime;
        }

        protected virtual void ApplyRotation()
        {
            if (BlockInput) return;
            
            Vector3 direction = JoystickController.GetDirection();
            Vector3 eulerAngle = transform.eulerAngles;
            Vector3 eulerAnglePrefab = carModelTransform.eulerAngles;
            
            float targetAngle = (Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
#if UNITY_EDITOR
            if (ghostRider)
            {
                MimicDriving.SaveJoystickDirection(targetAngle);   
            }
#endif
            var angleY = Mathf.SmoothDampAngle(eulerAngle.y, targetAngle, ref TurnSmoothVelocity, car.correctionTime);
            var angleX = Mathf.SmoothDampAngle(eulerAnglePrefab.x, XRotation, ref TurnSmoothVelocity2, car.suspensionSmoothTime);
            var angleZ = Mathf.SmoothDampAngle(eulerAnglePrefab.z, ZRotation, ref TurnSmoothVelocity3, car.suspensionSmoothTime);
            
            transform.rotation = Quaternion.Euler(0f, angleY, 0f);
            carModelTransform.rotation = Quaternion.Euler(angleX, angleY, angleZ);
        }

        private void SetRideHeight()
        {
            var prefabTransform = carModelTransform.position;
            var target = Mathf.SmoothDamp(prefabTransform.y, rideHeight, ref turnSmoothVelocity4,
                car.suspensionSmoothTime);
            carModelTransform.position = new Vector3(prefabTransform.x, target, prefabTransform.z);
        }

        protected virtual void ControlTireMarksAndSmoke()
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
                
                scoreManager.SetDrifting(true);
            }
            else
            {
                if (!TireMarkAndSmoke) return;
                TireMarkAndSmoke = false;
                
                rearLeftTrailRenderer.emitting = false;
                rearRightTrailRenderer.emitting = false;
                
                leftSmokeParticle.Stop();
                rightSmokeParticle.Stop();
                
                scoreManager.SetDrifting(false);
            }
        }

        protected void ScaleSmokeParticle()
        {
            if (TireMarkAndSmoke)
            {
                if (ReachedMaxScale()) return;
                
                driftCounterForSmokeParticle += Time.deltaTime;
                if (driftCounterForSmokeParticle < 0.5f) return;
                driftCounterForSmokeParticle = 0f;

                leftSmokeParticle.transform.localScale += Vector3.one * 0.1f;
                rightSmokeParticle.transform.localScale += Vector3.one * 0.1f;

                if (smokeParticleScale) return;
                smokeParticleScale = true;
            }
            else
            {
                if (!smokeParticleScale) return;
                smokeParticleScale = false;

                driftCounterForSmokeParticle = 0f;
            }
        }

        private bool ReachedMaxScale()
        {
            return rightSmokeParticle.transform.localScale.x >= 2.5f;
        }

        protected virtual void SetDirectionIndicator()
        {
            var moveDirection = (Vector3.Reflect(MoveForce.normalized, transform.forward) * -1).normalized * 6;
            directionIndicator.LookAt(transform.position + moveDirection);
            // directionIndicator.LookAt(transform.position + (_moveForce.normalized * 6));
        }

        private void SetTireRotation()
        {
            var eulerAngleY = directionIndicator.localRotation.eulerAngles.y;
            var direction = (TireMarkAndSmoke ? -1 : 1) * eulerAngleY;
            
            leftFrontTire.localRotation = Quaternion.Lerp(Quaternion.Euler(0f,  leftFrontTire.localRotation.eulerAngles.y, 0f), Quaternion.Euler(0f,  direction, 0f), 0.1f);
            rightFrontTire.localRotation = leftFrontTire.localRotation;
            // _rightFrontTire.localRotation = Quaternion.Lerp(Quaternion.Euler(0f,  _rightFrontTire.localRotation.eulerAngles.y, 0f), Quaternion.Euler(0f,  direction, 0f), 0.1f);
        }

        private void ControlExhaustBlow()
        {
            if (!TireMarkAndSmoke) return;

            var rand = Random.Range(0f, 100f);
            if (rand > 2f) return;

            leftExhaust.Play();
            rightExhaust.Play();
        }

        private IEnumerator EnableInput()
        {
            yield return WaitFor;
            BlockInput = false;
        }

        public void ReflectHitForce(Vector3 hitPointNormal, Vector3 hitPos)
        {
            if (ghostRider) return; // TODO remove
            
            if (myCoroutine != null)
            {
                StopCoroutine(EnableInput());
            }

            var directionToCollision = (hitPos - transform.position).normalized;
            var dotProduct = Vector3.Dot(hitPointNormal, MoveForce.normalized);

            if (dotProduct > 0)
            {
                manipulatedDirection = Vector3.Reflect(directionToCollision, hitPointNormal).normalized + transform.forward * 2f;
            }
            else
            {
                manipulatedDirection = Vector3.Reflect(MoveForce.normalized, hitPointNormal) * 2f + transform.forward;
            }

            hitPointNormal.y = 0f;
            BlockInput = true;
            
            manipulatedDirection.y = 0f;
            MoveForce = manipulatedDirection * MoveForce.magnitude * 0.3f;
            myCoroutine = StartCoroutine(EnableInput());
        }


        public void BlockPlayerInput()
        {
            BlockInput = true;
        }

        public Car GetCar() => car;
        public void SetZRotation(float val) => ZRotation = val;
        public void SetXRotation(float val) => XRotation = val;
        public void SetRideHeight(float val) => rideHeight = val;
        public float GetDisplaySpeed() => MoveForce.magnitude * ForceMagnitudeDisplayMultiplier;
        
        public float GetRealSpeed() => MoveForce.magnitude;
        
        public void ControlDirectionIndicator() //FIX ME remove
        {
            directionIndicator.gameObject.SetActive(!directionIndicator.gameObject.activeSelf);
        }


        public Vector3 GetMoveForce() => MoveForce.normalized;

        public Vector3 GetMoveDirection() => (transform.forward).normalized;
        
        public bool IsDrifting() => TireMarkAndSmoke;
    }
}
