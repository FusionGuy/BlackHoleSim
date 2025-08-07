using System;
using Raylib_cs;
using System.Numerics;

namespace BlackHoleSimulator
{
    public class Particle
    {
        public Vector2D Position { get; set; }
        public Vector2D Velocity { get; set; }
        public double Mass { get; set; }
        public Raylib_cs.Color Color { get; set; }
        public int Size { get; set; }
        public double LifeTime { get; set; }
        public double MaxLifeTime { get; set; }
        public bool IsAlive => LifeTime > 0;
        public Vector2D[] Trail { get; private set; }
        public int TrailIndex { get; private set; }
        public int TrailLength { get; private set; }

        private Random _random = new Random();

        public Particle(Vector2D position, Vector2D velocity, double mass, Raylib_cs.Color color, int size = 3, double lifeTime = 10.0)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Color = color;
            Size = size;
            LifeTime = lifeTime;
            MaxLifeTime = lifeTime;
            
            // Initialize trail
            TrailLength = 50;
            Trail = new Vector2D[TrailLength];
            TrailIndex = 0;
            
            for (int i = 0; i < TrailLength; i++)
            {
                Trail[i] = position;
            }
        }

        public void Update(double deltaTime, Vector2D blackHolePosition, double blackHoleMass, double eventHorizonRadius)
        {
            if (!IsAlive) return;

            // Calculate gravitational force
            Vector2D direction = blackHolePosition - Position;
            double distance = direction.Magnitude;
            
            // Check if particle has crossed the event horizon
            if (distance <= eventHorizonRadius)
            {
                LifeTime = 0; // Particle is consumed
                return;
            }

            // Apply gravitational force (F = G * M * m / r^2)
            const double G = 6.674e-11 * 1e15; // Scaled gravitational constant for visual effect
            double forceMagnitude = G * blackHoleMass * Mass / (distance * distance + 1); // +1 to avoid division by zero
            Vector2D force = direction.Normalized * forceMagnitude;

            // Update velocity (a = F/m)
            Vector2D acceleration = force / Mass;
            Velocity += acceleration * deltaTime;

            // Update position
            Position += Velocity * deltaTime;

            // Update trail
            Trail[TrailIndex] = Position;
            TrailIndex = (TrailIndex + 1) % TrailLength;

            // Update lifetime
            LifeTime -= deltaTime;

            // Add some visual effects based on distance to black hole
            UpdateColor(distance, eventHorizonRadius);
        }

        private void UpdateColor(double distanceToBlackHole, double eventHorizonRadius)
        {
            // Change color based on proximity to black hole
            double proximityFactor = Math.Max(0, 1 - (distanceToBlackHole - eventHorizonRadius) / (eventHorizonRadius * 3));
            
            if (proximityFactor > 0.8)
            {
                // Very close - red shift
                Color = new Raylib_cs.Color(255, (int)(255 * proximityFactor), 0, 255);
            }
            else if (proximityFactor > 0.5)
            {
                // Moderately close - orange
                Color = new Raylib_cs.Color(255, (int)(128 * (1 - proximityFactor)), 0, 255);
            }
            else
            {
                // Normal color with slight blue shift
                double alpha = Math.Max(0.3, LifeTime / MaxLifeTime);
                Color = new Raylib_cs.Color(100, 150, 255, (int)(255 * alpha));
            }
        }
    }
}
