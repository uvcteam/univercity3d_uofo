/**
 * Created by Jacob on 11/24/13.
 */
// Web Functions.
var lastEventID = -1;

var currentCategory = 'All Categories';
$(function () {
    $('.category-dropdown').val('All Categories');
    engine.call("GetCategories");

    engine.call("GetEvents", currentCategory);
    console.log(currentCategory);
});

$('.category-dropdown').change(function() {
    currentCategory = $('.category-dropdown').val();
    $('.events').empty();
    engine.call("GetEvents", currentCategory);
});

function CreateEvent(name, date, time, desc)
{
    var newEvent = '';
    newEvent += '<div class="event md-trigger" data-modal="modal-5">';
    newEvent += '<span class="event-name">' + name + '</span>';
    newEvent += '<span class="event-date">' + date + '</span>';
    newEvent += '<span class="event-time">' + time + '</span>';
    newEvent += '<p>' + desc + '</p>';
    newEvent += '</div>';

    $('.events').append(newEvent);
    ModalEffects();
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

function JoinEvent() {
    console.log("Join event " + lastEventID);
    engine.call("JoinEvent", lastEventID);
}

// Invoked by Unity3D.
engine.on("CreateEvent", function(name, date, time, desc, who, where, id){
    var newEvent = '';
    newEvent += '<div class="event" onclick="ShowEvent(this)"';
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
    $('.events').append(newEvent);
    ModalEffects();
});

engine.on("NoEvents", function(){
    $('.no-events').css('visibility', 'visible');
    $('.events').css('visibility', 'hidden');
});

engine.on("AddCategory", function(cat) {
    if (cat !== currentCategory)
        $('.category-dropdown').append('<option>' + cat + '</option>');
});

engine.on("CategoriesFinished", function() {
    $('.category-dropdown').val(currentCategory);
});