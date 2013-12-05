$(function () {
    var container = document.getElementById('st-container');
    $('#menu_btn').click(function (event) {
        event.stopPropagation();
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

function urlParam(name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results === null) {
        return null;
    } else {
        return results[1] || 0;
    }
}

function htmlEncode(value) {
    return $('<div />').text(value).html();
}

// Unity3D Functions.

function GoToDestination(destination) {
    console.log('Calling GoToDestination("' + destination + '") in Unity3D!');
    engine.call('GoToDestination', destination);
}