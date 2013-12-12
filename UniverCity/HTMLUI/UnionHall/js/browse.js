/**
 * Created by Jacob on 11/24/13.
 */
// Web Functions.
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

function JoinEvent() {
    console.log("Join event " + lastEventID);
    engine.call("JoinEvent", lastEventID);
}

// Invoked by Unity3D.
engine.on("CreateEvent", function(name, date, time, desc, who, where, id){
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