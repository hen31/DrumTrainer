using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrumTrainer.Controls
{
    public class NoteStickControl : Control
    {

        public const double SOLIDWIDTH = 40;
        static NoteStickControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NoteStickControl), new FrameworkPropertyMetadata(typeof(NoteStickControl)));
        }


        public static readonly DependencyProperty NoteProperty = DependencyProperty.Register(nameof(Note), typeof(Note), typeof(NoteStickControl), new PropertyMetadata(null, NoteChanged));

        private static void NoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NoteStickControl).NoteOrMeasureChanged();
        }

        private void NoteOrMeasureChanged()
        {
            if (Note != null && MusicMeasure != null && MusicMeasure.Notes.Contains(Note))
            {
                if(!Note.IsLowestNote(Note, MusicMeasure))
                {
                    Visibility = Visibility.Collapsed;
                }
                else
                {
                    Visibility = Visibility.Visible;
                }
            }
        }

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


        public static readonly DependencyProperty MusicMeasureProperty = DependencyProperty.Register(nameof(MusicMeasure), typeof(MusicMeasure), typeof(NoteStickControl), new PropertyMetadata(null, MusicMeasureChanged));

        private static void MusicMeasureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NoteStickControl).NoteOrMeasureChanged();
        }

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

    }
}
