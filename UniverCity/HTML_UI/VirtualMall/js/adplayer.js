

var mediaURL = "http://www.univercity3d.com/univercity/admedia?id=";
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";

    $.ajax({url: URL + urlParam("id"), success: function(adPlayerData){
        console.log(adPlayerData);
        PopulateAdPlayer(adPlayerData);
        SetMegaDeal(adPlayerData.megadeal);
        AttachEventToPages();
        SetNarrator(mediaURL + adPlayerData.expert.id);

    }});
    //document.getElementById('adplayer-style').setAttribute('href', 'styles/explorer-adplayer.css');
});

var PopulateAdPlayer = function(data ) {

    for (var i = 0; i < data.pages.length; ++i) {
        if (data.pages[i].title !== "") {
            AddPage(data.pages[i].title, data.pages[i].parts, data.pages[i].narrative,
                data.pages[i].more.title, data.pages[i].more.parts, data.pages[i].more.narrative);
        }

    }

}
var AddPage = function (adpageTitle, adpageParts, adpageNarrative, detailsTitle, detailsParts, detailsNarrative) {

    console.log(adpageTitle);
    var adpage = '<li>';
    for (var i = 0; i < adpageParts.length; ++i)
    {
        switch(adpageParts[i].type)
        {
            case "image":
                adpage += '<div class="adpage" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '"><a href="#"><img src="' + mediaURL + adpageParts[i].id + '"/></a></div>';
                break;
            case "video":
                adpage += '<div class="adpage" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '"><video id="video-' + adpageTitle + i.toString()+ '" class="video-js vjs-default-skin vjs-big-play-centered" controls preload="none"'
                    + 'poster="http://video-js.zencoder.com/oceans-clip.png">'
                    + '<source src="http://www.scherpbier.org/lovely.webm" type="video/webm" />'
              /*      + '<source src="http://video-js.zencoder.com/oceans-clip.webm" type="video/webm" />'
                    + '<source src="http://video-js.zencoder.com/oceans-clip.ogv" type="video/ogg" />'
                    + '<track kind="captions" src="demo.captions.vtt" srclang="en" label="English"></track>'
                    + '<track kind="subtitles" src="demo.captions.vtt" srclang="en" label="English"></track>'*/
                    + '</video></div>';
                break;
        }
    }

    if(detailsParts.length !== 0){

        for (var i = 0; i < detailsParts.length; ++i)
        {
            switch (detailsParts[i].type)
            {
                case "image":
                    adpage += '<div style="display:none" class="details-page" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '"><a href="#"><img src="' + mediaURL + detailsParts[i].id + '"/></a></div>';
                    break;
                case "video":
                    adpage += '<div style="display:none"  class="details-page" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '"><video id="video-' + detailsTitle + i.toString() + '" class="video-js vjs-default-skin vjs-big-play-centered" controls preload="none"'
                        + 'poster="http://video-js.zencoder.com/oceans-clip.png">'
                        + '<source src="http://video-js.zencoder.com/oceans-clip.mp4" type="video/mp4" />'
                        + '<source src="http://video-js.zencoder.com/oceans-clip.webm" type="video/webm" />'
                        + '<source src="http://video-js.zencoder.com/oceans-clip.ogv" type="video/ogg" />'
                        + '<track kind="captions" src="demo.captions.vtt" srclang="en" label="English"></track>'
                        + '<track kind="subtitles" src="demo.captions.vtt" srclang="en" label="English"></track>'
                        + '</video></div>';
                    break;
            }
        }
    }
    else //Ad an empty details page so ul item indexing gives the correct index.
        adpage += '<div style="display:none"  class="details-page"/>';

    document.getElementById('adpages').innerHTML += adpage + '</li>';

}

var SetMegaDeal = function(megaDeal){

    if (megaDeal){
        $('#title').text(megaDeal.title);
        $('#description').text(megaDeal.description);
        $('#price').text('Deal Price: ' + megaDeal.price);
        $('#list').text(megaDeal.list);
        $('#end').text('Hurry! Deal Ends ' + megaDeal.end);
    }
    else {
        $('#mega-deal').html('<p>There is currently no Mega Deal</p>');
        $('#mega-deal').css('margin-top', '20%');
    }
}

var SetNarrator = function (imageURL) {

    $('#narrator-img').attr('src', imageURL);

    SetPage(0);
}

var AttachEventToPages = function () {

    var listItemIndex = 0;

    $('#cbp-fwslider').cbpFWSlider();

    $('#cbp-fwslider nav span.cbp-fwnext').click(function () {
        console.log(listItemIndex);
        SetPage(++listItemIndex);
    })

    $('#cbp-fwslider nav span.cbp-fwprev').click(function () {
        console.log(listItemIndex);
        SetPage(--listItemIndex);
    })

    $('.cbp-fwdots span').click(function () {
        listItemIndex = $('.cbp-fwcurrent').index();
        var narration = $('.adpage')[listItemIndex].dataset.narration;
        $('#narrator-text').text(narration);
        SetPage(listItemIndex);

    })

    $('#details-btn').click(function () {

        var adpage = $('.adpage')[listItemIndex];
        var pageDetails = $('.details-page')[listItemIndex];
        console.log(listItemIndex);
        console.log($('.details-page'));
        $(adpage).toggle();
        $(pageDetails).toggle();

        SetPage(listItemIndex);

    })

    $('#home').click(function(){
        $('#home').css('color', 'black');
    })

    // Once the video is ready
    var videos = Array.prototype.slice.call(document.querySelectorAll('.video-js'));

    for (var i = 0; i < videos.length; ++i) {
        _V_(videos[i].id).ready(function () {
            var myPlayer = _V_(videos[i].id);    // Store the video object
            _V_(videos[i].id, {}, function () { });
            var aspectRatio = 9 / 16; // Make up an aspect ratio
            function resizeVideoJS() {
                // Get the parent element's actual width
                var width = document.getElementById(myPlayer['Q']).parentElement.offsetWidth;
                var height = $('#cbp-fwslider').height();
                // Set width to fill parent element, Set height
 
                myPlayer.width(width).height(height);
            }

            resizeVideoJS(); // Initialize the function
            window.onresize = resizeVideoJS; // Call the function on resize
        });
    }

    $('#tabs a:first').tab('show');
}

var HideSpeechBubble = function () {

        $('#narrator-bubble').toggle();

}

var SetPage = function (index) {
    var adpage = $('.adpage')[index];
    var pageDetails = $('.details-page')[index];

    var narration;
    var details;

    if($(adpage).css('display') == 'none'){
        narration = pageDetails.dataset.narration;
        details = pageDetails.dataset.details;
    }
    else{
        narration = adpage.dataset.narration;
        details = adpage.dataset.details;
    }

    if (narration === "")
        $('#narrator-bubble').hide();
    $('#narrator-text').text(narration);

    if (details === "")
        $('#details-btn').css('visibility', 'hidden');
    else {
        $('#details-btn').css('visibility', 'visible');
        $('#details-text').text(details);
    }


}

engine.on('ChangeToExplorerStyle', function(){
    console.log("Herp");
    document.getElementById('adplayer-style').setAttribute('href', 'styles/explorer-adplayer.css');
    document.getElementById('style').setAttribute('href', '../Explorer/styles/style.css');

})


var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}