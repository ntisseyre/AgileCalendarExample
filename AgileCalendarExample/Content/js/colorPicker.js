var colorPicker;
var isInsideColorPicker = false;

var pickingColorForControl;

//Const values to be used in javascript
var ColorPickerConsts =
{
    EmptyColor: "slonic-calendar-colors-none",
    ClassNamePrefix: "slonic-calendar-colors-"
};

$(document).ready(function ()
{
    colorPicker = $('#colorPicker');
    colorPicker.mouseleave(function () { leaveColorPicker(); });
    colorPicker.mouseover(function () { hoverColorPicker(); });

    // Color Grid init
    colorPicker.find(' > div > div').bind("click", function () { setColor($(this)); });
});

/// <summary>
/// Init Html element: attach colorPicker behaviour
/// </summary>
/// <param name="control">Html element</param>
function initColorPicker(control) {
    if (control.data('isInited'))
        return;

    control.data('isInited', true);

    //adding tabIndex attribute to support keydown event
    if (!control.attr('tabindex')) control.attr('tabindex', '0');

    control.bind("click", function () { showColorPicker(control); });
    control.bind("keydown", function (e) { if (e.keyCode == 27) { hideColorPicker(); } }); //if Esc - hide control
    control.bind("blur", function () { if (!isInsideColorPicker) { hideColorPicker(); } });
}

/// <summary>
/// Show a ColorPicker-control
/// </summary>
/// <param name="control">Html-element for setting a color</param>
function showColorPicker(control) {

    pickingColorForControl = control;

    this.colorPicker.css
	({
	    top: control.offset().top + control.height() + 4,
	    left: control.offset().left
	});
    this.colorPicker.show();
}

/// <summary>
/// Hide a ColorPicker-control
/// </summary>
function hideColorPicker()
{
    this.colorPicker.hide();
}

/// <summary>
/// Set color to the current input control
/// </summary>
/// <param name="selectedColorCell">Html-element with a selected color</param>
function setColor(selectedColorCell)
{
    var colorName = selectedColorCell.attr('title');
    var classNames = $.map(pickingColorForControl.attr('class').split(/\s+/), function (value, index)
    {
        if (value.indexOf(ColorPickerConsts.ClassNamePrefix) == 0)
            return ColorPickerConsts.ClassNamePrefix + colorName;
        else
            return value;
    }).join(' ');

    pickingColorForControl.attr('class', classNames);
    hideColorPicker();

    $(document).trigger('colorSelected', { selectedForControl: pickingColorForControl, value: colorName });
}

/// <summary>
/// Set a flag that Color Picker is in focus right now.
/// </summary>
function hoverColorPicker()
{
    isInsideColorPicker = true;
}

/// <summary>
/// Set a flag that Color Picker is not in focus right now
/// </summary>
function leaveColorPicker()
{
    isInsideColorPicker = false;
}