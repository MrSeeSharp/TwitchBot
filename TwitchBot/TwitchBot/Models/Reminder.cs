﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public bool[] IsReminderDay { get; set; }
        public TimeSpan TimeOfEvent { get; set; }
        public int?[] ReminderSeconds { get; set; }
        public int? RemindEveryMin { get; set; }
        public string Message { get; set; }
    }
}
