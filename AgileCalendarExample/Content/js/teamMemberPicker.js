var teamMemberPicker;
var isInsideTeamMemberPicker = false;

var pickingTeamMemberForControl;

$(document).ready(function () {
    teamMemberPicker = $('#teamMemberPicker');
    teamMemberPicker.mouseleave(function () { leaveTeamMemberPickerr(); });
    teamMemberPicker.mouseover(function () { hoverTeamMemberPicker(); });
});

/// <summary>
/// Init Html element: attach teamMemberPicker behaviour
/// </summary>
/// <param name="control">Html element</param>
function initTeamMemberPicker(control) {
    if (control.data('isInited'))
        return;

    control.data('isInited', true);

    //adding tabIndex attribute to support keydown event
    if (!control.attr('tabindex')) control.attr('tabindex', '0');

    control.bind("click", function () { showTeamMemberPicker(control); });
    control.bind("keydown", function (e) { if (e.keyCode == 27) { hideTeamMemberPicker(); } }); //if Esc - hide control
    control.bind("blur", function () { if (!isInsideTeamMemberPicker) { hideTeamMemberPicker(); } });
}

/// <summary>
/// Show a TeamMemberPicker-control
/// </summary>
/// <param name="control">Html-element for setting a team member</param>
function showTeamMemberPicker(control) {

    pickingTeamMemberForControl = control;

    this.teamMemberPicker.css
	({
	    top: control.offset().top + control.height() + 4,
	    left: control.offset().left
	});
    this.teamMemberPicker.show();
}

/// <summary>
/// Hide a TeamMemberPicker-control
/// </summary>
function hideTeamMemberPicker() {
    this.teamMemberPicker.hide();
}

/// <summary>
/// Set a flag that Team Member Picker is in focus right now.
/// </summary>
function hoverTeamMemberPicker() {
    isInsideTeamMemberPicker = true;
}

/// <summary>
/// Set a flag that Team Member Picker is not in focus right now
/// </summary>
function leaveTeamMemberPickerr() {
    isInsideTeamMemberPicker = false;
}