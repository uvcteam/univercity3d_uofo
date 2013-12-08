/**
 * Created by Avengix on 12/8/13.
 */
$(document).ready(function () {
    console.log("Ready for Unity.");

    var URL = "http://www.univercity3d.com/univercity/getAd?b=";


    engine.call('LoadFlashDeal');
});
engine.on('LoadFlashPlayer', function(flashdeal){
    console.log(flashdeal);
    var data = jQuery.parseJSON(flashdeal);
    console.log(data);
    $('#container').append(data.parts[0].text);
})

var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}