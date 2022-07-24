using DrumTrainer.Core;
using DrumTrainer.Core.Entities;
using MahApps.Metro.SimpleChildWindow;
using Microsoft.Win32;
using NAudio.Midi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DrumTrainer.Spotify;
using SpotifyAPI.Web.Models;
using System.Windows.Input;

namespace DrumTrainer.ViewModels
{
    class MainViewModel : ViewModel, IBeatListener, IMidiListener
    {
        private float MaxTimeBetweenExpectedAndHitNote = 200f;
        private MidiService _currentMidiService;

        private MidiServiceListener _midiListener;

        Properties.DrumTrainer _settings;
        private IView _view;
        public AsyncRelayCommand EditMeasureCommand { get; private set; }
        public RelayCommand DeleteMeasureCommand { get; private set; }
        public RelayCommand AddMeasureCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand OpenCommand { get; private set; }
        public AsyncRelayCommand MidiSettingsCommand { get; private set; }
        public RelayCommand SelectMidiDeviceCommand { get; private set; }
        public RelayCommand SelectSpotifyDeviceCommand { get; private set; }

        public RelayCommand LoginSpotifyCommand { get; private set; }
        public AsyncRelayCommand SelectSpotifySongCommand { get; private set; }

        private DrumTrainerSong _song;
        public DrumTrainerSong Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song = value;
                OnPropertyChanged();
            }
        }


        private MusicMeasure _nextPlusOneMusicMeasure;
        public MusicMeasure NextPlusOneMusicMeasure
        {
            get
            {
                return _nextPlusOneMusicMeasure;
            }
            set
            {
                _nextPlusOneMusicMeasure = value;
                OnPropertyChanged();
            }
        }
        private MusicMeasure _nextMusicMeasure;
        public MusicMeasure NextMusicMeasure
        {
            get
            {
                return _nextMusicMeasure;
            }
            set
            {
                _nextMusicMeasure = value;
                OnPropertyChanged();
            }
        }


        private MusicMeasure _currentMeasure;
        public MusicMeasure CurrentMusicMeasure
        {
            get
            {
                return _currentMeasure;
            }
            set
            {
                _currentMeasure = value;
                OnPropertyChanged();
            }
        }
        private SpotifyService _spotifyService;

        public MainViewModel(IView view)
        {
            _settings = new Properties.DrumTrainer();
            _view = view;
            System.Diagnostics.Debug.WriteLine($"Running on thread {Thread.CurrentThread.ManagedThreadId}");
            _settings = new Properties.DrumTrainer();
            if (!string.IsNullOrWhiteSpace(_settings.MidiSettings))
            {
                DrumMappingSetting = JsonConvert.DeserializeObject<DrumMappingSetting>(_settings.MidiSettings);
            }
            else
            {
                DrumMappingSetting = DrumMappingSetting.CreateNew();
            }
            InitializeCollections();

            SelectedSpotifyDeviceId = _settings.LatestSpotifyDevice;

            InitializeCommands();
            _spotifyService = new SpotifyService();
            _spotifyService.OnRefreshTokenRecieved += async (s, newRefreshToken) =>
            {
                _settings.LastRefreshToken = newRefreshToken;
                _settings.Save();
                SpotifyDevices = (await _spotifyService.GetDevicesAsync()).Devices;
            };
            _spotifyService.StartUp();
            if (!string.IsNullOrWhiteSpace(_settings.LastRefreshToken))
            {
                _spotifyService.TryRefreshToken(_settings.LastRefreshToken);
            }
            Metronome = new Metronome(WpfUIContext.Instance);
            Metronome.AddListener(this);

            Song = new DrumTrainerSong();
            var Measure = new MusicMeasure();

            Measure.AddNote(new Note() { Drum = Drum.OpenHihat, NoteType = NoteType.Eight, Beat = 1, Position = 1 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Eight, Beat = 1, Position = 2 });
            Measure.AddNote(new Note() { Drum = Drum.Snare, NoteType = NoteType.Eight, Beat = 1, Position = 1 });
            Measure.AddNote(new Note() { Drum = Drum.RightBase, NoteType = NoteType.Eight, Beat = 1, Position = 1 });


            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Sixteenth, Beat = 2, Position = 1 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Sixteenth, Beat = 2, Position = 2 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Sixteenth, Beat = 2, Position = 3 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Sixteenth, Beat = 2, Position = 4 });
            Measure.AddNote(new Note() { Drum = Drum.Snare, NoteType = NoteType.Sixteenth, Beat = 2, Position = 1 });


            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Eight, Beat = 3, Position = 1 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.Eight, Beat = 3, Position = 2 });
            Measure.AddNote(new Note() { Drum = Drum.Snare, NoteType = NoteType.Eight, Beat = 3, Position = 1 });

            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 1 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 2 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 3 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 4 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 5 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 6 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 7 });
            Measure.AddNote(new Note() { Drum = Drum.ClosedHihat, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 8 });
            Measure.AddNote(new Note() { Drum = Drum.Snare, NoteType = NoteType.ThirtySecond, Beat = 4, Position = 1 });
            Song.Measures.Add(Measure);
            CurrentNote = Measure.Notes[0];
            BeatsPerMeasure = Measure.Measure;
        }

        private void InitializeCollections()
        {
            FullTimeLineNotes = new ObservableCollection<object>();
            ExpectedNotes = new ObservableCollection<ExpectedNote>();
            AllBeatNotes = new ObservableCollection<BeatNote>();
            CurrentBeatNotes = new ObservableCollection<BeatNote>();
            PlayedNotes = new ObservableCollection<PlayedNote>();
            ExtraHits = new ObservableCollection<PlayedNote>();
            ExpectedNotesPerBeat = new ObservableCollection<ExpectedNotesPerBeat>();
            MidiDevices = new ObservableCollection<MidiInCapabilities>(MidiService.GetMidiDevices());
            SelectedMidiDevice = MidiDevices.FirstOrDefault(b => b.ProductId == _settings.LatestMidiDevice);
        }

        private void InitializeCommands()
        {

            EditMeasureCommand = new AsyncRelayCommand(EditMeasureExecute, CanEditMeasure);
            SaveCommand = new RelayCommand(ExecuteSave);
            OpenCommand = new RelayCommand(OpenSave);
            MidiSettingsCommand = new AsyncRelayCommand(MidiSettingsExecute);
            SelectMidiDeviceCommand = new RelayCommand(SelectMidiDeviceExecute);
            SelectSpotifyDeviceCommand = new RelayCommand(SelectSpotifyDeviceExecute);
            LoginSpotifyCommand = new RelayCommand(LoginSpotifyExecute);
            SelectSpotifySongCommand = new AsyncRelayCommand(SelectSpotifySongExecute, SelectSpotifySongCanExecute);
            DeleteMeasureCommand = new RelayCommand(DeleteMeasureExecute, CanDeleteMeasure);
            AddMeasureCommand = new RelayCommand(AddMeasureExecute);
        }

        private bool CanDeleteMeasure(object obj)
        {
            return Song.Measures.Count > 1;
        }

        private void DeleteMeasureExecute(object arg)
        {
            Song.Measures.Remove(arg as MusicMeasure);
        }

        private void AddMeasureExecute(object arg)
        {
            Song.Measures.Insert(Song.Measures.IndexOf((arg as MusicMeasure)) + 1, new MusicMeasure() { Measure = Metronome.BeatsPerMeasure, RepeatCount = 1, Notes = CopyNotes((arg as MusicMeasure)) });
        }

        private ObservableCollection<Note> CopyNotes(MusicMeasure musicMeasure)
        {
            ObservableCollection<Note> copied = new ObservableCollection<Note>();
            foreach (var note in musicMeasure.Notes)
            {
                copied.Add(new Note() { Beat = note.Beat, NoteType = note.NoteType, Drum = note.Drum, Position = note.Position });
            }
            return copied;
        }

        private async Task SelectSpotifySongExecute(object arg)
        {
            var resultItem = await _view.ShowChildWindowAsync<SpotifyAPI.Web.Models.FullTrack>(new SearchSpotifyView(_spotifyService)).ConfigureAwait(true);
            if (resultItem != null)
            {
                Song.SpotifyTrack = resultItem.Uri;
                Song.SpotifyName = resultItem.Name + " - " + resultItem.Artists[0].Name;
                OnPropertyChanged(nameof(Song));
            }
        }

        private bool SelectSpotifySongCanExecute(object arg)
        {
            return _spotifyService.IsAuthenticated();
        }

        private void LoginSpotifyExecute(object obj)
        {
            _spotifyService.LoginSpotify();
        }

        private async Task MidiSettingsExecute(object arg)
        {
            var midiDevice = SelectedMidiDevice;
            SelectedMidiDevice = new MidiInCapabilities();
            MidiSettingsView view = new MidiSettingsView(midiDevice) { IsModal = true };
            var newMapping = await _view.ShowChildWindowAsync<DrumMappingSetting>(view).ConfigureAwait(true);
            if (newMapping != null)
            {
                StopMidiListener();
                _settings.MidiSettings = JsonConvert.SerializeObject(newMapping);
                _settings.Save();
                DrumMappingSetting = newMapping;
                StartMidiListener();
            }
            SelectedMidiDevice = midiDevice;
        }

        private void SelectMidiDeviceExecute(object obj)
        {
            var selectedMidiDevice = (MidiInCapabilities)obj;
            _settings.LatestMidiDevice = selectedMidiDevice.ProductId;
            _settings.Save();
            SelectedMidiDevice = selectedMidiDevice;
        }

        private void SelectSpotifyDeviceExecute(object obj)
        {
            var selectedSpotifyDevice = (Device)obj;
            _settings.LatestSpotifyDevice = selectedSpotifyDevice.Id;
            _settings.Save();
            SelectedSpotifyDeviceId = selectedSpotifyDevice.Id;
        }


        private void ExecuteSave(object obj)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Song file | *.dts";
            dialog.Title = "Lied opslaan";
            if (dialog.ShowDialog() == true)
            {
                string dialogFile = dialog.FileName;
                if (!dialogFile.EndsWith(".dts", true, CultureInfo.InvariantCulture))
                {
                    dialogFile += ".dts";
                }
                Song.BPM = Metronome.BPM;
                File.WriteAllText(dialogFile, JsonConvert.SerializeObject(Song));
            }
        }

        private void OpenSave(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Song file | *.dts";
            dialog.Title = "Lied openen";
            if (dialog.ShowDialog() == true)
            {
                Song = JsonConvert.DeserializeObject<DrumTrainerSong>(File.ReadAllText(dialog.FileName));
                Metronome.BPM = Song.BPM;
                var met = Metronome;
                Metronome = null;
                Metronome = met;
                OnPropertyChanged(nameof(Song));
            }
        }

        private async Task EditMeasureExecute(object arg)
        {

            EditMeasureView view = new EditMeasureView(arg as MusicMeasure) { IsModal = true };
            var editedBeats = await _view.ShowChildWindowAsync<ObservableCollection<EditableBeat>>(view).ConfigureAwait(true);
            if (editedBeats != null)
            {
                SyncEditableBeatsWithMeasure(editedBeats, (arg as MusicMeasure));
            }
        }

        private void SyncEditableBeatsWithMeasure(ObservableCollection<EditableBeat> editedBeats, MusicMeasure musicMeasure)
        {
            foreach (var editableBeat in editedBeats)
            {

                foreach (var note in musicMeasure.Notes.Where(b => b.Beat == editableBeat.Beat).ToList())
                {
                    musicMeasure.RemoveNote(note);
                }

                foreach (var notesForDrum in editableBeat.NotesForDrum)
                {
                    foreach (var editableNote in notesForDrum.Notes)
                    {
                        if (editableNote.HitOnNote)
                        {
                            musicMeasure.AddNote(new Note() { Drum = notesForDrum.Drum, Beat = editableBeat.Beat, Position = editableNote.Position, NoteType = editableBeat.NoteType });
                        }
                    }
                }

            }
        }

        private bool CanEditMeasure(object arg)
        {
            return true;
        }

        ObservableCollection<MidiInCapabilities> _midiDevices;
        public ObservableCollection<MidiInCapabilities> MidiDevices
        {
            get
            {
                return _midiDevices;
            }
            set
            {
                _midiDevices = value;
                OnPropertyChanged();
            }
        }

        Note _currentNote;
        public Note CurrentNote
        {

            get
            {
                return _currentNote;
            }
            set
            {
                _currentNote = value;
                OnPropertyChanged();
            }
        }

        private int _beatsPerMeasure;
        public int BeatsPerMeasure
        {
            get
            {
                return _beatsPerMeasure;
            }
            set
            {
                _beatsPerMeasure = value;
                foreach (MusicMeasure measure in Song.Measures)
                {
                    measure.Measure = value;
                }
                OnPropertyChanged();
            }
        }

        private Metronome _metronome;
        public Metronome Metronome
        {
            get
            {
                return _metronome;
            }
            set
            {
                _metronome = value;
                OnPropertyChanged();
            }
        }

        int _currentBeat;
        public int CurrentBeat
        {
            get
            {
                return _currentBeat;
            }
            set
            {
                _currentBeat = value;
                OnPropertyChanged();
            }
        }

        int _currentMeasureCount = 1;
        public int CurrentMeasureCount
        {
            get
            {
                return _currentMeasureCount;
            }
            set
            {
                _currentMeasureCount = value;
                OnPropertyChanged();
            }
        }

        int beatCount = 0;
        public void IsBeating(bool tock, int beat, float happendAt)
        {
            if (beatCount == 0)
            {
                if (ShouldUseSpotifyForSong())
                {
                    _spotifyService.StartSongAsync(SelectedSpotifyDeviceId, Song.SpotifyTrack);
                }
            }
            if (tock)
            {
                CurrentMusicMeasure = GetMusicMeasure(CurrentMeasureCount);
                NextMusicMeasure = GetMusicMeasure(CurrentMeasureCount + 1);
                NextPlusOneMusicMeasure = GetMusicMeasure(CurrentMeasureCount + 2);
                CurrentMeasureCount++;
                if (CurrentMusicMeasure == null)
                {
                    Metronome.Stop();
                    return;
                }
            }
            beatCount++;
            CurrentBeat = beat;
            if (beat == 1)
            {
                foreach (BeatNote beatNote in CurrentBeatNotes)
                {
                    AllBeatNotes.Add(beatNote);
                }
                CurrentBeatNotes.Clear();
                ExpectedNotesPerBeat.Clear();
                var expectedNotes = AddExpectedNotesForMeasure(happendAt);
                CreateOrUpdateBeatNotes(expectedNotes);
                TrySyncHits(CurrentBeatNotes);
            }
        }

        private MusicMeasure GetMusicMeasure(int currentMeasureCount)
        {
            return Song.Measures.Where(b =>
            currentMeasureCount >= GetLowestMeasureCount(b, Song) && currentMeasureCount <= GetHighestMeasureCount(b, Song)).SingleOrDefault();
        }

        public static int GetHighestMeasureCount(MusicMeasure measure, DrumTrainerSong song)
        {
            return GetLowestMeasureCount(measure, song) -1 + measure.RepeatCount;
        }

        public static int GetLowestMeasureCount(MusicMeasure measure, DrumTrainerSong song)
        {
            var previousMeasures = song.Measures.Take(song.Measures.IndexOf(measure));
            return previousMeasures.Sum(b => b.RepeatCount) +1;
        }

        private void CreateOrUpdateBeatNotes(List<ExpectedNote> expectedNotes)
        {
            foreach (ExpectedNote expectedNote in expectedNotes)
            {
                CurrentBeatNotes.Add(new BeatNote(expectedNote));
            }
            CurrentBeatNotes.GroupBy(b => b.Beat)
                .Select(b => new ExpectedNotesPerBeat()
                {
                    Beat = b.Key,
                    NotesForBeat = b.ToList().OrderBy(b => Note.GetPosisitionsInThirthySecond(b.ExpectedNote.Note)[0]).ToList()

                })
                .ToList()
                .ForEach(c => ExpectedNotesPerBeat.Add(c));
            TrySyncHits(CurrentBeatNotes);
        }

        private List<ExpectedNote> AddExpectedNotesForMeasure(float happendAt)
        {
            List<ExpectedNote> _expectedNotesForBeat = new List<ExpectedNote>();
            float milisecondsPerPosistion = ((float)60 / ((float)Metronome.BPM)) / 32f;
            foreach (var note in CurrentMusicMeasure.Notes)
            {
                var positionsIn32 = Note.GetPosisitionsInThirthySecond(note);
                float hitAt = (milisecondsPerPosistion * positionsIn32[0]) + (milisecondsPerPosistion * 32f * (note.Beat - 1f));
                float calculatedTimeForNote = happendAt + (hitAt * 1000f);
                var expectedNote = new ExpectedNote() { Drum = note.Drum, ShouldBePlayedAt = calculatedTimeForNote, Beat = note.Beat, Position = note.Position, Note = note };
                ExpectedNotes.Add(expectedNote);
                _expectedNotesForBeat.Add(expectedNote);
            }



            return _expectedNotesForBeat;
        }

        public void HitDrum(Drum drum, float happendAt)
        {
            var playedNote = new PlayedNote() { Drum = drum, HappendAt = happendAt };
            PlayedNotes.Add(playedNote);
        }



        public void TrySyncHits(ObservableCollection<BeatNote> notes)
        {
            foreach (var beatNote in notes.Where(b => !b.Played))
            {
                PlayedNote playedNote = PlayedNotes
                    .Where(b => b.Drum == beatNote.Drum
                    && Math.Abs(beatNote.ShouldBePlayedAt - b.HappendAt) < MaxTimeBetweenExpectedAndHitNote
                    && b.Matched == false)
                    .OrderBy(b=> Math.Abs(beatNote.ShouldBePlayedAt - b.HappendAt))
                    .FirstOrDefault();
                if (playedNote != null)
                {
                    beatNote.Played = true;
                    beatNote.PlayedAt = playedNote.HappendAt;
                    beatNote.PlayedNote = playedNote;
                    playedNote.Matched = true;
                }
            }
            UpdateAllCollections();
        }

        private void UpdateAllCollections()
        {
            PlayedNotes.Where(b => !AllBeatNotes.Union(CurrentBeatNotes).Any(c => c.PlayedNote == b) && !ExtraHits.Contains(b)).ToList().ForEach(b => ExtraHits.Add(b));
            ExtraHits.Where(b => AllBeatNotes.Union(CurrentBeatNotes).Any(c => c.PlayedNote == b)).ToList().ForEach(b => ExtraHits.Remove(b));

            ExtraHitsCount = ExtraHits.Count;
            MissedHitsCount = AllBeatNotes.Union(CurrentBeatNotes).Where(b => !b.Played).Count();
            HitsCount = AllBeatNotes.Union(CurrentBeatNotes).Where(b => b.Played).Count();
            if (HitsCount != 0)
            {
                GoodPercentage = (MissedHitsCount / (HitsCount + MissedHitsCount)) * 100f;
            }
        }

        private void SyncCompleteTimeLine()
        {
            FullTimeLineNotes.Clear();
            AllBeatNotes.Where(b => !FullTimeLineNotes.Contains(b)).ToList().ForEach(b => FullTimeLineNotes.Add(b));
            ExtraHits.Where(b => !FullTimeLineNotes.Contains(b)).ToList().ForEach(b => FullTimeLineNotes.Add(b));
            FullTimeLineNotes = new ObservableCollection<object>(FullTimeLineNotes.OrderBy(b => (b is PlayedNote) ? (b as PlayedNote).HappendAt : (b as BeatNote).ShouldBePlayedAt).ToList());

        }

        public void Started()
        {
            beatCount = 0;
            WpfUIContext.Instance.ResetTimer();
            CurrentMeasureCount = 1;
            GoodPercentage = 0;
            HitsCount = 0;
            ExtraHitsCount = 0;
            MissedHitsCount = 0;

            FullTimeLineNotes.Clear();
            PlayedNotes.Clear();
            ExpectedNotes.Clear();
            ExtraHits.Clear();

        }

        private bool ShouldUseSpotifyForSong()
        {
            return _spotifyService.IsAuthenticated() && !string.IsNullOrWhiteSpace(Song.SpotifyTrack) && !string.IsNullOrWhiteSpace(SelectedSpotifyDeviceId);
        }

        public ObservableCollection<BeatNote> CurrentBeatNotes { get; private set; }

        ObservableCollection<object> _fullTimeLineNotes;
        public ObservableCollection<object> FullTimeLineNotes
        {
            get
            {
                return _fullTimeLineNotes;
            }
            private set
            {
                _fullTimeLineNotes = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ExpectedNote> ExpectedNotes { get; private set; }
        public ObservableCollection<BeatNote> AllBeatNotes { get; private set; }
        public ObservableCollection<PlayedNote> PlayedNotes { get; set; }

        private ObservableCollection<PlayedNote> _extraHits;
        public ObservableCollection<PlayedNote> ExtraHits
        {
            get
            {
                return _extraHits;
            }
            set
            {
                _extraHits = value;
                OnPropertyChanged();
            }
        }

        MidiInCapabilities _selectedMidiDevice;
        public MidiInCapabilities SelectedMidiDevice
        {
            get
            {
                return _selectedMidiDevice;
            }
            set
            {
                _selectedMidiDevice = value;
                OnPropertyChanged();
                if (_currentMidiService != null)
                {
                    StopMidiListener();
                    _currentMidiService.Stop();
                    _currentMidiService = null;
                }
                if (value.ProductId != 0)
                {
                    _currentMidiService = new MidiService(value, WpfUIContext.Instance);
                    StartMidiListener();
                }

            }
        }

        private void StopMidiListener()
        {
            _midiListener.Stop();
        }

        private void StartMidiListener()
        {
            _midiListener = new MidiServiceListener(_currentMidiService, WpfUIContext.Instance, this, DrumMappingSetting);
        }

        public void Stop()
        {
            if (ShouldUseSpotifyForSong())
            {
                _spotifyService.StopSongAsync(SelectedSpotifyDeviceId);
            }
            foreach (BeatNote beatNote in CurrentBeatNotes)
            {
                AllBeatNotes.Add(beatNote);
            }
            CommandManager.InvalidateRequerySuggested();
            CurrentBeatNotes.Clear();
            TrySyncHits(AllBeatNotes);
            SyncCompleteTimeLine();
        }

        ObservableCollection<ExpectedNotesPerBeat> _expectedNotesPerBeat;
        public ObservableCollection<ExpectedNotesPerBeat> ExpectedNotesPerBeat
        {
            get
            {
                return _expectedNotesPerBeat;
            }
            set
            {
                _expectedNotesPerBeat = value;
                OnPropertyChanged();
            }
        }

        public DrumMappingSetting DrumMappingSetting { get; set; }

        double _goodPercentage;
        public double GoodPercentage
        {
            get
            {
                return _goodPercentage;
            }
            set
            {
                _goodPercentage = value;
                OnPropertyChanged();
            }
        }

        int _hitsCount;
        public int HitsCount
        {
            get
            {
                return _hitsCount;
            }
            set
            {
                _hitsCount = value;
                OnPropertyChanged();
            }
        }

        int _missedHitsCount;
        public int MissedHitsCount
        {
            get
            {
                return _missedHitsCount;
            }
            set
            {
                _missedHitsCount = value;
                OnPropertyChanged();
            }
        }

        int _extraHitsCount;
        public int ExtraHitsCount
        {
            get
            {
                return _extraHitsCount;
            }
            set
            {
                _extraHitsCount = value;
                OnPropertyChanged();
            }
        }

        string _selectedSpotifyDeviceId;
        public string SelectedSpotifyDeviceId
        {
            get
            {
                return _selectedSpotifyDeviceId;
            }
            set
            {
                _selectedSpotifyDeviceId = value;
                OnPropertyChanged();
            }
        }

        List<Device> _spotifyDevices;
        public List<Device> SpotifyDevices
        {
            get
            {
                return _spotifyDevices;
            }
            private set
            {
                _spotifyDevices = value;
                OnPropertyChanged();
            }
        }
    }
}
