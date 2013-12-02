/**
 * Created by Jacob on 11/24/13.
 */
// Web Functions.
$(function () {
    engine.call("GetCategories");

    var category = "";
    if (urlParam('category')) {
        category = decodeURIComponent(urlParam('category'));
        $('.category-dropdown').val(category);
        console.log(category);
    } else {
        $('.category-dropdown').val("All Categories");
    }

    engine.call("GetEvents", category);
    console.log(category);
});

$('.category-dropdown').change(function() {
    cat = $('.category-dropdown').val();
    window.location.href='browse.html?category=' + encodeURIComponent(cat);
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
    $('.category-dropdown').append('<option>' + cat + '</option>');
});

engine.on("CategoriesFinished", function() {
    if (urlParam('category')) {
        category = decodeURIComponent(urlParam('category'));
        $('.category-dropdown').val(category);
        console.log(category);
    } else {
        $('.category-dropdown').val("All Categories");
    }
});