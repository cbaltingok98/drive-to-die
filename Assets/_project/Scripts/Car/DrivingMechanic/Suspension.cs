using TMPro;
using UnityEngine;

namespace _project.Scripts.Car.DrivingMechanic
{
    public class Suspension : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valText;
        [SerializeField] private LayerMask groundCheck;

        private SuspensionManager suspensionManager;
        
        private Vector3 refVector;
    
        private RaycastHit hitInfo;

        private float height;
        private float stiffness;
    
        private void LateUpdate()
        {
            var transformPosition = transform.position;
            var down = transform.TransformDirection(Vector3.down);

            if (!Physics.Raycast(transformPosition, down, out hitInfo, 10, groundCheck)) return;
            
            // var height = _suspensionManager.GetHeight();        // TODO cache it
            // var stiffness = _suspensionManager.GetStiffness();  // TODO cache it
            
            var hitInfoPoint = hitInfo.point;                

            var pos = hitInfoPoint + ((transformPosition - hitInfoPoint).normalized) * height;
            pos.y = Mathf.Clamp(pos.y, 0f, float.MaxValue);
                
            transform.position = Vector3.SmoothDamp(transformPosition, pos, ref refVector, stiffness);
        }

        public void SetSuspension(SuspensionManager suspensionManager)
        {
            this.suspensionManager = suspensionManager;
            height = this.suspensionManager.GetHeight();
            stiffness = this.suspensionManager.GetStiffness();
        }

        public void ControlDebugText(bool set)
        {
            valText.gameObject.SetActive(set);
        }
    }
}
