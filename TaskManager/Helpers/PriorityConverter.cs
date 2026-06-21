using System.Globalization;
using System.Windows.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Helpers
{
    class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Priority priority)
            {
                return priority switch
                {
                    Priority.Low => "Низкий",
                    Priority.Medium => "Средний",
                    Priority.High => "Высокий",
                    _ => priority.ToString()
                };
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
