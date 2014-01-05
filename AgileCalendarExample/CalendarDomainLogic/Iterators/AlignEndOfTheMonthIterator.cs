using AgileCalendarExample.Models.ViewModels;
using System;

namespace AgileCalendarExample.CalendarDomainLogic.Iterators
{
    /// <summary>
    /// Iterator to get empty dates to align the end of the week when a month ends
    /// </summary>
    public class AlignEndOfTheMonthIterator : DatesIteratorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startDate">Start date for the iterator</param>
        /// <param name="calendarBuilder">Reference to the builder</param>
        public AlignEndOfTheMonthIterator(DateTime startDate, CalendarBuilder calendarBuilder)
            :base(startDate, calendarBuilder)
        {
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
            model.WeekPeriod = this.GetCurrentWeekPeriod();
            model.IsNewMonth = false;

            this.currentDate = currentDate.AddDays(1);
            return model;
        }
    }
}