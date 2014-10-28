/* Replaces the default MVC 5 validation summary with
 * flash messages (requires flash-messages.js)
 */
$(function () {

    var $validationSummary = $('.validation-summary-errors');

    if ($validationSummary.length > 0) {
        var errorMessages = [];
        ('li', $validationSummary).each(function (index, errorLi) {
            errorMessages.push($(errorLi).text());
        });
        var msg = errorMessages.join(', ');

        FlashMessage.sad(msg);
    }
});