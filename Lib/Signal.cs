using System;
using System.Collections.Generic;

namespace Lib
{
    public abstract class Signal<T>
    {
        public double Begin { get; protected set; }

        public double SamplingFrequency { get; protected set; }

        public List<T> Points { get;  set; }

        public double? Period { get; protected set; }

        public double Step { get; set; }

        public double Probability { get; set; }

        public int Interval { get; set; }

        public double SamplingPeriod => 1.0 / SamplingFrequency;

        public double Length => EndsAt - Begin;

        public double EndsAt => Begin + SamplingPeriod * Points.Count;

    }
}
