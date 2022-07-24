using DrumTrainer.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DrumTrainer.Controls
{
    public class MetronomeControl : Control
    {

        static MetronomeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MetronomeControl), new FrameworkPropertyMetadata(typeof(MetronomeControl)));
        }

        public MetronomeControl()
        {
            StartCommand = new RelayCommand(StartMetronome, CanStartMetronome);
            PauseCommand = new RelayCommand(PauseMetronome, CanPauseMetronome);
            SettingsCommand = new RelayCommand(ShowSettings, CanShowSettings);
        }

        private void ShowSettings(object obj)
        {
            new MetronomeSettingsView(Metronome).ShowDialog();
        }

        private bool CanShowSettings(object obj)
        {
            return Metronome != null && !Metronome.IsPlaying;
        }

        private bool CanPauseMetronome(object obj)
        {
            return Metronome != null && Metronome.IsPlaying;
        }

        private void PauseMetronome(object obj)
        {
            Metronome.Stop();
        }

        private bool CanStartMetronome(object obj)
        {
            return Metronome != null && !Metronome.IsPlaying;
        }

        private void StartMetronome(object obj)
        {
            Metronome.Start();
        }


        public static readonly DependencyProperty SettingsCommandProperty = DependencyProperty.Register(nameof(SettingsCommand), typeof(RelayCommand), typeof(MetronomeControl), new PropertyMetadata(null));

        public RelayCommand SettingsCommand
        {
            get
            {
                return GetValue(SettingsCommandProperty) as RelayCommand;
            }
            set
            {
                SetValue(SettingsCommandProperty, value);
            }
        }

        public static readonly DependencyProperty PauseCommandProperty = DependencyProperty.Register(nameof(PauseCommand), typeof(RelayCommand), typeof(MetronomeControl), new PropertyMetadata(null));

        public RelayCommand PauseCommand
        {
            get
            {
                return GetValue(PauseCommandProperty) as RelayCommand;
            }
            set
            {
                SetValue(PauseCommandProperty, value);
            }
        }

        public static readonly DependencyProperty StartCommandProperty = DependencyProperty.Register(nameof(StartCommand), typeof(RelayCommand), typeof(MetronomeControl), new PropertyMetadata(null));

        public RelayCommand StartCommand
        {
            get
            {
                return GetValue(StartCommandProperty) as RelayCommand;
            }
            set
            {
                SetValue(StartCommandProperty, value);
            }
        }

        public static readonly DependencyProperty MetronomeProperty = DependencyProperty.Register(nameof(Metronome), typeof(Metronome), typeof(MetronomeControl),
            new PropertyMetadata(null, MetronomeChanged));

        private static void MetronomeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                (d as MetronomeControl).BPM = (e.NewValue as Metronome).BPM;
            }
            //  (d as MetronomeControl).BeatsPerMeasure = (e.NewValue as Metronome).BeatsPerMeasure;
        }

        public Metronome Metronome
        {
            get
            {
                return GetValue(MetronomeProperty) as Metronome;
            }
            set
            {
                SetValue(MetronomeProperty, value);
            }
        }

        public static readonly DependencyProperty BPMProperty = DependencyProperty.Register(nameof(BPM), typeof(int), typeof(MetronomeControl),
            new PropertyMetadata(0, BPMChanged));

        private static void BPMChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MetronomeControl).Metronome.BPM = (int)e.NewValue;
            if ((d as MetronomeControl).Metronome.IsPlaying)
            {
                (d as MetronomeControl).Metronome.Stop();
                (d as MetronomeControl).Metronome.Start();
            }
        }

        public int BPM
        {
            get
            {
                return (int)GetValue(BPMProperty);
            }
            set
            {
                SetValue(BPMProperty, value);
            }
        }



        public static readonly DependencyProperty BeatsPerMeasureProperty = DependencyProperty.Register(nameof(BeatsPerMeasure), typeof(int), typeof(MetronomeControl),
            new PropertyMetadata(0, BeatsPerMeasureChanged));

        private static void BeatsPerMeasureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as MetronomeControl).Metronome.BeatsPerMeasure = (int)e.NewValue;
            if ((d as MetronomeControl).Metronome.IsPlaying)
            {
                (d as MetronomeControl).Metronome.Stop();
                (d as MetronomeControl).Metronome.Start();
            }
        }

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



    }
}
