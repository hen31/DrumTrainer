using System.Collections.Generic;

namespace DrumTrainer.ViewModels
{
    internal class NotesPerPosition
    {
        public List<BeatNote> Notes { get; internal set; }
        public int PositionIn32 { get; internal set; }
    }
}