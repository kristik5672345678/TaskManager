using System.Net.NetworkInformation;
using System.Reflection;
using TaskManager.Data.Context;
using TaskManager.Data.Enums;
using TaskManager.Data.Services;
using Xunit;

namespace TaskManager.Test
{
    public class TaskServiceTests
    {
        private ApplicationContext CreateEmptyTestContext()
        {
            string tempPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"test_tasks_{Guid.NewGuid()}.json");

            return new ApplicationContext(tempPath);
        }

        [Fact]
        public void Get_ReturnsEmptyCollection_WhenNoTasks()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var tasks = service.Get();

            Assert.NotNull(tasks);
            Assert.Empty(tasks);
        }

        [Fact]
        public void Add_ValidTask_ReturnsTrue_AndAddsToCollection()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var newTask = new Data.Models.Task
            {
                Name = "Тестовая задача",
                Description = "Описание теста",
                Priority = Priority.High,
                Status = Status.New,
                IsImportant = true
            };

            bool result = service.Add(newTask);

            Assert.True(result);
            Assert.Single(service.Get());
            Assert.Equal("Тестовая задача", service.Get().First().Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Add_InvalidName_ReturnsFalse(string name)
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var task = new Data.Models.Task { Name = name };

            bool result = service.Add(task);

            Assert.False(result);
            Assert.Empty(service.Get());
        }

        [Fact]
        public void Delete_ExistingTask_ReturnsTrue()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var task = new Data.Models.Task { Name = "Задача для удаления" };
            service.Add(task);

            bool result = service.Delete((int)task.Id);

            Assert.True(result);
            Assert.Empty(service.Get());
        }

        [Fact]
        public void Delete_NonExistingTask_ReturnsFalse()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            bool result = service.Delete(9999);

            Assert.False(result);
        }

        [Fact]
        public void Update_ExistingTask_UpdatesSuccessfully()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var task = new Data.Models.Task { Name = "Исходная" };
            service.Add(task);

            task.Name = "Обновлённое название";
            task.Status = Status.Completed;
            task.Priority = Priority.Low;
            task.IsImportant = true;

            bool result = service.Update(task);

            Assert.True(result);

            var updatedTask = service.Get().First();
            Assert.Equal("Обновлённое название", updatedTask.Name);
            Assert.Equal(Status.Completed, updatedTask.Status);
            Assert.Equal(Priority.Low, updatedTask.Priority);
            Assert.True(updatedTask.IsImportant);
        }

        [Fact]
        public void Update_NonExistingTask_ReturnsFalse()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var fakeTask = new Data.Models.Task
            {
                Id = 99999,
                Name = "Несуществующая"
            };

            bool result = service.Update(fakeTask);

            Assert.False(result);
        }

        [Fact]
        public void Get_WithFilters_WorksCorrectly()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            // Подготовка данных
            service.Add(new Data.Models.Task { Name = "Alpha", Status = Status.New, Priority = Priority.Low });
            service.Add(new Data.Models.Task { Name = "Beta Important", Status = Status.InProgress, Priority = Priority.High, IsImportant = true });
            service.Add(new Data.Models.Task { Name = "Gamma", Status = Status.Completed, Priority = Priority.Medium });

            var result = service.Get(
                id: null,
                searchText: "beta",
                status: Status.InProgress,
                priority: Priority.High,
                date: null);

            Assert.Single(result);
            Assert.Contains("Beta Important", result.First().Name);
        }

        [Fact]
        public void GetStatistics_ReturnsCorrectCounts_OnEmptyCollection()
        {
            var context = CreateEmptyTestContext();
            var service = new TaskService(context);

            var stats = service.GetStatistics();

            Assert.Equal(0, stats.TotalTasks);
            Assert.Equal(0, stats.New);
            Assert.Equal(0, stats.InProgress);
            Assert.Equal(0, stats.Completed);
            Assert.Equal(0, stats.Important);
            Assert.Equal(0, stats.Expired);
        }
    }
}
