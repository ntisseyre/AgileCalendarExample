using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.Models.Domain;
using AgileCalendarExample.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// An extension class for the HtmlHelper<ReleaseCycleModel>.
    /// Assumes that a model is normolized (all collections are initialized and sorted)
    /// </summary>
    public static class AgileCalendarHtmlHelper
    {
        public static DayOfWeek WeekStart = DayOfWeek.Monday;
        public static DayOfWeek WeekEnd = (DayOfWeek)(AgileCalendarHtmlHelper.WeekStart == DayOfWeek.Sunday ? 6 : (int)AgileCalendarHtmlHelper.WeekStart - 1);

        /// <summary>
        /// Returns an enumerator for the days of the week in a custom order.
        /// <see cref="WeekStart"/>
        /// </summary>
        /// <param name="htmlHelper">Represents support for rendering HTML controls in a strongly typed view</param>
        /// <returns>The name of the day of the week</returns>
        public static IEnumerable<String> GetDaysOfWeek(this HtmlHelper<ReleaseCycleModel> htmlHelper)
        {
            int dayIndex = (int)AgileCalendarHtmlHelper.WeekStart;
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

        public static IEnumerable<AgileDateBase> GetAllDatesInReleaseCycle(this HtmlHelper<ReleaseCycleModel> htmlHelper)
        {
            ReleaseCycleModel model = htmlHelper.ViewData.Model;
            DateTime startDate = model.Planning.StartDate;
            DateTime endDate = AgileCalendarHtmlHelper.GetEndDate(model);

            AgileItemsFactory agileItemsFactory = new AgileItemsFactory(model);
            MonthPeriodIterator monthIterator = new MonthPeriodIterator(startDate, endDate);

            IDatesIterator alignIterator;
            while (monthIterator.HasNext)
            {
                alignIterator = new AlignStartOfTheMonthIterator(monthIterator.CurrentDate);
                while (alignIterator.HasNext)
                    yield return alignIterator.ReadNext(agileItemsFactory.GetEmptyViewModel());

                monthIterator.IsNewMonth = false;
                while (!monthIterator.IsNewMonth && monthIterator.HasNext)
                    yield return monthIterator.ReadNext(agileItemsFactory.GetAgileItem(monthIterator.CurrentDate));

                alignIterator = new AlignEndOfTheMonthIterator(monthIterator.CurrentDate);
                while (alignIterator.HasNext)
                    yield return alignIterator.ReadNext(agileItemsFactory.GetEmptyViewModel());
            }
        }

        /// <summary>
        /// Gets the state of a week
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>An enum value which indicates whether the week starts, ends or is current</returns>
        public static PeriodEnum GetWeekPeriod(DateTime date)
        {
            if (date.DayOfWeek == AgileCalendarHtmlHelper.WeekStart)
                return PeriodEnum.Start;
            else if (date.DayOfWeek == AgileCalendarHtmlHelper.WeekEnd)
                return PeriodEnum.End;
            else
                return PeriodEnum.Current;
        }

        /// <summary>
        /// Gets the latest date from a model
        /// </summary>
        /// <param name="model">ReleaseCycle normolized model</param>
        /// <returns>The latest date</returns>
        private static DateTime GetEndDate(ReleaseCycleModel model)
        {
            DateTime sprintsLastDate = model.Sprints.Last().EndDate;
            DateTime holidaysLastDate = model.Holidays.Last().EndDate;
            DateTime vacationsLastDate = model.Vacations.Last().EndDate;

            DateTime endDate = sprintsLastDate > holidaysLastDate ? sprintsLastDate : holidaysLastDate;
            return endDate > vacationsLastDate ? endDate : vacationsLastDate;
        }
    }
}