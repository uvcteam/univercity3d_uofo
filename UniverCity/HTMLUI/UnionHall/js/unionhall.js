// Web Functions.
$(function() {
    console.log("Getting week's events.");
    engine.call("GetWeekEvents");
    setTimeout(function() {
        console.log("Populating calendar.");
        engine.call("PopulateCalendar");
    }, 500);
});

function InitializeCalendar() {
    var transEndEventNames = {
            'WebkitTransition': 'webkitTransitionEnd',
            'MozTransition': 'transitionend',
            'OTransition': 'oTransitionEnd',
            'msTransition': 'MSTransitionEnd',
            'transition': 'transitionend'
        },
        transEndEventName = transEndEventNames[Modernizr.prefixed('transition')],
        $wrapper = $('#custom-inner'),
        $calendar = $('#calendar'),
        cal = $calendar.calendario({
            onDayClick: function ($el, $contentEl, dateProperties) {

                if ($contentEl.length > 0) {
                    showEvents($contentEl, dateProperties);
                }
            },
            caldata: codropsEvents,
            displayWeekAbbr: true
        }),
        $month = $('#custom-month').html(cal.getMonthName()),
        $year = $('#custom-year').html(cal.getYear());
    $('#custom-next').on('click', function () {
        cal.gotoNextMonth(updateMonthYear);
    });
    $('#custom-prev').on('click', function () {
        cal.gotoPreviousMonth(updateMonthYear);
    });
    function updateMonthYear() {
        $month.html(cal.getMonthName());
        $year.html(cal.getYear());
    }

    // just an example..
    function showEvents($contentEl, dateProperties) {
        hideEvents();
        var $events = $('<div id="custom-content-reveal" class="custom-content-reveal"><h4>Events for ' + dateProperties.monthname + ' ' + dateProperties.day + ', ' + dateProperties.year + '</h4></div>'),
            $close = $('<span class="custom-content-close"></span>').on('click', hideEvents);
        $events.append($contentEl.html(), $close).insertAfter($wrapper);
        setTimeout(function () {
            $events.css('top', '0%');
        }, 25);
    }

    function hideEvents() {
        var $events = $('#custom-content-reveal');
        if ($events.length > 0) {

            $events.css('top', '100%');
            Modernizr.csstransitions ? $events.on(transEndEventName, function () { $(this).remove(); }) : $events.remove();
        }
    }
}

// Invoked by Unity3D.
engine.on("AddEvent", function(date, name, time) {
    console.log('Adding event - ' + name);
    codropsEvents[date] = '<span>' + name + ' - ' + time + '</span>';
});

engine.on("AddWeekEvent", function(name, date, time, desc, who, where, id){
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

engine.on("NoEvents", function() {
    console.log('No events');
    $('.no-events').css('visibility', 'visible');
    $('.events').css('visibility', 'hidden');
});

engine.on("EventsFinished", function() {
    InitializeCalendar();
});