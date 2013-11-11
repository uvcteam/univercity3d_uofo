
// Web Functions.
$(function() {
    RequestUsername();
});

function RequestUsername() {
    console.log('Calling RequestUsername() in Unity3D!');
    engine.call('RequestUsername');
}

// Invoked by Unity3D.

engine.on('UpdateUsername', function (name) {
    document.getElementById("user-name").innerHTML = name;
});