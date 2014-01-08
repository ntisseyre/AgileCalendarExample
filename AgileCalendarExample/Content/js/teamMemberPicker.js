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
/// <param name="control">Html element</param>
function showTeamMemberPicker(control)
{
    teamMemberPicker.dialog({
        width: 300,
        height: 'auto',
        modal: true,
        resizable: true,
        closeOnEscape: true,
        title: 'Team Member Selection',
        buttons:
		{
		    Ok: function () {
		        if (validateBreaks(breaksDialog.find('input.time'))) {
		            breaksDialog.dialog("close");

		            setBreaksContainerText(getBreaksContainerForDialog(breaksDialog), breaksDialog);

		            if (isMonday(breaksDialogId))
		                copyBreaksFromMonday();
		        }

		    },
		    Cancel: function () { teamMemberPicker.dialog("close"); }
		},
        open: function () { createAutoComplete(); }
    });
}

/// <summary>
/// Creat AutoComplete control to search team members from a static source
/// </summary>
function createAutoComplete()
{
    var teamMembersControl = $('#teamMembersControl');
    teamMembersControl
        .autocomplete(
        {
            minLength: 0,
            source: teamMembersSource,
            focus: function (event, ui) {
                teamMembersControl.val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                teamMembersControl.val(ui.item.label);
                alert(ui.item.value + " " + ui.item.icon);
                return false;
            }
        })
        .data("ui-autocomplete")._renderItem = function (ul, item) {
            return $("<li class=\"teamMemberPicker-item\">")
              .append("<a><div>" + item.label + "</div><div>" + "<img src=\"/Content/img/32X32/" + item.icon + ".png\" />" + "</div></a>")
              .appendTo(ul);
        };
}