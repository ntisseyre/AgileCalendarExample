using AgileCalendarExample.App_GlobalResources;
using AgileCalendarExample.Models.DomainModels;
using System;

namespace AgileCalendarExample.HtmlHelperExtension
{
    /// <summary>
    /// Static class - extensions for agile items
    /// </summary>
    public static class AgileItemExtension
    {
        /// <summary>
        /// If agile item is an empty record used like a template
        /// </summary>
        /// <param name="agileItem">Agile Item</param>
        /// <returns>True - is empty, False - not empty</returns>
        public static bool IsEmpty(this AgileItemBase agileItem)
        {
            return String.IsNullOrEmpty(agileItem.Name);
        }

        /// <summary>
        /// Format Start date for an agile item
        /// </summary>
        /// <param name="agileItem">Agile Item</param>
        /// <returns>Formatted date</returns>
        public static String GetFormattedStartDate(this AgileItemBase agileItem)
        {
            return AgileItemExtension.GetFormattedDate(agileItem.StartDate);
        }

        /// <summary>
        /// Format End date for an agile item
        /// </summary>
        /// <param name="agileItem">Agile Item</param>
        /// <returns>Formatted date</returns>
        public static String GetFormattedEndDate(this AgileItemBase agileItem)
        {
            return AgileItemExtension.GetFormattedDate(agileItem.EndDate);
        }

        /// <summary>
        /// Format color for an agile item
        /// </summary>
        /// <param name="agileItem">Agile Item</param>
        /// <returns>Formatted color</returns>
        public static String GetFormattedColor(this AgileItemColoredBase agileItem)
        {
            return String.IsNullOrEmpty(agileItem.Color) ? "none" : agileItem.Color;
        }

        /// <summary>
        /// Format teamMember icon
        /// </summary>
        /// <param name="vacation">Vacation</param>
        /// <returns>Formatted teamMember icon</returns>
        public static String GetFormattedTeamMember(this Vacation vacation)
        {
            return String.IsNullOrEmpty(vacation.TeamMemberIcon) ? "none" : vacation.TeamMemberIcon;
        }

        /// <summary>
        /// Format date
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>Formatted date</returns>
        private static String GetFormattedDate(DateTime date)
        {
            if (date == DateTime.MinValue)
                return "";

            return date.ToString(AgileResources.DateFormatForTitle);
        }
    }
}