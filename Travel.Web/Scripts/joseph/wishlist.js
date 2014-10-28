$(function () {
 
    var wishlistDebounceDelay = 1500;

    var toggleWishlist = function (e) {
       
        var $btn = $(this);
        var $destination = $btn.closest('[data-destination]');
        var name = $btn.data('name');

        if (!$btn.hasClass('wish-guest-btn')) {

            $btn.addClass('spin');

            $.ajax({
                url: $btn.data('url'),
                type: 'put'
            }).success(function (data) {

                var destination = $btn.data('name');

                if (data.added === true) {
                    var title = $btn.data('in-wishlist-message');
                    var message = name + ' added to wishlist';
                    $btn.addClass('wish-on-btn icon-wish-on').removeClass('wish-off-btn icon-wish-off');
                    $destination.addClass('in-wishlist');
                }
                else {
                    var title = $btn.data('not-in-wishlist-message');
                    var message = name + ' removed from wishlist';
                    $btn.addClass('wish-off-btn icon-wish-off').removeClass('wish-on-btn icon-wish-on');

                    $destination.removeClass('in-wishlist');
                }

                $btn.attr('title', title);
                $btn.qtip('option', 'content.text', title);
                
                FlashMessage.happy(message);

                if (Travel.isWishlistPage) {

                    if (data.added === false) {
                        $destination.fadeOut(500, function () {
                            $destination.parent().remove();
                        });
                    }

                    if (data.wishes === 0) {
                        window.location = $('#homeUrl').val();
                        return;
                    }
                }
                
                var $wishlistButtonItem = $('#wishlist-button-item').first();

                if (data.wishes === 0) {
                  //  console.log('removing wishlist button');
                    $wishlistButtonItem.fadeOut();
                } else {
                   // console.log('revealing wishlist button');
                    $wishlistButtonItem.fadeIn();
                }
                


            }).fail(function () {
                FlashMessage.sad('Sorry, there wa a problem updating your wishlist');
            }).always(function () {
                $btn.removeClass('spin');
            });

        } else {
            FlashMessage.happy("Please sign in to access your wishlist.");
        }
        return false;
    }

    var $wishBtns = $('.wish-btn');
    if ($wishBtns.length == 1) {
        $wishBtns.on('click', toggleWishlist);
    } else if ($wishBtns.length > 1) {
        $('[wish-btn-owner]').on('click', '.wish-btn', toggleWishlist);
    } else {
        console.error('No wish buttons.');
    }

    if (Travel.isWishlistPage) {
        $('#load-btn').remove();
    }    
});