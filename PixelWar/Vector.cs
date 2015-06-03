using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    public struct Vector
    {
        public static readonly Vector Zero = new Vector();

        double x;
        double y;

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double AngleTo(Vector pos)
        {
            return Math.Atan2(pos.y - y, pos.x - x);
        }

        public Vector(double v)
        {
            this.x = this.y = v;
        }

        public static Vector operator +(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            return new Vector(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Performs element-wise subtraction from the given vector with the provided value. 
        /// </summary>
        public static Vector operator -(Vector a, double v)
        {
            return new Vector(a.X - v, a.Y - v);
        }

        /// <summary>
        /// Performs element-wise addition from the given vector with the provided value. 
        /// </summary>
        public static Vector operator +(Vector a, double v)
        {
            return new Vector(a.X + v, a.Y + v);
        }

        public static Vector operator *(Vector a, double mult)
        {
            return new Vector(a.X * mult, a.Y * mult);
        }

        public static Vector operator /(Vector a, double mult)
        {
            return new Vector(a.X / mult, a.Y / mult);
        }

        /// <summary>
        /// Performs element-wise division of a by b. 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vector operator /(Vector a, Vector b)
        {
            return new Vector(a.X / b.X, a.Y / b.Y);
        }

        public static bool operator ==(Vector a, Vector b)
        {
            return a.X.Equals(b.X) && a.Y.Equals(b.Y);
        }

        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }

        public double LengthSquared()
        {
            return X * X + Y * Y;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public Vector Normalize()
        {
            return (this / this.Length());
        }

        public double DistanceToSquared(Vector other)
        {
            var dx = X - other.X;
            var dy = Y - other.Y;
            return dx * dx + dy * dy;
        }

        public double DistanceTo(Vector other)
        {
            return Math.Sqrt(DistanceToSquared(other));
        }

        public bool Inside(Vector pos, Vector size)
        {
            return X >= pos.x && y >= pos.y && x <= pos.x + size.x && y <= pos.y + size.y;
        }

        public Point ToPoint()
        {
            return new Point((int)x, (int)y);
        }
        public Point Floor()
        {
            return new Point((int)Math.Floor(x), (int)Math.Floor(y));
        }
        public Point Ceiling()
        {
            return new Point((int)Math.Ceiling(x), (int)Math.Ceiling(y));
        }

        public Vector PolarProjection(double angle, double distance)
        {
            return new Vector(x + Math.Cos(angle) * distance, y + Math.Sin(angle) * distance);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}", x.ToString("0.00"), y.ToString("0.00"));
        }
    }
}
