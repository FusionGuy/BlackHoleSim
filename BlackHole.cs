using System;
using Raylib_cs;

namespace BlackHoleSimulator
{
    public class BlackHole
    {
        public Vector2D Position { get; set; }
        public double Mass { get; set; }
        public double SchwarzschildRadius { get; private set; }
        public double EventHorizonRadius { get; private set; }
        public double AccretionDiskRadius { get; private set; }
        public double PhotonSphereRadius { get; private set; }
        
        private double _rotationAngle;
        private Random _random = new Random();

        public BlackHole(Vector2D position, double mass)
        {
            Position = position;
            Mass = mass;
            
            // Calculate various radii based on mass
            // Schwarzschild radius: Rs = 2GM/c² (scaled for visualization)
            const double G = 6.674e-11;
            const double c = 299792458;
            SchwarzschildRadius = 2 * G * mass / (c * c) * 1e12; // Scaled for visualization
            
            EventHorizonRadius = Math.Max(20, SchwarzschildRadius * 0.5); // Visual event horizon
            AccretionDiskRadius = EventHorizonRadius * 8;
            PhotonSphereRadius = EventHorizonRadius * 1.5;
        }

        public void Update(double deltaTime)
        {
            // Rotate the accretion disk
            _rotationAngle += deltaTime * 2; // 2 radians per second
            if (_rotationAngle > 2 * Math.PI)
                _rotationAngle -= 2 * Math.PI;
        }

        public double RotationAngle => _rotationAngle;

        public bool IsInsideEventHorizon(Vector2D position)
        {
            return Vector2D.Distance(Position, position) <= EventHorizonRadius;
        }

        public bool IsInsideAccretionDisk(Vector2D position)
        {
            double distance = Vector2D.Distance(Position, position);
            return distance > EventHorizonRadius && distance <= AccretionDiskRadius;
        }
    }
}
