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
    $('.modal-title').html('Are you sure you wish to cancel');
    $('.m-event-title').html(element.attr('event-name'));
    $('.m-event-who').html(element.attr('event-who'));
    $('.m-event-what').html(element.attr('event-what'));
    $('.m-event-where').html(element.attr('event-where'));
    $('.m-event-date').html(element.attr('event-date'));
    $('.m-event-when').html(element.attr('event-time'));
    $('.btn-primary').click(function() {
        CancelEvent();
    });
    $('#myModal').modal('show');
}

function ShowWithdrawEvent(caller) {
    var element = $(caller);
    lastEventID = element.attr('event-id');
    $('.modal-title').html('Are you sure you wish to withdraw from');
    $('.m-event-title').html(element.attr('event-name'));
    $('.m-event-who').html(element.attr('event-who'));
    $('.m-event-what').html(element.attr('event-what'));
    $('.m-event-where').html(element.attr('event-where'));
    $('.m-event-date').html(element.attr('event-date'));
    $('.m-event-when').html(element.attr('event-time'));
    $('.btn-primary').click(function() {
        WithdrawEvent();
    });
    $('#myModal').modal('show');
}

// Invoked by Unity3D.
engine.on("CreateMyEvent", function(name, date, time, desc, who, where, id){
    var newEvent = '';
    newEvent += '<div class="event" onclick="';
    if (location.pathname === '/UnionHall/myevents.html')
        newEvent += 'ShowEvent(this)"';
    else
        newEvent += 'ShowCancelEvent(this)"';
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
    $('#my-events > .events').append(newEvent);
    ModalEffects();
})

engine.on("CreateOtherEvent", function(name, date, time, desc, who, where, id){
    var newEvent = '';
    newEvent += '<div class="event" onclick="';
    if (location.pathname === '/UnionHall/myevents.html')
        newEvent += 'ShowEvent(this)"';
    else
        newEvent += 'ShowWithdrawEvent(this)"';
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
    $('#other-events > .events').append(newEvent);
    ModalEffects();
})