using System;
using System.Drawing;

namespace Stump.Plugins.DebugPlugin.Benchmarking
{
    public abstract class Benchmarker<T> where T : IComparable
    {
        public abstract T Average
        {
            get;
        }

        public abstract T Minimal
        {
            get;
        }

        public abstract T Maximal
        {
            get;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Pause()
        {
        }

        public Bitmap GenerateGraphic(DateTime startTime, DateTime endTime)
        {
        }

        public Bitmap GenerateGraphic()
        {
        }
    }
}