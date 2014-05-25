// Web Functions.
var CommerceChildren = [];
var CategoriesChildren = [];
var changedCommerce = false;
var changeCategories = false;

$(window).load(function () {
    console.log("GET THE CATEGORIES");
    engine.call("GetCategories");
});

function SavePreferences() {  
    if (changeCategories) {
        engine.call("UpdateCategories", GetCheckedNodes(
            $('.category-preferences').fancytree('getTree').getSelectedNodes()));

    }

    if(changedCommerce) {
        engine.call("UpdateCommerce", GetCheckedNodes(
            $(".commerce-preferences").fancytree('getTree').getSelectedNodes()));
    }

    window.location.href = 'memorybank.html';
}

function QuickSavePreferences() {
    var checkboxes = $(':input');
    var newString = '';
    for (var i = 2; i < checkboxes.length; ++i) {
        if (checkboxes[i].name.search('cat') !== -1 && $('#' + checkboxes[i].name).prop('checked')) {
            newString += '&i=' + checkboxes[i].name.substring(3);
        }
    }

    if (changeCategories) {
        engine.call("UpdateCategories", GetCheckedNodes(
            $('.category-preferences').dynatree("getSelectedNodes")));

    }

    if(changedCommerce) {
        engine.call("UpdateCommerce", GetCheckedNodes(
            $(".commerce-preferences").dynatree("getSelectedNodes")));
    }
    console.log("PREFERENCES QUICK SAVED!");
}

// Invoked by Unity3D.
engine.on("AddCategory", function (id, name, parentID, checked) {
    var child = {title: name, key: id, selected: checked, children: []};
    if(parentID !== -1) {
        for(var i = 0; i < CategoriesChildren.length; ++i) {
            if(CategoriesChildren[i]['key'] === parentID) {
                CategoriesChildren[i]['children'].push(child);
            }
        }
    }
    else
        CategoriesChildren.push(child);
});

engine.on("CategoriesFinished", function() {

    $('.category-preferences').fancytree({
        source: CategoriesChildren,
        checkbox: true,
        selectMode: 3,
        beforeSelect: function(event, data){
            changeCategories = true;
/*            console.log(GetCheckedNodes(
                $(".commerce-preferences").dynatree("getSelectedNodes")));*/
        },
        cookieId: "category-tree",
        idPrefix: "category-tree-"

    });
});

engine.on('AddCommerce', function(id, name, parentID, checked) {
    var child = {title: name, key: id, selected: checked, children: []};
    if(parentID !== -1) {
        for(var i = 0; i < CommerceChildren.length; ++i) {
            if(CommerceChildren[i]['key'] === parentID) {
                CommerceChildren[i]['children'].push(child);
            }
        }
    }
    else
        CommerceChildren.push(child);
})

engine.on('CommerceFinished', function(){
    console.log(CommerceChildren);
    $('.commerce-preferences').fancytree({
        source: CommerceChildren,
        checkbox: true,
        selectMode: 3,
        beforeSelect: function(event, data){
            changedCommerce = true;
        },
        cookieId: "commerce-tree",
        idPrefix: "commerce-tree-"
    });
})

function GetCheckedNodes(nodes)
{
    var prefIds = "";
    for( var i = 0; i < nodes.length; ++i) {
        prefIds += '&i=' + nodes[i].key;

    }


    return prefIds;
}

function SelectAll()
{
    console.log("Hello");
    $(".preferences-list").children().prop('checked', true);
}