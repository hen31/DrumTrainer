using DrumTrainer.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace DrumTrainer
{
    public class WpfUIContext : IUIContext
    {

        private Stopwatch _stopWatch;
        private WpfUIContext()
        {
            _stopWatch = new Stopwatch();
            _stopWatch.Start();
        }

        public void ResetTimer()
        {
            _stopWatch.Restart();
        }

        static WpfUIContext _instance;
        public static WpfUIContext Instance
        {
            get
            {
                if(_instance== null)
                {
                    _instance = new  WpfUIContext();
                }
                return _instance;
            }
        }

        public float GetCurrentElapsedTimeSinceStart()
        {
            return _stopWatch.ElapsedMilliseconds;
        }

        public void BeginInvoke(Action action)
        {
            Application.Current?.Dispatcher?.BeginInvoke(action);
        }
    }
}
