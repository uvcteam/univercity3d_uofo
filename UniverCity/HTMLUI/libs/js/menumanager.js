var menuShown = false;
var prevWidth = 0;
var shouldChangeMenu = true;
var num_invitations = 0;

$(function () {
    prevWidth = $(window).width();
    if (prevWidth > 960 && shouldChangeMenu)
        ShowMenu();
    var container = document.getElementById('st-container');
    $('#menu_btn').click(function (event) {
        event.stopPropagation();
        OpenMenu();
    });
    $('nav li').click(function (event) {
        event.stopPropagation();
        if (this.getAttribute('destination') != 'internal') {
            GoToDestination(this.getAttribute('destination'));
        }
    });
    $('.st-container').click(function (event) {
        classie.remove(document.getElementById('st-container'), 'st-menu-open');
    });
    engine.call("GetInvitationCount");
});

$(window).resize(function() {
    if (!shouldChangeMenu) return;
    if ($(this).width() > 960 && prevWidth > 960) return;
    if ($(this).width() <= 960 && prevWidth <= 960) return;

   if ($(this).width() >= 961) {
        ShowMenu();
   } else {
        HideMenu();
   }
   prevWidth = $(this).width();
});

function ShowMenu() {
    var menu = document.getElementById('menu-1');
    if (!menu) return;
    classie.add(menu, 'st-menu-open');
    classie.add(menu, 'st-no-transform');
    $('.st-pusher').css('margin-left', '225px');
    $('#menu_btn').css('display', 'none');
}

function HideMenu() {
    var menu = document.getElementById('menu-1');
    if (!menu) return;
    classie.remove(menu, 'st-menu-open');
    classie.remove(menu, 'st-no-transform');
    $('.st-pusher').css('margin-left', '');
    $('#menu_btn').css('display', '');
}

function urlParam(name) {
    var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results === null) {
        return null;
    } else {
        return results[1] || 0;
    }
}

function OpenMenu() {
    var container = document.getElementById('st-container');
    container.className = 'st-container';
    classie.add(container, 'st-effect-1');
    setTimeout(function () {
        classie.add(container, 'st-menu-open');
    }, 25);
}

function htmlEncode(value) {
    return $('<div />').text(value).html();
}

// Unity3D Functions.

function GoToDestination(destination) {
    console.log('Calling GoToDestination("' + destination + '") in Unity3D!');
    engine.call('GoToDestination', destination);
}

engine.on("InvitationCount", function(count) {
    console.log(count);
    num_invitations = count;
    var el = $('.invitation-count');
    if (el) {
        el.html(count);
    }
});

engine.on("UserToken", function(token) {
    localStorage.setItem('userToken', token);
});