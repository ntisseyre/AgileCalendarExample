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
                return CalendarHtmlHelper.GetWeekPeriod(this.currentDate) != PeriodEnum.Start;
            }
        }

        /// <summary>
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        public CalendarDateBase ReadNext(CalendarDateBase model)
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