// Web Functions.
function OnJournalSubmitted() {
    var values = $(":input").serializeArray();
    console.log(values[0]['value'] + ' ---- ' + values[1]['value']);
    engine.call('OnSaveEntryClicked', values[0]['value'], values[1]['value']);
}

// Invoked by Unity3D.
engine.on("PinCorrect", function() {
    $('.pin-div').css("visibility", "hidden");
    $('.journals').css("visibility", "visible");
});

engine.on("AddJournal", function(title, date, content) {
    console.log('adding journal: ' + title + ' - ' + date + ' - ' + content);
    $('.journals').append('<div class="journal"><h1>' + title + '</h1><h6>' + date + '</h6><p>' + content + '</p></div>');
});

engine.on("CreateSuccess", function() {
    console.log("Created successfully!");
    $("input[name=title]").val('');
    $("textarea[name=entry]").val('');
});