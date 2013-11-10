$(function() {
	var container = document.getElementById('st-container');
	$('#menu_btn').click(function() {
		container.className = 'st-container';
		classie.add(container, 'st-effect-1');
		setTimeout(function() {
			classie.add(container, 'st-menu-open');
		}, 25);
	});
	$('.st-container li').click(function(event) {
		event.stopPropagation();
	});
	$('.st-container').click(function(event) {
		classie.remove(document.getElementById('st-container'), 'st-menu-open');
	});
});