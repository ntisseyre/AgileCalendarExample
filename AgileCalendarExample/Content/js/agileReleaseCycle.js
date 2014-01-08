$(document).ready(function () {
   
    var agileReleaseCycle = $('.agile-releaseCycle');

    //bind ColorPicker to controls
    agileReleaseCycle.find(' > div > div.agile-item-colored > div:last-child').each(function () { initColorPicker($(this)); });

    //bind TeamMemberPicker to controls
    agileReleaseCycle.find(' > div > div.agile-item-vacation > div:last-child').bind("click", function () { showTeamMemberPicker($(this)); });

    //bind DatePickers to controls
    bindDatePickerIntervals(agileReleaseCycle.find(' > div > div > div:nth-child(2)'));

    //bind to TeamMemberPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); });
});

/// <summary>
/// Handler of an event "Team member selected from a list"
/// </summary>
/// <param name="eventArgs">Args of an event: { selectedForControl: control that triggered an event, value: team member name, icon: team memeber icon }</param>
function onTeamMemberSelected(eventArgs)
{
    //update image
    var teamMemberImg = eventArgs.selectedForControl.find(' > img ');
    var src = teamMemberImg.attr('src');
    eventArgs.selectedForControl.find(' > img ').attr('src', src.substring(0, src.lastIndexOf('/') + 1) + eventArgs.icon + ".png");

    //update text
    var inputWithName = $(eventArgs.selectedForControl.siblings()[0]).find(' >  input'); //div -> first div in the same list -> input
    inputWithName.val(eventArgs.value + "'s vacation");
}

/// <summary>
/// Bind datePicker's control to each "start date" and "end date"
/// </summary>
/// <param name="listOfDivsWithStartDate">List of all divs that contain start date</param>
function bindDatePickerIntervals(listOfDivsWithStartDate)
{    
    for (var c = 0; c < listOfDivsWithStartDate.length; c++)//iterate trhough all the divs with start date
    {
        var div = $(listOfDivsWithStartDate[c]);
        var startDate = div.find(' > input ');
        var endDate = div.next().find(' > input ');

        startDate.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 2,
            onClose: function (selectedDate) {
                endDate.datepicker("option", "minDate", selectedDate);
            }
        });

        endDate.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            numberOfMonths: 2,
            onClose: function (selectedDate) {
                startDate.datepicker("option", "maxDate", selectedDate);
            }
        });
    }
}