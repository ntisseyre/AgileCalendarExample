using AgileCalendarExample.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    public class MonthPeriodIterator : IDatesIterator
    {
        private bool isNewMonth;
        private DateTime date;

        public MonthPeriodIterator(DateTime date)
        {
            this.date = date;
            this.isNewMonth = false;
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public bool HasNext
        {
            get { return !this.isNewMonth; }
        }

        /// <summary>
        /// Gets the next date for the calendar and shifts the pointer
        /// </summary>
        /// <returns>Date for the calendar</returns>
        public AgileDate Next()
        {
            AgileDate result = new AgileDate
            {
                Date = this.date,
                Color = "",
                WeekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.date),
                IsNewMonth = false
            };

            DateTime nextDate = this.date.AddDays(1);
            this.isNewMonth = nextDate.Month > this.date.Month;
            this.date = nextDate;

            return result;
        }
    }
}