$(function () {
    var container = document.getElementById('st-container');
    $('#menu_btn').click(function () {
        container.className = 'st-container';
        classie.add(container, 'st-effect-1');
        setTimeout(function () {
            classie.add(container, 'st-menu-open');
        }, 25);
    });
    $('.st-menu li').click(function (event) {
        event.stopPropagation();
        if (this.getAttribute('destination') != 'internal') {
            GoToDestination(this.getAttribute('destination'));
        }
    });
    $('.st-container').click(function (event) {
        classie.remove(document.getElementById('st-container'), 'st-menu-open');
    });
});

// Unity3D Functions.

function GoToDestination(destination) {
    console.log('Calling GoToDestination("' + destination + '") in Unity3D!');
    engine.call('GoToDestination', destination);
}