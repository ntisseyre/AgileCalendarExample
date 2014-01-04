using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models.View
{
    public abstract class CalendarDateBase
    {
        public DateTime Date { get; set; }

        public PeriodEnum WeekPeriod { get; set; }

        public bool IsNewMonth { get; set; }
    }
}