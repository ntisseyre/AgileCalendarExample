var agileItemColoredTemplate;
var vacationTemplate;

$(document).ready(function () {
   
    var agileReleaseCycle = $('.agile-releaseCycle');

    //bind to TeamMemberPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); });

    /*------------------- Agile items' rows -------------------*/
    var agileItemRowsList = agileReleaseCycle.find(' > div > div:not(.agile-releaseCycle-header)');

    //bind datePickers and text-change handlers
    for (var c = 0; c < agileItemRowsList.length; c++)
    {        
        var agileItemRow = $(agileItemRowsList[c]);

        //save template rows
        initTemplateRows(agileItemRow);

        initInputControls(agileItemRow);
    }

    //row removing
    initDraggableToTrash(agileItemRowsList);

    //bind ColorPicker to controls
    agileReleaseCycle.find('.agile-item-colored-color').each(function () { initColorPicker($(this)); });

    //bind TeamMemberPicker to controls
    agileReleaseCycle.find('.agile-item-vacation-teamMember').bind("click", function () { showTeamMemberPicker($(this)); });
});

/// <summary>
/// Init template rows to add new agile items
/// </summary>
/// <param name="agileItemRow">Agile item's row</param>
function initTemplateRows(agileItemRow)
{
    if (agileItemRow.hasClass("agile-item-template"))
    {
        if (agileItemRow.hasClass("agile-item-colored"))
            agileItemColoredTemplate = agileItemRow.clone();
        else
            vacationTemplate = agileItemRow.clone();
    }
}

/// <summary>
/// Handler of an event "Team member selected from a list"
/// </summary>
/// <param name="eventArgs">Args of an event: { selectedForControl: control that triggered an event, value: team member name, icon: team memeber icon }</param>
function onTeamMemberSelected(eventArgs) {
    //update image
    var teamMemberImg = eventArgs.selectedForControl.find(' > img ');
    var src = teamMemberImg.attr('src');
    eventArgs.selectedForControl.find(' > img ').attr('src', src.substring(0, src.lastIndexOf('/') + 1) + eventArgs.icon + ".png");

    //update text
    var inputWithName = $(eventArgs.selectedForControl.siblings()[0]).find(' >  input'); //div -> first div in the same list -> input
    inputWithName.val(eventArgs.value + "'s vacation");
}

/// <summary>
/// Init input controls in the agile item row: datepickers, text-change handlers
/// </summary>
/// <param name="agileItemRow">Agile item's row</param>
function initInputControls(agileItemRow)
{
    //row Highlightning on focus
    agileItemRow.hover(
		function () { $(this).addClass('ui-state-hover'); },
		function () { $(this).removeClass('ui-state-hover'); });

    //inputs
    var inputs = agileItemRow.find('input');

    var $name = $(inputs[0]);
    var $startDate = $(inputs[1]);
    var $endDate = $(inputs[2]);

    inputs.bind("keyup change paste", createOnTextChangedCallback(agileItemRow, $name, $startDate, $endDate));

    //bind DatePickers to controls
    bindDatePickerIntervals($startDate, $endDate);
}

/// <summary>
/// Create OnTextChanged event hanlder function
/// </summary>
/// <param name="$agileItemRow">Agile item's row which contains controls</param>
/// <param name="$name">Input: name</param>
/// <param name="$startDate">Input: start date</param>
/// <param name="$endDate">Input: end date</param>
function createOnTextChangedCallback($agileItemRow, $name, $startDate, $endDate)
{
    return function () { onTextChanged($agileItemRow, $name, $startDate, $endDate); }
}

function onTextChanged($agileItemRow, $name, $startDate, $endDate)
{
    if ($agileItemRow.index() != $agileItemRow.siblings().length)//If row is not the last one -> skip
        return;

    if (isValidAgileItem($name, $startDate, $endDate))
        cloneAgileItemRow($agileItemRow);
}

/// <summary>
/// Clone a row for a new agile item
/// </summary>
/// <param name="lastRow">A row to copy from</param>
function cloneAgileItemRow(lastRow)
{
    var clone = (lastRow.hasClass("agile-item-colored")) ? agileItemColoredTemplate.clone() : vacationTemplate.clone();
    initInputControls(clone);
	clone.insertAfter(lastRow);
}

/// <summary>
/// Bind datePicker's control to "start date" and "end date"
/// </summary>
/// <param name="$startDate">Input: start date</param>
/// <param name="$endDate">Input: end date</param>
function bindDatePickerIntervals($startDate, $endDate)
{
    $startDate.datepicker({
        defaultDate: "+1w",
        dateFormat: ReleaseCycleConsts.DateFormat,
        changeMonth: true,
        numberOfMonths: 2,
        onClose: function (selectedDate)
        {
            $endDate.datepicker("option", "minDate", selectedDate);
        }
    });

    $endDate.datepicker({
        defaultDate: "+1w",
        dateFormat: ReleaseCycleConsts.DateFormat,
        changeMonth: true,
        numberOfMonths: 2,
        onClose: function (selectedDate)
        {
            $startDate.datepicker("option", "maxDate", selectedDate);
        }
    });
}

//===================================================================== Remove support functions =====================================================================//
/// <summary>
/// Init behaviour to remove items:
/// Make all agile items draggable to trash
/// </summary>
/// <param name="agileItemRowsList">List of agile items' rows</param>
function initDraggableToTrash(agileItemRowsList)
{
    //filter out template rows for creating new records
    agileItemRowsList = agileItemRowsList.not(".agile-item-template");

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
	    top: agileItemRow.offset().top + agileItemRow.height(),
	    left: agileItemRow.offset().left + agileItemRow.width() + 20
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

//===================================================================== Validation functions =====================================================================//

function isValidAgileItem($name, $startDate, $endDate)
{
    if (!isNameValid($name))
        return false;

    var startDate = tryParseDate($startDate);
    if (startDate == null)
        return false;
    
    
    var endDate = tryParseDate($endDate);
    if (endDate == null)
        return false;

    if (startDate > endDate)
        return false;

    return true;
}

function isNameValid($name)
{
    return ($name.val().trim() != "")
}

function tryParseDate($date)
{
    if ($date.val().trim() == "")
        return null;

    try
    {
        var date = $.datepicker.parseDate(ReleaseCycleConsts.DateFormat, $date.val());
        return date;
    }
    catch (ex)
    {
        return null;
    }
}