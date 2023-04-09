using DG.Tweening;
using UnityEngine;

namespace _project.Scripts.ShowRoom
{
    public class ShowRoomCamera : MonoBehaviour
    {
        public Transform parent;
        public Camera thisCamera;
        public static ShowRoomCamera Instance;

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
        }

        public void ControlCamera(bool set)
        {
            gameObject.SetActive(set);
        }


        public void MoveToPosition(Transform target, int fov)
        {
            parent.DOMove(target.position, 0.5f);
            parent.DORotate(target.eulerAngles, 0.5f);
            
            float fieldOfView = thisCamera.fieldOfView;
            DOTween.To(() => fieldOfView, x => fieldOfView = x, fov, 0.5f).OnUpdate(() =>
            {
                thisCamera.fieldOfView = fieldOfView;
            });
        }
    }
}