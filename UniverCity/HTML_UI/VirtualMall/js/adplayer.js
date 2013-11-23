


$(document).ready(function () {
    console.log("Ready for Unity.");
    engine.call("StartAdPlayer");

});
engine.on('PopulateAdPlayer', function (URL, MediaType, narration, details, index) {


    switch(MediaType)
    {
        case "Image":
            document.getElementById('adpages').innerHTML += '<li><div class="adpage" data-details="' + details + '" data-narration="' + narration + '"><a href="#"><img src="' + URL + '"/></a></div>'
            + '<div style="display:none" class="details-page" data-details="' + details + '" data-narration="' + narration + '"><a href="#"><img src="images/1.jpg"/></a></div></li>';
            break;
        case "Video":
            document.getElementById('adpages').innerHTML
            += '<li class="adpage" data-details="' + details + '" data-narration="' + narration + '"><video id="video' + index + '" class="video-js vjs-default-skin vjs-big-play-centered" controls preload="none"'
            + 'poster="http://video-js.zencoder.com/oceans-clip.png">'
            + '<source src="http://video-js.zencoder.com/oceans-clip.mp4" type="video/mp4" />'
            + '<source src="http://video-js.zencoder.com/oceans-clip.webm" type="video/webm" />'
            + '<source src="http://video-js.zencoder.com/oceans-clip.ogv" type="video/ogg" />'
            + '<track kind="captions" src="demo.captions.vtt" srclang="en" label="English"></track>'
            + '<track kind="subtitles" src="demo.captions.vtt" srclang="en" label="English"></track>'
            + '</video></li>';
            break;
        default:
            break;
    }
})

engine.on('SetFirstPage', function (imageURL) {

    $('#narrator-img').attr('src', imageURL);

    SetPage(0);
})

engine.on('AttachEventToPages', function () {

    var listItemIndex = 0;

    $('#cbp-fwslider').cbpFWSlider();

    $('#cbp-fwslider nav span.cbp-fwnext').click(function () {

        SetPage(++listItemIndex);
    })

    $('#cbp-fwslider nav span.cbp-fwprev').click(function () {

        SetPage(--listItemIndex);
    })

    $('.cbp-fwdots span').click(function () {
        listItemIndex = $('.cbp-fwcurrent').index();
        console.log(listItemIndex);
        var narration = $('.adpage')[listItemIndex].dataset.narration;
        $('#narrator-text').text(narration);

    })

    $('#details-btn').click(function () {
        var page = $('.adpage')[listItemIndex];
        var details = $('.details-page')[listItemIndex];
        $(page).toggle();
        $(details).toggle();

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
})

var HideSpeechBubble = function () {

        $('#narrator-bubble').toggle();

}

function getChildNumber(node) {

    if (node.parentNode !== undefined)
        return Array.prototype.indexOf.call(node.parentNode.getChildNumber, node);
}

var SetPage = function (index) {
    var narration = $('.adpage')[index].dataset.narration;
    var details = $('.adpage')[index].dataset.details;

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