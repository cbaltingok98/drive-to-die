using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace _project.Scripts.Car.DrivingMechanic
{
    public class SuspensionManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> suspensions;
        [SerializeField] private List<Transform> zAngleRef;
        [SerializeField] private List<Transform> xAngleRef;

        private CarController carController;
        private Car car;
        
        private void Awake()
        {
            carController = GetComponent<CarController>();
        }

        private void Start()
        {
            car = carController.GetCar();
            for (var i = 0; i < suspensions.Count; i++)
            {
                suspensions[i].GetComponent<Suspension>().SetSuspension(this);
            }
        }

        private void LateUpdate()
        {
            CalculateZRotation();
            CalculateXRotation();
            CalculateCarHeight();
            // AlignReferencePoints();
        }

        private void CalculateZRotation()
        {
            var sum = 0f;
            var counter = 0;
            
            for (var i = 0; i <= 2; i+=2)
            {
                var zAnglePos = zAngleRef[counter].position;
                var zAngleRight = zAngleRef[counter].right;
                
                var direction = suspensions[i].position - zAnglePos;
                var angle = Vector3.Angle(direction, -zAngleRight);

                var direction2 = suspensions[i + 1].position - zAnglePos;
                var angle2 = Vector3.Angle(direction2, zAngleRight);

                sum += (angle - angle2);
                counter += 1;
            }
            carController.SetZRotation(-sum/2f);
        }

        private void CalculateXRotation()
        {
            var sum = 0f;
            var counter = 0;
            
            for (var i = 0; i <= 1; i++)
            {
                var xAnglePos = xAngleRef[counter].position;
                var xAngleForward = xAngleRef[counter].forward;
                
                var direction = suspensions[i].position - xAnglePos;
                var angle = Vector3.Angle(direction, xAngleForward);

                var direction2 = suspensions[i + 2].position - xAnglePos;
                var angle2 = Vector3.Angle(direction2, -xAngleForward);

                sum += (angle - angle2);
                counter += 1;
            }
            carController.SetXRotation(-sum/2f);
        }

        private void CalculateCarHeight()
        {
            var sumY = 0f;

            for (var i = 0; i < suspensions.Count; i++)
            {
                sumY += suspensions[i].position.y;
            }

            var average = sumY / suspensions.Count;
            var height = average + car.bodyHeight;
            
            carController.SetRideHeight(height);
        }
        
        private void AlignReferencePoints()
        {
            var frontLeft = suspensions[0];
            var frontRight = suspensions[1];
            var rearLeft = suspensions[2];
            var rearRight = suspensions[3];

            var angle1 = Mathf.Lerp(zAngleRef[0].localPosition.y ,(frontLeft.localPosition.y + frontRight.localPosition.y) / 2f, 1f);
            zAngleRef[0].localPosition = new Vector3(zAngleRef[0].localPosition.x, angle1, zAngleRef[0].localPosition.z);
    
            var angle2 = Mathf.Lerp(xAngleRef[1].localPosition.y ,(frontRight.localPosition.y + rearRight.localPosition.y) / 2f, 1f);
            xAngleRef[1].localPosition = new Vector3(xAngleRef[1].localPosition.x, angle2, xAngleRef[1].localPosition.z);
    
            var angle3 = Mathf.Lerp(zAngleRef[1].localPosition.y ,(rearLeft.localPosition.y + rearRight.localPosition.y) / 2f, 1f);
            zAngleRef[1].localPosition = new Vector3(zAngleRef[1].localPosition.x, angle3, zAngleRef[1].localPosition.z);
    
            var angle4 = Mathf.Lerp(xAngleRef[0].localPosition.y ,(frontLeft.localPosition.y + rearLeft.localPosition.y) / 2f, 1f);
            xAngleRef[0].localPosition = new Vector3(xAngleRef[0].localPosition.x, angle4, xAngleRef[0].localPosition.z);
        }

        public float GetHeight() => car.carHeight;
        public float GetStiffness() => car.suspensionStiffness;
        public float GetRideHeight() => car.bodyHeight;

        public void ControlDebugUI(bool set)
        {
            foreach (var suspension in suspensions)
            {
                suspension.GetComponent<Suspension>().ControlDebugText(set);
            }
        }

        public void SetCar(Car newCar)
        {
            car = newCar;
        }
    }
}