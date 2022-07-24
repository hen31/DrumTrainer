using NAudio.Midi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DrumTrainer.Core
{
    public class MidiService
    {
        private ConcurrentStack<TrainerMidiEvent> _eventStack;
        private MidiIn _midiIn;
        private readonly IUIContext _context;

        public MidiService(MidiInCapabilities midiDeviceToListenTo, IUIContext context)
        {
            _eventStack = new ConcurrentStack<TrainerMidiEvent>();
            Task.Run(() => InitializeMidiService(midiDeviceToListenTo));
            _context = context;
        }

        private void InitializeMidiService(MidiInCapabilities midiDeviceToListenTo)
        {
            _midiIn = new MidiIn(GetMidiDevices().IndexOf(midiDeviceToListenTo));
            _midiIn.MessageReceived += _midiIn_MessageReceived;
            _midiIn.Start();
        }

        private void _midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            _eventStack.Push(new TrainerMidiEvent(e.MidiEvent, _context.GetCurrentElapsedTimeSinceStart()));
            if (e.MidiEvent is NoteEvent noteE && e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && noteE.Velocity != 0)
            {
                Debug.WriteLine("drum hit at: " + _context.GetCurrentElapsedTimeSinceStart() + " event: " + noteE.NoteName);
            }
        }

        public ConcurrentStack<TrainerMidiEvent> EventStack
        {
            get
            {
                return _eventStack;
            }
        }


        public void Stop()
        {
            _midiIn.Stop();
            _midiIn.MessageReceived -= _midiIn_MessageReceived;
            _midiIn.Dispose();
            _midiIn = null;
        }


        public static List<MidiInCapabilities> GetMidiDevices()
        {
            List<MidiInCapabilities> _devices = new List<MidiInCapabilities>();
            for (int device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                _devices.Add(MidiIn.DeviceInfo(device));
            }
            return _devices;
        }

        


    }
}
