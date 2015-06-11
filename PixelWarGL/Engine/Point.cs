using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    public struct Point
    {
        public static readonly Point Empty = new Point();

        public int X;
        public int Y;

        public Point(int v)
        {
            X = Y = v;
        }

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public IEnumerable<Point> IterateToInclusive(Point b)
        {
            for (int ix = X; ix <= b.X; ix++)
                for (int iy = Y; iy <= b.Y; iy++)
                    yield return new Point(ix, iy);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static Point operator /(Point a, int divisor)
        {
            return new Point(a.X / divisor, a.Y / divisor);
        }
        public static Point operator %(Point a, int divisor)
        {
            return new Point((a.X % divisor + divisor) % divisor, (a.Y % divisor + divisor) % divisor);
        }
        public static Point operator *(Point a, int divisor)
        {
            return new Point(a.X * divisor, a.Y * divisor);
        }

        public static Point operator *(Point a, Point b)
        {
            return new Point(a.X * b.X, a.Y * b.Y);
        }

        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public static bool operator !=(Point a, Point b)
        {
            return a.X != b.X || a.Y != b.Y;
        }

        public double DistanceTo(Point other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public Point Rotate (Point origin, double angle)
        {
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var a = this - origin;
            var b = new Point((int)(cos * a.X - sin * a.Y), (int)(sin * a.X + cos * a.Y));
            var c = b + origin;
            return c;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
                return false;
            var p = (Point)obj;
            return p == this;
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
}
