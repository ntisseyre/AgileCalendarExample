var agileItemColoredTemplate;
var vacationTemplate;
var agileItemsTrash;

$(document).ready(function () {
   
    //row removing
    agileItemsTrash = $(".agile-releaseCycle-trash");
    initTrash();

    $(document).bind("colorSelected", function (event, data) { onColorSelected(data); }); //bind to colorPicker's change event
    $(document).bind("teamMemberSelected", function (event, data) { onTeamMemberSelected(data); }); //bind to TeamMemberPicker's change event

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
/// Handler of an event "Color selected from a grid"
/// </summary>
/// <param name="eventArgs">Args of an event: { selectedForControl: control that triggered an event, value: color name }</param>
function onColorSelected(eventArgs)
{
    onPickerSelectedBase(eventArgs.selectedForControl);
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

    onPickerSelectedBase(eventArgs.selectedForControl);
}

/// <summary>
/// Handler of any picker's event
/// </summary>
/// <param name="control">Control that invoked an event</param>
function onPickerSelectedBase(control)
{
    agileItemRow = control.parent();
    addNewRowIfRequired(agileItemRow);
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
        initColorPicker(getColorPicker(agileItemRow));
    }
    else
    {
        getTeamMemberPicker(agileItemRow).bind("click", function () { showTeamMemberPicker($(this)); });
    }

    //bind removable behaviour
    if (!isTemplateRow)
        initDraggableToTrash(agileItemRow);

    //inputs
    var data = getAgileItemData(agileItemRow);
    for (var item in data)
        data[item].bind("keyup change paste", createOnTextChangedCallback(agileItemRow));

    //bind DatePickers to controls
    bindDatePickerIntervals(data.startDate, data.endDate);
}

/// <summary>
/// Create OnTextChanged event hanlder function
/// </summary>
/// <param name="$agileItemRow">Agile item's row which contains controls</param>
/// <returns>Callback function to handle Text Changed event</returns>
function createOnTextChangedCallback($agileItemRow)
{
    return function () { addNewRowIfRequired($agileItemRow); }
}

/// <summary>
/// Function checks if a new row is required to be added. If yes - adss a new row
/// </summary>
/// <param name="addRowAfterMe">Agile item's row which contains controls</param>
function addNewRowIfRequired(addRowAfterMe)
{    
    if (addRowAfterMe.index() != addRowAfterMe.siblings().length)//If row is not the last one -> skip
        return;

    addRowAfterMe.removeClass("agile-item-template");
    cloneAgileItemRow(addRowAfterMe);
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
        accept: ".agile-item-colored, .agile-item-vacation",
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

function validateAgileReleaseCycle()
{
    var agileItemRowsList = getAgileItemsRows();
    for (var c = 0; c < agileItemRowsList.length; c++)
    {
        var agileItemRow = $(agileItemRowsList[c]);

        if (isTemplateRow(agileItemRow))
            continue;

        var data = getAgileItemData(agileItemRow);

        if (!isValidAgileItem(agileItemRow, data.name, data.startDate, data.endDate, true /* if show error message*/))
        {
            return false;
        }
    }

    return true;
}

/// <summary>
/// Check whether an agile item is valid
/// </summary>
/// <param name="$agileItemRow">Agile item's row which contains controls</param>
/// <param name="$name">Input: name</param>
/// <param name="$startDate">Input: start date</param>
/// <param name="$endDate">Input: end date</param>
/// <param name="ifShowErrorMessage">Flag that indicates if an error message should be shown</param>
/// <returns>True - is valid, False - not valid</returns>
function isValidAgileItem(agileItemRow, $name, $startDate, $endDate, ifShowErrorMessage)
{
    if (!isNameValid($name))
    {
        if (ifShowErrorMessage)
            showWarning(ReleaseCycleErrors.EmptyName, $name);

        return false;
    }

    //=============== Start date ===============
    var startDate = tryParseDate($startDate);
    if (startDate.value == null)
    {
        if (ifShowErrorMessage)
            startDate.errorCallback();

        return false;
    }
    
    //=============== End date ===============
    var endDate = tryParseDate($endDate);
    if (endDate.value == null)
    {
        if (ifShowErrorMessage)
            endDate.errorCallback();

        return false;
    }

    //=============== Date interval ===============
    if (startDate.value > endDate.value)
    {
        if (ifShowErrorMessage)
            showWarning(ReleaseCycleErrors.InvalidDateInterval, $startDate);

        return false;
    }

    if (isColoredRow(agileItemRow))
    {
        var colorPicker = getColorPicker(agileItemRow);
        if (isColorPickerEmpty(colorPicker))
        {
            if (ifShowErrorMessage)
                showWarning(ReleaseCycleErrors.EmptyColorPicker, colorPicker);

            return false;
        }
    }
    else
    {
        var teamMemberPicker = getTeamMemberPicker(agileItemRow);
        if (isTeamMemberEmpty(teamMemberPicker))
        {
            if (ifShowErrorMessage)
                showWarning(ReleaseCycleErrors.EmptyTeamMemberPicker, teamMemberPicker);

            return false;
        }
    }

    return true;
}


/// <summary>
/// Check whether a name is valid
/// </summary>
/// <param name="$name">Input: name</param>
/// <returns>True - is valid, False - not valid</returns>
function isNameValid($name)
{
    return ($name.val().trim() != "")
}

/// <summary>
/// Try to parse a day, if success - date object, if no - null
/// </summary>
/// <param name="$date">Input: datepicker</param>
/// <returns>If success - date object, if no - null</returns>
function tryParseDate($date)
{
    if ($date.val().trim() == "")
        return getValidationResult(null, function () { showWarning(ReleaseCycleErrors.EmptyDate, $date); });

    try
    {
        var date = $.datepicker.parseDate(ReleaseCycleConsts.DateFormat, $date.val());
        return getValidationResult(date, null);
    }
    catch (ex)
    {
        return getValidationResult(null, function () { showWarning(ReleaseCycleErrors.InvalidDateFormat, $date); });
    }
}

function getValidationResult(value, errorCallback)
{
    var result = [];

    result["value"] = value;
    result["errorCallback"] = errorCallback;

    return result;
}

/// <summary>
/// Show a validation error
/// </summary>
/// <param name="message">Error message</param>
/// <param name="failedControl">Failed control to focus</param>
function showWarning(message, failedControl) {
    alert(message);
    failedControl.focus();
}

//===================================================================== Save functions =====================================================================//

/// <summary>
/// Get Agile Release Cycle in JSON format
/// </summary>
/// <returns>JSON object for the agile release cycle</returns>
function getJsonAgileReleaseCycle()
{
    var result = {};  

    result["Planning"] = getJsonPlanning();
    result["Sprints"] = getJsonAgileItemsList("sprints");
    result["Holidays"] = getJsonAgileItemsList("holidays");
    result["Vacations"] = getJsonAgileItemsList("vacations");

    return result;
}

/// <summary>
/// Get planning in JSON format
/// </summary>
/// <returns>JSON object for the planning</returns>
function getJsonPlanning() {
    var planning = $('.agile-releaseCycle').find(' > div.agile-releaseCycle-planning > div:not(.agile-releaseCycle-header)');
    return getJsonAgileItem(planning);
}

/// <summary>
/// Get agile items' list in JSON format
/// </summary>
/// <param name="agileItemName">Name of the agile item</param>
/// <returns>JSON object for any list of agile items</returns>
function getJsonAgileItemsList(agileItemName)
{
    var result = [];
    var agileItems = $('.agile-releaseCycle').find(' > div.agile-releaseCycle-' + agileItemName + ' > div:not(.agile-releaseCycle-header)');

    for (var c = 0; c < agileItems.length - 1; c++) //skip the last element, because it is always a template to enter a new data
        result.push(getJsonAgileItem($(agileItems[c])));

    return result;
}

/// <summary>
/// Get agile item in JSON format
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>JSON object for any agile item</returns>
function getJsonAgileItem(agileItemRow)
{
    var data = getAgileItemData(agileItemRow);

    if (isColoredRow(agileItemRow))
        return { Name: data.name.val(), StartDate: data.startDate.val(), EndDate: data.endDate.val(), Color: getSelectedColor(agileItemRow) };
    else
        return { Name: data.name.val(), StartDate: data.startDate.val(), EndDate: data.endDate.val(), TeamMemberIcon: getSelectedTeamMember(agileItemRow) };
}
//===================================================================== DOM navigation functions =====================================================================//

/// <summary>
/// If the row has a colorPicker
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>True - row contains colorPicker, False - no colorPicker</returns>
function isColoredRow(agileItemRow)
{
    return agileItemRow.hasClass("agile-item-colored");
}

/// <summary>
/// Get a colorPicker for the agile item
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>ColorPicker</returns>
function getColorPicker(agileItemRow)
{
    return agileItemRow.find('.agile-item-colored-color');
}

/// <summary>
/// Get a colorPicker value for the agile item
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>colorPicker value</returns>
function getSelectedColor(agileItemRow)
{
    var classNames = getColorPicker(agileItemRow).attr('class').split(/\s+/);
    for(var c = 0; c < classNames.length; c++)
        if (classNames[c].indexOf(ColorPickerConsts.ClassNamePrefix) == 0)
            return classNames[c].replace(ColorPickerConsts.ClassNamePrefix, "");

    return "";
}

/// <summary>
/// If the colorPicker is empty
/// </summary>
/// <param name="colorPickerDiv">ColorPicker div</param>
/// <returns>True - is empty: no color selected, False - color is selected</returns>
function isColorPickerEmpty(colorPickerDiv)
{
    return colorPickerDiv.hasClass(ColorPickerConsts.EmptyColor);
}

/// <summary>
/// Get a teamMemberPicker for the agile item
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>TeamMemberPicker</returns>
function getTeamMemberPicker(agileItemRow) {
    return agileItemRow.find('.agile-item-vacation-teamMember');
}

/// <summary>
/// If the teamMemberPicker is empty
/// </summary>
/// <param name="teamMemberPickerDiv">TeamMemberPicker div</param>
/// <returns>True - is empty: no team member selected, False - team member is selected</returns>
function isTeamMemberEmpty(teamMemberPickerDiv)
{
    return teamMemberPickerDiv.find(" > img ").attr("src").indexOf("none") >= 0;
}

/// <summary>
/// Get a teamMemberPicker value for the agile item
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>TeamMemberPicker value</returns>
function getSelectedTeamMember(agileItemRow)
{
    var imgSrc = getTeamMemberPicker(agileItemRow).find(" > img ").attr("src");
    return imgSrc.substring(imgSrc.lastIndexOf("/") + 1, imgSrc.lastIndexOf("."));
}

/// <summary>
/// If the row is a template row
/// </summary>
/// <param name="agileItemRow">Agile item row</param>
/// <returns>True - is template row, False - not a template row</returns>
function isTemplateRow(agileItemRow)
{
    return agileItemRow.hasClass("agile-item-template");
}

/// <summary>
/// Get all agile items' rows
/// </summary>
/// <returns>A list of agile items' rows</returns>
function getAgileItemsRows()
{
    return $('.agile-releaseCycle').find(' > div > div:not(.agile-releaseCycle-header)');
}

function getAgileItemData(agileItemRow)
{
    var result = [];
    var inputs = agileItemRow.find('input');

    result["name"] = $(inputs[0]);
    result["startDate"] =$(inputs[1]);
    result["endDate"] = $(inputs[2]);

    return result;
}