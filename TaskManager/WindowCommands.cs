using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using TaskManager.Views;

namespace TaskManager
{
    public class WindowCommands
    {
        public static RoutedCommand TaskOk { get; set; }
        public static RoutedCommand TaskCancel { get; set; }
        public static RoutedCommand NewTask { get; set; }
        public static RoutedCommand EditTask { get; set; }
        public static RoutedCommand DeleteTask { get; set; }
        public static RoutedCommand ShowStatistics { get; set; }

        static WindowCommands()
        {
            TaskOk = new RoutedCommand("TaskOk", typeof(TaskModView));
            TaskCancel = new RoutedCommand("TaskCancel", typeof(TaskModView));
            NewTask = new RoutedCommand("NewTask", typeof(TasksView));
            ShowStatistics = new RoutedCommand("ShowStatistics", typeof(TasksView));
            EditTask = new RoutedCommand("EditTask", typeof(TasksView));
            DeleteTask = new RoutedCommand("DeleteTask", typeof(TasksView));
        }
    }
}
