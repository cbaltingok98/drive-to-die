using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.MyDebug
{
    public class FpsCounter : MonoBehaviour
    {
        [SerializeField] private Text uiText;
        
        private int lastFrameIndex;
        private float[] frameDeltaTimeArray;

        private void Awake()
        {
            frameDeltaTimeArray = new float[50];
        }

        private void Update()
        {
            frameDeltaTimeArray[lastFrameIndex] = Time.deltaTime;
            lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

            uiText.text = Mathf.Round(CalculateFPS()).ToString("0");
        }

        private float CalculateFPS()
        {
            var total = 0f;
            
            foreach (var deltaTime in frameDeltaTimeArray)
            {
                total += deltaTime;
            }

            return frameDeltaTimeArray.Length / total;
        }
    }
}