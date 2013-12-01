/**
 * Created by Jacob on 11/24/13.
 */
// Web Functions.

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

// Invoked by Unity3D.
engine.on("CreateMyEvent", function(name, date, time, desc, who, where, id){
    var newEvent = '';
    newEvent += '<div class="event md-trigger" data-modal="modal-5"';
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
    newEvent += '<div class="event md-trigger" data-modal="modal-6"';
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