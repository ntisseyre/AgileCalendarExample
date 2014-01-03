using AgileCalendarExample.Models.Domain;
using AgileCalendarExample.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    public class AgileItemsFactory
    {
        private ReleaseCycleModel releaseCycle;

        public AgileItemsFactory(ReleaseCycleModel normolizedReleaseCycle)
        {
            this.releaseCycle = normolizedReleaseCycle;
        }

        public EmptyDate GetEmptyViewModel()
        {
            return new EmptyDate();
        }

        public AgileDateBase GetAgileItem(DateTime date)
        {
            Holiday holiday = this.releaseCycle.Holidays.FirstOrDefault(item => this.IsInside(item, date));
            if (holiday != null)
                return new HolidayDate() { Name = holiday.Name };

            Vacation vacation = this.releaseCycle.Vacations.FirstOrDefault(item => this.IsInside(item, date));
            if (vacation != null)
                return new VacationDate() { Name = vacation.Name };

            Sprint sprint = this.releaseCycle.Sprints.FirstOrDefault(item => this.IsInside(item, date));
            if (sprint != null)
                return new SprintDate() { Name = sprint.Name, Color = sprint.Color };

            return new PlanningDate();
        }

        private bool IsInside(AgileItemBase agileItem, DateTime date)
        {
            return agileItem.EndDate >= date && agileItem.StartDate <= date;
        }
    }
}