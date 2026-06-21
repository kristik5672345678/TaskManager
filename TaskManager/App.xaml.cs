using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using TaskManager.Data.Context;
using TaskManager.Data.Services;
using TaskManager.Views;

namespace TaskManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IConfiguration Config;

        private ApplicationContext _context;
        public TaskService service { get; set; }

        public App()
        {
            SetConfig();
            var path = Config["StorageFilePath"];
            _context = new ApplicationContext(path);
            service = new TaskService(_context);

            TasksView orgsView = new TasksView(service);
            orgsView.Show();
        }

        private void SetConfig()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("config.json")
                .Build();
        }
    }

}
