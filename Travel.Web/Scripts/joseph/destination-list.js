$(function () {
   DestinationHighlighter.init();
   DestinationAjaxLoader.init();
});

DestinationAjaxLoader = (function () {

    var $destinationList = $('#destinations'),
        $loadBtn = $('#load-btn'),
        loadUrl = $loadBtn.data('url'),
        pageNum = Travel.pageConfig.page;


    var init = function () {       
        $loadBtn.on('click', Foundation.utils.debounce(injectDestinations, 300, true));
    };


    var scrollToDestination = function ($destination) {        
        $('html, body').animate({
            scrollTop: $destination.offset().top
        }, 1000);
    };


    var injectDestinationsSuccess = function (html) {
        var $newDestinationItems = $(html);
        $destinationList.append($newDestinationItems);
        var $first = $newDestinationItems.first();
        if ($first.hasClass('last-page')) {            
            $loadBtn.hide();
        } 
        scrollToDestination($first);
    };
 

    var injectDestinations = function () {
        var currentDestinations = $('.destination', $destinationList).addClass('destination-loading');
        $loadBtn.addClass('loading');

        var url = loadUrl + '?sort=' + $('#sort-val').val() + "&page=" + (++pageNum);
        var country = $('#country-val').val();
        if (country) {
            url += '&country=' + country;
        }

        $.ajax({
            url: url,
            type: 'get'
        })
        .success(injectDestinationsSuccess)
        .fail(function () {
            console.error(arguments);
        })
        .always(function () {
            DestinationHighlighter.reset();
            $loadBtn.removeClass('loading');
            setTimeout(function () {
                currentDestinations.removeClass('destination-loading')
            }, 1000);
        });
    };

    return {
        init: init,
    };

})();


DestinationHighlighter = (function () {

    var $slideshowLabel = $('.slideshow-label').first(),
        $maximage = $('#maximage'),
        _$selDest = null,
        _$obscured;

    var injectSlideshowImages = function (html, textStatus, jqXHR) {
        $maximage.html(html).maximage();
        $slideshowLabel.text(_$selDest.data('name'));
    };

    var selectDestination = function ($d) {
        if (_$selDest) {
            
            _$selDest.removeClass('destination-highlighted');
        }
        
        _$selDest = $d;

        updateSlideshow();

        _$selDest.addClass('destination-highlighted')
                 .removeClass('destination-obscured');

        $obscured = $('.destination').not(_$selDest).addClass('destination-obscured');
    };

    
    var deselectDestination = function ($d) {
        reset();
    }


    var updateSlideshow = function () {
        var slideShowUrl = _$selDest.data('slideshow-url');
        $.get(slideShowUrl, null, injectSlideshowImages);
    };


    var reset = function () {
        $_selDest = null;
        $('.destination').removeClass('destination-highlighted destination-obscured');

        // clear the slideshow
        //$maximage.html('').maximage();
        //$slideshowLabel.text('');      
    };


    var init = function () {
        var $destinations = $('#destinations');

        $destinations.on('click', '.destination', function () {
            $slideshowLabel.hide();

            var $d = $(this);

            if (_$selDest) {

                var same = $d.data('destination') === _$selDest.data('destination');

                if (same && $d.hasClass('destination-highlighted')) {
                    deselectDestination($d);
                    return;
                } else {

                }
            }
            selectDestination($d);
        }).on('mouseover', '.destination-highlighted', function () {            
            $obscured.addClass('destination-obscured');

        }).on('mouseout', '.destination-highlighted', function () {
            $obscured.removeClass('destination-obscured');
        });
      
        $destinations.on('click', '.moreLink', function (e) {
            e.stopPropagation();
        });
   }; 

    return {
        init: init,
        reset: reset
    };
})();
