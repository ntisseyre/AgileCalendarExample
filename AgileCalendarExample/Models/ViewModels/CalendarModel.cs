using System;
using System.Collections.Generic;

namespace AgileCalendarExample.Models.ViewModels
{
    /// <summary>
    /// Model to draw a calendar
    /// </summary>
    public class CalendarModel
    {
        /// <summary>
        /// List of the names of the days of the week in a custom order
        /// </summary>
        public IList<String> DaysOfTheWeek { get; set; }

        /// <summary>
        /// List of the dates to be displayed in a calendar
        /// </summary>
        public IList<CalendarDateBase> Dates { get; set; }
    }
}