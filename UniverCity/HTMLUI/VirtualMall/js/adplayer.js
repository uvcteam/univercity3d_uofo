

var mediaURL = "http://www.univercity3d.com/univercity/admedia?id=";
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";
    //Parameter passing breaks iOS so comment out hen building to iOS
/*    $.ajax({url: URL + urlParam("id"), success: function(adPlayerData){
        console.log(adPlayerData);
        PopulateAdPlayer(adPlayerData);
        SetMegaDeal(adPlayerData.megadeal);
        AttachEventToPages();
        SetNarrator(mediaURL + adPlayerData.expert.id);

    }});*/

    engine.call('LoadAdData');
});

engine.on('LoadAdPlayer', function(id, URL){
    mediaURL = URL + "admedia?id=";
    URL += "getAd?b=";
    console.log('LoadAdPlauer');
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
    console.log(data.background.color);
    console.log(data.background.color2);
    $('.st-content').css('background', 'linear-gradient(#' + data.background.color + ',#' + data.background.color2 + ')');

}
var AddPage = function (adpageTitle, adpageParts, adpageNarrative, detailsTitle, detailsParts, detailsNarrative, index) {

    var adpage = '<div>';
    for (var i = 0; i < adpageParts.length; ++i)
    {
        switch(adpageParts[i].type)
        {
            case "image":
                adpage += '<div class="adpage" data-title="' + adpageTitle + '" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '"><img src="' + mediaURL + adpageParts[i].id + '"/></div>';
                break;
            case "video":
                adpage += '<div class="adpage vzaar_media_player" data-title="' + adpageTitle + '" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '">'
                    +
                    '<video data-played="false" preload="metadata" controls id="htmlvid'+ index +'" onclick="this.play();" poster="'+ mediaURL + adpageParts[i].id +'&thumbnail=1'
                    +'"preload="none" src="' + mediaURL + adpageParts[i].id + '"></video>'
                    +'</div>';
                break;
            case "text":
                adpage += '<div class="adpage" data-title="' + adpageTitle + '" data-details="' + detailsTitle + '" data-narration="' + adpageNarrative + '">'
                    + adpageParts[i].text + '</div>';
        }
    }

    if(detailsParts.length !== 0){

        for (var i = 0; i < detailsParts.length; ++i)
        {
            switch (detailsParts[i].type)
            {
            case "image":
                adpage += '<div class="details-page" data-title="' + detailsTitle + '" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '"><img src="' + mediaURL + detailsParts[i].id + '"/></div>';
                break;
            case "video":
                adpage += '<div class="details-page vzaar_media_player" data-title="' + detailsTitle + '" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '">'

                    +'<video data-played="false" preload="metadata" controls id="htmlvid-details'+ index +'" onclick="this.play();" poster="'+ mediaURL + detailsParts[i].id +'&thumbnail=1'
                    +'"preload="none" src="' + mediaURL + detailsParts[i].id + '"></video>'
                    +'</div>';
                break;
            case "text":
                adpage += '<div class="details-page" data-title="' + detailsTitle + '" data-details="' + adpageTitle + '" data-narration="' + detailsNarrative + '">'
                    + detailsParts[i].text + '</div>';
        	}
        }
    }
    else //Ad an empty details page so ul item indexing gives the correct index.
        adpage += '<div style="display:none"  class="details-page"/>';

    document.getElementById('adpages').innerHTML += adpage + '</div>';
    $('.details-page').css('display', 'none');
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
    	
        if ($('#htmlvid'+listItemIndex).length)
            $('#htmlvid'+listItemIndex).get(0).pause();
        if ($('#htmlvid-details'+listItemIndex).length)
            $('#htmlvid-details'+listItemIndex).get(0).pause();
        $($('.details-page')[listItemIndex]).hide();
        $($('.adpage')[listItemIndex]).show();
        listItemIndex = $('.owl-page.active').index();
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

        if ($('#htmlvid-details'+listItemIndex).length && $('#htmlvid'+listItemIndex)[0].dataset.played === "false") {
            $('#htmlvid-details'+listItemIndex)[0].dataset.played = "true";
            $('#htmlvid-details'+listItemIndex).get(0).play();
        }
        else if($('#htmlvid-details'+listItemIndex).length)
            $('#htmlvid-details'+listItemIndex).get(0).pause();

    })


    $('.owl-page').each(function(i){
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
    
	$('.st-content').css('transform', 'rotate(360deg)');

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
