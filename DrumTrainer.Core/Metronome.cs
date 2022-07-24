using DrumTrainer.Core.SoundEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DrumTrainer.Core
{
    public class Metronome : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }



        public Metronome(IUIContext uiContext = null)
        {
            TickSound = DrumTrainerUtils.GetStartPath() + "Audio\\click.mp3";
            AlternateTickSound = DrumTrainerUtils.GetStartPath() + "Audio\\click2.mp3";
            _uiContext = uiContext;
            _tickAudio = new CachedSound(TickSound);
            _tockAudio = new CachedSound(AlternateTickSound);
        }
        public string TickSound { get; set; }
        public string AlternateTickSound { get; set; }

        private IDisposable _subscription;
        public void Start()
        {
            if (_uiContext != null)
            {
                if (_listeners != null)
                {
                    foreach (var listener in _listeners)
                    {
                        _uiContext.BeginInvoke(new Action(() => listener.Started()));
                    }
                }
            }
            _currentBeat = 1;
            _subscription = Observable.Generate(0, _ => true, it => it + 1, it => it,
                it => TimeSpan.FromSeconds((double)60 / (double)BPM)).Select(it => it % BeatsPerMeasure == 0)
                .Subscribe(PlaySound);
            _uiContext.BeginInvoke(new Action(() => OnPropertyChanged(nameof(IsPlaying))));

        }

        private int _currentBeat;
        private CachedSound _tickAudio;
        private CachedSound _tockAudio;
        private void PlaySound(bool tock)
        {
            NotifyBeatsChanged(tock, _currentBeat, _uiContext.GetCurrentElapsedTimeSinceStart());

            if (tock)
            {
                AudioPlaybackEngine.Instance.PlaySound(_tockAudio);
            }
            else
            {
                AudioPlaybackEngine.Instance.PlaySound(_tickAudio);
            }
            _currentBeat++;
            if (_currentBeat > BeatsPerMeasure)
            {
                _currentBeat = 1;
            }
        }

        private void NotifyBeatsChanged(bool tock, int beat, float happendAt)
        {
            if (_listeners != null && _uiContext != null)
            {
                foreach (var listener in _listeners)
                {
                    _uiContext.BeginInvoke(new Action(() => listener.IsBeating(tock, beat, happendAt)));
                }
            }
        }

        public void Stop()
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
                if (_uiContext != null)
                {
                    _uiContext.BeginInvoke(new Action(() => OnPropertyChanged(nameof(IsPlaying))));
                    if (_listeners != null)
                    {
                        foreach (var listener in _listeners)
                        {
                            _uiContext.BeginInvoke(new Action(() => listener.Stop()));
                        }
                    }
                }
            }
        }

        public bool IsPlaying
        {
            get { return _subscription != null; }
        }

        int _BPM = 80;
        public int BPM
        {
            get
            {
                return _BPM;
            }
            set
            {
                _BPM = value;
                OnPropertyChanged();
            }
        }

        public int BeatsPerMeasure { get; set; } = 4;

        private List<IBeatListener> _listeners;
        private readonly IUIContext _uiContext;

        public void AddListener(IBeatListener listener)
        {
            if (_listeners == null)
            {
                _listeners = new List<IBeatListener>()
                {
                    listener
                };
            }
            else
            {
                _listeners.Add(listener);
            }
        }
    }

}
