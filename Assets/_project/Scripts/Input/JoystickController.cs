using _project.Scripts.Managers;
using UnityEngine;

namespace _project.Scripts.Input
{
    public class JoystickController : MonoBehaviour
    {
        public static JoystickController Instance;
    
        [SerializeField] private Joystick joystick;

        private FuelManager fuelManager;

        private Vector3 lastDirection;

        private bool inMotion;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            gameObject.SetActive(false);
        }


        public void Init()
        {
            fuelManager = FuelManager.Instance;
        }

        public Vector3 GetDirection()
        {
            if (joystick.Direction.magnitude != 0)
            {
                lastDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

                if (!inMotion)
                {
                    inMotion = true;
                    fuelManager.SetConsumption(true);
                }
            }
            else
            {
                if (inMotion)
                {
                    inMotion = false;
                    fuelManager.SetConsumption(false);
                }
            }
            
            return lastDirection;
        }

        public float GetHorizontalDirection() => joystick.Horizontal;

        public float GetVerticalDirection()
        {
            // return joystick.Vertical != 0f ? 1f : 0f;
            return joystick.Direction.magnitude != 0 ? 1f : 0f;
        }


        public void ResetJoystick()
        {
            joystick.OnPointerUp(null);
            lastDirection = Vector3.zero;
            inMotion = false;
            gameObject.SetActive(false);
        }
    }
}
