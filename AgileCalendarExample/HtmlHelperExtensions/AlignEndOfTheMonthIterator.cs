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
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        public AgileDateBase ReadNext(AgileDateBase model)
        {
            model.WeekPeriod = AgileCalendarHtmlHelper.GetWeekPeriod(this.currentDate);
            model.IsNewMonth = false;

            this.currentDate = currentDate.AddDays(1);
            return model;
        }
    }
}