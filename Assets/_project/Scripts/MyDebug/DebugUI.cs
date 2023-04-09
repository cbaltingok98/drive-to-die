using _project.Scripts.Car.DrivingMechanic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _project.Scripts.MyDebug
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moveSpeed;
        [SerializeField] private TextMeshProUGUI maxSpeed;
        [SerializeField] private TextMeshProUGUI rideHeight;
        [SerializeField] private TextMeshProUGUI traction;
        [SerializeField] private TextMeshProUGUI correction;
        [SerializeField] private TextMeshProUGUI drag;
        [SerializeField] private TextMeshProUGUI suspension;
        [SerializeField] private TextMeshProUGUI speedText;

        private Car.DrivingMechanic.Car car;
        private CarController carController;
        private SuspensionManager suspensionManager;

        private bool active;

        public void Init(GameObject newCar)
        {
            carController = newCar.GetComponent<CarController>();
            suspensionManager = newCar.GetComponent<SuspensionManager>();

            UpdateCar();
            active = true;
        }

        public void UpdateCar()
        {
            car = carController.GetCar();
            UpdateUI();
        }

        private void UpdateUI()
        {
            moveSpeed.text = car.acceleration.ToString("0.00");
            maxSpeed.text = car.maxSpeed.ToString("0.00");
            rideHeight.text = car.bodyHeight.ToString("0.00");
            traction.text = car.traction.ToString("0.00");
            correction.text = car.correctionTime.ToString("0.00");
            drag.text = car.drag.ToString("0.00");
            suspension.text = car.suspensionSmoothTime.ToString("0.00");
        }

        private void Update()
        {
            if (!active) return;
            
            speedText.text = carController.GetDisplaySpeed().ToString("0");
        }

        public void ToggleDebugUI()
        {
            active = !active;
            gameObject.SetActive(active);
            suspensionManager.ControlDebugUI(active);
        }

        public void ResetGame()
        {
            SceneManager.LoadScene(0);
        }

        public void IncreaseMoveSpeed()
        {
            car.acceleration += 0.1f;
            UpdateUI();
        }
        
        public void DecreaseMoveSpeed()
        {
            car.acceleration -= 0.1f;
            UpdateUI();
        }
        
        public void IncreaseMaxSpeed()
        {
            car.maxSpeed += 0.1f;
            UpdateUI();
        }
        
        public void DecreaseMaxSpeed()
        {
            car.maxSpeed -= 0.1f;
            UpdateUI();
        }
        
        public void IncreaseRideHeight()
        {
            car.bodyHeight += 0.01f;
            UpdateUI();
        }
        
        public void DecreaseRideHeight()
        {
            car.bodyHeight -= 0.01f;
            UpdateUI();
        }

        public void IncreaseDrag()
        {
            car.drag += 0.01f;
            UpdateUI();
        }
        
        public void DecreaseDrag()
        {
            car.drag -= 0.01f;
            UpdateUI();
        }

        public void IncreaseStiffness()
        {
            car.suspensionSmoothTime -= 0.01f;
            UpdateUI();
        }

        public void DecreaseStiffness()
        {
            car.suspensionSmoothTime += 0.01f;
            UpdateUI();
        }

        public void IncreaseTraction()
        {
            car.traction += 0.01f;
            UpdateUI();
        }
        
        public void DecreaseTraction()
        {
            car.traction -= 0.01f;
            UpdateUI();
        }
        
        public void IncreaseSmoothTime()
        {
            car.correctionTime += 0.01f;
            UpdateUI();
        }
        
        public void DecreaseSmoothTime()
        {
            car.correctionTime -= 0.01f;
            UpdateUI();
        }
    }
}