// Web Functions.
function OnPinSubmitted() {
    var values = $(":input").serializeArray();
    engine.call('CheckPin', values[0]['value']);
}

// Invoked by Unity3D.
engine.on("PinCorrect", function() {
    $('.pin-div').css("visibility", "hidden");
    $('.journals').css("visibility", "visible");
    $('.general-button').css("visibility", "visible");
});

engine.on("AddJournal", function(title, date, content) {
    console.log('adding journal: ' + title + ' - ' + date + ' - ' + content);
    $('.journals').append('<div class="journal"><h1>' + title + '</h1><h6>' + date + '</h6><p>' + content + '</p></div>');
});