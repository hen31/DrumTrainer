using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace DrumTrainer.Controls
{
    public static class DrumTrainerControlSetting
    {
        public const double NoteBarWidth = 40f;
        public const double NoteStickHeight = 35f;

        public static Color SelectedNoteColor { get; internal set; } = Colors.Blue;
    }
}
