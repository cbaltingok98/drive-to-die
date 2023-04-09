using System;

namespace _project.Scripts.Car
{ 
    [Serializable]
    public class DrivingStats
    {
        public float level;
        public float health;
        public float fuel;
        public float fuelConsumption;
        public float acceleration;
        public float maxSpeed;
        public float drag;
        public float traction;
        public float correctionTime;
        public float suspensionSmoothTime;
        public float suspensionStiffness;
        public float bodyHeight;
        public float carHeight;
    }
}