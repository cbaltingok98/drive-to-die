using _project.Scripts.Berk;
using _project.Scripts.Car.DrivingMechanic;
using UnityEngine;

namespace _project.Scripts.Car
{
    public class CollisionHandler : MonoBehaviour
    {
        private CarController carController;

        private void Awake()
        {
            carController = GetComponent<CarController>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // if (collision.transform.CompareTag("Cone"))
            // {
            //     CheckCollisionSection(collision);   
            // }
            // else 
            if (collision.transform.CompareTag("Block"))
            {
                carController.ReflectHitForce(collision.GetContact(0).normal, collision.transform.position);
            }
        }

        private void CheckCollisionSection(Collision collision)
        {
            var myTransform = transform;
            var collidedObjPos = collision.transform.position;
            var directionToCollidedObj = (collidedObjPos - myTransform.position).normalized;
                
            var dotProduct = Vector3.Dot(myTransform.forward, directionToCollidedObj);

            if (dotProduct > 0.6f)
            {
                Printer.Print("Front Collision", DesiredColor.Red);
                //VibrationManager.HeavyVibrate();
            }
            else
            {
                Printer.Print("Rear Collision", DesiredColor.Green);
                //VibrationManager.LightVibrate();
            }
        }
    }
}