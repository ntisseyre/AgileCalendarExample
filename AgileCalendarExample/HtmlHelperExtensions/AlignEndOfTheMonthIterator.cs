using AgileCalendarExample.Models.View;
using System;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// Iterator to get empty dates to align the end of the week
    /// </summary>
    public class AlignEndOfTheMonthIterator : IDatesIterator
    {
        /// <summary>
        /// Current date
        /// </summary>
        private DateTime currentDate;

        public AlignEndOfTheMonthIterator(DateTime startDate)
        {
            this.currentDate = startDate;
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public bool HasNext
        {
            get
            {
                return AgileCalendarHtmlHelper.GetWeekPeriod(this.currentDate) != PeriodEnum.Start;
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
                WeekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.currentDate),
                IsNewMonth = false
            };

            this.currentDate = currentDate.AddDays(1);
            return result;
        }
    }
}