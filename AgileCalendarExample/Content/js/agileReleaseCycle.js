$(document).ready(function () {
   
    var agileReleaseCycle = $('.agile-releaseCycle');

    //bind ColorPicker to controls
    agileReleaseCycle.find('.agile-item-colored-color').each(function () { initColorPicker($(this)); });

    //bind TeamMemberPicker to controls
    agileReleaseCycle.find('.agile-item-vacation-teamMember').bind("click", function () { showTeamMemberPicker($(this)); });
    //bind to TeamMemberPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); });

    //bind DatePickers to controls
    bindDatePickerIntervals(agileReleaseCycle.find(' > div > div > div:nth-of-type(2)'));

    /*------------------- Agile items' rows -------------------*/
    var agileItemRowsList = agileReleaseCycle.find(' > div > div:not(.agile-releaseCycle-header)');
    //row Highlightning
    addAgileItemRowsHighlightning(agileItemRowsList);
    //row removing
    initDraggableToTrash(agileItemRowsList);
});

/// <summary>
/// Apply highlighting on all agile items' rows
/// </summary>
/// <param name="agileItemRowsList">List of agile items' rows</param>
function addAgileItemRowsHighlightning(agileItemRowsList) {
    agileItemRowsList.hover(
			function () { $(this).addClass('ui-state-hover'); },
			function () { $(this).removeClass('ui-state-hover'); });
}

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
            dateFormat: "dd M y",
            changeMonth: true,
            numberOfMonths: 2,
            onClose: createStartDateCallback(endDate)
        });

        endDate.datepicker({
            defaultDate: "+1w",
            dateFormat: "dd M y",
            changeMonth: true,
            numberOfMonths: 2,
            onClose: createEndDateCallback(startDate)
        });
    }
}

/// <summary>
/// Creat a callback function when start date changed
/// </summary>
/// <param name="endDate">Pointer to the corresponding end date</param>
/// <returns>Callback function</returns>
function createStartDateCallback(endDate)
{
    return function (selectedDate)
    {
        endDate.datepicker("option", "minDate", selectedDate);
    };
}

/// <summary>
/// Creat a callback function when end date changed
/// </summary>
/// <param name="startDate">Pointer to the corresponding start date</param>
/// <returns>Callback function</returns>
function createEndDateCallback(startDate)
{
    return function (selectedDate)
    {
        startDate.datepicker("option", "maxDate", selectedDate);
    }
}

/// <summary>
/// Init behaviour to remove items:
/// Make all agile items draggable to trash
/// </summary>
/// <param name="agileItemRowsList">List of agile items' rows</param>
function initDraggableToTrash(agileItemRowsList)
{
    var trash = $(".agile-releaseCycle-trash");
    trash.droppable({
        tolerance: "touch",
        accept: agileItemRowsList,
        drop: function (event, ui)
        {
            removeAgileItem(ui.draggable);
        }
    });

    agileItemRowsList.draggable({
        cancel: "input, .agile-item-colored-color, .agile-item-vacation-teamMember", // clicking an [input, div with color, div with team member] won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        helper: "clone",
        cursor: "move",
        start: function () { var agileItemRow = $(this); showTrash(trash, agileItemRow); },
        stop: function () { trash.fadeOut(); },
    });
}

/// <summary>
/// Show trash basket to put agile items
/// </summary>
/// <param name="trash">Html element that reperesents "Trash basket"</param>
/// <param name="agileItemRow">Agile item to remove</param>
function showTrash(trash, agileItemRow)
{ 
    trash.css
	({
	    top: agileItemRow.offset().top + agileItemRow.height() + 20,
	    left: agileItemRow.offset().left + agileItemRow.width() - 20
	});
    
    trash.fadeIn();
}

/// <summary>
/// Remove agile item row
/// </summary>
/// <param name="agileItemRow">Agile item's row</param>
function removeAgileItem(agileItemRow)
{
    agileItemRow.fadeOut(function () { agileItemRow.remove(); });
}