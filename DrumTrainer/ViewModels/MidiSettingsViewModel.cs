using DrumTrainer.Core;
using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DrumTrainer.ViewModels
{
    class MidiSettingsViewModel : ViewModel
    {
        private readonly IChildView _view;
        private Properties.DrumTrainer _settings;

        public MidiSettingsViewModel(IChildView view, MidiInCapabilities midiDevice)
        {
            _settings = new Properties.DrumTrainer();
            if (!string.IsNullOrWhiteSpace(_settings.MidiSettings))
            {
                DrumMappingSetting = JsonConvert.DeserializeObject<DrumMappingSetting>(_settings.MidiSettings);
            }
            else
            {
                DrumMappingSetting = DrumMappingSetting.CreateNew();
                DrumMappingSetting.DrumName = midiDevice.ProductName;
            }

            _view = view;
            CancelCommand = new RelayCommand(CancelEditExecute);
            SaveCommand = new RelayCommand(SaveExecute);

            //Export command and import command
            _currentMidiService = new MidiService(midiDevice, WpfUIContext.Instance);
            timer = new System.Threading.Timer(TimerTick, null, 1000, 200);
        }

        private void TimerTick(object state)
        {
            if(_currentMidiService.EventStack.TryPop(out TrainerMidiEvent trainerMidiEvent))
            {
                _currentMidiService.EventStack.Clear();
            }
            if(trainerMidiEvent != null && trainerMidiEvent.MidiEvent is NoteEvent noteEvent)
            {
                LatestNote = noteEvent.NoteNumber;
            }
        }

        private void SaveExecute(object obj)
        {
            timer.Dispose();
            _currentMidiService.Stop();
            _currentMidiService = null;
            _view.Close(DrumMappingSetting);
        }

        private void CancelEditExecute(object obj)
        {
            timer.Dispose();
            _currentMidiService.Stop();
            _currentMidiService = null;
            _view.Close(null);
        }

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }

        private MidiService _currentMidiService;
        private Timer timer;
        int _latestNote;
        public int LatestNote
        {
            get
            {
                return _latestNote;
            }
            set
            {
                _latestNote = value;
                OnPropertyChanged();
            }
        }

        public DrumMappingSetting DrumMappingSetting { get; set; }



    }
}
