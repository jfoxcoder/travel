var TravelTooltips = (function (document) {
  
    var tooltips;

    $(document).on('mouseover', '[title]', function (event) {
        tooltips = $(this).qtip({




            overwrite: false,
            content: $(this).attr('title'),
            style: { classes: 'qtip-jtools' },
            position: {
                my: 'bottom center',
                at: 'top center',
                target: $(this)
            },
            show: {
                event: event.type,
                ready: true
            },
            events: {
                show: function (event, api) {
                    var $el = $(api.elements.target[0]);

                    if ($el.hasClass('right-tip')) {
                        $el.qtip('option', 'position.my', 'left center');
                        $el.qtip('option', 'position.at', 'right center');
                    } 
                }
            }
        }, event);      
    });
  
})(document);


