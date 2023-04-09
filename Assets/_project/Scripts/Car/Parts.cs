using UnityEngine;

namespace _project.Scripts.Car
{
    [System.Serializable]
    public struct Parts
    {
        public Transform mainBody;
        public Transform leftFrontTire;
        public Transform rightFrontTire;
        public Transform leftRearTire;
        public Transform rightRearTire;
        public Transform leftExhaust;
        public Transform rightExhaust;
    }
}