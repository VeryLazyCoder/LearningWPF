using System;
using System.Globalization;
using System.Windows.Data;

namespace LearningWPF
{
    public class MapIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int targetValue = System.Convert.ToInt32(parameter);
            return (int)value == targetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return System.Convert.ToInt32(parameter);
            }
            return Binding.DoNothing;
        }
    }
}
