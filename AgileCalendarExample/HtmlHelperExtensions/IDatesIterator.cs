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
        /// Gets the next date for the calendar and shifts the pointer
        /// </summary>
        /// <returns>Date for the calendar</returns>
        AgileDate Next();
    }
}
