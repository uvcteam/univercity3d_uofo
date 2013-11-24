// Web Functions.
$(function() {
   if (document.getElementById('calendar')) {
       $('#calendar').calendario();
   }
});

function ChangeModal(name, date, time, who, desc) {
    $('.m-event-title').html(name);
}

// Invoked by Unity3D.