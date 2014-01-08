using AgileCalendarExample.Models.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace AgileCalendarExample.HtmlHelperExtension
{
    public static class TeamMembersControlExtension
    {
        /// <summary>
        /// Get a JSON string that represents the collection of team Members
        /// </summary>
        /// <param name="htmlHelper">Represents support for rendering HTML controls in a strongly typed view</param>
        /// <returns>JSON array</returns>
        public static string GetJsonForTeamMembers(this HtmlHelper<IList<TeamMember>> htmlHelper)
        {
            bool isFirst = true;
            StringBuilder result = new StringBuilder();
            foreach (TeamMember teamMember in htmlHelper.ViewData.Model)
            {
                result.Append(String.Format("{0}{{\"value\":\"{1}\", \"label\":\"{2}\", \"icon\":\"{3}\"}}",
                        isFirst ? "[" : ",",
                        teamMember.Name,
                        teamMember.Name,
                        teamMember.Icon));

                if (isFirst)
                    isFirst = false;
            }

            result.Append(']');
            return result.ToString();
        }
    }
}