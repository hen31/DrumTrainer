using DrumTrainer.Core;
using DrumTrainer.Core.Entities;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DrumTrainer
{
    public class MidiServiceListener
    {
        private bool keepRunning = true;
        private readonly MidiService _service;
        private readonly IUIContext _uiContext;
        private readonly IMidiListener _midiListener;
        private DrumMappingSetting _drumMapping;



        public MidiServiceListener(MidiService service, IUIContext uiContext, IMidiListener midiListener, DrumMappingSetting drumMapping)
        {
            _drumMapping = drumMapping;
            _service = service;
            _uiContext = uiContext;
            _midiListener = midiListener;
            Task.Run(() => InitializeMidiListener());
        }

        public void Stop()
        {
            keepRunning = false;
        }

        private async Task InitializeMidiListener()
        {
            while (keepRunning)
            {
                await Task.Delay(100).ConfigureAwait(false);
                if (keepRunning)
                {
                    while (_service != null && _service.EventStack.TryPop(out TrainerMidiEvent midiEvent))
                    {
                        if (midiEvent.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                        {
                            NoteEvent noteEvent = (NoteEvent)midiEvent.MidiEvent;
                            _uiContext.BeginInvoke(() =>
                            {
                                if (_drumMapping.ContainsNote(noteEvent.NoteNumber) && noteEvent.Velocity >2f)
                                {
                                    _midiListener.HitDrum(_drumMapping[noteEvent.NoteNumber], midiEvent.HappendAt);
                                }
                            });
                        }
                    }
                }

            }

        }
    }
}