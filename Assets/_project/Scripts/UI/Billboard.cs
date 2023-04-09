using UnityEngine;

namespace _project.Scripts.UI
{
    public class Billboard : MonoBehaviour
    {
        private Transform target;
    
        private void Start()
        {
            target = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(transform.position + target.forward);
        }
    }
}