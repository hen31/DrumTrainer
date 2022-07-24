using DrumTrainer.Core.Entities;

namespace DrumTrainer.ViewModels
{
    internal class BeatNote : ViewModel
    {
        public float ShouldBePlayedAt { get; internal set; }
        public Drum Drum { get; internal set; }
        public int Beat { get; internal set; }
        public int Position { get; internal set; }

        public PlayedNote PlayedNote { get; set; }
        public ExpectedNote ExpectedNote { get; set; }


        float _playedAt;
        public float PlayedAt
        {
            get
            {
                return _playedAt;
            }
            set
            {
                _playedAt = value;
                OnPropertyChanged();
            }
        }

        bool _played = false;
        public bool Played
        {
            get
            {
                return _played;
            }
            set
            {
                _played = value;
                OnPropertyChanged();
            }
        }


        public BeatNote(ExpectedNote expectedNote)
        {
            ExpectedNote = expectedNote;
            ShouldBePlayedAt = expectedNote.ShouldBePlayedAt;
            Drum = expectedNote.Drum;
            Beat = expectedNote.Beat;
            Position = expectedNote.Position;
        }
    }
}