using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskManager.Data.Enums;
using TaskManager.Data.Services;

namespace TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для TasksView.xaml
    /// </summary>
    public partial class TasksView : Window
    {
        private TaskService _service;

        private string _curSearchText = "";
        private Status? _curStatusFilter = null;
        private Priority? _curPriorityFilter = null;
        private DateTime? _curDateFilter = null;

        public TasksView()
        {
            InitializeComponent();
        }

        public TasksView(TaskService service)
        {
            InitializeComponent();

            _service = service;

            FillCollection();
        }

        private void FillCollection(bool applyFilters = false)
        {
            int idx = 0;
            if (DgTasks.SelectedIndex > 0)
                idx = DgTasks.SelectedIndex;

            DgTasks.ItemsSource = null;
            DgTasks.Items.Clear();
            DgTasks.ItemsSource = !applyFilters ? _service.Get()
                : _service.Get(null, _curSearchText, _curStatusFilter, _curPriorityFilter, _curDateFilter);

            if (DgTasks.Items.Count > 0)
            {
                try
                {
                    DgTasks.SelectedIndex = idx;
                }
                catch
                {
                    DgTasks.SelectedIndex = -1;
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _curSearchText = TxtSearch.Text;
            FillCollection(true);
        }

        private void StatusFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)StatusFilter.SelectedItem;

            if (selectedItem != null)
            {
                _curStatusFilter = selectedItem.Content.ToString() switch
                {
                    "Новые" => Status.New,
                    "В процессе" => Status.InProgress,
                    "Завершены" => Status.Completed,
                    _ => null
                };

                FillCollection(true);
            }
        }

        private void PriorityFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)PriorityFilter.SelectedItem;

            if (selectedItem != null)
            {
                _curPriorityFilter = selectedItem.Content.ToString() switch
                {
                    "Низкий" => Priority.Low,
                    "Высокий" => Priority.High,
                    "Средний" => Priority.Medium,
                    _ => null
                };

                FillCollection(true);
            }
        }

        private void DateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _curDateFilter = DateFilter.SelectedDate;
            FillCollection(true);
        }

        private void DgTasks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedTask();
        }

        private void EditTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditSelectedTask();
        }

        private void DeleteTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (DgTasks.SelectedItem is Data.Models.Task selectedTask)
            {
                var res = _service.Delete(selectedTask.Id);
                if (res)
                {
                    FillCollection(true);
                }
            }
        }

        private void NewTask_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var addWindow = new TaskModView(_service);
            if (addWindow.ShowDialog() == true)
            {
                FillCollection(true);
            }
        }

        private void EditSelectedTask()
        {
            if (DgTasks.SelectedItem is Data.Models.Task selectedTask)
            {
                var editWindow = new TaskModView(_service, selectedTask);
                if (editWindow.ShowDialog() == true)
                {
                    FillCollection(true);
                }
            }
        }

        private void ShowStatistics_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var stats = _service.GetStatistics();

            string message = $"Статистика задач\n\n" +
                             $"Всего задач: {stats.TotalTasks}\n\n" +
                             $"Новые: {stats.New}\n" +
                             $"В процессе: {stats.InProgress}\n" +
                             $"Выполнены: {stats.Completed}\n\n" +
                             $"Важные: {stats.Important}\n" +
                             $"Просроченно: {stats.Expired}\n\n";

            MessageBox.Show(message, "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
