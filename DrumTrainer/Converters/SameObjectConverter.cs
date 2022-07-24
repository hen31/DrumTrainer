using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DrumTrainer.Converters
{
    public class SameObjectConverter : MarkupExtension, IMultiValueConverter
    {


        SameObjectConverter _converter;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new SameObjectConverter();
            return _converter;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(Visibility))
            {

                if (values[0].Equals(values[1]))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            else
            {
                if (values[0].Equals(values[1]))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
