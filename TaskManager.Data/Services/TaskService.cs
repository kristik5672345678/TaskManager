using System.Collections.ObjectModel;
using System.Numerics;
using TaskManager.Data.Context;
using TaskManager.Data.Enums;
using TaskManager.Data.Models;

namespace TaskManager.Data.Services
{
    public class TaskService
    {
        private ApplicationContext _context;

        public TaskService(ApplicationContext context)
        {
            if (context != null)
            {
                _context = context;
            }
        }

        public ObservableCollection<Models.Task> Get()
        {
            return _context.Tasks;
        }

        public ObservableCollection<Models.Task> Get(int? id, string searchText, Status? status, Priority? priority, DateTime? date)
        {
            var tasks = _context.Tasks.ToList();

            if (!string.IsNullOrEmpty(searchText))
            {
                tasks = tasks
                    .Where(x => x.Name.ToLower().Contains(searchText.ToLower()) 
                    || (!string.IsNullOrEmpty(x.Description) && x.Description.ToLower().Contains(searchText.ToLower())))
                    .ToList();
            }

            if (id.HasValue)
            {
                tasks = tasks
                    .Where(x => x.Id == id.Value)
                    .ToList();
            }

            if (status.HasValue)
            {
                tasks = tasks
                    .Where(x => x.Status == status.Value)
                    .ToList();
            }

            if (priority.HasValue)
            {
                tasks = tasks
                    .Where(x => x.Priority == priority.Value)
                    .ToList();
            }

            if (date.HasValue)
            {
                tasks = tasks
                    .Where(x => x.Date == date.Value)
                    .ToList();
            }

            return new ObservableCollection<Models.Task>(tasks);
        }

        public bool Add(Models.Task newTask)
        {
            if (newTask == null || string.IsNullOrEmpty(newTask.Name))
            {
                return false;
            }

            _context.Tasks.Add(newTask);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(int id)
        {
            Models.Task? target = _context.Tasks
                .FirstOrDefault(t => t.Id == id);

            if (target == null)
            {
                return false;
            }

            _context.Tasks.Remove(target);
            _context.SaveChanges();
            return true;
        }

        public bool Update(Models.Task task)
        {
            Models.Task? target = _context.Tasks
                .FirstOrDefault(t => t.Id == task.Id);


            if (target == null)
            {
                return false;
            }

            target.Priority = task.Priority;
            target.Description = task.Description;
            target.Status = task.Status;
            target.Date = task.Date;
            target.Name = task.Name;

            _context.SaveChanges();
            return true;
        }

        public TaskStatistics GetStatistics()
        {
            var tasks = _context.Tasks.ToList();

            var stats = new TaskStatistics
            {
                TotalTasks = tasks.Count,
                New = tasks.Count(t => t.Status == Status.New),
                InProgress = tasks.Count(t => t.Status == Status.InProgress),
                Completed = tasks.Count(t => t.Status == Status.Completed),
                Important = tasks.Count(t => t.IsImportant),
                Expired = tasks.Count(t => t.Date.Date < DateTime.Now.Date && t.Status != Status.Completed),
            };

            return stats;
        }
    }
}
