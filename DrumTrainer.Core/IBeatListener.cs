using System;
using System.Collections.Generic;
using System.Text;

namespace DrumTrainer.Core
{
    public interface IBeatListener
    {
        void IsBeating(bool tock, int beat, float happendAt);
        void Started();

        void Stop();
    }
}
