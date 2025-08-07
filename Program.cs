using System;
using System.Numerics;
using Raylib_cs;

namespace BlackHoleSimulator
{
    internal static class Program
    {
        private const int ScreenWidth = 1200;
        private const int ScreenHeight = 800;
        private const int SimulationWidth = 1000;
        private const int SimulationHeight = 800;
        private const int ControlPanelWidth = 200;
        
        static void Main()
        {
            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Black Hole Simulator");
            Raylib.SetTargetFPS(60);
            
            var simulation = new SimulationEngine(SimulationWidth, SimulationHeight);
            var lastTime = DateTime.Now;
            bool isRunning = false;
            double timeAcceleration = 1.0;
            int maxParticles = 200;
            
            while (!Raylib.WindowShouldClose())
            {
                // Handle input
                HandleInput(simulation, ref isRunning, ref timeAcceleration, ref maxParticles);
                
                // Update simulation
                if (isRunning)
                {
                    var now = DateTime.Now;
                    double deltaTime = (now - lastTime).TotalSeconds;
                    lastTime = now;
                    
                    simulation.SetTimeAcceleration(timeAcceleration);
                    simulation.SetMaxParticles(maxParticles);
                    simulation.Update(deltaTime);
                }
                
                // Render
                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(0, 0, 0, 255)); // Black
                
                // Draw simulation area
                Raylib.DrawRectangle(ControlPanelWidth, 0, SimulationWidth, SimulationHeight, new Color(0, 0, 0, 255));
                
                // Draw simulation
                DrawSimulation(simulation);
                
                // Draw UI
                DrawUI(simulation, isRunning, timeAcceleration, maxParticles);
                
                Raylib.EndDrawing();
            }
            
            Raylib.CloseWindow();
        }
        
        private static void HandleInput(SimulationEngine simulation, ref bool isRunning, ref double timeAcceleration, ref int maxParticles)
        {
            // Keyboard input
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                isRunning = !isRunning;
                if (isRunning) simulation.Start();
                else simulation.Stop();
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                simulation.Reset();
                simulation = new SimulationEngine(SimulationWidth, SimulationHeight);
                isRunning = false;
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.C))
            {
                simulation.ClearParticles();
            }
            
            if (Raylib.IsKeyPressed(KeyboardKey.A))
            {
                for (int i = 0; i < 10; i++)
                {
                    simulation.CreateOrbitingParticle(SimulationWidth, SimulationHeight);
                }
            }
            
            // Time acceleration controls
            if (Raylib.IsKeyDown(KeyboardKey.Up))
            {
                timeAcceleration = Math.Min(10.0, timeAcceleration + 0.1);
            }
            
            if (Raylib.IsKeyDown(KeyboardKey.Down))
            {
                timeAcceleration = Math.Max(0.1, timeAcceleration - 0.1);
            }
            
            // Mouse input for adding particles
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                var mousePos = Raylib.GetMousePosition();
                if (mousePos.X > ControlPanelWidth)
                {
                    Vector2D position = new Vector2D(mousePos.X - ControlPanelWidth, mousePos.Y);
                    Vector2D velocity = new Vector2D(0, 0);
                    simulation.AddParticle(position, velocity);
                }
            }
        }
        
        private static void DrawSimulation(SimulationEngine simulation)
        {
            // Translate drawing context to simulation area
            Raylib.BeginScissorMode(ControlPanelWidth, 0, SimulationWidth, SimulationHeight);
            
            // Draw stars
            for (int i = 0; i < 100; i++)
            {
                int x = ControlPanelWidth + (i * 73) % SimulationWidth;
                int y = (i * 127) % SimulationHeight;
                int size = (i % 3) + 1;
                Raylib.DrawCircle(x, y, size, new Color(255, 255, 255, 255)); // White
            }
            
            // Draw black hole
            var blackHole = simulation.BlackHole;
            Vector2 center = new Vector2((float)(blackHole.Position.X + ControlPanelWidth), (float)blackHole.Position.Y);
            
            // Draw accretion disk
            for (int ring = 8; ring >= 1; ring--)
            {
                float radius = (float)(blackHole.AccretionDiskRadius * ring / 8f);
                Color ringColor = new Color(255, (int)(150 * 0.6f), (int)(50 * 0.3f), 80);
                Raylib.DrawCircleV(center, radius, ringColor);
            }
            
            // Draw event horizon
            Raylib.DrawCircleV(center, (float)blackHole.EventHorizonRadius, new Color(255, 200, 0, 100));
            
            // Draw black hole core
            Raylib.DrawCircleV(center, (float)(blackHole.EventHorizonRadius * 0.8), new Color(0, 0, 0, 255)); // Black
            
            // Draw particles
            foreach (var particle in simulation.GetParticles())
            {
                Vector2 particlePos = new Vector2((float)(particle.Position.X + ControlPanelWidth), (float)particle.Position.Y);
                Raylib.DrawCircleV(particlePos, particle.Size, particle.Color);
                
                // Draw simplified trail
                for (int i = 1; i < Math.Min(particle.TrailLength, 20); i += 2)
                {
                    int trailIndex = (particle.TrailIndex - i + particle.TrailLength) % particle.TrailLength;
                    Vector2 trailPos = new Vector2((float)(particle.Trail[trailIndex].X + ControlPanelWidth), (float)particle.Trail[trailIndex].Y);
                    float alpha = 1.0f - (float)i / 20.0f;
                    Color trailColor = new Color(particle.Color.R, particle.Color.G, particle.Color.B, (byte)(particle.Color.A * alpha * 0.5f));
                    Raylib.DrawCircleV(trailPos, 1, trailColor);
                }
            }
            
            Raylib.EndScissorMode();
        }
        
        private static void DrawUI(SimulationEngine simulation, bool isRunning, double timeAcceleration, int maxParticles)
        {
            // Draw control panel background
            Raylib.DrawRectangle(0, 0, ControlPanelWidth, ScreenHeight, new Color(30, 30, 30, 255));
            
            int y = 10;
            
            // Status
            Raylib.DrawText($"Status: {(isRunning ? "Running" : "Paused")}", 10, y, 16, new Color(255, 255, 255, 255)); // White
            y += 25;
            
            // Particles
            Raylib.DrawText($"Particles: {simulation.ParticleCount}/{maxParticles}", 10, y, 16, new Color(255, 255, 255, 255));
            y += 25;
            
            // Time acceleration
            Raylib.DrawText($"Time: {timeAcceleration:F1}x", 10, y, 16, new Color(255, 255, 255, 255));
            y += 25;
            
            // Black hole info
            Raylib.DrawText($"Mass: {simulation.BlackHole.Mass:E1} kg", 10, y, 12, new Color(200, 200, 200, 255)); // Light gray
            y += 20;
            Raylib.DrawText($"Horizon: {simulation.BlackHole.EventHorizonRadius:F0}", 10, y, 12, new Color(200, 200, 200, 255));
            y += 40;
            
            // Instructions
            string[] instructions = {
                "Controls:",
                "SPACE - Play/Pause",
                "R - Reset",
                "C - Clear particles",
                "A - Add 10 particles",
                "UP/DOWN - Time speed",
                "Click - Add particle",
                "",
                "Features:",
                "• Gravitational physics",
                "• Event horizon",
                "• Accretion disk",
                "• Real-time simulation"
            };
            
            for (int i = 0; i < instructions.Length; i++)
            {
                Color color = i == 0 || i == 8 ? new Color(255, 255, 255, 255) : new Color(200, 200, 200, 255);
                int fontSize = i == 0 || i == 8 ? 14 : 12;
                Raylib.DrawText(instructions[i], 10, y + i * 18, fontSize, color);
            }
        }
    }
}
