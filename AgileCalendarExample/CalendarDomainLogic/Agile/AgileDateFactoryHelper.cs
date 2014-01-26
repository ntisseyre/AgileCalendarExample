using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.Models.DomainModels;
using AgileCalendarExample.Models.ViewModels.Agile;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileCalendarExample.CalendarDomainLogic.Agile
{
    /// <summary>
    /// Class that contains simple methods used by <see cref="AgileDateFactory"/>
    /// Done on purpose to be easily covered with tests.
    /// </summary>
    public static class AgileDateFactoryHelper
    {
        public static String DefaultColor = "greyLight";

        /// <summary>
        /// Gets an agile item from the list to which belongs the specified date.
        /// If no item found - returns null.
        /// </summary>
        /// <typeparam name="TAgileItem">Type of the agile item</typeparam>
        /// <param name="agileItemsList">List of the agile items</param>
        /// <param name="date">Current date in a calendar</param>
        /// <returns>An agile item to which the specified date belongs. If no item found - returns null.</returns>
        public static TAgileItem LookForItem<TAgileItem>(IList<TAgileItem> agileItemsList, DateTime date) where TAgileItem : AgileItemBase
        {
            return agileItemsList.FirstOrDefault(item => AgileDateFactoryHelper.IsInside(item, date));
        }

        /// <summary>
        /// Gets all agile items from the list to which belongs the specified date.
        /// If no item found - empty list.
        /// </summary>
        /// <typeparam name="TAgileItem">Type of the agile item</typeparam>
        /// <param name="agileItemsList">List of the agile items</param>
        /// <param name="date">Current date in a calendar</param>
        /// <returns>An agile items to which the specified date belongs. If no item found - empty list.</returns>
        public static IList<TAgileItem> LookForItems<TAgileItem>(IList<TAgileItem> agileItemsList, DateTime date) where TAgileItem : AgileItemBase
        {
            return agileItemsList.Where(item => AgileDateFactoryHelper.IsInside(item, date)).ToList();
        }

        /// <summary>
        /// Detects if a date is inside an agile item
        /// </summary>
        /// <param name="agileItem">Agile item</param>
        /// <param name="date">Current date in a calendar</param>
        /// <returns>True - is inside, False - doesn't belong to the agile item</returns>
        public static bool IsInside(AgileItemBase agileItem, DateTime date)
        {
            return agileItem.EndDate >= date && agileItem.StartDate <= date;
        }

        /// <summary>
        /// Get a model to display vacation date
        /// </summary>
        /// <param name="releaseCycle">Release Cycle model</param>
        /// <param name="vacationsList">List of vacations on the current date</param>
        /// <param name="date">Current date in a calendar</param>
        /// <returns>View's model for the vacation</returns>
        public static AgileVacation GetVacationDate(ReleaseCycleModel releaseCycle, IList<Vacation> vacationsList, DateTime date)
        {
            //Vacation date must inherit colors from current sprint/planning
            AgileItemColoredBase basedOnAgileItem = null;
            Sprint sprint = AgileDateFactoryHelper.LookForItem(releaseCycle.Sprints, date);
            if (sprint != null)
            {
                basedOnAgileItem = sprint;
            }
            else if (IsInside(releaseCycle.Planning, date))
            {
                basedOnAgileItem = releaseCycle.Planning;
            }
            else
                throw new InvalidOperationException(String.Format(AgileResources.E_VacationMustBeInsideSprintOrPlanningWithParam, date));

            AgileVacation agileDate = AgileDateFactoryHelper.CreateAgileDate<AgileVacation>(basedOnAgileItem, date);

            //Set team members who have vacation on the current date
            IList<AgileTeamMemberEvent> teamMemberVacationsList = new List<AgileTeamMemberEvent>();
            foreach (Vacation vacation in vacationsList)
                teamMemberVacationsList.Add(new AgileTeamMemberEvent() { Event = vacation.Name, Icon = vacation.TeamMemberIcon });

            agileDate.TeamMembersVacations = teamMemberVacationsList;
            return agileDate;
        }

        /// <summary>
        /// Create a view's model to display the specified agile item for the current date
        /// </summary>
        /// <typeparam name="TAgileDate">Type of the agile date</typeparam>
        /// <param name="basedOnAgileItem">Agile item based on which to create a view's model</param>
        /// <param name="date">Current Date</param>
        /// <returns>View's model to display the specified agile item for the current date</returns>
        public static TAgileDate CreateAgileDate<TAgileDate>(AgileItemColoredBase basedOnAgileItem, DateTime date) where TAgileDate : AgileDateBase, new()
        {
            TAgileDate agileDate = new TAgileDate()
            {
                Color = basedOnAgileItem.Color,

                Name = (basedOnAgileItem.StartDate == date) ? basedOnAgileItem.Name : String.Empty,

                Title = String.Format(AgileResources.TitleFormat,
                        basedOnAgileItem.Name,
                        basedOnAgileItem.StartDate.ToString(AgileResources.DateFormatForTitle),
                        basedOnAgileItem.EndDate.ToString(AgileResources.DateFormatForTitle))
            };
            return agileDate;
        }

        /// <summary>
        /// If the date specified is a day off.
        /// Sometimes in some countries weekends can be working dates
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>True - dayoff, False - working date</returns>
        public static bool IsDayOff(DateTime date)
        {
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday);
        }
    }
}