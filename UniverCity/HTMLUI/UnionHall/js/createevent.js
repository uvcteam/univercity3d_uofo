/**
 * Created by Jacob on 11/21/13.
 */
// Web Functions.
// cache selects for use later
var selects = $('.chzn-select');

// whenever the selection changes, either disable or enable the 
// option in the other selects
selects.chosen().change(function() {
    var selected = [];

    // add all selected options to the array in the first loop
    selects.find("option").each(function() {
        if (this.selected) {
            selected[this.value] = this;
        }
    })

    // then either disabled or enable them in the second loop:
    .each(function() {

        // if the current option is already selected in another select disable it.
        // otherwise, enable it.
        this.disabled = selected[this.value] && selected[this.value] !== this;
    });

    // trigger the change in the "chosen" selects
    selects.trigger("chosen:updated");
});


function OnEventCreate() {
    $('#myModal').modal('show');
}

function SubmitEvent() {
    var values = $(":input").serializeArray();
    var inputs = new Array();
    inputs[0]  = values[0]['value'];
    inputs[1]  = values[1]['value'];
    inputs[2]  = values[6]['value'];
    inputs[3]  = values[4]['value'];
    inputs[4]  = values[2]['value'];
    inputs[5]  = values[7]['value'];
    inputs[6]  = values[5]['value'];
    inputs[7]  = values[9]['value'];
    inputs[8]  = values[3]['value'];
    inputs[9] = values[8]['value'];
    
    engine.call('CreateEvent', inputs);
}

// Unity3D Functions.

// Invoked by Unity3D.
engine.on("CreateSuccess", function() {
    $(':input:not(:button)').val('');
    window.location.href = 'myevents.html';
});