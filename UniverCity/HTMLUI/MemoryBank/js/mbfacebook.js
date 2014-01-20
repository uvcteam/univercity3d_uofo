$(function() {
    engine.call("IsFacebookLoggedIn");
});

function FBSignIn() {
    engine.call("SignIntoFacebook");
}

function FBSignOut() {
    $('#fb_link').html('<a href="#" onclick="FBSignIn()">Connect to Facebook!</a>');
    engine.call("SignOutOfFacebook");
    $('#profile-pic').attr('src', 'images/profile.svg');
    $('#user-name').html('Loading Name');
    $('#user-quotes').html('');
    engine.call("RequestUsername");
}

engine.on('FacebookInfoMB', function(result) {
    var fbo = jQuery.parseJSON(result);
      $('#fb_link').html('<a href="#" onclick="FBSignOut()">Disconnect from Facebook!</a>');
    $('#user-name').html(fbo.name);
    $('#profile-pic').attr('src', fbo.picture.data.url);
    $('#user-quotes').html(fbo.quotes);
});