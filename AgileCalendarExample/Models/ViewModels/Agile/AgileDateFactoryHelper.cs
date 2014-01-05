using AgileCalendarExample.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgileCalendarExample.Models.ViewModels.Agile
{
    /// <summary>
    /// Class that contains simple methods used by <see cref="AgileDateFactory"/>
    /// Done on purpose to be easily covered with tests.
    /// </summary>
    public static class AgileDateFactoryHelper
    {
        public static String DayOffColor = "whiteSmoke";
        public static String DefaultColor = "greyLight";
        public static String DateFormatForTitle = "dd-MMM-yy";

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
        /// Get a model to display agile date
        /// </summary>
        /// <param name="agileItem">Agile item based on which to build a view's model</param>
        /// <param name="date">Current date in a calendar</param>
        /// <param name="agileItemType">Agile item's type</param>
        /// <returns>View's model for the agile item</returns>
        public static AgileDateBase GetAgileDate(AgileItemColoredBase agileItem, DateTime date, AgileItemsEnum agileItemType)
        {
            AgileDateBase agileDate = AgileDateFactoryHelper.GetAgileDateNoColor(agileItem, date, agileItemType);
            agileDate.Color = agileItem.Color;
            return agileDate;
        }

        /// <summary>
        /// Get a model to display vacation date
        /// </summary>
        /// <param name="releaseCycle">Release Cycle model</param>
        /// <param name="vacation">Vacation</param>
        /// <param name="date">Current date in a calendar</param>
        /// <returns>View's model for the vacation</returns>
        public static AgileDateBase GetVacationDate(ReleaseCycleModel releaseCycle, Vacation vacation, DateTime date)
        {
            AgileDateBase agileDate = AgileDateFactoryHelper.GetAgileDateNoColor(vacation, date, AgileItemsEnum.Vacation);

            //Vacation date must inherit colors from current sprint/planning
            Sprint sprint = AgileDateFactoryHelper.LookForItem(releaseCycle.Sprints, date);
            if (sprint != null)
            {
                agileDate.Color = sprint.Color;
            }
            else if (IsInside(releaseCycle.Planning, date))
            {
                agileDate.Color = releaseCycle.Planning.Color;
            }
            else
                agileDate.Color = AgileDateFactoryHelper.DefaultColor;

            return agileDate;
        }


        /// <summary>
        /// Get a model to display agile date, no color set
        /// </summary>
        /// <param name="agileItem">Agile item based on which to build a view's model</param>
        /// <param name="date">Current date in a calendar</param>
        /// <param name="agileItemType">Agile item's type</param>
        /// <returns>View's model for the agile item. No color is set</returns>
        public static AgileDateBase GetAgileDateNoColor(AgileItemBase agileItem, DateTime date, AgileItemsEnum agileItemType)
        {
            AgileDateBase agileDate = new AgileDateBase()
            {
                AgileItem = agileItemType,
                Name = (agileItem.StartDate == date) ? agileItem.Name : String.Empty,
                Title = agileItem.Name + "\r\n"
                + "from "
                + agileItem.StartDate.ToString(AgileDateFactoryHelper.DateFormatForTitle)
                + " to "
                + agileItem.EndDate.ToString(AgileDateFactoryHelper.DateFormatForTitle)
            };
            return agileDate;
        }

        /// <summary>
        /// Get a model to display day-off
        /// </summary>
        /// <returns>Day-off model</returns>
        public static AgileDateBase GetDayOff()
        {
            return new AgileDateBase
            {
                AgileItem = AgileItemsEnum.DayOff,
                Color = AgileDateFactoryHelper.DayOffColor
            };
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