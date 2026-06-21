using System.Globalization;
using System.Windows.Data;
using TaskManager.Data.Enums;

namespace TaskManager.Helpers
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Status status)
            {
                return status switch
                {
                    Status.New => "Новая",
                    Status.InProgress => "В процессе",
                    Status.Completed => "Завершена",
                    _ => status.ToString()
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
