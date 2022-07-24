using DrumTrainer.Core;
using DrumTrainer.Core.Entities;
using DrumTrainer.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
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
    public partial class EditMeasureView : ChildWindow, IChildView
    {
        public EditMeasureView(MusicMeasure measure)
        {
            InitializeComponent();
            this.DataContext = new EditMeasureViewModel(measure, this);
        }

        void IChildView.Close(object result)
        {
            (this as ChildWindow).Close(result);
        }
    }
}
