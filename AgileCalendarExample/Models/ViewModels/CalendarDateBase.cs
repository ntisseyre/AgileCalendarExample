using System;

namespace AgileCalendarExample.Models.ViewModels
{
    /// <summary>
    /// An abstract class for the calendar's date
    /// </summary>
    public abstract class CalendarDateBase
    {
        /// <summary>
        /// Current date in the calendar
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Week period for the current date.
        /// Is used in a view to distinguish between the beginning and the end of the week.
        /// </summary>
        public PeriodEnum WeekPeriod { get; set; }

        /// <summary>
        /// Flag indicates the beginnning of a new month
        /// </summary>
        public bool IsNewMonth { get; set; }

        /// <summary>
        /// An optional value.
        /// If not null, the specified template will be used
        /// </summary>
        public String DisplayTemplateHint { get; set; }
    }
}