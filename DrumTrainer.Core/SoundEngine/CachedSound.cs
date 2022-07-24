using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrumTrainer.Core.SoundEngine
{
    public class CachedSound
    {
        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                // TODO: could add resampling in here if required
                var resampledFile = new WdlResamplingSampleProvider(audioFileReader, 44100);
                WaveFormat = resampledFile.WaveFormat;
                var wholeFile = new List<float>();
                var readBuffer = new float[resampledFile.WaveFormat.SampleRate * resampledFile.WaveFormat.Channels];
                int samplesRead;
                while ((samplesRead = resampledFile.Read(readBuffer, 0, readBuffer.Length)) > 0)
                {
                    wholeFile.AddRange(readBuffer.Take(samplesRead));
                }
                AudioData = wholeFile.ToArray();
            }
        }
    }
}
