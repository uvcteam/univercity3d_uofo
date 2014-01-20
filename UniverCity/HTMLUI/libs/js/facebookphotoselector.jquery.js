var FacebookPhotoSelector =
{
	fbsdk : false,
	
	// Method to inject the Facebook Javascript SDK dependency
	setFacebookSDK : function(sdk)
	{
		FacebookPhotoSelector.fbsdk = sdk;
	},

	// Populate the list of albums
	loadAlbums : function(container, callback)
	{
		FacebookPhotoSelector.fbsdk.api('/me/albums?access_token=' + FBAUTH, function(response)
		{
			var html = FacebookPhotoSelector.renderAlbumList(response.data);
			container.html(html);
			callback(container, response.data);
			container.change();
		});
	},

	// Populate the photos based on the album selected
	loadPhotos : function(container, ref, settings, callback)
	{
		var params = null;

		if (ref.indexOf('://') > 0)
		{
			var uri = ref;
		}
		else
		{
			var uri = '/' + ref + '/photos?access_token=' + FBAUTH;
			var params = { limit: settings.photosPerPage };
		}
		
		FacebookPhotoSelector.fbsdk.api(uri, params, function(response)
		{
			container.find('.loading').remove();
			var html = FacebookPhotoSelector.renderPhotoList(response.data, settings.photoColumns);
			container.append(html);
			if (response.paging.next)
			{
				container.append(FacebookPhotoSelector.renderPhotoPagination(response.paging.next));
			}
			callback(container, response.data);
		});
	},

	renderLoadingIndicator : function()
	{
		return '<div class="loading"><img src="assets/img/loading.gif" alt="Loading, please wait..."></div>';
	},
	
	// Render a Facebook photo album as HTML
	renderAlbum : function(album)
	{
		return '<option value="' + album.id + '">' + album.name + '</option>';
	},

	// Render an array of Facebook photo albums as HTML
	renderAlbumList : function(albums)
	{
		var html = '';

		for (var i = 0; i < albums.length; i++)
		{
			html += FacebookPhotoSelector.renderAlbum(albums[i]);
		}

		return html;
	},

	// Render photo pagination
	renderPhotoPagination : function(url)
	{
		return '<a href="' + url + '" class="btn btn-default btn-block next">Load more photos</a>';
	},

	// Render a Facebook photo as HTML
	renderPhoto : function(photo)
	{
		return '<div class="col-md-3"><a href="#" class="thumbnail" data-facebook-id="' + photo.id + '"><img src="' + photo.picture + '" alt="' + photo.name + '""></a></div>';
	},

	// Render an array of Facebook photos as HTML
	renderPhotoList : function(photos, columns)
	{
		var html = '';
		
		// Render all the photos in groups of columns
		for (var i = 0; i < photos.length; i += columns)
		{
			html += '<div class="row">';

			// Add columns of photos to the row
			for (var j = 0; j < columns && (i + j) < photos.length; j++)
			{
				html += FacebookPhotoSelector.renderPhoto(photos[i + j]);
			}

			html += '</div><br>';
		}

		return html;
	}
};

// Facebook Photo Selector jQuery Plugin
(function ($)
{
	// Main plugin body
	$.fn.facebookPhotoSelector = function(settings)
	{
		settings = $.extend({}, $.fn.facebookPhotoSelector.defaults, settings);

		// Wait for the Facebook SDK before initializing the modals
		var modals = $(this);
		var fbsdkPoller = setInterval(function()
		{
			settings.fbsdk = settings.fbsdk || FacebookPhotoSelector.fbsdk;
			if (settings.fbsdk)
			{
				clearInterval(fbsdkPoller);
				modals.each(function(i, modal)
				{
					initModal(modal, settings);
				});
			}
		}, settings.fbsdkPollInterval);

		// Allow chaining
		return this;
	};

	// Plugin defaults
	$.fn.facebookPhotoSelector.defaults = {
		fbsdkPollInterval : 100, // in milliseconds
		selectors : {
			albums    : 'select.fbps-albums',
			photos    : '.fbps-photos',
			btnCancel : '.fbps-cancel',
			btnSelect : '.fbps-select'
		},
		onBeforeInit  : function() {},
		onLoadPhotos  : function() {},
		onLoadAlbums  : function() {},
		onAlbumChange : function() {},
		onSelect      : function() {},
		onFinalSelect : function() {},
		fbsdk         : false,
		fbscope       : 'user_photos',
		photoColumns  : 4,
		photosPerPage : 4 * 5,
		selectLimit   : 1
	};

	// Modal one-time initialization
	function initModal(modal, settings)
	{
		// Delay initializing the modal until it is opened the first time
		$(modal).on('show.bs.modal', function()
		{
			var albumSelectBox = $(modal).find(settings.selectors.albums);
			var photoList      = $(modal).find(settings.selectors.photos);

			// Deselect any photos that were selected
			photoList.find('a.selected').removeClass('selected');			

			// Don't re-initialize modals that have already been initialized
			if ($(modal).data('facebookPhotoSelector') === 'initialized')
			{
				return;
			}
			else
			{
				$(modal).data('facebookPhotoSelector', 'initialized');
			}

			settings.onBeforeInit(modal);

			// Facebook connect
			settings.fbsdk.getLoginStatus(function(response)
			{
				if (response.status === 'connected')
				{
					// Load the photo albums
					FacebookPhotoSelector.loadAlbums(albumSelectBox, settings.onLoadAlbums);
				}
				else
				{
					settings.fbsdk.login(function(response)
					{
						if (response.authResponse)
						{
							// Load the photo albums
							FacebookPhotoSelector.loadAlbums(albumSelectBox, settings.onLoadAlbums);
						}
					}, { scope: settings.fbscope });
				}
			});

			// Refresh the photos when the album changes
			$(modal).on('change', settings.selectors.albums, function(e)
			{
				var albumId = $(this).val();

				// Signal that the album was changed
				settings.onAlbumChange(albumId.toString());

				// Show loading indicator
				photoList.html(FacebookPhotoSelector.renderLoadingIndicator());

				// Load the photos
				FacebookPhotoSelector.loadPhotos(photoList, albumId, settings, settings.onLoadPhotos);
			});

			// Paginate the photos list
			$(settings.selectors.photos, modal).on('click', 'a.next', function(e)
			{
				e.preventDefault();
				var url = $(this).attr('href');

				// Remove the paging link
				$(this).remove();

				// Show loading indicator
				photoList.append(FacebookPhotoSelector.renderLoadingIndicator());

				// Load the photos
				FacebookPhotoSelector.loadPhotos(photoList, url, settings, settings.onLoadPhotos);
			});

			// Toggle the selection status of a photo on click
			$(settings.selectors.photos, modal).on('click', 'a.thumbnail', function(e)
			{
				e.preventDefault();
				var selectedCount = photoList.find('.selected').length;

				// Only check the limit if we're selecting a new photo that wasn't already selected
				if ( ! $(this).hasClass('selected'))
				{
					if (settings.selectLimit == 1)
					{
						// Auto-unselect if the limit was set to 1
						photoList.find('.selected').removeClass('selected');
					}
					else if (settings.selectLimit > 1 && selectedCount >= settings.selectLimit)
					{
						// Enforce selection limit
						return false;
					}

					settings.onSelect($(this).data('facebook-id').toString());
				}

				$(this).toggleClass('selected');
			});

			// Get a list of the selected photo IDs when finished
			$(modal).on('click', settings.selectors.btnSelect, function(e)
			{
				var photos = [];

				photoList.find('a.selected').each(function()
				{
					photos.push($(this).data('facebook-id').toString());
				});

				settings.onFinalSelect(photos);
			});
		});
	};

} (jQuery));