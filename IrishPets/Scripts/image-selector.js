$(function () {

    if ($('.hero .gallery img').length > 1) {
        $('.hero .gallery img:first').addClass('first').addClass('active');
        $('.hero .gallery img:last').addClass('last');

        setInterval(function () {

            var active = $('.hero .gallery img.active');
            var next = active.next();
            if (active.hasClass('last')) {
                next = $('.hero .gallery img.first');
            }

            next.fadeIn(300).addClass('active');
            active.fadeOut(300).removeClass('active');

        }, 10000);
    }
});

