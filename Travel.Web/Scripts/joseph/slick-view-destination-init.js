$(function () {
    $('.carousel').slick({

        // for faster initial page load
        lazyLoad: 'progressive',

        // enable slide selection
        dots: true,
        
        // for portable devices
        swipe: true,

        autoplay: true,
        speed: 2500,
        infinite: true,
        fade: true,        

        // ensure the slides change
        pauseOnHover: false        
    });
});