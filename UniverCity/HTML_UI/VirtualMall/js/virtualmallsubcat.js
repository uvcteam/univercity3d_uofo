$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call("ReadyForCategories");
});

engine.on('CreateCategory', function (cat) {
    console.log("Here");
    document.getElementById('container').innerHTML += '<div class="category">' +
        '<h4>' + cat + '</h4>' +
        '<div class="owl-carousel"></div>'
        '</div>';
})

engine.on('PopulateCategory', function (name, desc, id, image, index, hasAd) {
    console.log('HERE');
    if(hasAd === true)
        document.getElementsByClassName('owl-carousel')[index].innerHTML += '<div class="business">' +
            '<header>' + name + '</header>' +
            '<figure>' +
            '<img src="data:image/png;base64,' + image + '" />' +
            '<p>' + desc + '</p>' +
            '</figure>' +
            '<button type="button" busid="' + id + '"class="btn btn-see-more">See More</button>' +
            '<a busid="'+ id +'">Contact Info</a></div>';
    else
        document.getElementsByClassName('owl-carousel')[index].innerHTML += '<div class="business">' +
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
    $('.owl-carousel').owlCarousel({
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
