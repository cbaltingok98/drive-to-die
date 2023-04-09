namespace _project.Scripts.Car.DrivingMechanic
{
    [System.Serializable]
    public class Car
    {
        public float level;
        public float health;
        public float fuel;
        public float fuelConsumption;
        public float acceleration = 50;
        public float maxSpeed = 15;
        public float drag = 0.98f;
        public float traction = 1;
        public float correctionTime = 0.1f;
        public float suspensionSmoothTime;
        public float suspensionStiffness;
        public float bodyHeight;
        public float carHeight;

        public Car()
        {}

        public Car(DrivingStats drivingProfile)
        {
            level = drivingProfile.level;
            health = drivingProfile.health;
            fuel = drivingProfile.fuel;
            fuelConsumption = drivingProfile.fuelConsumption;
            acceleration = drivingProfile.acceleration;
            maxSpeed = drivingProfile.maxSpeed;
            drag = drivingProfile.drag;
            traction = drivingProfile.traction;
            correctionTime = drivingProfile.correctionTime;
            suspensionSmoothTime = drivingProfile.suspensionSmoothTime;
            suspensionStiffness = drivingProfile.suspensionStiffness;
            bodyHeight = drivingProfile.bodyHeight;
            carHeight = drivingProfile.carHeight;
        }
    }
}