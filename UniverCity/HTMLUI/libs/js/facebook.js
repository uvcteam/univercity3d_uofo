/**
 * Created by Jacob on 12/11/13.
 */

var appID = '178630832342722';
var appURL = 'http://www.univercity3d.com/mobileapp.html';
var auth;

$(function() {
    if (window.location.hash.length == 0)
        engine.call("IsFacebookSignedIn");
});

function SetPortrait(fbAvatar) {
    var url = fbAvatar.picture.data.url;
    var picture = new Image;
    picture.onload = function () { $('#profile-pic').attr('src', url) };
    picture.src = url;
}

function parseAuthorization(url) {
    var v = url.split('#')[1].split('&');
    var n = v.length;
    var r = {}
    for (var i = 0; i != n; ++i) {
        var c = v[i].split('=');
        r[c[0]] = decodeURIComponent(c[1]);
    }
    return r;
}

(function(d){
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) {return;}
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "http://connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));

 window.fbAsyncInit = function() {
     // parse the authorization response details from the URL
     var parsed = parseAuthorization(window.location.href);
     // convert to FB authResponse
     auth = {
         accessToken : parsed.access_token,
         signedRequest : parsed.signed_request,
         expiresIn : parsed.expires_in,
         code: parsed.code
     };

     engine.call("StoreFacebook", auth.accessToken, auth.signedRequest, auth.expiresIn, auth.code);
 }

engine.on("FacebookAuthorized", function(at, sr, ei, c) {
    console.log("Facebook authorized!");
    auth = {
        accessToken : at,
        signedRequest : sr,
        expiresIn : ei,
        code: c
    };

    FB.init({
        appId      :  appID,
        authResponse: auth,
        status     : true, // check login status
        cookie     : true, // enable cookies to allow the server to access the session
        xfbml      : false  // parse XFBML
    });
});

engine.on("FacebookNotAuthorized", function() {
    console.log("Facebook needs to be authorized.");

    if (window.location.hash.length == 0)
    {
        $(function () {
            var button = $('<button id="login">Facebook Login</button>').button().click(function () {
                var path = 'https://www.facebook.com/dialog/oauth?';
                var queryParams = {
                    client_id: appID,
                    redirect_uri: appURL,
                    response_type: 'token,signed_request,code',
                    scope: 'user_photos'
                };
                var url = path + $.param(queryParams);
                // redirect the view to the facebook authorization dialog
                window.location.href = url;
            });
            $('.st-content-inner').append(button);
        });
    }
});