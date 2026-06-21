using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using TaskManager.Data.Enums;

namespace TaskManager.Data.Models
{
    public class Task
    {
        public static int Counter = 0;
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public Priority Priority { get; set; } = Priority.Low;
        public DateTime Date { get; set; } = DateTime.Now.AddDays(1);
        public Status Status { get; set; } = Status.New;
        public bool IsImportant { get; set; } = false;

        public Task()
        {
            Id = ++Counter;
        }
    }
}
