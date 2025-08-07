#!/bin/bash

# Black Hole Simulator Run Script
echo "Starting Black Hole Simulator..."
echo "Building project..."

if dotnet build --configuration Release; then
    echo "Build successful! Starting simulation..."
    echo ""
    echo "Controls:"
    echo "  SPACE - Play/Pause simulation"
    echo "  R - Reset simulation"  
    echo "  C - Clear particles"
    echo "  A - Add 10 orbiting particles"
    echo "  UP/DOWN - Adjust time acceleration"
    echo "  Click - Add particle at mouse position"
    echo ""
    dotnet run --configuration Release
else
    echo "Build failed. Please check for errors."
    exit 1
fi
