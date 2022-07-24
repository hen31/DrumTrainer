using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrumTrainer
{
    public class DrumMappingSetting
    {
        public DrumMappingSetting()
        {
            DrumMapping = new List<DrumNoteBinding>();
        }

        public static DrumMappingSetting CreateNew()
        {
            DrumMappingSetting drumMappingSetting = new DrumMappingSetting();
            foreach(Drum value in Enum.GetValues(typeof(Drum)))
            {
                drumMappingSetting.DrumMapping.Add(new DrumNoteBinding() { Drum = value, Key = 0 });
            }
            return drumMappingSetting;
        }
        public string DrumName { get; set; }

        public List<DrumNoteBinding> DrumMapping { get; set; }

        public Drum this[int index]
        {
            get
            {
                return DrumMapping.First(b => b.Key == index).Drum;
            }
        }

        internal bool ContainsNote(int noteNumber)
        {
            return DrumMapping.Any(b => b.Key == noteNumber);
        }
    }

    public class DrumNoteBinding
    {
        public Drum Drum { get; set; }
        public int Key { get; set; }
    }
}
