using System.Collections.ObjectModel;
using System.Text.Json;

namespace TaskManager.Data.Context
{
    public class ApplicationContext
    {
        public ObservableCollection<Models.Task> Tasks;
        private readonly string _filePath;

        public ApplicationContext(string saveFilePath)
        {
            Tasks = new ObservableCollection<Models.Task>();
            _filePath= saveFilePath;
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (!File.Exists(_filePath))
                return;

            var json = File.ReadAllText(_filePath);
            var loadedTasks = JsonSerializer.Deserialize<List<Models.Task>>(json);

            if (loadedTasks != null && loadedTasks.Any())
            {
                Tasks = new ObservableCollection<Models.Task>(loadedTasks);
  
                var maxId = Tasks.Max(t => t.Id);
                if (maxId >= Models.Task.Counter)
                {
                    Models.Task.Counter = maxId + 1;
                }
            }
        }

        public void SaveChanges()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var json = JsonSerializer.Serialize(Tasks.ToList(), options);
            File.WriteAllText(_filePath, json);
        }
    }
}
