using DrumTrainer.Core.Entities;
using System;

namespace DrumTrainer.ViewModels
{
    public class PlayedNote
    {
        public Drum Drum { get; internal set; }
        public float HappendAt { get; internal set; }
        public bool Matched { get; internal set; }
    }
}