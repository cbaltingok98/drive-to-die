using UnityEngine;
using UnityEngine.EventSystems;

namespace _project.Scripts.Utils
{
    public class EventSytemRemover : MonoBehaviour
    {
        private void Awake() // FIXME DEVELOPER NOTE: facebook guncellemesi ile silinecek
        {
            var eventSystems = FindObjectsOfType<EventSystem>();
            var selfEventSystem = GetComponent<EventSystem>();

            foreach (var eventSystem in eventSystems)       
            {
                if (eventSystem != selfEventSystem)
                {
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}