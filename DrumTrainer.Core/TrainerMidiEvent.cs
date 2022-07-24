using NAudio.Midi;
using System;

namespace DrumTrainer.Core
{
    public class TrainerMidiEvent
    {
        public MidiEvent MidiEvent { get; set; }
        public float HappendAt {get;set;}

        public TrainerMidiEvent(MidiEvent midiEvent, float now)
        {
            MidiEvent = midiEvent;
            HappendAt = now;
        }
    }
}