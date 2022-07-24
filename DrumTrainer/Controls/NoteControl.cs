using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrumTrainer.Controls
{
    public class NoteControl : Control
    {
        static NoteControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NoteControl), new FrameworkPropertyMetadata(typeof(NoteControl)));
        }


        public NoteControl()
        {

        }

        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(nameof(Note), typeof(Note), typeof(NoteControl), new PropertyMetadata(null));

        public Note Note
        {
            get
            {
                return GetValue(NoteProperty) as Note;
            }
            set
            {
                SetValue(NoteProperty, value);
            }
        }


        public static readonly DependencyProperty HideStickProperty = DependencyProperty.Register(nameof(HideStick), typeof(bool), typeof(NoteControl), new PropertyMetadata(false));

        public bool HideStick
        {
            get
            {
                return (bool)GetValue(HideStickProperty);
            }
            set
            {
                SetValue(HideStickProperty, value);
            }
        }
        


        public static readonly DependencyProperty MusicMeasureProperty = DependencyProperty.Register(nameof(MusicMeasure), typeof(MusicMeasure), typeof(NoteControl), new PropertyMetadata(null));

        public MusicMeasure MusicMeasure
        {
            get
            {
                return GetValue(MusicMeasureProperty) as MusicMeasure;
            }
            set
            {
                SetValue(MusicMeasureProperty, value);
            }
        }


        public static readonly DependencyProperty SelectNoteCommandProperty = DependencyProperty.Register(nameof(SelectNoteCommand), typeof(RelayCommand), typeof(NoteControl), new PropertyMetadata(null, CommandChanged));

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public RelayCommand SelectNoteCommand
        {
            get
            {
                return GetValue(SelectNoteCommandProperty) as RelayCommand;
            }
            set
            {
                SetValue(SelectNoteCommandProperty, value);
            }
        }
        
    }
}
