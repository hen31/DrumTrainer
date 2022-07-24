using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DrumTrainer.Converters
{
    public class IsLastNoteInBeatConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 2 && values[0] is Note note && values[1] is MusicMeasure measure)
            {
                if (Note.IsLastNoteOfBeat(note, measure))
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
        IsLastNoteInBeatConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new IsLastNoteInBeatConverter();
            return _converter;
        }
    }
}
