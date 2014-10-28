$(function () {

    // show/hide the descendent jf-toolbar when hovering in it's owner.    

    $('[data-jf-toolbar-owner]').hover(function () {
        $(this).find('.jf-toolbar').addClass('jf-toolbar-active');
    }, function () {
        $(this).find('.jf-toolbar').removeClass('jf-toolbar-active');
    });
});

