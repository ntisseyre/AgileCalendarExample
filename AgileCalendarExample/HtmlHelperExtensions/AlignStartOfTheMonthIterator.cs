using AgileCalendarExample.Models.View;
using System;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    public class AlignStartOfTheMonthIterator : IDatesIterator
    {
        /// <summary>
        /// Flag indicates the start of a week.
        /// Is used because internally iteration happens in a reverse order.
        /// </summary>
        private bool isStart;

        /// <summary>
        /// Current date
        /// </summary>
        private DateTime currentDate;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="notStartOfTheWeekDate">A date before which empty cells should be inserted to start a new week</param>
        public AlignStartOfTheMonthIterator(DateTime notStartOfTheWeekDate)
        {
            this.isStart = true;
            this.currentDate = notStartOfTheWeekDate;
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
                Date = this.currentDate,
                WeekPeriod = this.isStart ? PeriodEnum.Start : PeriodEnum.Current,
                IsNewMonth = this.isStart
            };

            this.currentDate = currentDate.AddDays(-1);
            this.isStart = false;

            return result;
        }
    }
}