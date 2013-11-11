$(function () {
    $('.sub-category').click(function () {
        console.log(this.getAttribute('catid'));
        engine.call('GetBusinessSubCat', this.getAttribute('catid'));
    });

});

$(function () {
    var container = document.getElementById('st-container');
    $('#menu_btn').click(function () {
        container.className = 'st-container';
        classie.add(container, 'st-effect-1');
        setTimeout(function () {
            classie.add(container, 'st-menu-open');
        }, 25);
    });
    $('.st-container li').click(function (event) {
        event.stopPropagation();
        if (this.getAttribute('destination') != 'internal')
            GoToDestination(this.getAttribute('destination'));
    });
    $('.st-container').click(function (event) {
        classie.remove(document.getElementById('st-container'), 'st-menu-open');
    });
});
