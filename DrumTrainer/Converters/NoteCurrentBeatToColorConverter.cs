using DrumTrainer.Controls;
using DrumTrainer.Core.Entities;
using DrumTrainer.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace DrumTrainer.Converters
{
    public class NoteCurrentBeatToColorConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 9 && values[0] is Note note && values[1] is int beat && values[2] is bool playing 
                && values[3] is Note currentNote && values[4] is bool isInEditMode 
                && values[5] is int currentBeatCount 
                && values[6] is MusicMeasure currentMeasure
                && values[7] is MusicMeasure measure
                && values[8] is DrumTrainerSong song)
            {
                if (isInEditMode && currentNote == note)
                {
                    return new SolidColorBrush(DrumTrainerControlSetting.SelectedNoteColor);
                }
                else if
                    (measure == currentMeasure  && beat == note.Beat
                    && playing)
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                else
                {
                    return new SolidColorBrush(Colors.Black);
                }
            }
            return new SolidColorBrush(Colors.Black);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        NoteCurrentBeatToColorConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new NoteCurrentBeatToColorConverter();
            return _converter;
        }
    }
}
