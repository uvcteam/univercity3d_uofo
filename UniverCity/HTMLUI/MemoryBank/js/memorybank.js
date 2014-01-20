// Web Functions.
function SignInClicked() {
    var values = $(":input").serializeArray();

    //engine.call('CheckLoginInformation', values[0]["value"], values[1]["value"]).then(function () {
    //    console.log("Calling 'CheckLoginInformation' in Unity3D!");
    //});
}

function SignOut() {
    engine.call('SignOut');
}

$(function() {
    //RequestUsername();
});

function RequestUsername() {
    console.log('Calling RequestUsername() in Unity3D!');
    engine.call('RequestUsername');
}

// Invoked by Unity3D.

engine.on('UpdateUsername', function (name) {
    document.getElementById("user-name").innerHTML = name;
});