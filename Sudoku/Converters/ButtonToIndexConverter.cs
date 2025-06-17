using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Collections.ObjectModel;

namespace Sudoku.Converters
{
    public class ButtonToIndexConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 3)
            {
                // values[0] = ItemsControl.Tag (внешний ItemsControl)
                // values[1] = текущее значение ячейки (bool)
                // values[2] = DataContext кнопки (родительская коллекция)

                if (values[0] is ObservableCollection<ObservableCollection<bool>> matrix &&
                    values[2] is ObservableCollection<bool> row &&
                    values[1] is bool cellValue)
                {
                    int rowIndex = matrix.IndexOf(row);
                    int colIndex = row.IndexOf(cellValue);

                    return new Tuple<int, int>(rowIndex, colIndex);
                }
            }
            return Tuple.Create(-1, -1); // Если что-то пошло не так
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
