using AgileCalendarExample.Models.View;
using System;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    public class AlignStartOfTheMonthIterator : IDatesIterator
    {
        private bool isStart;
        private DateTime date;
        private PeriodEnum weekPeriod;

        public AlignStartOfTheMonthIterator(DateTime notStartOfTheWeekDate)
        {
            this.isStart = true;
            this.date = notStartOfTheWeekDate;
            this.weekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(notStartOfTheWeekDate);
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
                Date = this.date,
                WeekPeriod = this.isStart ? PeriodEnum.Start : PeriodEnum.Current,
                IsNewMonth = this.isStart
            };

            this.date = date.AddDays(-1);
            this.weekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.date);
            this.isStart = false;

            return result;

        }
    }
}