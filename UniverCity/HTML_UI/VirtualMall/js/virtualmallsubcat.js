$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call("ReadyForCategories");
    engine.call('LoadFlashDeals');

});

engine.on('LoadFlashDeals', function(URL){

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
        itemsTablet: [768, 3],
        pagination: true,
        autoPlay: 3000
    });
}

engine.on('CreateCategory', function (cat) {
    console.log("Here");
    document.getElementById('container').innerHTML += '<div class="category">' +
        '<h4>' + cat + '</h4>' +
        '<div class="owl-carousel subcat-carousel"></div>'
        '</div>';
})

engine.on('PopulateCategory', function (name, desc, id, image, index, hasAd) {
    console.log('HERE');
    if(hasAd === true)
        document.getElementsByClassName('subcat-carousel')[index].innerHTML += '<div class="business">' +
            '<header>' + name + '</header>' +
            '<figure>' +
            '<img src="data:image/png;base64,' + image + '" />' +
            '<p>' + desc + '</p>' +
            '</figure>' +
            '<button type="button" busid="' + id + '"class="btn btn-see-more">See More</button>' +
            '<a busid="'+ id +'">Contact Info</a></div>';
    else
        document.getElementsByClassName('subcat-carousel')[index].innerHTML += '<div class="business">' +
            '<header>' + name + '</header>' +
            '<figure>' +
            '<img src="data:image/png;base64,' + image + '" />' +
            '<p>' + desc + '</p>' +
            '</figure>' +
            '<a busid="'+id +'">Contact Info</a></div>';
})

engine.on('CreateEmptyCategory', function(index) {

    document.getElementsByClassName('category')[index].innerHTML += '<p>There are currently no businesses in this category.</p>'
    
})

engine.on('AttachEventToBusinesses', function () {
    $('.subcat-carousel').owlCarousel({
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3],
        itemsTablet: [768, 3],
        pagination: true
    });


    $(".business > button").click(function () {
        console.log(this.getAttribute('busid'));
        engine.call('SetBusinessID', this.getAttribute('busid'));
    });
    $(".business > a").click(function () {
        console.log(this.getAttribute('busid'));
        engine.call('SetBusinessIDForCard', this.getAttribute('busid'));
    });
})
