using DrumTrainer.Controls;
using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace DrumTrainer.Converters
{
    public class NotePositionConverter : MarkupExtension, IMultiValueConverter
    {
        //-20
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string parameterAsString && values?.Length == 4 && values[0] is Note note && values[1] is double width && values[2] is double height && values[3] is MusicMeasure measure)
            {
                switch (parameterAsString)
                {
                    case "y":
                        return CalculateYOfNote(note, height, measure);
                    case "x":
                        return CalculateXOfNote(note, width, measure);
                }
            }
            return 0;
        }

        private double CalculateXOfNote(Note note, double width, MusicMeasure measure)
        {
            var notesBeforeThisBeatNumberOf = measure.Notes.Where(b => b.Beat < note.Beat).Select(b => "b" + b.Beat.ToString(CultureInfo.InvariantCulture) + string.Join(",", Note.GetPosisitionsInThirthySecond(b))).Distinct().Count();
            var notesAtPosistions = measure.Notes.Where(b => b.Beat == note.Beat).Select(b => string.Join(",", Note.GetPosisitionsInThirthySecond(b))).OrderBy(b => b).Distinct();
            int indexOfNote = notesAtPosistions.ToList().IndexOf(string.Join(",", Note.GetPosisitionsInThirthySecond(note)));
            return (double)((note.Beat * 30) + (notesBeforeThisBeatNumberOf + indexOfNote) * DrumTrainerControlSetting.NoteBarWidth);
        }
        float offset = 0;
        private double CalculateYOfNote(Note note, double height, MusicMeasure measure)
        {
            if (note.Drum == Drum.ClosedHihat)
            {
                return 40 + offset;
            }
            if (note.Drum == Drum.OpenHihat)
            {
                return 38 + offset;
            }
            else if (note.Drum == Drum.Snare)
            {
                return 75 + offset;
            }
            else if (note.Drum == Drum.RightBase)
            {
                return 100 + offset;
            }
            return 0f;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        NotePositionConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new NotePositionConverter();
            return _converter;
        }
    }
}
