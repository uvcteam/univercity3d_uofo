/**
 * Created by Jacob on 11/24/13.
 */
// Web Functions.
var lastEventID = -1;

$(function() {
    engine.call("GetMyEvents");
    engine.call("GetOtherEvents");
});

function CancelEvent() {
    console.log("Cancel event " + lastEventID);
    engine.call("CancelEvent", lastEventID);
}

function WithdrawEvent() {
    console.log("Withdraw from: " + lastEventID);
    engine.call("WithdrawEvent", lastEventID);
}

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

function ShowCancelEvent(caller) {
    var element = $(caller);
    lastEventID = element.attr('event-id');
    $('.modal-title').html('Are you sure you wish to cancel this event?');
    $('.m-event-title').html(element.attr('event-name'));
    $('.m-event-who').html(element.attr('event-who'));
    $('.m-event-what').html(element.attr('event-what'));
    $('.m-event-where').html(element.attr('event-where'));
    $('.m-event-date').html(element.attr('event-date'));
    $('.m-event-when').html(element.attr('event-time'));
    $('.button.tiny.radius.alert').html("Cancel Event");
    $('.button.tiny.radius.alert').click(function() {
        $('#myModal').foundation('reveal', 'close');
        CancelEvent();
    });
    $('#myModal').foundation('reveal', 'open');
}

function ShowWithdrawEvent(caller) {
    var element = $(caller);
    lastEventID = element.attr('event-id');
    $('.modal-title').html('Are you sure you wish to withdraw from this event?');
    $('.m-event-title').html(element.attr('event-name'));
    $('.m-event-who').html(element.attr('event-who'));
    $('.m-event-what').html(element.attr('event-what'));
    $('.m-event-where').html(element.attr('event-where'));
    $('.m-event-date').html(element.attr('event-date'));
    $('.m-event-when').html(element.attr('event-time'));
    $('.button.tiny.radius.alert').html("Withdraw From Event");
    $('.button.tiny.radius.alert').click(function() {
        $('#myModal').foundation('reveal', 'close');
        WithdrawEvent();
    });
    $('#myModal').foundation('reveal', 'open');
}

// Invoked by Unity3D.
engine.on("CreateMyEvent", function(name, date, time, desc, who, where, id){
    if ($('#my-events > .no-events')) 
        $('#my-events > .no-events').css('display', 'none');
    var newEvent = '';
    newEvent += '<div class="event" onclick="ShowCancelEvent(this)"';
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

    $('#my-events > .events').append(newEvent);
    ModalEffects();
})

engine.on("CreateOtherEvent", function(name, date, time, desc, who, where, id){
    if ($('#other-events > .no-events')) 
        $('#other-events > .no-events').css('display', 'none');
    var newEvent = '';
    newEvent += '<div class="event" onclick="ShowWithdrawEvent(this)"';
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
    console.log('Creating event ' + name);
    $('#other-events > .events').append(newEvent);
    ModalEffects();
})