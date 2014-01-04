using AgileCalendarExample.Models.View;

namespace AgileCalendarExample.HtmlHelperExtensions
{
    /// <summary>
    /// Interface of an iterator to obtain dates for the calendar view
    /// </summary>
    public interface IDatesIterator
    {
        /// <summary>
        /// If there are elements in a list
        /// </summary>
        /// <returns>True - yes, False - it is empty</returns>
        bool HasNext { get; }

        /// <summary>
        /// Populates the agileItem for the calendar
        /// and shifts the pointer to the next date
        /// </summary>
        /// <param name="model">Abstract view model</param>
        /// <returns>Populated model. Same pointer to an object.</returns>
        CalendarDateBase ReadNext(CalendarDateBase model);
    }
}
