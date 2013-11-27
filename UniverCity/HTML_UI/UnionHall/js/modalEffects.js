/**
 * modalEffects.js v1.0.0
 * http://www.codrops.com
 *
 * Licensed under the MIT license.
 * http://www.opensource.org/licenses/mit-license.php
 * 
 * Copyright 2013, Codrops
 * http://www.codrops.com
 */
$(function(){
   ModalEffects();
});

function ModalEffects() {

	function init() {

		var overlay = document.querySelector( '.md-overlay' );

		[].slice.call( document.querySelectorAll( '.md-trigger' ) ).forEach( function( el, i ) {

			var modal = document.querySelector( '#' + el.getAttribute( 'data-modal' ) ),
				close = modal.querySelector( '.md-close' );

			function removeModal( hasPerspective ) {
				classie.remove( modal, 'md-show' );

				if( hasPerspective ) {
					classie.remove( document.documentElement, 'md-perspective' );
				}
			}

			function removeModalHandler() {
				removeModal( classie.has( el, 'md-setperspective' ) ); 
			}

			el.addEventListener( 'click', function( ev ) {
                var parent = $(ev.target).parent("div");
                console.log(parent);
                ChangeModal(parent.attr('event-name'),
                            parent.attr('event-date'),
                            parent.attr('event-time'),
                            parent.attr('event-who'),
                            parent.attr('event-what'),
                            parent.attr('event-where'));
				classie.add( modal, 'md-show' );
				overlay.removeEventListener( 'click', removeModalHandler );
				overlay.addEventListener( 'click', removeModalHandler );

				if( classie.has( el, 'md-setperspective' ) ) {
					setTimeout( function() {
						classie.add( document.documentElement, 'md-perspective' );
					}, 25 );
				}
			    $('md-modal').hisResText();
			});

            if (close) {
                close.addEventListener( 'click', function( ev ) {
                    ev.stopPropagation();
                    removeModalHandler();
                });
            }
		} );

	}

	init();
};

function ChangeModal(name, date, time, who, desc, where) {
    $('.m-event-title').html(name);
    $('.m-event-when').html(date + ' - ' + time);
    $('.m-event-who').html(who);
    $('.m-event-desc').html(desc);
    $('.m-event-where').html(where);
}