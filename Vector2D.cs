using System;

namespace BlackHoleSimulator
{
    public struct Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Magnitude => Math.Sqrt(X * X + Y * Y);
        public double MagnitudeSquared => X * X + Y * Y;

        public Vector2D Normalized
        {
            get
            {
                double mag = Magnitude;
                return mag > 0 ? new Vector2D(X / mag, Y / mag) : new Vector2D(0, 0);
            }
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
            => new Vector2D(a.X + b.X, a.Y + b.Y);

        public static Vector2D operator -(Vector2D a, Vector2D b)
            => new Vector2D(a.X - b.X, a.Y - b.Y);

        public static Vector2D operator *(Vector2D a, double scalar)
            => new Vector2D(a.X * scalar, a.Y * scalar);

        public static Vector2D operator /(Vector2D a, double scalar)
            => new Vector2D(a.X / scalar, a.Y / scalar);

        public static double Distance(Vector2D a, Vector2D b)
            => (a - b).Magnitude;

        public static double DistanceSquared(Vector2D a, Vector2D b)
            => (a - b).MagnitudeSquared;

        public override string ToString() => $"({X:F2}, {Y:F2})";
    }
}
