$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call("ReadyForCategories");
});

engine.on('CreateCategory', function (cat) {
    console.log("Here");
    document.getElementById('container').innerHTML += '<div class="category"><h4>' + cat + '</h4></div>';
})

engine.on('PopulateCategory', function (name, desc, id, image, index) {
    document.getElementsByClassName('category')[index].innerHTML += '<div class="business"><header>' + name + '</header><figure><img src="data:image/png;base64,' + image
        + '" /><figcaption>' + desc + '</figcaption></figure><button type="button" busid="' + id + '"class="btn btn-see-more">See More</button><a href="businesscard.html?id='+ id
        +'">Contact Info</a></div>';
})

engine.on('CreateEmptyCategory', function(index) {

    document.getElementsByClassName('category')[index].innerHTML += '<p>There are currently no businesses in this category.</p>'
    
})

engine.on('AttachEventToBusinesses', function () {
    $(".business > button").click(function () {
        console.log(this.getAttribute('busid'));
        engine.call('SetBusinessID', this.getAttribute('busid'));
    });
})
