/**
 * Created by Avengix on 12/8/13.
 */
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";


    engine.call('LoadFlashDeal');
});

engine.on('LoadFlashPlayer', function(flashdeal, URL){
    var mediaURL = URL + "admedia?id=";
    var adpage = jQuery.parseJSON(flashdeal);
    console.log(adpage);

    var style = "";
    var  pageItem = '<div class="page"><table class="adpage" data-title="" data-details="" data-narration="' + adpage.narrative + '">';
    var partType = "one";

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


    for (var i =0; i < adpage.parts.length; ++i){

        if( adpage.parts.length == 4 && i == 2)
        {
            pageItem += '</tr><tr>';
        }

        pageItem += '<td class="'+ partType +'">';

        switch(adpage.parts[i].type)
        {
            case "image":
                pageItem += '<img src="' + mediaURL + adpage.parts[i].id + '"/>';
                break;
            case "video":
                pageItem += '<div class="vzaar_media_player">'
                    +'<video draggable="true" data-played="false" preload="metadata" controls id="" onclick="this.play();" poster="'+ mediaURL + adpage.parts[i].id +'&thumbnail=1'
                    +'"preload="none" src="' + mediaURL + adpage.parts[i].id + '"></video>'
                    +'</div>';
                break;
            case "text":
                pageItem += '<div>'
                    + adpage.parts[i].text + '</div>';
        }

        pageItem += '</td>';
    }
    pageItem += '</tr>';

    pageItem += '</table>';
    
    if (adpage.expert)
    	SetNarrator(URL + adpage.expert.id);
    else
    	$('#narrator-img').attr('style', 'display:none');
    	
    if (adpage.narrative === "")
        $('#narrator-bubble').hide();
    $('#narrator-text').text(adpage.narrative);

    $('#container').append(pageItem);
    
})

var SetNarrator = function (imageURL) {

    $('#narrator-img').attr('src', imageURL);
}

var HideSpeechBubble = function () {

        $('#narrator-bubble').toggle();

}
var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}