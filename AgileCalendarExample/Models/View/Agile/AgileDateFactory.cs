using AgileCalendarExample.HtmlHelperExtensions;
using AgileCalendarExample.Models.Domain;
using AgileCalendarExample.Models.View;
using AgileCalendarExample.Models.View.Agile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileCalendarExample.Models.View.Agile
{
    /// <summary>
    /// Factory for creating dates for Agile
    /// </summary>
    public class AgileDateFactory : CalendarDateFactoryBase
    {
        private ReleaseCycleModel releaseCycle;

        public AgileDateFactory(ReleaseCycleModel normolizedReleaseCycle)
        {
            this.releaseCycle = normolizedReleaseCycle;
        }

        /// <summary>
        /// Start date for the calendar
        /// </summary>
        public override DateTime StartDate
        {
            get { return this.releaseCycle.Planning.StartDate; }
        }

        /// <summary>
        /// End date for the calendar
        /// </summary>
        public override DateTime EndDate
        {
            get
            {
                DateTime sprintsLastDate = this.releaseCycle.Sprints.Last().EndDate;
                DateTime holidaysLastDate = this.releaseCycle.Holidays.Last().EndDate;
                DateTime vacationsLastDate = this.releaseCycle.Vacations.Last().EndDate;

                DateTime endDate = sprintsLastDate > holidaysLastDate ? sprintsLastDate : holidaysLastDate;
                return endDate > vacationsLastDate ? endDate : vacationsLastDate;
            }
        }

        /// <summary>
        /// Fabric method that creates a model for the calendar
        /// </summary>
        /// <param name="date">Date in calendar</param>
        /// <returns>Model</returns>
        public override CalendarDateBase GetCalendarDate(DateTime date)
        {
            Holiday holiday = AgileDateFactory.LookForItem(this.releaseCycle.Holidays, date);
            if (holiday != null)
                return AgileDateFactory.GetAgileDate<HolidayDate>(holiday, date);

            Vacation vacation = AgileDateFactory.LookForItem(this.releaseCycle.Vacations, date);
            if (vacation != null)
                return AgileDateFactory.GetAgileDate<VacationDate>(vacation, date);

            Sprint sprint = AgileDateFactory.LookForItem(this.releaseCycle.Sprints, date);
            if (sprint != null)
            {
                SprintDate sprintDate = AgileDateFactory.GetAgileDate<SprintDate>(sprint, date);
                sprintDate.Color = sprint.Color;
                return sprintDate;
            }

            return AgileDateFactory.GetAgileDate<PlanningDate>(this.releaseCycle.Planning, date);
        }

        private static TAgileItem LookForItem<TAgileItem>(IList<TAgileItem> agileItemsList, DateTime date) where TAgileItem : AgileItemBase
        {
            return agileItemsList.FirstOrDefault(item => item.EndDate >= date && item.StartDate <= date);
        }

        private static TAgileDate GetAgileDate<TAgileDate>(AgileItemBase agileItem, DateTime date) where TAgileDate : AgileDateBase, new()
        {
            TAgileDate agileDate = new TAgileDate()
            {
                Name = (agileItem.StartDate == date) ? agileItem.Name : String.Empty,
                Title = agileItem.Name + "\r\n" + "from " + agileItem.StartDate.ToString("dd-MMM-yy") + " to " + agileItem.EndDate.ToString("dd-MMM-yy")
            };
            return agileDate;
        }
    }
}