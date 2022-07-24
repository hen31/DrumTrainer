using DrumTrainer.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrumTrainer.ViewModels
{
    class MetronomeSettingsViewModel : ViewModel
    {
        public MetronomeSettingsViewModel(Metronome metronome)
        {
            Metronome = metronome;
        }

        public Metronome Metronome { get; }
    }
}
