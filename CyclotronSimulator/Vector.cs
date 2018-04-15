using System;
using System.Collections.Generic;
using System.Text;

namespace CyclotronSimulator
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double LengthSquared()
        {
            return (X * X) + (Y * Y);
        }
        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }
        public void Plus(Vector v)
        {
            X += v.X;
            Y += v.Y;
        }

        public void Plus(double s)
        {
            X += s;
            Y += s;
        }

        public Vector Times(double s)
        {
            return new Vector(X * s, Y * s);
        }
    }
}
