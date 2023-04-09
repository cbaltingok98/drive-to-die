using System;
using System.Collections.Generic;
using _project.Scripts.Berk;
using Unity.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Utils
{
    public class CustomCameraPositioner : MonoBehaviour
    {
        [SerializeField] private Camera currentCamera;
        [SerializeField] private bool changePositionAtStart = true;
        [SerializeField] private List<AspectCameraPropertyStruct> aspectsAndCameras;

        private AspectCameraPropertyStruct usingAspectCameraStruct;

        private void Start()
        {
            if (changePositionAtStart)
            {
                UpdateCameraForCurrentView();
            }
        }

        public void Reset()
        {
            currentCamera = GetComponent<Camera>();
        }



        [ContextMenu("UpdateCameraForCurrentView")]
        private void UpdateCameraForCurrentView()
        {
            if (aspectsAndCameras.Count == 0)
            {
                return;
            }

            AspectCameraPropertyStruct closestAspect = GetClosestAspectStruct(currentCamera.aspect);

            usingAspectCameraStruct = closestAspect;

            ChangeCameraProperties(closestAspect);
        }


        private AspectCameraPropertyStruct GetClosestAspectStruct(float aspectRatio)
        {
            if (aspectsAndCameras.Count == 0)
            {
                return null;
            }


            AspectCameraPropertyStruct closestAspect = aspectsAndCameras[0];

            foreach (var aspectCamera in aspectsAndCameras)
            {
                if (Mathf.Abs(aspectCamera.GetAspectRatio() - aspectRatio) < Mathf.Abs(closestAspect.GetAspectRatio() - aspectRatio))
                {
                    closestAspect = aspectCamera;
                }
            }

            Printer.Print("Closest aspect found :" + closestAspect.aspect);

            return closestAspect;
        }


        private void ChangeCameraProperties(AspectCameraPropertyStruct aspectCameraProperty)
        {
            currentCamera.fieldOfView = aspectCameraProperty.fieldOfView;
            currentCamera.transform.localPosition = aspectCameraProperty.localPosition;
            currentCamera.transform.localEulerAngles = aspectCameraProperty.localEulerAngles;
        }


        [ContextMenu("CopyCurrentAspectProperties")]
        private void CopyCurrentAspectProperties()
        {
            float currentAspect = currentCamera.aspect;

            foreach (var aspect in aspectsAndCameras)
            {
                if ((aspect.GetAspectRatio() < currentAspect + 0.01f) && (aspect.GetAspectRatio() > currentAspect - 0.01f))
                {
#if UNITY_EDITOR 
                    OverrideAspectStruct.AreYouSureDialog(() =>
                    {
                        aspect.aspect = new Vector2Int(currentCamera.pixelWidth, currentCamera.pixelHeight);
                        aspect.fieldOfView = currentCamera.fieldOfView;
                        aspect.localPosition = currentCamera.transform.localPosition;
                        aspect.localEulerAngles = currentCamera.transform.localEulerAngles;

                        Printer.Print("Available aspect properies changed");

                    }, aspect.GetAspectRatio());
#endif
                    return;
                }
            }

            var newAspectCamera = new AspectCameraPropertyStruct(new Vector2Int(currentCamera.pixelWidth, currentCamera.pixelHeight));
             
            newAspectCamera.fieldOfView = currentCamera.fieldOfView;
            newAspectCamera.localPosition = currentCamera.transform.localPosition;
            newAspectCamera.localEulerAngles = currentCamera.transform.localEulerAngles;

            Printer.Print("New aspect properies added: " + newAspectCamera.aspect + ", Ratio: " + newAspectCamera.GetAspectRatio());
            aspectsAndCameras.Add(newAspectCamera);
        }



        public void UpdateCameraForThisAspect(float aspectRatio)
        {
            if (aspectsAndCameras.Count == 0)
            {
                return;
            }

            AspectCameraPropertyStruct closestAspect = GetClosestAspectStruct(aspectRatio);

            ChangeCameraProperties(closestAspect);
        }


        #region Screenshot Taker

        public void ScreenshotTakingIsStarted()
        {
            usingAspectCameraStruct = GetClosestAspectStruct(currentCamera.aspect);            
        }

        public void ResetToLastUsingCameraAspect()
        {
            if (usingAspectCameraStruct != null)
            {
                ChangeCameraProperties(usingAspectCameraStruct);
            }
        }

        #endregion

    }
}


[System.Serializable]
public class AspectCameraPropertyStruct
{
    [ReadOnly] public Vector2Int aspect;
    [ReadOnly] public float aspectRatio;
    public float fieldOfView = 60f;
    public Vector3 localPosition;
    public Vector3 localEulerAngles;

    public AspectCameraPropertyStruct(Vector2Int aspect)
    {
        this.aspect = aspect;
        aspectRatio = (float)aspect.x / (float)aspect.y;

    }

    public float GetAspectRatio()
    {
        aspectRatio = (float)aspect.x / (float)aspect.y;
        return aspectRatio;
    }
}


#if UNITY_EDITOR 

public class OverrideAspectStruct : ScriptableObject
{
    public static void AreYouSureDialog(Action action, float aspect)
    {
        if (EditorUtility.DisplayDialog("Bu aspectRatio zaten var: " + aspect,
                "Override etmek istediğine emin misin?", "Evet", "Iptal"))
        {
            action?.Invoke();
        }
    }
}

#endif
