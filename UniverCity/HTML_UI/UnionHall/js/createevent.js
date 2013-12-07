/**
 * Created by Jacob on 11/21/13.
 */
// Web Functions.
function isTextInput(node) {
    return ['INPUT', 'TEXTAREA'].indexOf(node.nodeName) !== -1;
}

document.addEventListener('touchstart', function(e) {
    if (!isTextInput(e.target) && isTextInput(document.activeElement)) {
        document.activeElement.blur();
    }
}, false);

function OnEventCreate() {
    $('#myModal').modal('show');
}

function SubmitEvent() {
    var values = $(":input").serializeArray();
    var inputs = new Array();
    inputs[0]  = values[0]['value'];
    inputs[1]  = values[1]['value'];
    inputs[2]  = values[2]['value'];
    inputs[3]  = values[3]['value'];
    inputs[4]  = values[8]['value'];
    inputs[5]  = values[9]['value'];
    inputs[6]  = values[7]['value'];
    inputs[7]  = values[4]['value'];
    inputs[8]  = values[5]['value'];
    inputs[9] = values[6]['value'];

    engine.call('CreateEvent', inputs);
}

// Unity3D Functions.

// Invoked by Unity3D.
engine.on("CreateSuccess", function()
{
    $(':input:not(:button)').val('');
});