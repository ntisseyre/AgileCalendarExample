using AgileCalendarExample.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgileCalendarExample
{
    public static class AgileCalendarHtmlHelper
    {
        public static DayOfWeek WeekStart = DayOfWeek.Monday;
        public static DayOfWeek WeekEnd = DayOfWeek.Sunday;

        public static IEnumerable<DateTime> GetAllDatesInReleaseCycle(this HtmlHelper<ReleaseCycleModel> htmlHelper)
        {
            DateTime startDate = htmlHelper.ViewData.Model.Items.Min(item => item.StartDate);
            DateTime endDate = htmlHelper.ViewData.Model.Items.Max(item => item.EndDate);

            //align startDate witha a beginning of the week
            while (startDate.DayOfWeek != AgileCalendarHtmlHelper.WeekStart)
                startDate = startDate.AddDays(-1);

            //align endDate witha an end of the week
            while (endDate.DayOfWeek != AgileCalendarHtmlHelper.WeekEnd)
                endDate = endDate.AddDays(1);

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }
    }
}