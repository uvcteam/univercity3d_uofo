// Web Functions.

$(window).load(function () {
    console.log("GET THE CATEGORIES");
    engine.call("GetCategories");
});

function SavePreferences() {

    var checkboxes = $(':input');
    var newString = '';
    for (var i = 2; i < checkboxes.length; ++i) {
        if (checkboxes[i].name.search('cat') !== -1 && $('#' + checkboxes[i].name).prop('checked')) {
            newString += '&i=' + checkboxes[i].name.substring(3);
        }
    }
    
    if (changedPrefs) {
        engine.call("UpdateCategories", newString);
    }
    
    window.location.href = 'memorybank.html';
}

// Invoked by Unity3D.
engine.on("AddCategory", function (catid, category, checked) {
    var catdivid = '#cat' + catid;
    console.log('Adding category (' + catid + ') ' + category + ' -- ' + checked);
    $('.preferences-list').append('<li><input id="cat' + catid +
        '" name="cat' + catid + '" type="checkbox"><label for="cat' + catid + '">' + category + '</label></li>');

    $(catdivid).prop('checked', checked);
});

engine.on("CategoriesFinished", function() {
    UpdateCheckboxes();
});