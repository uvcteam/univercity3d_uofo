// Web Functions.
function SignInClicked() {
    var values = $(":input").serializeArray();

    engine.call('CheckLoginInformation', values[0]["value"], values[1]["value"]).then(function () {
        console.log("Calling 'CheckLoginInformation' in Unity3D!");
    });
}

function SignOut() {
    engine.call('SignOut');
}

$(function() {
    RequestUsername();
});

function RequestUsername() {
    console.log('Calling RequestUsername() in Unity3D!');
    //engine.call('RequestUsername');
    engine.call('GetFacebookInfoMB');
}

// Invoked by Unity3D.

engine.on('UpdateUsername', function (name) {
    document.getElementById("user-name").innerHTML = name;
});

engine.on('FacebookInfoMB', function(result) {
    alert(result);
    $('#user-name').html(result.name);
    $('#profile-pic').attr('src', result.picture.data.url);
    $('#user-quotes').html(result.quotes);
});