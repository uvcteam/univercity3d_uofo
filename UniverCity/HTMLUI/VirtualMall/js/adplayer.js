

var mediaURL = "http://app2.univercity3d.com/univercity/admedia?id=";
var objectToLike = 'http://samples.ogp.me/126210144220828';
var urlToUse = "http://app2.univercity3d.com/univercity/";
var userToken = "";
var businessID = -1;
var hasLiked = false;
var businessIsSaved = false;
var likeID;

$(document).ready(function () {
    //console.log("Ready for Unity.");

    var URL = "http://app2.univercity3d.com/univercity/getAd?b=";
    //Parameter passing breaks iOS so comment out hen building to iOS
    $.ajax({
        url: URL + urlParam("id"),
        success: function(adPlayerData){
            console.log(adPlayerData);
            PopulateAdPlayer(adPlayerData);
            SetMegaDeal(adPlayerData.megadeal);
            AttachEventToPages();
            SetNarrator(mediaURL + adPlayerData.expert.id);
            $('#turtle').hide();
            $('#container').show();

    }});


    engine.call('LoadAdData');
});


engine.on('LoadAdPlayer', function(id, URL, token){

    urlToUse = URL;
    userToken = token;
    businessID = id;
    objectToLike = URL + '/fbo?b=' + id;
    mediaURL = URL + "admedia?id=";
    URL += "getAd?b=";
    console.log(URL+id);

    $.ajax({
        url: URL + id,
        success: function(adPlayerData){
            PopulateAdPlayer(adPlayerData);
            SetMegaDeal(adPlayerData.megadeal);
            AttachEventToPages();
            SetNarrator(mediaURL + adPlayerData.expert.id);
            engine.call("CheckIfBusinessIsLiked");

    }});

    console.log(urlToUse + "ListSaved?token=" + token);

    $.ajax({
        url: urlToUse + "ListSaved?token=" + token,
        success: function(result){

            for(var i = 0; result.savedBusinesses && i < result.savedBusinesses.length; ++i)
            {
                if(result.savedBusinesses[i] == businessID)
                {
                    $('.fav-btn').html("Unsave");
                    businessIsSaved = true;
                    break;
                }
            }
            $('#turtle').hide();
            $('#container').show();

        }
    });

/*    $('.owl-page').each(function(i){
        console.log($(this).html());
        $(this).innerHTML = $('.adpage')[i].dataset.title;
        console.log($(this).html());
    })*/

})

var PopulateAdPlayer = function(adpage ) {

    for (var i = 0; i < adpage.pages.length; ++i) {
        if (adpage.pages[i].title !== "") {

            if (adpage.more){
                AddPage(adpage.pages[i], adpage.more.pages[i], i);
            }

            else
                AddPage(adpage.pages[i], null, i);

        }

    }

    $('.st-content').css('background', 'linear-gradient(#' + adpage.background.color + ',#' + adpage.background.color2 + ')');

}
var AddPage = function (adpage, detailsPage, index) {

    console.log(adpage);
    var style = "";
    var  pageItem = '<div class="page"><table class="adpage" data-title="' + adpage.title + '" data-details="' + adpage.more.title + '" data-narration="' + adpage.narrative + '">';
    var partType = "one";

    if(adpage.parts[0].type !== "video")
    {
            switch (adpage.parts.length)
        {
            case 1 :
                partType = "one";
                break;
            case  2:
                partType = "two";
                break;
            case 3:
                partType = "three";
                break;
            case 4:
                partType = "four";
                break;
        }
        pageItem += '<tr>';

        if(adpage.audio)
        {
            pageItem += '<audio autoplay="true" preload="auto" id="htmlaudio'+ index +'"><source type="audio/mpeg; codecs=\'mp3\';" src="'+ mediaURL + adpage.audio.id +'">' +
            + '<source type="audio/ogg; codecs=\'vorbis\'" src="'+ mediaURL + adpage.audio.id + '&alt=yes' +'">'
            + '</audio>';
        }


        for (var i =0; i < adpage.parts.length; ++i){

            if( adpage.parts.length == 4 && i == 2)
            {
                pageItem += '</tr><tr>';
            }

            console.log(adpage.parts[i].type);
            pageItem += '<td class="'+ partType +'">';

            switch(adpage.parts[i].type)
            {
                case "image":
                    pageItem += '<img src="' + mediaURL + adpage.parts[i].id + '"/>';
                    break;
                case "video":
                    pageItem += '<div class="vzaar_media_player">'
                        +'<video draggable="true" data-played="false" preload="metadata" controls class="htmlvid" id="htmlvid'+ index +'" onclick="this.play();" poster="'+ mediaURL + adpage.parts[i].id +'&thumbnail=1'
                        +'"preload="none" src="' + mediaURL + adpage.parts[i].id + '"></video>'
                        +'</div>';
                    break;
                case "text":
                    pageItem += '<div>'
                        + adpage.parts[i].text + '</div>';
                    break;
                case "audio":
                    pageItem += '<audio><srouce src="'+ + mediaURL + adpage.parts[i].id +'" type="audio/ogg"/></autio>';

            }

            pageItem += '</td>';
        }
        pageItem += '</tr>';

        pageItem += '</table>';

    }
    else //videos need to be in div to size properly.
    {
        pageItem = '<div class="page"><div class="adpage" data-title="' + adpage.title + '" data-details="' + adpage.more.title + '" data-narration="' + adpage.narrative + '"><div class="vzaar_media_player">'
        +'<video draggable="true" data-played="false" preload="metadata" controls id="htmlvid'+ index +'" onclick="this.play();" poster="'+ mediaURL + adpage.parts[0].id +'&thumbnail=1'
        +'"preload="none" src="' + mediaURL + adpage.parts[0].id + '"></video>'
        +'</div></div>';
    }



    if(adpage.more.parts.length !== 0){
        switch (adpage.more.parts.length)
        {
            case 1 :
                partType = "one";
                break;
            case  2:
                partType = "two";
                break;
            case 3:
                partType = "three";
                break;
            case 4:
                partType = "four";
                break;
        }

        pageItem += '<table class="details-page" data-title="' + adpage.more.title + '" data-details="' + adpage.title + '" data-narration="' + adpage.more.narrative + '">';

        pageItem += '<tr>';

        for (var i = 0; i < adpage.more.parts.length; ++i)
        {
            if( adpage.more.parts.length == 4 && i == 2)
            {
                pageItem += '</tr><tr>';
            }

            pageItem += '<td class="'+ partType +'">';
            switch (adpage.more.parts[i].type)
            {
                case "image":
                    pageItem += '<img src="' + mediaURL + adpage.more.parts[i].id + '"/></div>';
                    break;
                case "video":
                    pageItem += '<div class="vzaar_media_player">'
                        +'<video draggable="true" data-played="false" preload="metadata" controls id="htmlvid-details'+ index +'" onclick="this.play();" poster="'+ mediaURL + adpage.more.parts[i].id +'&thumbnail=1'
                        +'"preload="none" src="' + mediaURL + adpage.more.parts[i].id + '"></video>'
                        +'</div>';
                    break;
                case "text":
                    pageItem += '<div>'
                        + adpage.more.parts[i].text + '</div>';
            }
            pageItem += '</td>';
        }

        pageItem += '</tr>';

        pageItem += '</table>';

    }
    else //Ad an empty details page so ul item indexing gives the correct index.
        pageItem += '<div style="display:none"  class="details-page"/>';

    document.getElementById('adpages').innerHTML += pageItem + '</div>';
    $('.details-page').css('display', 'none');
}

var SetMegaDeal = function(megaDeal){
    if (megaDeal){
        $('.mega-btn').click(function (e) {
            e.preventDefault();
            $('#mega-deal').show();
            $('#adpages').hide();
            $('#details-btn').hide();
            engine.call("TrackUserAction",businessID, "MegaDeal", "deal");
        })
        //$('.mega-btn')[0].css('background','radial-gradient(yellow, yellowgreen, limegreen)');
        $('#title').text(megaDeal.title);
        $('#description').append(megaDeal.description);
        //$('#description').text(megaDeal.description);
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

    $('.owl-carousel').owlCarousel({
        navigation : true, // Show next and prev buttons
        slideSpeed : 300,
        paginationSpeed : 400,
        singleItem: true,
        navigation: false,
        afterMove: function(){
            $('.owl-page.active').trigger('click');
        }
    });


    $('.owl-page').click(function () {
        var htmlVideo = $('#htmlvid'+listItemIndex);
        var htmlAudio = $('#htmlaudio'+listItemIndex);

        if (htmlVideo.length)
            htmlVideo.get(0).pause();

        if(htmlAudio.length) {
            htmlAudio.get(0).pause();
        }

        if ($('#htmlvid-details'+listItemIndex).length)
            $('#htmlvid-details'+listItemIndex).get(0).pause();

        $($('.details-page')[listItemIndex]).hide();

        $($('.adpage')[listItemIndex]).show();

        listItemIndex = $('.owl-page.active').index();

        htmlVideo = $('#htmlvid'+listItemIndex);
        htmlAudio = $('htmlaudio'+listItemIndex);

        var narration = $('.adpage')[listItemIndex].dataset.narration;
        $('#narrator-text').text(narration);
        SetPage(listItemIndex);

        if(htmlAudio.length) {
            htmlAudio.get(0).play();
        }

        else if (htmlVideo.length && htmlVideo[0].dataset.played === "false") {
            htmlVideo[0].dataset.played = "true";
            htmlVideo.get(0).play();
        }

        engine.call("TrackUserAction",businessID, $('.owl-page.active').text(), "click");

    });

    $('#details-btn').click(function () {

        var adpage = $('.adpage')[listItemIndex];
        var pageDetails = $('.details-page')[listItemIndex];
        $(adpage).toggle();
        $(pageDetails).toggle();

        SetPage(listItemIndex);

        if ($('#htmlvid-details'+listItemIndex).length && $('#htmlvid'+listItemIndex)[0].dataset.played === "false") {
            $('#htmlvid-details'+listItemIndex)[0].dataset.played = "true";
            $('#htmlvid-details'+listItemIndex).get(0).play();
        }
        else if($('#htmlvid-details'+listItemIndex).length)
            $('#htmlvid-details'+listItemIndex).get(0).pause();

    })

    $('#side-btns button:first').tab('show');

    if($('#htmlvid0').length){
        $('#htmlvid0').get(0).addEventListener('canplay',function(){
            $('#htmlvid0')[0].dataset.played = "true";
            $('#htmlvid0').get(0).play();
        });
    }

    $('.st-content').css('transform', 'rotate(360deg)');

    $('audio').bind("ended", function(){
        var htmlVideo = $('#htmlvid'+listItemIndex);

        if (htmlVideo.length)
            htmlVideo.get(0).play();
    });

    $("video").bind("ended", function() {
        engine.call("TrackUserAction",businessID, $('.owl-page.active').text(), "media");
    });


    $('#home').click(function(){
        $('#home').css('color', 'black');
    })



    $('.owl-page').each(function(i){
        $(this).append($('.adpage')[i].dataset.title);
    })

}

var HideSpeechBubble = function () {

    var narrator = $('#narrator-bubble');
    var htmlAudio =  $('#htmlaudio' + $('.owl-page.active').index());

    if (narrator.css('visibility') === 'hidden') {

        if(htmlAudio.length)
            htmlAudio.get(0).play();

        narrator.css('visibility', 'visible');
    }
    else {

        if(htmlAudio.length)
            htmlAudio.get(0).pause();

        narrator.css('visibility', 'hidden')
    }

}

var SetPage = function (index) {
    var adpage = $('.adpage')[index];
    var pageDetails = $('.details-page')[index];

    var narration;
    var details;

    if(!adpage)
        return;

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
/*
    $('.deal-btn').click(function(){
        console.log("deal");
        $('.selected').removeClass('selected');
        $(this).addClass('selected');
    });*/

    $(this).children('button').click(function(){
        $(this).addClass('selected');
    })

});

$('.deal-btn').click(function(){
    $('.deal-btn').removeClass('selected');
    $(this).addClass('selected');
});

$('.done-btn').click(function(){
    $('#adpages').show();
    $('#details-btn').show();
    $('#mega-deal').hide();
    $('.deal-btn').removeClass('selected');
});

function postLike() {
    if (!hasLiked)
    {
        $('.facebook').attr("style", "color:red");
        engine.call("FacebookLike", objectToLike);
        //alert("Liked");
    }

    else
    {
        $('.facebook').attr("style", "color:white");
        engine.call("FacebookUnLike", likeID);
        //alert("Unliked");
    }

}

engine.on("CheckIfLiked", function(likes) {
    //alert(objectToLike);
    var response = jQuery.parseJSON(likes);
    var id;
    for ( var i = 0; i < response.data.length; ++i)
    {
        //alert(response.data[i].data.object.url);

        id  = response.data[i].data.object.url.split("=")[1];
        if( id === businessID )
        {
            hasLiked = true;
            likeID = response.data[i].id;
            $('.facebook').attr("style", "color:red");
        }
    }
})

function SaveBusiness()
{
    var saveURL = ""
    if (businessIsSaved === true)
    {
        saveURL = urlToUse + "UnsaveBusiness?token=" + userToken
            + "&id=" + businessID;
        engine.call("UnsaveBusiness", businessID);
    }

    else
    {
        saveURL = urlToUse + "SaveBusiness?token=" + userToken
            + "&id=" + businessID;
        engine.call("SaveBusiness", businessID);
    }

    $.ajax({
        type: 'POST',
        url: saveURL,
        success: function(result){
        }
    });
}

function PurchaseMegaDeal()
{
   var  purchaseURL = urlToUse + "mdpurchase?b=" + businessID
        + "&token=" + userToken;

    //$('#mega-deal').load(purchaseURL);

    location.href = purchaseURL;
}
