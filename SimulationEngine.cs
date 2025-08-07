using System;
using System.Collections.Generic;
using System.Linq;
using Raylib_cs;

namespace BlackHoleSimulator
{
    public class SimulationEngine
    {
        private List<Particle> _particles;
        private BlackHole _blackHole;
        private Random _random;
        private double _timeAcceleration = 1.0;
        private bool _isRunning = false;
        private int _maxParticles = 200;

        public bool IsRunning => _isRunning;
        public double TimeAcceleration => _timeAcceleration;
        public int ParticleCount => _particles.Count(p => p.IsAlive);
        public int MaxParticles => _maxParticles;
        public BlackHole BlackHole => _blackHole;

        public SimulationEngine(int width, int height)
        {
            _particles = new List<Particle>();
            _random = new Random();
            
            // Create black hole at center
            Vector2D center = new Vector2D(width / 2.0, height / 2.0);
            _blackHole = new BlackHole(center, 5.972e24); // Earth mass scaled
            
            InitializeParticles(width, height);
        }

        private void InitializeParticles(int width, int height)
        {
            // Create initial particles orbiting the black hole
            for (int i = 0; i < _maxParticles / 4; i++)
            {
                CreateOrbitingParticle(width, height);
            }
        }

        public void Start()
        {
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void SetTimeAcceleration(double acceleration)
        {
            _timeAcceleration = Math.Max(0.1, Math.Min(10.0, acceleration));
        }

        public void SetMaxParticles(int maxParticles)
        {
            _maxParticles = Math.Max(10, Math.Min(1000, maxParticles));
        }

        public void AddParticle(Vector2D position, Vector2D velocity)
        {
            if (_particles.Count >= _maxParticles) return;

            double mass = _random.NextDouble() * 1e20 + 1e19;
            Color color = GetRandomParticleColor();
            int size = _random.Next(2, 6);
            double lifetime = _random.NextDouble() * 20 + 10;

            var particle = new Particle(position, velocity, mass, color, size, lifetime);
            _particles.Add(particle);
        }

        public void CreateOrbitingParticle(int width, int height)
        {
            if (_particles.Count >= _maxParticles) return;

            // Create particle at random distance from black hole
            double distance = _random.NextDouble() * (Math.Min(width, height) * 0.4) + _blackHole.EventHorizonRadius * 2;
            double angle = _random.NextDouble() * 2 * Math.PI;

            Vector2D position = new Vector2D(
                _blackHole.Position.X + distance * Math.Cos(angle),
                _blackHole.Position.Y + distance * Math.Sin(angle)
            );

            // Calculate orbital velocity for stable orbit (approximately)
            const double G = 6.674e-11 * 1e15; // Scaled gravitational constant
            double orbitalSpeed = Math.Sqrt(G * _blackHole.Mass / distance) * 0.7; // Slightly less for elliptical orbits
            
            // Add some randomness to make orbits more interesting
            orbitalSpeed *= (0.8 + _random.NextDouble() * 0.4);

            Vector2D velocity = new Vector2D(
                -orbitalSpeed * Math.Sin(angle),
                orbitalSpeed * Math.Cos(angle)
            );

            AddParticle(position, velocity);
        }

        public void Update(double deltaTime)
        {
            if (!_isRunning) return;

            double adjustedDeltaTime = deltaTime * _timeAcceleration;

            // Update black hole
            _blackHole.Update(adjustedDeltaTime);

            // Update particles
            for (int i = _particles.Count - 1; i >= 0; i--)
            {
                _particles[i].Update(adjustedDeltaTime, _blackHole.Position, _blackHole.Mass, _blackHole.EventHorizonRadius);

                // Remove dead particles
                if (!_particles[i].IsAlive)
                {
                    _particles.RemoveAt(i);
                }
            }

            // Randomly spawn new particles if we're below the maximum
            if (_particles.Count < _maxParticles && _random.NextDouble() < 0.02) // 2% chance each frame
            {
                CreateOrbitingParticle(800, 600); // Default size, should be passed as parameters
            }
        }

        public IEnumerable<Particle> GetParticles()
        {
            return _particles.Where(p => p.IsAlive);
        }

        private Raylib_cs.Color GetRandomParticleColor()
        {
            Raylib_cs.Color[] colors = {
                new Raylib_cs.Color(100, 150, 255, 255), // Blue
                new Raylib_cs.Color(150, 255, 150, 255), // Green
                new Raylib_cs.Color(255, 150, 100, 255), // Orange
                new Raylib_cs.Color(255, 100, 255, 255), // Magenta
                new Raylib_cs.Color(100, 255, 255, 255), // Cyan
                new Raylib_cs.Color(255, 255, 100, 255)  // Yellow
            };

            return colors[_random.Next(colors.Length)];
        }

        public void Reset()
        {
            _particles.Clear();
            _timeAcceleration = 1.0;
            _isRunning = false;
        }

        public void ClearParticles()
        {
            _particles.Clear();
        }
    }
}
