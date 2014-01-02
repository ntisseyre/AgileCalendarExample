using AgileCalendarExample.Models.View;
using System;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// Iterator to get empty dates to align the end of the week
    /// </summary>
    public class AlignEndOfTheMonthIterator : IDatesIterator
    {
        private DateTime date;
        private PeriodEnum weekPeriod;

        public AlignEndOfTheMonthIterator(DateTime startDate)
        {
            this.date = startDate;
            this.weekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(startDate);
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public bool HasNext
        {
            get
            {
                return this.weekPeriod != PeriodEnum.Start;
            }
        }

        /// <summary>
        /// Gets the next date for the calendar and shifts the pointer
        /// </summary>
        /// <returns>Date for the calendar</returns>
        public AgileDate Next()
        {
            AgileDate result = new AgileDate
            {
                IsEmpty = true,
                WeekPeriod = this.weekPeriod,
                IsNewMonth = false
            };

            this.date = date.AddDays(1);
            this.weekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.date);

            return result;
        }
    }
}