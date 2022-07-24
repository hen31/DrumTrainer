using DrumTrainer.Core.Entities;
using System;

namespace DrumTrainer.ViewModels
{
    internal class ExpectedNote
    {
        public float ShouldBePlayedAt { get; internal set; }
        public Drum Drum { get; internal set; }
        public int Beat { get; internal set; }
        public int Position { get; internal set; }
        public Note Note { get; internal set; }
    }
}