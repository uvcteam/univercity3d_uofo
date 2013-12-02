/**
 * Created by Avengix on 12/1/13.
 */

$(document).ready(function(){
    console.log(urlParam('id'));
    $('#business-card').attr('src', 'http://www.univercity3d.com/univercity/bizcard?id=' + urlParam('id'));

})
var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}