var category;
var index = 0;
var interval;

$(document).ready(function(){

    engine.call('LoadFlashDeals');

    $(document).ready(function() {


        category = $('.category-carousel');


        interval = setInterval(function() {            

            if(index > category.length)
                clearInterval(interval);
            else
               $(category[index++]).owlCarousel({
                    itemsDesktop: [1199, 4],
                    itemsDesktopSmall: [979, 4],
                    itemsTablet: [1199, 4],
                    pagination: true,
                    itemsMobile: [640, 1]
                });


        }, 100);

    });

})

engine.on('LoadFlashDeals', function(URL){

    $(function () {
        $('.sub-category').click(function () {
            console.log(this.getAttribute('catid'));
            engine.call('GetBusinessSubCat', this.getAttribute('catid'));
        });

        var URL = "http://app2.univercity3d.com/univercity/GetFlashDeals";
        //Parameter passing breaks iOS so comment out hen building to iOS

    });

    console.log(URL);
    $.ajax({url: URL + 'GetFlashDeals', success: function(flashDealData){
        console.log(flashDealData);
        AddFlashDeals(flashDealData);

        $('.flash-deal').click(function(){
            console.log($(this).data('id'));
            engine.call('SetFlashDealID', $(this).data('id'));
        });
    }});
})


function AddFlashDeals(flashDealData) {

    if (!flashDealData.s && flashDealData.deals.length <= 0) return;
    engine.call('ClearFlashDeals');
    for(var i = 0; i < flashDealData.deals.length; ++i) {
        console.log(flashDealData.deals[i].page);
        $('.flash-carousel').append('<div data-id="' + flashDealData.deals[i].id +'" class="flash-deal">' +
            '<i class="fa fa-bolt fa-lg"></i><h1> FLASH DEAL </h1><i class="fa fa-bolt fa-lg"></i>' +
            '<p class="line-1">' + flashDealData.deals[i].title1 + '</p>' +
            '<p class="line-2">' + flashDealData.deals[i].title2 + '</p>' +
            '<p class="line-3">' + flashDealData.deals[i].title3 + '</p>' +
        '</div>');
        engine.call('SetJsonString', JSON.stringify(flashDealData.deals[i].page), flashDealData.deals[i].id);
    }

    $('.flash-carousel').owlCarousel({
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3],
        itemsTablet: [1199, 3],
        itemsMobile:[960, 1],
        pagination: true,
        autoPlay: 3000
    });
}

