using System;
using System.Numerics;

namespace CyclotronSimulator
{
    class Cyclotron
    {
        //speed of light in Mm/s
        private static readonly double C = 299.792458f;
        //electron charge in 10^-19 C
        private static readonly double e = 1.6022f;
        //set relativistic corrections on
        private readonly bool Relativistic, Correction;
        //timesteps of the simulation in micro-sec
        private readonly double Step;
        //The magnetic field in Telsa
        private readonly double B;
        //particle charge in respect to electron charge
        private readonly int Q;
        //particle rest mass in 10^-27g
        private readonly double M0;
        //frequency of the cyclotron in Mhz
        private readonly double F;
        //gap between the dees in mm
        private readonly int D;
        //the electric potential between the dees in kV
        private readonly int U;
        //current acceleration, position and velocity
        public Vector a, pos, v;
        //other cyclotron params
        private double Size, freqTime, gamma, Start;
        public double T;
        private readonly double qB, qe;
        public bool bounced = false;

        public Cyclotron(double size, double stepSize, double startTime, int d, int q, double m0, int u, double b, double f, bool relativistic, bool correction)
        {
            a = new Vector();
            v = new Vector();
            pos = new Vector();
            Size = size;
            Step = stepSize;
            Start = startTime;
            freqTime = startTime;
            T = startTime;
            D = d / 2;
            Q = q;
            M0 = m0;
            U = u;
            B = b;
            F = f;
            Relativistic = relativistic;
            Correction = correction;
            qe = q * e;
            qB = qe * B;
        }

        //do one simulation step
        public void SimulationStep()
        {
            double prevPosX = pos.X;
            double prevVX = v.X;
            Accel();
            NewPos();
            NewSpeed();
            T += Step * 0.01;
            freqTime += Step * gamma * 0.01;
            if (Math.Abs(prevPosX) < D && Math.Sign(prevVX) != Math.Sign(v.X) && T > Start + Step) bounced = true;
            //Console.WriteLine(pos.X + " " + pos.Y + " " + v.Length() + " " + pos.Length());
        }

        //Calc the acceleration for the current position/speed/time
        private void Accel()
        {
            double fx = qB * v.Y;
            double fy = -qB * v.X;
            double m = Mass();
            if (Math.Abs(pos.X) < D) fx += qe * E();
            a.X = (fx / m);
            a.Y = (fy / m);
        }
        //calculate the new Position
        private void NewPos()
        {
            pos.Plus(v.Times(Step * 10)); //forward euler
        }

        //calculate the new speed
        private void NewSpeed()
        {
            v.Plus(a.Times(Step)); //forward euler
        }

        //calculate the mass
        private double Mass()
        {
            if (!Relativistic) return M0;
            double speed = v.Length();
            double beta = speed / C;
            gamma = Math.Sqrt(1 - (beta * beta));
            return (M0 / gamma);
        }

        //calculate the current electrical field
        private double E()
        {
            if (Correction) return (Math.Sin(freqTime * F * 2 * Math.PI) * U) / (2 * D);
            double V = Math.Sin(T * F * 2 * Math.PI) * U;
            return (V / (2 * D));
        }
    }
}
