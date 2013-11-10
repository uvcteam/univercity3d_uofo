// Web Functions.
$(function () {
    $(".location").click(function() {
        GoToDestination(this.getAttribute('destination'));
    });
});

function SignInClicked() {
	var values = $(":input").serializeArray();

    engine.call('CheckLoginInformation', values[0]["value"], values[1]["value"]).then(function() {
        console.log("Calling 'CheckLoginInformation' in Unity3D!");
    });
}

function SignOut() {
    $('input[name=password]').val('');
    classie.remove(document.getElementById("main"), "hidden");
    classie.add(document.getElementById("logged-in"), "hidden");
    UnitySignOut();
}

// Unity3D Functions.

function GoToDestination(destination) {
    console.log('Calling GoToDestination("' + destination + '") in Unity3D!');
    engine.call('GoToDestination', destination);
}

function UnitySignOut() {
    engine.call('SignOut');
}

// Invoked by Unity3D.

engine.on('LoggedIn', function(name) {
    document.getElementById("logged-in").innerHTML = '<img src="images/logo.png" /><br /><span>Welcome back <strong><em>' + name + '</em></strong>!</span><br /><span>Not you? <a href="#" onclick="SignOut()">Sign out</a>.';
    classie.add(document.getElementById("main"), "hidden");
    classie.remove(document.getElementById("logged-in"), "hidden");
});