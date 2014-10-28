$(function () {
    TravelFinder.init();
});



var TravelFinder = (function () {

    var $find, baseUrl;
    

    var init = function () {        
        $find = $('#terms');
        baseUrl = $find.data('url');

        $find.on('keyup', requestFind);
    };

    var requestFind =  Foundation.utils.throttle(function(e){
        console.log('search', baseUrl + "?" + $find.serialize());
        $.ajax({
            url : baseUrl + "?" + $find.serialize()
        }).done(requestFindDone);

    }, 300);

    var requestFindDone = function (html) {
        console.log(html);
    };

    var requestFindFail = function (jqXHR, textStatus, errorThrown) {
        FlashMessage.sad(errorThrown);
        console.error(arguments);
    };


    return {
        init : init
    };

})();