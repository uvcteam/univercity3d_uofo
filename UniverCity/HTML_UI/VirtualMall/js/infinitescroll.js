$(window).scroll(function() {
    if ($(window).scrollTop() >= $(document).height() - $(window).height()) {
        $(window).scrollTop(1);
    }
    else if ($(window).scrollTop() == 0) {
        $(window).scrollTop($(document).height() - $(window).height() - 1);
    }
});