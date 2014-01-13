var agileItemColoredTemplate;
var vacationTemplate;
var agileItemsTrash;

$(document).ready(function () {
   
    //row removing
    agileItemsTrash = $(".agile-releaseCycle-trash");
    initTrash();

    //bind to TeamMemberPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); });

    /*------------------- Agile items' rows -------------------*/
    var agileItemRowsList = getAgileItemsRows();

    //bind datePickers and text-change handlers
    for (var c = 0; c < agileItemRowsList.length; c++)
    {        
        var agileItemRow = $(agileItemRowsList[c]);
        var isTemplate = isTemplateRow(agileItemRow);

        if (isTemplate)
            initTemplateRows(agileItemRow);//save template rows

        initInputControls(agileItemRow, isTemplate);
    }
});

/// <summary>
/// Init template rows to add new agile items
/// </summary>
/// <param name="agileItemRow">Agile item's row</param>
function initTemplateRows(agileItemRow)
{
    if (isColoredRow(agileItemRow))
        agileItemColoredTemplate = agileItemRow.clone();
    else
        vacationTemplate = agileItemRow.clone();
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
/// <param name="isTemplateRow">If a row is a template row</param>
function initInputControls(agileItemRow, isTemplateRow)
{
    //row Highlightning on focus
    agileItemRow.hover(
		function () { $(this).addClass('ui-state-hover'); },
		function () { $(this).removeClass('ui-state-hover'); });

    //bind ColorPicker or TeamMemberPicker
    if (isColoredRow(agileItemRow))
    {
        agileItemRow.find('.agile-item-colored-color').each(function () { initColorPicker($(this)); });
    }
    else
    {
        agileItemRow.find('.agile-item-vacation-teamMember').bind("click", function () { showTeamMemberPicker($(this)); });
    }

    //bind removable behaviour
    if (!isTemplateRow)
        initDraggableToTrash(agileItemRow);

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
    var clone = isColoredRow(lastRow) ? agileItemColoredTemplate.clone() : vacationTemplate.clone();
    initInputControls(clone, true /* isTemplateRow */);
    clone.insertAfter(lastRow);
    initDraggableToTrash(lastRow); // the last row to be deleted
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
/// Init behaviour to remove item:
/// Make an agile item draggable to trash
/// </summary>
/// <param name="agileItemRow">Agile item's row</param>
function initDraggableToTrash(agileItemRow)
{
    agileItemRow.draggable({
        cancel: "input, .agile-item-colored-color, .agile-item-vacation-teamMember", // clicking an [input, div with color, div with team member] won't initiate dragging
        revert: "invalid", // when not dropped, the item will revert back to its initial position
        containment: "document",
        helper: "clone",
        cursor: "move",
        start: function () { showTrash(agileItemRow); },
        stop: function () { agileItemsTrash.fadeOut(); },
    });
}

/// <summary>
/// Init trash control
/// </summary>
function initTrash()
{
    //filter out template rows for creating new records
    agileItemsTrash.droppable({
        tolerance: "touch",
        accept: function () { return getAgileItemsRows().not(".agile-item-template") },
        drop: function (event, ui)
        {
            removeAgileItem(ui.draggable);
        }
    });
}

/// <summary>
/// Show trash basket to put agile items
/// </summary>
/// <param name="agileItemRow">Agile item to remove</param>
function showTrash(agileItemRow)
{ 
    agileItemsTrash.css
	({
	    top: agileItemRow.offset().top + agileItemRow.height(),
	    left: agileItemRow.offset().left + agileItemRow.width() + 20
	});
    
    agileItemsTrash.fadeIn();
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

//===================================================================== DOM functions =====================================================================//

/// <summary>
/// If the row has a colorPicker
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
function isColoredRow(agileItemRow)
{
    return agileItemRow.hasClass("agile-item-colored");
}

/// <summary>
/// If the row is a template row
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
function isTemplateRow(agileItemRow)
{
    return agileItemRow.hasClass("agile-item-template");
}

function getAgileItemsRows()
{
    return $('.agile-releaseCycle').find(' > div > div:not(.agile-releaseCycle-header)');
}
