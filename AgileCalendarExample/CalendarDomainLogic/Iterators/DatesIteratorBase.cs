using AgileCalendarExample.Models.ViewModels;
using System;

namespace AgileCalendarExample.CalendarDomainLogic.Iterators
{
    /// <summary>
    /// Interface of an iterator to obtain dates for the calendar view
    /// </summary>
    public abstract class DatesIteratorBase
    {
        /// <summary>
        /// Current date
        /// </summary>
        protected DateTime currentDate;

        /// <summary>
        /// Calendar builder
        /// </summary>
        protected CalendarBuilder calendarBuilder;

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="startDate">Start date for the iterator</param>
        /// <param name="calendarBuilder">Reference to the builder</param>
        public DatesIteratorBase(DateTime startDate, CalendarBuilder calendarBuilder)
        {
            this.currentDate = startDate;
            this.calendarBuilder = calendarBuilder;
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public abstract bool HasNext { get; }

        /// <summary>
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        public abstract CalendarDateBase ReadNext(CalendarDateBase model);

        /// <summary>
        /// Gets the state of a week for the current date <see cref="currentDate"/>
        /// </summary>
        /// <returns>An enum value which indicates whether the week starts, ends or is current</returns>
        protected PeriodEnum GetCurrentWeekPeriod()
        {
            if (this.currentDate.DayOfWeek == this.calendarBuilder.WeekStart)
                return PeriodEnum.Start;
            else if (this.currentDate.DayOfWeek == this.calendarBuilder.WeekEnd)
                return PeriodEnum.End;
            else
                return PeriodEnum.Current;
        }
    }
}
