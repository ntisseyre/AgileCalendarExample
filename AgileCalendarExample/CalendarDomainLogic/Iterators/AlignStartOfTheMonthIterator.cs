using AgileCalendarExample.Models.ViewModels;
using System;

namespace AgileCalendarExample.CalendarDomainLogic.Iterators
{
    /// <summary>
    /// Iterator to get empty dates to align the beginning of the week when a new month starts
    /// </summary>
    public class AlignStartOfTheMonthIterator : DatesIteratorBase
    {
        /// <summary>
        /// Flag indicates the start of a week.
        /// Is used because internally iteration happens in a reverse order.
        /// </summary>
        private bool isStart;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startDate">A date before which empty cells should be inserted to start a new week</param>
        /// <param name="calendarBuilder">Reference to the builder</param>
        public AlignStartOfTheMonthIterator(DateTime startDate, CalendarBuilder calendarBuilder)
            :base(startDate, calendarBuilder)
        {
            this.isStart = true;
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public override bool HasNext
        {
            get
            {
                return this.GetCurrentWeekPeriod() != PeriodEnum.Start;
            }
        }

        /// <summary>
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        public override CalendarDateBase ReadNext(CalendarDateBase model)
        {
            model.Date = this.currentDate;
            model.WeekPeriod = this.isStart ? PeriodEnum.Start : PeriodEnum.Current;
            model.IsNewMonth = this.isStart;

            this.currentDate = currentDate.AddDays(-1);
            this.isStart = false;

            return model;
        }
    }
}