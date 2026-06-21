using System.Windows;
using System.Windows.Input;
using TaskManager.Data.Enums;
using TaskManager.Data.Services;

namespace TaskManager.Views
{
    /// <summary>
    /// Логика взаимодействия для TaskModView.xaml
    /// </summary>
    public partial class TaskModView : Window
    {
        private readonly TaskService _service;
        private Data.Models.Task _task;
        private readonly bool _isEdit;

        public TaskModView(TaskService service, Data.Models.Task? taskToEdit = null)
        {
            InitializeComponent();
            _service = service;

            _task = taskToEdit ?? new Data.Models.Task();
            _isEdit = taskToEdit != null;

            Title = _isEdit ? "Редактирование задачи" : "Новая задача";

            DataContext = _task;
            FillFields();
        }

        private void FillFields()
        {
            PriorityField.ItemsSource = Enum.GetValues(typeof(Priority));
            PriorityField.SelectedItem = _isEdit ? _task.Priority : Priority.Low;

            StatusField.ItemsSource = Enum.GetValues(typeof(Status));
            StatusField.SelectedItem = _isEdit ? _task.Status : Status.New;

            TxtName.Text = _task.Name;
            TxtDescription.Text = _task.Description;
            DpDate.SelectedDate = _task.Date;
            ChkImportant.IsChecked = _task.IsImportant;
        }

        private void BtnOk_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveTask();
        }

        private void SaveTask()
        {
            _task.Name = TxtName.Text.Trim();
            _task.Description = TxtDescription.Text.Trim();
            _task.Priority = (Priority)PriorityField.SelectedItem;
            _task.Status = (Status)StatusField.SelectedItem;
            _task.Date = DpDate.SelectedDate ?? DateTime.Now.AddDays(1);
            _task.IsImportant = ChkImportant.IsChecked == true;

            if (string.IsNullOrWhiteSpace(_task.Name))
            {
                MessageBox.Show("Название задачи обязательно!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool res = _isEdit ? _service.Update(_task) : _service.Add(_task);

            if (res)
            {
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Не удалось сохранить задачу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

