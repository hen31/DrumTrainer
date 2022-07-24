using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DrumTrainer.Controls
{
    public class TimelineControl : Control
    {
        static TimelineControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TimelineControl), new FrameworkPropertyMetadata(typeof(TimelineControl)));
        }

        public TimelineControl()
        {
            SelectNoteCommand = new RelayCommand(NoteSelectedExecute, CanExecuteNoteSelected);
        }

        private bool CanExecuteNoteSelected(object obj)
        {
            return IsInEditMode;
        }

        private void NoteSelectedExecute(object obj)
        {
            CurrentNote = obj as Note;
        }

        public RelayCommand SelectNoteCommand { get; set; }


        public static readonly DependencyProperty CurrentMeasureProperty = DependencyProperty.Register(nameof(CurrentMeasure), typeof(MusicMeasure), typeof(TimelineControl), new PropertyMetadata(null));

        public MusicMeasure CurrentMeasure
        {
            get
            {
                return (MusicMeasure)GetValue(CurrentMeasureProperty);
            }
            set
            {
                SetValue(CurrentMeasureProperty, value);
            }
        }

        public static readonly DependencyProperty MusicMeasureProperty = DependencyProperty.Register(nameof(MusicMeasure), typeof(MusicMeasure), typeof(TimelineControl), new PropertyMetadata(null));

        public MusicMeasure MusicMeasure
        {
            get
            {
                return (MusicMeasure)GetValue(MusicMeasureProperty);
            }
            set
            {
                SetValue(MusicMeasureProperty, value);
            }
        }


        public static readonly DependencyProperty SongProperty = DependencyProperty.Register(nameof(Song), typeof(DrumTrainerSong), typeof(TimelineControl), new PropertyMetadata(null));

        public DrumTrainerSong Song
        {
            get
            {
                return (DrumTrainerSong)GetValue(SongProperty);
            }
            set
            {
                SetValue(SongProperty, value);
            }
        }
        
        public static readonly DependencyProperty CurrentMeasureCountProperty = DependencyProperty.Register(nameof(CurrentMeasureCount), typeof(int), typeof(TimelineControl), new PropertyMetadata(0));

        public int CurrentMeasureCount
        {
            get
            {
                return (int)GetValue(CurrentMeasureCountProperty);
            }
            set
            {
                SetValue(CurrentMeasureCountProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentBeatProperty = DependencyProperty.Register(nameof(CurrentBeat), typeof(int), typeof(TimelineControl), new PropertyMetadata(0));

        public int CurrentBeat
        {
            get
            {
                return (int)GetValue(CurrentBeatProperty);
            }
            set
            {
                SetValue(CurrentBeatProperty, value);
            }
        }


        public static readonly DependencyProperty CurrentNoteProperty = DependencyProperty.Register(nameof(CurrentNote), typeof(Note), typeof(TimelineControl), new PropertyMetadata(null));

        public Note CurrentNote
        {
            get
            {
                return (Note)GetValue(CurrentNoteProperty);
            }
            set
            {
                SetValue(CurrentNoteProperty, value);
            }
        }



        public static readonly DependencyProperty PlayingProperty = DependencyProperty.Register(nameof(Playing), typeof(bool), typeof(TimelineControl), new PropertyMetadata(false));

        public bool Playing
        {
            get
            {
                return (bool)GetValue(PlayingProperty);
            }
            set
            {
                SetValue(PlayingProperty, value);
            }
        }

        public static readonly DependencyProperty BeatsPerMeasureProperty = DependencyProperty.Register(nameof(BeatsPerMeasure), typeof(int), typeof(TimelineControl),
    new PropertyMetadata(0));

        public int BeatsPerMeasure
        {
            get
            {
                return (int)GetValue(BeatsPerMeasureProperty);
            }
            set
            {
                SetValue(BeatsPerMeasureProperty, value);
            }
        }


        public static readonly DependencyProperty IsInEditModeProperty = DependencyProperty.Register(nameof(IsInEditMode), typeof(bool), typeof(TimelineControl), new PropertyMetadata(false));

        public bool IsInEditMode
        {
            get
            {
                return (bool)GetValue(IsInEditModeProperty);
            }
            set
            {
                SetValue(IsInEditModeProperty, value);
            }
        }


        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(ICollection<Note>), typeof(TimelineControl), new PropertyMetadata(null));

        public ICollection<Note> Notes
        {
            get
            {
                return (ICollection<Note>)GetValue(NotesProperty);
            }
            set
            {
                SetValue(NotesProperty, value);
            }
        }





    }
}
