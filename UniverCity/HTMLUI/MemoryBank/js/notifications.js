/**
 * Created by Jacob on 14/01/17.
 */
// Web Functions.
var lastEventID = -1;

$(function() {
    console.log("Searching for saved invitations.");
    engine.call("RetrieveInvitations");
});

function ShowEvent(caller) {
    var element = $(caller);
    lastEventID = element.attr('event-id');
    $('.m-event-title').html(element.attr('event-name'));
    $('.m-event-who').html(element.attr('event-who'));
    $('.m-event-what').html(element.attr('event-what'));
    $('.m-event-where').html(element.attr('event-where'));
    $('.m-event-date').html(element.attr('event-date'));
    $('.m-event-when').html(element.attr('event-time'));
    $('#myModal').modal('show');
}

function JoinEvent() {
    console.log("Join event " + lastEventID);
    engine.call("JoinEvent", lastEventID);
}

// Invoked by Unity3D.
engine.on("AddInvitation", function(name, date, time, desc, who, where, id){
    var newEvent = '';
    newEvent += '<div class="business" onclick="ShowEvent(this)"';
    newEvent += ' event-name="' + name + '"';
    newEvent += ' event-who="' + who + '"';
    newEvent += ' event-what="' + desc + '"';
    newEvent += ' event-date="' + date + '"';
    newEvent += ' event-time="' + time + '"';
    newEvent += ' event-where="' + where + '"';
    newEvent += ' event-id="' + id + '">';
    newEvent += '<span class="event-name">' + name + '</span>';
    newEvent += '<span class="event-date">' + date + '</span>';
    newEvent += '<span class="event-time">' + time + '</span>';
    newEvent += '<p>' + desc + '</p>';
    newEvent += '</div>';
    console.log('Creating event ' + name);
    $('#invitations').append(newEvent);
    ModalEffects();
});

engine.on("AddBusiness", function (name, desc, id, image) {
    console.log("ADDING BUSINESS: " + name);
    $('#business-c').append('<div class="business" busid="'+id+'">' +
        '<header>' + name + '</header>' +
        '<figure>' +
        '<img src="data:image/png;base64,' + image + '" />' +
        '<p>' + desc + '</p>' +
        '</figure></div>');
});

engine.on("BusinessesFinished", function() {
    console.log("BUSINESSES DONE!");
    $('#business-c').owlCarousel({
        items: 3,
        itemsDesktop: [1199, 3],
        itemsDesktopSmall: [979, 3],
        itemsTablet: [1199, 3],
        itemsMobile:[960, 1],
        pagination: true
    });

    $('.business').click(function(){
        engine.call("BusinessClicked", this.getAttribute('busid'));
    })
});

engine.on("InvitationsFinished", function() {
    console.log("INVITATIONS DONE!");
    engine.call("RetrieveBusinesses");
});

engine.on("NoInvitations", function() {
    console.log("NO INVITATIONS!");
    $("#invitations").html('<h1>You have no new notifications.</h1>');
    engine.call("RetrieveBusinesses");
});

engine.on("NoBusinesses", function() {
    console.log("NO BUSINESSES!");
   $("#businesses").html('<h1>You have no saved businesses.</h1>');
});