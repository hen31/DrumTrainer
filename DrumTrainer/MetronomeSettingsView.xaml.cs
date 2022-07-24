using DrumTrainer.Core;
using DrumTrainer.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrumTrainer
{
    /// <summary>
    /// Interaction logic for MetronomeSettingsView.xaml
    /// </summary>
    public partial class MetronomeSettingsView : MetroWindow
    {
        public MetronomeSettingsView(Metronome metronome)
        {
            InitializeComponent();
            this.DataContext = new MetronomeSettingsViewModel(metronome);
        }
    }
}
