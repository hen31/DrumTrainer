using DrumTrainer.Core.Entities;
using System;

namespace DrumTrainer
{
    public interface IMidiListener
    {
        void HitDrum(Drum drum,float happendAt);
    }
}