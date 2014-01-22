// Web Functions.
var FBAUTH = null;

$(function() {
    engine.call("GetFacebookAuth");
});

$(document).ready(function () {
    
});

function ShowFacebookPhotos() {
    //alert("SHOW FACEBOOK PHOTOS!");
    //$('#plusgallery').attr('style', 'display:block');
    //$('.st-pusher').attr('style', 'display:none');
    //fbphotoSelect(null);
    window.location.href = "photos.html";
}

function OnJournalSubmitted() {
    var values = $(":input").serializeArray();
    var entryText = $('#editor').html();
    //console.log(values[0]['value'] + ' ---- ' + values[1]['value']);
    //alert('Save entry ' + values[0]['value'] + ' -- ' + entryText);
    engine.call('OnSaveEntryClicked', values[0]['value'], entryText);
}

// Invoked by Unity3D.
engine.on("PinCorrect", function() {
    $('.pin-div').css("visibility", "hidden");
    $('.journals').css("visibility", "visible");
});

engine.on("AddJournal", function(title, date, content) {
    console.log('adding journal: ' + title + ' - ' + date + ' - ' + content);
    $('.journals').append('<div class="journal"><h1>' + title + '</h1><h6>' + date + '</h6><p>' + content + '</p></div>');
});

engine.on("CreateSuccess", function() {
    console.log("Created successfully!");
    $("input[name=title]").val('');
    $("textarea[name=entry]").val('');
});

engine.on("FacebookAuth", function(auth) {
    window.fbAsyncInit = function() {
        FB.init({ appId: '178630832342722', cookie: true, status: true, xfbml: true });
    };
    (function(d){
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) {return;}
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    }(document));
    /*FB.init({
        appId        : '178630832342722',
        status       : true,
        xfbml        : true,
    });*/

    FBAUTH = auth;     
    //FacebookPhotoSelector.setFacebookSDK(FB);
});