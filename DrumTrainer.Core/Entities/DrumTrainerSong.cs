using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DrumTrainer.Core.Entities
{
    public class DrumTrainerSong
    {
        public int BPM { get; set; } = 100;

        public ObservableCollection<MusicMeasure> Measures { get; set; } = new ObservableCollection<MusicMeasure>();
       // public MusicMeasure Measure { get; set; }
        public string SpotifyTrack { get; set; }
        public string SpotifyName { get; set; }
    }
}
