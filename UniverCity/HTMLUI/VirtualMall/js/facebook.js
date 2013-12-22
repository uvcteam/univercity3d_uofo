var appID = '178630832342722';
var appURL = 'http://www.univercity3d.com/mobileapp.html';
var auth = null;

$(function() {
    if (window.location.hash.length == 0)
        engine.call('IsFacebookSignedIn');
    else
        SignedIn();
});


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

function NeedToSignIn() {
    $('.facebook').click(function(){
        alert("You must be signed in to Facebook to use this feature.");
    })

}

function SignedIn() {
// Init the SDK upon load
    window.fbAsyncInit = function() {
        // listen for and handle auth.statusChange events
        FB.Event.subscribe('auth.statusChange', function(response) {
            console.log("stuff happened");
            if (response.authResponse) {
                // user has auth'd your app and is logged into Facebook
                // request users' first name and profile picture
                FB.api('/me?fields=picture.height(500),name,quotes', function(me){
                    $('#user-name').html(me.name);
                    $('#user-quotes').html(me.quotes);
                });
            }
        })

        if (!auth) {
            // parse the authorization response details from the URL
            var parsed = parseAuthorization(window.location.href);

            // convert to FB authResponse
            auth = {
                accessToken : parsed.access_token,
                signedRequest : parsed.signed_request,
                expiresIn : parsed.expires_in,
                code: parsed.code
            };
        }
        engine.call("StoreFacebook", auth.accessToken, auth.signedRequest, auth.expiresIn, auth.code);
        FB.init({
            appId      :  appID,
            authResponse: auth,
            status     : true, // check login status
            cookie     : true, // enable cookies to allow the server to access the session
            xfbml      : true  // parse XFBML
        });

        var button = $('<button id="login">Facebook Logout</button>').button().click(function () {
            engine.call("FacebookSignOut");
            var path = 'https://www.facebook.com/logout.php?';
            path += 'next=' + encodeURIComponent(appURL);
            path += '&access_token=' + encodeURIComponent(auth.accessToken);
            // redirect the view to the facebook authorization dialog
            window.location.href = path;
        });
        $('.st-content-inner').append(button);
    }
}

(function(d){
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) {return;}
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "http://connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));

engine.on("FacebookAuthorized", function(at, sr, ei, c) {
    console.log("Facebook is authorized!");
    auth = {
        accessToken : at,
        signedRequest : sr,
        expiresIn : ei,
        code: c
    };

    SignedIn();
});

engine.on("FacebookNotAuthorized", function() {
    console.log("Facebook is not authorized!");
    NeedToSignIn();
});/**
 * Created by Avengix on 12/15/13.
 */
