$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call("ReadyForCategories");
});

engine.on('CreateCategory', function (cat) {
    console.log("Here");
    document.getElementById('container').innerHTML += '<div class="category"><h4>' + cat + '</h4></div>';
})

engine.on('PopulateCategory', function (name, desc, id, image, index) {
    document.getElementsByClassName('category')[index].innerHTML += '<div class="business" busid="' + id + '"><header>' + name + '</header><figure><img src="data:image/png;base64,' + image + '" /><figcaption>' + desc + '</figcaption></figure></div>'
})

engine.on('CreateEmptyCategory', function(index) {

    document.getElementsByClassName('category')[index].innerHTML += '<p>There are currently no businesses in this category.</p>'
    
})

engine.on('AttachEventToBusinesses', function () {
    $(".business").click(function () {
        console.log(this.getAttribute('busid'));
        engine.call('LoadAdPlayer', this.getAttribute('busid'));
    });
})

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