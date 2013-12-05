/**
 * Created by Avengix on 12/1/13.
 */

$(document).ready(function(){
    engine.call("LoadBusinessCard");
})
var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}

engine.on("LoadBusinessCard", function(id){

    $('#business-card').attr('src', 'http://www.univercity3d.com/univercity/bizcard?id=' + id);

})