// Web Functions.
var FBAUTH = null;

$(function() {
    engine.call("GetFacebookAuth");
});

$(document).ready(function () {
    var selector, logActivity, callbackAlbumSelected, callbackPhotoUnselected, callbackSubmit;
    var buttonOK = $('#CSPhotoSelector_buttonOK');
    var o = this;
    
    
    /* --------------------------------------------------------------------
     * Photo selector functions
     * ----------------------------------------------------------------- */
    
    fbphotoSelect = function(id) {
        // if no user/friend id is sent, default to current user
        if (!id) id = 'me';
        
        callbackAlbumSelected = function(albumId) {
            var album, name;
            album = CSPhotoSelector.getAlbumById(albumId);
            // show album photos
            selector.showPhotoSelector(null, album.id);
        };

        callbackAlbumUnselected = function(albumId) {
            var album, name;
            album = CSPhotoSelector.getAlbumById(albumId);
        };

        callbackPhotoSelected = function(photoId) {
            var photo;
            photo = CSPhotoSelector.getPhotoById(photoId);
            buttonOK.show();
            //logActivity('Selected ID: ' + photo.id);
        };

        callbackPhotoUnselected = function(photoId) {
            var photo;
            album = CSPhotoSelector.getPhotoById(photoId);
            buttonOK.hide();
        };

        callbackSubmit = function(photoId) {
            var photo;
            photo = CSPhotoSelector.getPhotoById(photoId);
            //logActivity('<br><strong>Submitted</strong><br> Photo ID: ' + photo.id + '<br>Photo URL: ' + photo.source + '<br>');
        };


        // Initialise the Photo Selector with options that will apply to all instances
        CSPhotoSelector.init();

        // Create Photo Selector instances
        selector = CSPhotoSelector.newInstance({
            callbackAlbumSelected   : callbackAlbumSelected,
            callbackAlbumUnselected : callbackAlbumUnselected,
            callbackPhotoSelected   : callbackPhotoSelected,
            callbackPhotoUnselected : callbackPhotoUnselected,
            callbackSubmit          : callbackSubmit,
            maxSelection            : 1,
            albumsPerPage           : 6,
            photosPerPage           : 200,
            autoDeselection         : true
        });

        alert('ready to go.');

        // reset and show album selector
        selector.reset();
        selector.showAlbumSelector(id);
    }
    
    
    /* --------------------------------------------------------------------
     * Click events
     * ----------------------------------------------------------------- */
    $(".photoSelect").click(function (e) {
        alert('Showing FB photos.');
        e.preventDefault();
        id = null;
        if ( $(this).attr('data-id') ) id = $(this).attr('data-id');
        fbphotoSelect(id);
    });
});

function ShowFacebookPhotos() {
    alert("SHOW FACEBOOK PHOTOS!");
    //fbphotoSelect(null);
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