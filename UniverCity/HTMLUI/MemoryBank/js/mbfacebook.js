$(function() {
    engine.call("IsFacebookLoggedIn");
});

function FBSignIn() {
    engine.call("SignIntoFacebook");
}

function FBSignOut() {
    $('#fb_link').html('<button class="btn btn-facebook" onclick="FBSignIn()"><i class="fa fa-facebook"></i> | Connect with Facebook</button>');
    engine.call("SignOutOfFacebook");
    $('#profile-pic').attr('src', 'images/profile.svg');
    $('#user-name').html('Loading Name');
    $('#user-quotes').html('');
    engine.call("RequestUsername");
}

engine.on('FacebookInfoMB', function(result) {
    var fbo = jQuery.parseJSON(result);
      $('#fb_link').html('<button class="btn btn-facebook" onclick="FBSignOut()"><i class="fa fa-facebook"></i> | Disconnect from Facebook</button>');
    $('#user-name').html(fbo.name);
    $('#profile-pic').attr('src', fbo.picture.data.url);
    $('#user-quotes').html(fbo.quotes);
});