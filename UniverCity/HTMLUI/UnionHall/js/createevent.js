/**
 * Created by Jacob on 11/21/13.
 */
// Web Functions.

// cache selects for use later
var selects = $('.chosen-select');

$(document).ready(function() {
    window.interests = [];

    $.get(server_url + "/ListSocialInterests", function(data) {
        $(data).each(function() {
            interests.push(this);
            selects.append('<option data-catid="' + this.id + '">' + this.int +'</option>');
        });
         selects.trigger("chosen:updated");
    });
   
});


// whenever the selection changes, either disable or enable the 
// option in the other selects
selects.chosen().change(function() {
    var selected = [];

    // add all selected options to the array in the first loop
    selects.find("option").each(function() {
        if (this.selected && this.value != 'Choose a category') {
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
    $('#myModal').foundation('reveal', 'open');
}

function SubmitEvent() 
{
    var event_name = $("#event_name");
    var event_date = $("#event_date");
    var event_time = $("#event_time");
    var event_what = $("#event_what");
    var event_who = $("#event_who");
    var event_min = $("#event_min");
    var event_max = $("#event_max");
    var event_where = $("#event_where");
    var event_phone = $("#event_phone");
    var categories = [];
    var inputs = [];

    window.phone = event_phone;

    inputs.push($(event_name).val());
    inputs.push($(event_who).val());
    inputs.push($(event_what).val());
    inputs.push($(event_where).val());
    inputs.push($(event_date).val());
    inputs.push($(event_time).val());
    inputs.push($(event_phone).val());
    inputs.push($(event_min).val());
    inputs.push($(event_max).val());

    $('.chosen-select').find("option").each(function() {
        if (this.selected && this.value != 'Choose a category')
            categories.push($(this).data('catid'));
    });

    // var values = $(":input").serializeArray();
    // var inputs = new Array();
    // inputs[0]  = values[0]['value'];
    // inputs[1]  = values[1]['value'];
    // inputs[2]  = values[6]['value'];
    // inputs[3]  = values[4]['value'];
    // inputs[4]  = values[2]['value'];
    // inputs[5]  = values[7]['value'];
    // inputs[6]  = values[5]['value'];
    // inputs[7]  = values[9]['value'];
    // inputs[8]  = values[3]['value'];
    // inputs[9] = values[8]['value'];
    console.log(inputs);
    console.log(categories);
    engine.call('CreateEvent', inputs, categories);
}

// Unity3D Functions.

// Invoked by Unity3D.
engine.on("CreateSuccess", function() {
    $(':input:not(:button)').val('');
    window.location.href = 'cancelwithdraw.html';
});