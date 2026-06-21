using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManager.Data.Models
{
    public class TaskStatistics
    {
        public int TotalTasks { get; set; }
        public int New { get; set; }
        public int InProgress { get; set; }
        public int Completed { get; set; }
        public int Important { get; set; }
        public int Expired { get; set; }
    }
}
