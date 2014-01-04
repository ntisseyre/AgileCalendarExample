using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.Models.Domain;
using AgileCalendarExample.Models.View;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// An extension class for the HtmlHelper<ReleaseCycleModel>.
    /// Assumes that a model is normolized (all collections are initialized and sorted)
    /// </summary>
    public static class CalendarHtmlHelper
    {
        public static DayOfWeek WeekStart = DayOfWeek.Monday;
        public static DayOfWeek WeekEnd = (DayOfWeek)(CalendarHtmlHelper.WeekStart == DayOfWeek.Sunday ? 6 : (int)CalendarHtmlHelper.WeekStart - 1);

        /// <summary>
        /// Returns an enumerator for the days of the week in a custom order.
        /// <see cref="WeekStart"/>
        /// </summary>
        /// <param name="htmlHelper">Represents support for rendering HTML controls in a strongly typed view</param>
        /// <returns>The name of the day of the week</returns>
        public static IEnumerable<String> GetDaysOfWeek(this HtmlHelper<CalendarDateFactoryBase> htmlHelper)
        {
            int dayIndex = (int)CalendarHtmlHelper.WeekStart;
            for (int c = 0; c < 7; c++)
            {
                DayOfWeek dayOfWeek = (DayOfWeek)dayIndex;
                switch (dayOfWeek)
                {
                    case DayOfWeek.Monday:
                        yield return Resources.Monday;
                        break;
                    case DayOfWeek.Tuesday:
                        yield return Resources.Tuesday;
                        break;
                    case DayOfWeek.Wednesday:
                        yield return Resources.Wednesday;
                        break;
                    case DayOfWeek.Thursday:
                        yield return Resources.Thursday;
                        break;
                    case DayOfWeek.Friday:
                        yield return Resources.Friday;
                        break;
                    case DayOfWeek.Saturday:
                        yield return Resources.Saturday;
                        break;
                    default:
                        yield return Resources.Sunday;
                        break;
                }

                if (++dayIndex == 7)
                    dayIndex = 0;
            }
        }

        /// <summary>
        /// Gets all the dates for the calendar
        /// </summary>
        /// <param name="htmlHelper">Represents support for rendering HTML controls in a strongly typed view</param>
        /// <returns>A list of date's models</returns>
        public static IEnumerable<CalendarDateBase> GetDates(this HtmlHelper<CalendarDateFactoryBase> htmlHelper)
        {
            CalendarDateFactoryBase factory = htmlHelper.ViewData.Model;

            DateTime startDate = factory.StartDate;
            DateTime endDate = factory.EndDate;
            MonthPeriodIterator monthIterator = new MonthPeriodIterator(startDate, endDate);

            IDatesIterator alignIterator;
            while (monthIterator.HasNext)
            {
                alignIterator = new AlignStartOfTheMonthIterator(monthIterator.CurrentDate);
                while (alignIterator.HasNext)
                    yield return alignIterator.ReadNext(factory.GetEmptyViewModel());

                monthIterator.IsNewMonth = false;
                while (!monthIterator.IsNewMonth && monthIterator.HasNext)
                    yield return monthIterator.ReadNext(factory.GetCalendarDate(monthIterator.CurrentDate));

                alignIterator = new AlignEndOfTheMonthIterator(monthIterator.CurrentDate);
                while (alignIterator.HasNext)
                    yield return alignIterator.ReadNext(factory.GetEmptyViewModel());
            }
        }

        /// <summary>
        /// Gets the state of a week
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>An enum value which indicates whether the week starts, ends or is current</returns>
        public static PeriodEnum GetWeekPeriod(DateTime date)
        {
            if (date.DayOfWeek == CalendarHtmlHelper.WeekStart)
                return PeriodEnum.Start;
            else if (date.DayOfWeek == CalendarHtmlHelper.WeekEnd)
                return PeriodEnum.End;
            else
                return PeriodEnum.Current;
        }
    }
}