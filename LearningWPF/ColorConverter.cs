using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LearningWPF
{
    public class SymbolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string symbol)
            {
                return symbol switch
                {
                    // Стена
                    "#" => new SolidColorBrush(Colors.Black),
                    // Игрок
                    "P" => new SolidColorBrush(Colors.Blue),
                    // Сокровище
                    "X" => new SolidColorBrush(Colors.Green),
                    " " => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.White),// Если символ не соответствует ни одному из указанных, вернуть белый цвет
                };
            }

            return Binding.DoNothing; // Если value не является строкой, ничего не делать
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
