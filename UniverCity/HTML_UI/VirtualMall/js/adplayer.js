

var mediaURL = "http://www.univercity3d.com/univercity/admedia?id=";
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";
    //Parameter passing breaks iOS so comment out hen building to iOS
    $.ajax({url: URL + urlParam("id"), success: function(adPlayerData){
        console.log(adPlayerData);
        PopulateAdPlayer(adPlayerData);
        SetMegaDeal(adPlayerData.megadeal);
        AttachEventToPages();
        SetNarrator(mediaURL + adPlayerData.expert.id);

    }});

    engine.call('LoadAdData');
});

engine.on('LoadAdPlayer', function(id, URL){
    mediaURL = URL + "admedia?id=";
    URL += "getAd?b=";
    $.ajax({url: URL + id, success: function(adPlayerData){
        console.log(adPlayerData);
        PopulateAdPlayer(adPlayerData);
        SetMegaDeal(adPlayerData.megadeal);
        AttachEventToPages();
        SetNarrator(mediaURL + adPlayerData.expert.id);

    }});
})

var PopulateAdPlayer = function(data ) {

    for (var i = 0; i < data.pages.length; ++i) {
        if (data.pages[i].title !== "") {
            AddPage(data.pages[i].title, data.pages[i].parts, data.pages[i].narrative,
                data.pages[i].more.title, data.pages[i].more.parts, data.pages[i].more.narrative, i);
        }

    }

}
var AddPage = function (adpageTitle, adpageParts, adpageNarrative, detailsTitle, detailsParts, detailsNarrative, index) {

    var adpage = '<li>';
    for (var i = 0; i < adpageParts.length; ++i)
    {
        switch(adpageParts[i].type)
        {
            case "image":
                adpage += '<div class="adpage" data-title="' + adpageTitle + '" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '"><img src="' + mediaURL + adpageParts[i].id + '"/></div>';
                break;
            case "video":
                adpage += '<div class="adpage vzaar_media_player" data-title="' + adpageTitle + '" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '">'
                    + '<object data="http://view.vzaar.com/1417694/flashplayer" height="324" id="vzvid'+ i +'" type="application/x-shockwave-flash" width="576">' +
                    '<param name="wmode" value="transparent" /><param name="allowFullScreen" value="true" />' +
                    '<param name="movie" value="http://view.vzaar.com/1417694/flashplayer" />' +
                    '<param name="allowScriptAccess" value="always" />' +
                    '<param name="autoStart" value="true" />' +
                    '<param name="flashvars" value="border=none&amp;showplaybutton=rollover" />' +
                    '<video data-played="false" preload="metadata" controls height="324" id="htmlvid'+ index +'" onclick="this.play();" poster="http://view.vzaar.com/1417694/image" preload="none" src="http://www.univercity3d.com/univercity/admedia?id=' + adpageParts[i].id + '" width="576" ></video></object>'
                    +'</div>';
                break;
        }
    }

    if(detailsParts.length !== 0){

        for (var i = 0; i < detailsParts.length; ++i)
        {
            switch (detailsParts[i].type)
            {
                case "image":
                    adpage += '<div style="display:none" class="details-page" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '"><img src="' + mediaURL + detailsParts[i].id + '"/></div>';
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
        $('mega-btn')[0].css('background','radial-gradient(yellow, yellowgreen, limegreen)');
        $('#title').text(megaDeal.title);
        $('#description').text(megaDeal.description);
        $('#price').text('Deal Price: ' + megaDeal.price);
        $('#list').text(megaDeal.list);
        $('#end').text('Hurry! Deal Ends ' + megaDeal.end);
    }
    else {
        console.log($('.mega-btn'));
        $('.mega-btn').attr('disabled', 'disabled');
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

        if ($('#htmlvid'+listItemIndex).length)
            $('#htmlvid'+listItemIndex).get(0).pause();
        SetPage(++listItemIndex);
        if ($('#htmlvid'+listItemIndex).length && $('#htmlvid'+listItemIndex)[0].dataset.played === "false") {
            $('#htmlvid'+listItemIndex)[0].dataset.played = "true";
            $('#htmlvid'+listItemIndex).get(0).play();
        }

    })

    $('#cbp-fwslider nav span.cbp-fwprev').click(function () {

        if ($('#htmlvid'+listItemIndex).length)
            $('#htmlvid'+listItemIndex).get(0).pause();
        SetPage(--listItemIndex);
        if ($('#htmlvid'+listItemIndex).length && $('#htmlvid'+listItemIndex)[0].dataset.played === "false") {
            $('#htmlvid'+listItemIndex)[0].dataset.played = "true";
            $('#htmlvid'+listItemIndex).get(0).play();
        }

    })

    $('.cbp-fwdots span').click(function () {
        if ($('#htmlvid'+listItemIndex).length)
            $('#htmlvid'+listItemIndex).get(0).pause();
        listItemIndex = $('.cbp-fwcurrent').index();
        var narration = $('.adpage')[listItemIndex].dataset.narration;
        $('#narrator-text').text(narration);
        SetPage(listItemIndex);
        if ($('#htmlvid'+listItemIndex).length && $('#htmlvid'+listItemIndex)[0].dataset.played === "false") {
            $('#htmlvid'+listItemIndex)[0].dataset.played = "true";
            $('#htmlvid'+listItemIndex).get(0).play();
        }

    });

    $('#details-btn').click(function () {

        var adpage = $('.adpage')[listItemIndex];
        var pageDetails = $('.details-page')[listItemIndex];
        console.log(listItemIndex);
        console.log($('.details-page'));
        $(adpage).toggle();
        $(pageDetails).toggle();

        SetPage(listItemIndex);

    })


    $('.cbp-fwdots').children('span').each(function(i){
        $(this).html($('.adpage')[i].dataset.title);
    })


    $('#home').click(function(){
        $('#home').css('color', 'black');
    })


    $('#side-btns button:first').tab('show');

    if($('#htmlvid0').length){
        $('#htmlvid0').get(0).addEventListener('canplay',function(){
            $('#htmlvid0')[0].dataset.played = "true";
            $('#htmlvid0').get(0).play();
        });
    }

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
    document.getElementById('adplayer-style').setAttribute('href', 'styles/explorer-adplayer.css');
    document.getElementById('style').setAttribute('href', '../Explorer/styles/style.css');

})


var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}

$('#side-btns').children('li').each(function(){

    $(this).children('button').click(function(){

        $('.selected').removeClass('selected');
        $(this).addClass('selected');
    });
});

$('.done-btn').click(function(){
    console.log($('#cbp-fwslider'));
    $('#side-btns button:first').tab('show');
    $('.selected').removeClass('selected');
});
