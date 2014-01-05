using AgileCalendarExample.CalendarDomainLogic;
using AgileCalendarExample.Models.DomainModels;
using AgileCalendarExample.Models.ViewModels;
using AgileCalendarExample.Models.ViewModels.Agile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// Factory for creating dates for Agile
    /// </summary>
    public class AgileDateFactory : CalendarDateFactoryBase
    {
        private static String DefaultColor = "greyLight";
        private static String DateFormatForTitle = "dd-MMM-yy";
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
                return AgileDateFactory.GetAgileDate(holiday, date, AgileItemsEnum.Holiday);

            Vacation vacation = AgileDateFactory.LookForItem(this.releaseCycle.Vacations, date);
            if (vacation != null)
                return this.GetVacationDate(vacation, date);

            Sprint sprint = AgileDateFactory.LookForItem(this.releaseCycle.Sprints, date);
            if (sprint != null)
                return AgileDateFactory.GetAgileDate(sprint, date, AgileItemsEnum.Sprint);

            if (AgileDateFactory.IsInside(this.releaseCycle.Planning, date))
                return AgileDateFactory.GetAgileDate(this.releaseCycle.Planning, date, AgileItemsEnum.Planning);

            return base.GetEmptyViewModel();
        }

        private static TAgileItem LookForItem<TAgileItem>(IList<TAgileItem> agileItemsList, DateTime date) where TAgileItem : AgileItemBase
        {
            return agileItemsList.FirstOrDefault(item => AgileDateFactory.IsInside(item, date));
        }

        private static bool IsInside(AgileItemBase agileItem, DateTime date)
        {
            return agileItem.EndDate >= date && agileItem.StartDate <= date;
        }

        private static AgileDateBase GetAgileDate(AgileItemColoredBase agileItem, DateTime date, AgileItemsEnum agileItemType)
        {
            AgileDateBase agileDate = AgileDateFactory.GetAgileDateNoColor(agileItem, date, agileItemType);
            agileDate.Color = agileItem.Color;
            return agileDate;
        }

        private AgileDateBase GetVacationDate(Vacation vacation, DateTime date)
        {
            AgileDateBase agileDate = AgileDateFactory.GetAgileDateNoColor(vacation, date, AgileItemsEnum.Vacation);

            //Vacation date must inherit colors from current sprint/planning
            Sprint sprint = AgileDateFactory.LookForItem(this.releaseCycle.Sprints, date);
            if (sprint != null)
            {
                agileDate.Color = sprint.Color;
            }
            else if (AgileDateFactory.IsInside(this.releaseCycle.Planning, date))
            {
                agileDate.Color = this.releaseCycle.Planning.Color;
            }
            else
                agileDate.Color = AgileDateFactory.DefaultColor;

            return agileDate;
        }

        private static AgileDateBase GetAgileDateNoColor(AgileItemBase agileItem, DateTime date, AgileItemsEnum agileItemType)
        {
            AgileDateBase agileDate = new AgileDateBase()
            {
                AgileItem = agileItemType,
                Name = (agileItem.StartDate == date) ? agileItem.Name : String.Empty,
                Title = agileItem.Name + "\r\n"
                + "from "
                + agileItem.StartDate.ToString(AgileDateFactory.DateFormatForTitle)
                + " to "
                + agileItem.EndDate.ToString(AgileDateFactory.DateFormatForTitle)
            };
            return agileDate;
        }
    }
}