var teamMemberPicker;
var teamMembersSource;

$(document).ready(function ()
{
    teamMemberPicker = $('#teamMemberPicker');
    teamMembersSource = $.parseJSON($('#teamMembersSource').text());
});

/// <summary>
/// Show teamMemberPicker-dialog
/// </summary>
/// <param name="triggerControl">Html element which fired an event</param>
function showTeamMemberPicker(triggerControl)
{
    var teamMembersAutoCompleteControl;

    teamMemberPicker.dialog({
        width: 300,
        height: 'auto',
        modal: true,
        resizable: true,
        closeOnEscape: true,
        title: 'Team Member Selection',
        buttons:
		{
		    Cancel: function () { teamMemberPicker.dialog("close"); }
		},
        open: function () { teamMembersAutoCompleteControl = creatAutoComplete(triggerControl); },
        close: function () { cleanAutoComplete(teamMembersAutoCompleteControl); }
    });
}

/// <summary>
/// Creat AutoComplete control to search team members from a static source.
/// It is created each time a dialog box opens to support a proper z-index.
/// </summary>
/// <param name="triggerControl">Html element which fired an event to select a team member</param>
/// <returns>AutoComplete control</returns>
function creatAutoComplete(triggerControl)
{
    var teamMembersAutoCompleteControl = $('#teamMembersControl');
    teamMembersAutoCompleteControl
        .autocomplete(
        {
            minLength: 0,
            source: teamMembersSource,
            focus: function (event, ui) {
                teamMembersAutoCompleteControl.val(ui.item.label);
                return false;
            },
            select: function (event, ui)
            {
                teamMembersAutoCompleteControl.val(ui.item.label);
                teamMemberPicker.dialog("close");
                $(document).trigger('teamMemberSelected', { selectedForControl: triggerControl, value: ui.item.value, icon: ui.item.icon });                
                return false;
            }
        })
        .data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li class=\"teamMemberPicker-item\">")
              .append("<a><div>" + item.label + "</div><div>" + "<img src=\"/Content/img/32X32/" + item.icon + ".png\" />" + "</div></a>")
              .appendTo(ul);
        };

    return teamMembersAutoCompleteControl;
}

/// <summary>
/// Reset the value of the control
/// </summary>
/// <param name="teamMembersAutoCompleteControl">Autocomplete control</param>
function cleanAutoComplete(teamMembersAutoCompleteControl)
{
    teamMembersAutoCompleteControl.val('');
}