using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.Models.View
{
    public class AgileDate
    {
        public bool IsEmpty { get; set; }

        public DateTime Date { get; set; }

        public String Color { get; set; }

        public PeriodEnum WeekPeriod { get; set; }

        public bool IsNewMonth { get; set; }
    }
}