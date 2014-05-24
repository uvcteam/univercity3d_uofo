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

    $('.pin-div').css("visibility", "hidden");
    $('.journals').css("visibility", "visible");
    $('.general-button').css("visibility", "visible");

    $.get(url + 'ListJournal?token=' + token + '&pin=' + pin + '&start=0&count=50', function(data) {
        window.rdata = data;
        if (data.s) {
            data.entries.forEach(function(entry) {
                var date = new Date(entry.ts);
                var date_string = GetMonth(date.getMonth()) + ' ' + date.getDate() + ', ' + date.getFullYear();
                $('.journals').append('<div class="journal" id="' + entry.id + '"><h1>' + entry.title + '</h1><h6>' + date_string + '</h6><p>' + entry.entry + '</p><button type="button" class="btn btn-danger" journalid="' + entry.id + '">Delete</button></div>');
            });

            $('.btn-danger').click(function() {
                console.log('Delete entry: ' + this.getAttribute('journalid'));
                $.get(window.serverUrl + '/DeleteJournalEntry?token=' + window.token + '&pin=' + window.pin + '&id=' + this.getAttribute('journalid'), function(data) {
                    if (data.s) {
                        $("#" + this.getAttribute('journalid')).remove();
                    }
                });
            });
        }
    });

    //engine.call("JournalsLoaded");
});

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

engine.on("DeleteSuccess", function() {
	$('.journals').html('');
    var values = $(":input").serializeArray();
    console.log(values);
    engine.call('CheckPin', values[0]['value']);
});

function GetMonth(month) {
    var months = ['January', 'February', 'March',
                  'April', 'May', 'June', 'July',
                  'August', 'September', 'October',
                  'November', 'December'];
    return months[month - 1];
}