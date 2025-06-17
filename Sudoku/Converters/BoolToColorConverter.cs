using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Sudoku.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        // Конвертация из bool в Brush (цвет)
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Brushes.Green : Brushes.Red;
            }
            return Brushes.Gray; // Значение по умолчанию, если значение не bool
        }

        // Обратная конвертация (в данном случае не нужна)
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
