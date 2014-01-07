$(document).ready(function () {
   
    var agileReleaseCycle = $('.agile-releaseCycle');

    //bind ColorPicker
    agileReleaseCycle.find(' > div > div.agile-item-colored > div:last-child').each(function () { initColorPicker($(this)); });
});