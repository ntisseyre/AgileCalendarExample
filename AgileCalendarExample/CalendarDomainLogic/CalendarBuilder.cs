using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.CalendarDomainLogic.Iterators;
using AgileCalendarExample.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AgileCalendarExample.CalendarDomainLogic
{
    /// <summary>
    /// Class that builds a calendar model
    /// </summary>
    public class CalendarBuilder
    {
        /// <summary>
        /// The first day of the week
        /// </summary>
        private DayOfWeek weekStart;

        /// <summary>
        /// The last day of the week
        /// </summary>
        private DayOfWeek weekEnd;

        /// <summary>
        /// Custom factory to generate dates' models for the calendar
        /// </summary>
        private CalendarDateFactoryBase datesFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="weekStartsFrom">The first day of the week</param>
        /// <param name="datesFactory">Custom factory to generate dates' models</param>
        public CalendarBuilder(DayOfWeek weekStartsFrom, CalendarDateFactoryBase datesFactory)
        {
            this.weekStart = weekStartsFrom;
            this.weekEnd = (DayOfWeek)(this.weekStart == DayOfWeek.Sunday ? 6 : (int)this.weekStart - 1);
            this.datesFactory = datesFactory;
        }

        /// <summary>
        /// Build a model for the calendar
        /// </summary>
        /// <returns>Calendar's model</returns>
        public CalendarModel Build()
        {
            return new CalendarModel()
            {
                DaysOfTheWeek = this.BuildDaysOfTheWeek(),
                Dates = this.BuildDates()
            };
        }

        /// <summary>
        /// Build a list of the days of the week
        /// </summary>
        /// <returns>A list of the days of the week in a custom order</returns>
        public IList<String> BuildDaysOfTheWeek()
        {
            IList<String> result = new List<String>(7);

            int dayIndex = (int)this.weekStart;
            for (int c = 0; c < 7; c++)
            {
                DayOfWeek dayOfWeek = (DayOfWeek)dayIndex;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        result.Add(Resources.Monday);
                        break;
                    case DayOfWeek.Tuesday:
                        result.Add(Resources.Tuesday);
                        break;
                    case DayOfWeek.Wednesday:
                        result.Add(Resources.Wednesday);
                        break;
                    case DayOfWeek.Thursday:
                        result.Add(Resources.Thursday);
                        break;
                    case DayOfWeek.Friday:
                        result.Add(Resources.Friday);
                        break;
                    case DayOfWeek.Saturday:
                        result.Add(Resources.Saturday);
                        break;
                    default:
                        result.Add(Resources.Sunday);
                        break;
                }

                if (++dayIndex == 7)
                    dayIndex = 0;
            }

            return result;
        }

        /// <summary>
        /// Gets all the dates for the calendar
        /// </summary>
        /// <returns>A list of date's models</returns>
        public IList<CalendarDateBase> BuildDates()
        {
            IList<CalendarDateBase> result = new List<CalendarDateBase>();

            DateTime startDate = this.datesFactory.StartDate;
            DateTime endDate = this.datesFactory.EndDate;
            MonthPeriodIterator monthIterator = new MonthPeriodIterator(startDate, endDate, this);

            DatesIteratorBase alignIterator;
            while (monthIterator.HasNext)
            {
                alignIterator = new AlignStartOfTheMonthIterator(monthIterator.CurrentDate, this);
                while (alignIterator.HasNext)
                    result.Add(alignIterator.ReadNext(this.datesFactory.GetEmptyViewModel()));

                monthIterator.IsNewMonth = false;
                while (!monthIterator.IsNewMonth && monthIterator.HasNext)
                    result.Add(monthIterator.ReadNext(this.datesFactory.GetCalendarDate(monthIterator.CurrentDate)));

                alignIterator = new AlignEndOfTheMonthIterator(monthIterator.CurrentDate, this);
                while (alignIterator.HasNext)
                    result.Add(alignIterator.ReadNext(this.datesFactory.GetEmptyViewModel()));
            }

            return result;
        }

        /// <summary>
        /// The first day of the week
        /// </summary>
        public DayOfWeek WeekStart
        { get { return this.weekStart; } }

        /// <summary>
        /// The last day of the week
        /// </summary>
        public DayOfWeek WeekEnd
        { get { return this.weekEnd; } }
    }
}