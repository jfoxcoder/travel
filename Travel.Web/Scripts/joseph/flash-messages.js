/*

Foundation 5 alert classes
            success	           
            info, secondary      
            warning, alert 

 Custom icomoon icon names (without icon- prefix)

            success success-dark
            info info-dark
            warning warning-dark
            happy happy-dark
            sad sad-dark            
*/
var FlashMessage = (function () {

    var flashBoxes = [];
    var index = -1;

    // configure options
    var opts = {
        showDuration : 2500,
        icons: {
           success: 'happy-dark',
           info: 'info-dark',
           secondary: 'info-dark',
           warning: 'sad-dark',
           alert: 'warning-dark'
        }        
    };

    $('.flash-box').each(function (i, boxEl) {
        var $box = $(boxEl);

        flashBoxes.push({
            $box: $box,
            $icon: $box.children('i').first(),
            $message: $box.children('p').first()
        });
    });

    var flash = function (message, mode) {

        if (++index == flashBoxes.length) index = 0;

        fb = flashBoxes[index];        

        // set the message text, mode/style and icon
        fb.$message.text(message);
        fb.$box.attr('class', 'flash-box alert-box ' + mode);        
        fb.$icon.attr('class', '').addClass('icon-' + opts.icons[mode]);

        
        // display flash message
        fb.$box.addClass('flash-active flash-index-' + index);
        
        // hide flash message after a few seconds
        var timeoutId = setTimeout(function (fb) {
            fb.$box.removeClass('flash-active flash-index-' + index);
        }, opts.showDuration, fb);
    };

    var showSuccess = function (message) {
        flash(message, 'success');
    };
    var showWarning = function (message) {
        flash(message, 'warning');
    };
    var showInfo = function (message) {
        flash(message, 'info');
    };
    var showAlert = function (message) {
        flash(message, 'alert');
    };
    var showSecondary = function (message) {
        flash(message, 'secondary');
    };

    // used after redirects, e.g, after a contact message
    // is sent or to welcome a new user.
    var cookieFlash = function () {
        var message = $.cookie('flash-message');
        if (message) {

            flash(message, "success"); // todo: message modes

            //$.removeCookie('name', { path: '/' }); // => true
            if (!$.removeCookie('flash-message', { path: '/' })) {
                console.error('Error removing flash message cookie.');
            }
        } 
    }


    // Through this in here for convenience. 
    var scrollToSelector = function (sel) {        
        $('html, body').animate({
            scrollTop: $(sel).offset().top
        }, 1000);
    };


    cookieFlash();

    return {
        
        flash: flash,

        happy: showSuccess,
        sad: showWarning,

        info: showInfo,
        secondary: showSecondary,
        
        alert: showAlert,

        scrollToSelector: scrollToSelector
    }
})();

