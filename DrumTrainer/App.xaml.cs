using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DrumTrainer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
         /*   if(MidiIn.NumberOfDevices==1)
            {
               var midiIn = new MidiIn(0);
                midiIn.MessageReceived += midiIn_MessageReceived;
                midiIn.ErrorReceived += midiIn_ErrorReceived;
                midiIn.Start();
            }*/
        }

      /*  private void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
        }

        private void midiIn_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            if(e.MidiEvent.CommandCode != MidiCommandCode.TimingClock)
            {

            }
        }*/
    }
}
