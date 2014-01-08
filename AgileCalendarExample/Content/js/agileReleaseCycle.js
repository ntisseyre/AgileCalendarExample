$(document).ready(function () {
   
    var agileReleaseCycle = $('.agile-releaseCycle');

    //bind ColorPicker to controls
    agileReleaseCycle.find(' > div > div.agile-item-colored > div:last-child').each(function () { initColorPicker($(this)); });

    //bind TeamMemberPicker to controls
    agileReleaseCycle.find(' > div > div.agile-item-vacation > div:last-child').bind("click", function () { showTeamMemberPicker($(this)); });

    //bind to TeamMemberPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); });
});

/// <summary>
/// Handler of an event "Team member selected from a list"
/// </summary>
/// <param name="eventArgs">Args of an event: {selectedForControl: control that triggered an event, value: team member name, icon: team memeber icon  }</param>
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