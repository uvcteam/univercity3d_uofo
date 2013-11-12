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

engine.on("AddJournal", function(id, title, date, content) {
    console.log('adding journal: ' + id + ' - ' + title + ' - ' + date + ' - ' + content);
    $('.journals').append('<div class="journal"><h1>' + title + '</h1><h6>' + date + '</h6><p>' + content + '</p><button type="button" class="btn btn-danger" journalid="' + id + '">Delete</button></div>');
});

engine.on("JournalsFinished", function() {
	$('.btn-danger').click(function() {
		console.log('Delete entry: ' + this.getAttribute('journalid'));
		engine.call('DeleteEntry', this.getAttribute('journalid'));
	});
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