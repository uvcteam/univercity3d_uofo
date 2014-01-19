$(function() {
    engine.call("IsFacebookLoggedIn");
});

function FBSignIn() {
    engine.call("SignIntoFacebook");
}

function FBSignOut() {
    engine.call("SignOutOfFacebook");
    $('#profile-pic').attr('src', 'images/profile.svg');
    $('#user-name').html('Loading Name');
    $('#user-quotes').html('');
}

engine.on('FacebookInfoMB', function(result) {
    alert(result);
    $('#user-name').html(result.name);
    $('#profile-pic').attr('src', result.picture.data.url);
    $('#user-quotes').html(result.quotes);
});

engine.on("FacebookLoggedIn", function(loggedin) {
   if (loggedIn) {
       $('#fb_link').html('<a href="#" onclick="FBSignIn()">Connect to Facebook!</a>');
       engine.call('GetFacebookInfoMB');
   } else {
       $('#fb_link').html('<a href="#" onclick="FBSignOut()">Disconnect from Facebook!</a>');
   }
});