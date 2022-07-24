using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace DrumTrainer.Converters
{
    class ElapsedTimeToStringConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string format = @"{0:mm\:ss\.fff}";
            if (value is double doubleValue )
            {
                return string.Format(CultureInfo.InvariantCulture, format, new TimeSpan(0, 0, 0, 0, (int)doubleValue));
            }
            else if(value is int intValue)
            {
                return string.Format(CultureInfo.InvariantCulture, format, new TimeSpan(0, 0, 0, 0, intValue));
            }
            else if (value is float floatValue)
            {
                return string.Format(CultureInfo.InvariantCulture, format, new TimeSpan(0, 0, 0, 0, (int)floatValue));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        ElapsedTimeToStringConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new ElapsedTimeToStringConverter();
            return _converter;
        }
    }
}