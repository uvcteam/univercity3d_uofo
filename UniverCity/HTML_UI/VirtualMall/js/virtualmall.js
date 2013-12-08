$(function () {
    $('.sub-category').click(function () {
        console.log(this.getAttribute('catid'));
        engine.call('GetBusinessSubCat', this.getAttribute('catid'));
    });

    var URL = "http://app2.univercity3d.com/univercity/GetFlashDeals";
    //Parameter passing breaks iOS so comment out hen building to iOS
    $.ajax({url: URL, success: function(flashDealData){
        console.log(flashDealData);
        AddFlashDeals(flashDealData);
    }});
});

function AddFlashDeals(flashDealData) {
    if (!flashDealData.s && flashDealData.deals.length <= 0) return;
    for(var i = 0; i < flashDealData.deals.length; ++i) {
        $('.flash-carousel').append('<div class="flash-deal">' +
            '<h1><i class="fa fa-bolt fa-lg"></i> FLASH DEAL <i class="fa fa-bolt fa-lg"></i></h1>' +
            '<div class="flash-deal-inner">' +
            '<p class="line-1">' + flashDealData.deals[i].title1 + '</p>' +
            '<p class="line-2">' + flashDealData.deals[i].title2 + '</p>' +
            '<p class="line-3">' + flashDealData.deals[i].title3 + '</p>' +
        '</div>' +
        '</div>');
    }

    $('.flash-carousel').owlCarousel({
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3],
        itemsTablet: [768, 3],
        pagination: true
    });
}

$(function () {
    var container = document.getElementById('st-container');
    $('#menu_btn').click(function () {
        container.className = 'st-container';
        classie.add(container, 'st-effect-1');
        setTimeout(function () {
            classie.add(container, 'st-menu-open');
        }, 25);
    });
    $('.st-container li').click(function (event) {
        event.stopPropagation();
        if (this.getAttribute('destination') != 'internal')
            GoToDestination(this.getAttribute('destination'));
    });
    $('.st-container').click(function (event) {
        classie.remove(document.getElementById('st-container'), 'st-menu-open');
    });
});
$(function () {
    $('#demo4').scrollbox({
        direction: 'h',
        switchItems: 1,
        distance: 250
    });
});

