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
    var ele = $('.noinv');
    if ($('.noinv')) $('.noinv').remove();
    var newEvent = '';
    newEvent += '<div class="event" onclick="ShowEvent(this)"';
    newEvent += ' event-name="' + name + '"';
    newEvent += ' event-who="' + who + '"';
    newEvent += ' event-what="' + desc + '"';
    newEvent += ' event-date="' + date + '"';
    newEvent += ' event-time="' + time + '"';
    newEvent += ' event-where="' + where + '"';
    newEvent += ' event-id="' + id + '">';
    newEvent += '<h4>' + name + ' <small>' + date + ' at ' + time + '</small></h4>';
    newEvent += '<p>' + desc + '</p>';
    newEvent += '</div>';
    $('#invitations').append(newEvent);
    ModalEffects();
});

engine.on("AddBusiness", function (name, desc, id, image) {
    var ele = $('.nosav');
    if ($('.nosav')) $('.nosav').remove(); 
    $('#business-c').append(
        '<div class="business text-left" busid="'+id+'">' +
            '<dl>' +
                '<dt>' + name + '</dt>' +
                '<dd class="clearfix">' +
                    '<img src="data:image/png;base64,' + image + '" class="left" />' +
                    '<p class="right">' + desc + '</p>' +
                '</dd>' +
            '</dl>' +
        '</div>');
});

engine.on("BusinessesFinished", function() {
    console.log("BUSINESSES DONE!");
    // $('#business-c').owlCarousel({
    //     items: 3,
    //     itemsDesktop: [1199, 3],
    //     itemsDesktopSmall: [979, 3],
    //     itemsTablet: [1199, 3],
    //     itemsMobile:[960, 1],
    //     pagination: true
    // });

    $('.business').click(function(){
        engine.call("BusinessClicked", this.getAttribute('busid'));
    })
});

engine.on("InvitationsFinished", function() {
    console.log("INVITATIONS DONE!");
});

engine.on("NoInvitations", function() {
    console.log("NO INVITATIONS!");
});

engine.on("NoBusinesses", function() {
    console.log("NO BUSINESSES!");
});