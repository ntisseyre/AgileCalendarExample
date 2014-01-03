using AgileCalendarExample.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// Iterator to get all the dates in a period and
    /// to indicate the beginning of each new month
    /// </summary>
    public class MonthPeriodIterator : IDatesIterator
    {
        /// <summary>
        /// A flag that indicates if a new month starts
        /// </summary>
        private bool isNewMonth;

        /// <summary>
        /// Current date
        /// </summary>
        private DateTime currentDate;

        /// <summary>
        /// End date
        /// </summary>
        private DateTime endDate;

        public MonthPeriodIterator(DateTime startDate, DateTime endDate)
        {
            this.currentDate = startDate;
            this.endDate = endDate;
            this.isNewMonth = false;
        }

        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        public bool HasNext
        {
            get { return this.currentDate <= this.endDate; }
        }

        /// <summary>
        /// True - if a new month starts, False - current month
        /// </summary>
        public bool IsNewMonth
        {
            get { return this.isNewMonth; }
            set { this.isNewMonth = value; }
        }

        /// <summary>
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        public AgileDateBase ReadNext(AgileDateBase model)
        {
            model.Date = this.currentDate;            
            model.WeekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.currentDate);
            model.IsNewMonth = false;

            DateTime nextDate = this.currentDate.AddDays(1);
            this.isNewMonth = nextDate.Month != this.currentDate.Month;
            this.currentDate = nextDate;

            return model;
        }

        /// <summary>
        /// Get current date
        /// </summary>
        public DateTime CurrentDate
        {
            get { return this.currentDate; }
        }
    }
}