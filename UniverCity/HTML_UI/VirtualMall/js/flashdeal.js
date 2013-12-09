/**
 * Created by Avengix on 12/8/13.
 */
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";


    engine.call('LoadFlashDeal');
});

engine.on('LoadFlashPlayer', function(flashdeal, URL){
    console.log(flashdeal);
    URL += "admedia?id=";
    var data = jQuery.parseJSON(flashdeal);
    console.log(data);
    for(var i = 0; i < data.parts.length; ++i) {
    	switch(data.parts[i].type)
    	{
    		case "image":
    			$('#container').append('<img src="'+ URL + data.parts[i].id +'"></img>');
    		break;
    		
    		case "video":
    			$('#container').append('<div class="adpage vzaar_media_player" data-title="" data-details="" data-narration="">'
                    + '<object data="http://view.vzaar.com/1417694/flashplayer" height="324" id="vzvid'+ i +'" type="application/x-shockwave-flash" width="576">' +
                    '<param name="wmode" value="transparent" /><param name="allowFullScreen" value="true" />' +
                    '<param name="movie" value="http://view.vzaar.com/1417694/flashplayer" />' +
                    '<param name="allowScriptAccess" value="always" />' +
                    '<param name="autoStart" value="true" />' +
                    '<param name="flashvars" value="border=none&amp;showplaybutton=rollover" />' +
                    '<video data-played="false" preload="metadata" controls height="324" id="htmlvid" onclick="this.play();"'+
                    'poster="'+ URL + data.parts[i].id +'&thumbnail=1' +'" preload="none" src="http://www.univercity3d.com/univercity/admedia?id=' + data.parts[i].id+ '" width="576" ></video></object>'
                    +'</div>');
    		break;
    		
    		case "text":
    			$('#container').append(data.parts[i].text);
    		break;
    	}
    }
    
    if (data.expert)
    	SetNarrator(URL + data.expert.id);
    else
    	$('#narrator-img').attr('style', 'display:none');
    	
    if (data.narrative === "")
        $('#narrator-bubble').hide();
    $('#narrator-text').text(data.narrative);
    
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