using DrumTrainer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace DrumTrainer.Converters
{
    public class DrumToStickHeightConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((Drum)value)
            {
                case Drum.OpenHihat:
                    return 53f;
                case Drum.ClosedHihat:
                    return 53f;
                case Drum.Snare:
                    return 73f;
                case Drum.RightBase:
                    return 98f;
                default:
                    return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        DrumToStickHeightConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new DrumToStickHeightConverter();
            return _converter;
        }
    }
}
