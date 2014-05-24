// Web Functions.
function OnPinSubmitted() {
    var values = $(":input").serializeArray();
    engine.call('CheckPin', values[0]['value']);
}

// Invoked by Unity3D.
engine.on("PinCorrect", function(token, pin, url) {
    window.token = token;
    window.pin = pin;
    window.serverUrl = url;

    $('.pin-div').remove();
    //$('.journals').css("visibility", "visible");
    $('.st-content').append('<div class="journals"></div>');
    $('.general-button').css("visibility", "visible");

    $.get(url + 'ListJournal?token=' + token + '&pin=' + pin + '&start=0&count=50', function(data) {
        window.rdata = data;
        if (data.s) {
            data.entries.forEach(function(entry) {
                var date = new Date(entry.ts);
                var date_string = GetMonth(date.getMonth()) + ' ' + date.getDate() + ', ' + date.getFullYear();
                $('.journals').append('<div class="journal" id="' + entry.id + '"><h1>' + entry.title + '</h1><h6>' + date_string + '</h6><p>' + entry.entry + '</p><a href="#" onclick="DeleteJournal(this)" class="btn btn-danger" journalid="' + entry.id + '">Delete</a></div>');
            });
        }
    });

    //engine.call("JournalsLoaded");
});

function DeleteJournal(e) {
    engine.call('DeleteEntry', e.getAttribute('journalid'));
}

engine.on("AddJournal", function(id, title, date, content) {
    console.log('adding journal: ' + id + ' - ' + title + ' - ' + date + ' - ' + content);
    $('.journals').append('<div class="journal"><h1>' + title + '</h1><h6>' + date + '</h6><p>' + content + '</p><button type="button" class="btn btn-danger" journalid="' + id + '">Delete</button></div>');
});

engine.on("JournalsFinished", function() {
});

engine.on("CreateSuccess", function() {
	$('input[name=title]').val('');
	$('textarea[input=entry]').val('');	
});

engine.on("DeleteSuccess", function(id) {
    $("#" + id).remove();
});

function GetMonth(month) {
    var months = ['January', 'February', 'March',
                  'April', 'May', 'June', 'July',
                  'August', 'September', 'October',
                  'November', 'December'];
    return months[month - 1];
}