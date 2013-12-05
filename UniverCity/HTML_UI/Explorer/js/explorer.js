/**
 * Created by Avengix on 12/1/13.
 */
$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call('AddBusinesses');
    $(".st-container").click(function(){
        engine.call("MenuClosed");
    })
});
engine.on('PopulateCategory', function (name, desc, id, image, hasAd) {
    console.log( $('#business-list'));
    console.log(document.getElementById('business-list'));
    if (hasAd === true)
        document.getElementById('business-list').innerHTML += '<li><div class="business"><header>' + name + '</header><figure><img src="data:image/png;base64,' + image
            + '" /><figcaption>' + desc + '</figcaption></figure><button type="button" busid="' + id + '"class="btn btn-see-more">See More</button><a busid="'+ id
            +'">Contact Info</a></div></li>';
    else
        document.getElementById('business-list').innerHTML += '<li><div class="business"><header>' + name + '</header><figure><img src="data:image/png;base64,' + image
            + '" /><figcaption>' + desc + '</figcaption></figure><a busid="'+ id
            +'">Contact Info</a></div></li>';
})

engine.on('ClearBusinessList', function(){
    $(".st-container").unbind('click');
    document.getElementById('business-list').innerHTML = "";

    $('.st-container').click(function (event) {
        console.log("click");
        classie.remove(document.getElementById('st-container'), 'st-menu-open');
    });
    $(".st-container").click(function(){
        engine.call("MenuClosed");
    })
})

engine.on('AttachEventToBusinesses', function () {
    $(".business > button").click(function (ev) {
        ev.stopPropagation();
        engine.call('LoadAdPlayer', this.getAttribute('busid'));
        engine.call('OnBusinessWasSelected');
    });
    $(".business > a").click(function (ev) {
        ev.stopPropagation();
        engine.call('SetBusinessIDForCard', this.getAttribute('busid'));
        engine.call('OnBusinessWasSelected');
    });

    $(".st-container").click(function(){
        engine.call("CloseBusinessList");
    })
})

engine.on('OpenMenu', function(){
    console.log("OPenMenu");
    OpenMenu();
})
