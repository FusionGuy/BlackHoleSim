# Black Hole Simulator

A real-time physics simulation of a black hole with gravitational effects, particle dynamics, and interactive controls, built with C# .NET and Raylib for cross-platform compatibility.

## Features

### Physics Simulation
- **Gravitational Attraction**: Particles are attracted to the black hole using realistic gravitational force calculations
- **Event Horizon**: Particles that cross the event horizon are consumed by the black hole
- **Accretion Disk**: Visual representation of matter spiraling into the black hole
- **Photon Sphere**: Innermost stable circular orbit visualization
- **Particle Trails**: Visual trails showing particle movement history
- **Time Dilation**: Variable time acceleration for observing different scales of motion

### Interactive Controls
- **Mouse Interaction**: Click to add particles, drag to set initial velocity
- **Keyboard Shortcuts**:
  - `Space`: Play/Pause simulation
  - `R`: Reset simulation
  - `C`: Clear all particles
  - `A`: Add 10 orbiting particles

### User Interface
- **Control Panel** with buttons and sliders for:
  - Start/Stop simulation
  - Time acceleration (0.1x to 10x)
  - Maximum particle count (10 to 500)
  - Reset and clear functions
  - Batch particle addition
- **Real-time Information Display**:
  - Current particle count
  - Time acceleration factor
  - Black hole properties
  - Simulation status

### Visual Effects
- **Dynamic Colors**: Particles change color based on proximity to the black hole (red-shift effect)
- **Animated Accretion Disk**: Rotating disk with spiral arms
- **Glowing Event Horizon**: Visual boundary with glow effects
- **Starfield Background**: Ambient space environment
- **Anti-aliased Graphics**: Smooth rendering for better visual quality

## Requirements

- .NET 8.0 or later
- Cross-platform (Windows, macOS, Linux) thanks to Raylib
- Graphics acceleration recommended for smooth performance
- OpenGL 3.3 or higher support

## Building and Running

### Using Command Line
```bash
# Navigate to the project directory
cd BlackHoleSim

# Build the project
dotnet build

# Run the application
dotnet run
```

### Using Visual Studio
1. Open `BlackHoleSimulator.csproj` in Visual Studio
2. Press F5 to build and run

## How to Use

1. **Starting the Simulation**:
   - Click "Start" to begin the simulation
   - The black hole will appear at the center with some initial particles

2. **Adding Particles**:
   - **Simple Click**: Click anywhere in the simulation area to add a stationary particle
   - **Drag**: Click and drag to set initial velocity (longer drag = higher velocity)
   - **Batch Add**: Click "Add 10" to add 10 particles in stable orbits

3. **Controlling Time**:
   - Use the time acceleration slider to speed up or slow down the simulation
   - Range: 0.1x (slow motion) to 10x (fast forward)

4. **Managing Particles**:
   - Use the particle count slider to set the maximum number of particles
   - "Clear" removes all particles
   - "Reset" restarts the entire simulation

5. **Observing Physics**:
   - Watch particles spiral into the black hole
   - Notice color changes as particles approach the event horizon
   - Observe stable and unstable orbits
   - See the accretion disk rotate around the black hole

## Physics Implementation

### Gravitational Force
The simulation uses Newton's law of universal gravitation:
```
F = G * M * m / r²
```
Where:
- `G` is the gravitational constant (scaled for visualization)
- `M` is the black hole mass
- `m` is the particle mass
- `r` is the distance between objects

### Black Hole Properties
- **Schwarzschild Radius**: Theoretical event horizon based on mass
- **Event Horizon**: Visual boundary where particles are consumed
- **Accretion Disk**: Extends to ~8x the event horizon radius
- **Photon Sphere**: Located at ~1.5x the event horizon radius

### Numerical Integration
- Uses Euler integration for position and velocity updates
- Frame-rate independent timing for consistent physics
- Collision detection with event horizon

## Project Structure

```
BlackHoleSim/
├── BlackHoleSimulator.csproj  # Project file with Raylib-cs dependency
├── Program.cs                 # Main entry point with Raylib rendering
├── SimulationEngine.cs       # Physics simulation controller
├── BlackHole.cs              # Black hole object and properties
├── Particle.cs               # Particle physics and rendering
├── Vector2D.cs               # 2D vector mathematics
├── run.sh                    # Unix shell script for easy launching
└── README.md                 # This file
```

## Customization

### Modifying Physics
- Adjust gravitational constant in `SimulationEngine.cs`
- Change black hole mass in the constructor
- Modify particle lifetime and properties in `Particle.cs`

### Visual Customization
- Particle colors and effects in `Particle.cs`
- Black hole appearance in `BlackHole.cs`
- UI colors and layout in `Program.cs`

### Performance Tuning
- Adjust frame rate target in `Program.cs` 
- Modify particle trail length in `Particle.cs`
- Change maximum particle counts for performance/visual balance

## Known Limitations

- 2D simulation (no z-axis physics)
- Simplified relativistic effects
- No gravitational waves or frame-dragging
- Requires OpenGL 3.3+ support

## Future Enhancements

- 3D visualization
- More accurate relativistic physics
- Multiple black holes
- Save/load simulation states
- Particle collision physics
- Enhanced particle effects and shaders

## License

This project is open source and available under the MIT License.
