using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace DrumTrainer
{
    public class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            KeepActive = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool KeepActive { get; set; }

        public Dispatcher Dispatcher
        {
            get
            {
                return Application.Current.Dispatcher;
            }
        }
    }
}
