using AgileCalendarExample.CalendarDomainLogic;
using AgileCalendarExample.Models.DomainModels;
using AgileCalendarExample.Models.ViewModels;
using AgileCalendarExample.Models.ViewModels.Agile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileCalendarExample.CalendarDomainLogic.Agile
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
            if (AgileDateFactoryHelper.IsDayOff(date))
                return new AgileDayOff();

            Holiday holiday = AgileDateFactoryHelper.LookForItem(this.releaseCycle.Holidays, date);
            if (holiday != null)
                return AgileDateFactoryHelper.CreateAgileDate<AgileDateBase>(holiday, date);

            IList<Vacation> vacationsList = AgileDateFactoryHelper.LookForItems(this.releaseCycle.Vacations, date);
            if (vacationsList.Count > 0)
                return AgileDateFactoryHelper.GetVacationDate(releaseCycle, vacationsList, date);

            Sprint sprint = AgileDateFactoryHelper.LookForItem(this.releaseCycle.Sprints, date);
            if (sprint != null)
                return AgileDateFactoryHelper.CreateAgileDate<AgileDateBase>(sprint, date);

            if (AgileDateFactoryHelper.IsInside(this.releaseCycle.Planning, date))
                return AgileDateFactoryHelper.CreateAgileDate<AgileDateBase>(this.releaseCycle.Planning, date);

            return base.GetEmptyViewModel();
        }
    }
}