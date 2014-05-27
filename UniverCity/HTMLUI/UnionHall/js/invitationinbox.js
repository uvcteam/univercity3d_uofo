/**
 * Created by Jacob on 14/01/20.
 */
var lastEventID = -1;

// Web Functions.
$(function() {
    console.log("Getting events.");
    engine.call("GetInvitationEvents");
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
    $('#myModal').foundation('reveal', 'open');
}

function JoinEvent() {
    $('#myModal').foundation('reveal', 'close');
    console.log("Join event " + lastEventID);
    engine.call("JoinEvent", lastEventID);
}

// Invoked by Unity3D.
engine.on("AddEvent", function(name, date, time, desc, who, where, id){
    var ele = $('.no-events');
    if ($('.no-events')) $('.no-events').remove();
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

    $('.events').append(newEvent);
    ModalEffects();
});