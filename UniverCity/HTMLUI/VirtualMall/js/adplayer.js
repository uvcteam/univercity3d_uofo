

var mediaURL = "http://app2.univercity3d.com/univercity/admedia?id=";
var objectToLike = 'http://samples.ogp.me/126210144220828';
var urlToUse = "http://app2.univercity3d.com/univercity/";
var userToken = "";
var businessID = -1;
var hasLiked = false;
var likeID;
var URL = "http://app2.univercity3d.com/univercity/";

engine.call('LoadAdData');

engine.on('LoadAdPlayer', function(id, URL, token, businessName) {

/*    URL = "http://app2.univercity3d.com/univercity/getAd?b=" + id;
    userToken = token;
    businessID = id;
    console.log('LoadAdPlayer:' + token);*/

    scope.$apply(function(){
        scope.BusinessName = businessName;
    });

});

angular.module('AdPlayer', [])
    .factory('AdPlayerData', function($http){
        return {
            get: function(id) {
                 return $http.get("http://app2.univercity3d.com/univercity/getAd?b=" + id);
            }
        };
    })
    .service('BusinessSaved', ['$http', function($http){
        return {
            get: function(token) {
                console.log("Token "  + token);
                return $http.get("http://app2.univercity3d.com/univercity/ListSaved?token=" + token);
            }
            
        };
    }])
    .controller('AdPlayerController', function($scope, $http, AdPlayerData, BusinessSaved ){
        window.scope = $scope;
        $scope.mediaURL = "http://app2.univercity3d.com/univercity/admedia?id=";

        $scope.landscape = (window.innerWidth > window.innerHeight) && isMobile.any() ? true : false;


        $scope.portrait =  (window.innerHeight > window.innerWidth) || !isMobile.any() ? true : false;


        if ($(window).height() > $(window).width() || !isMobile.any()) {

            $('.orientation').removeClass('small-7');
            $('.orientation').addClass('small-12 large-9');
            $scope.orientation = 'portrait'
        }
        else {

            $('.orientation').removeClass('small-12 large-9');
            $('.orientation').addClass('small-7');
            $scope.orientation = 'landscape';

        }



        $(window).on("orientationchange",function(event){
                //location.reload();
            
            scope.$apply(function()
            {
               if ($(window).height() > $(window).width() ) {
                    scope.landscape = false;
                    scope.portrait = true;
                    $('.orientation').removeClass('small-7');
                    $('.orientation').addClass('small-12 large-9');
                    scope.orientation = 'portrait';
                }
                else {
                    scope.portrait = false;
                    scope.landscape = true;
                    $('.orientation').removeClass('small-12 large-9');
                    $('.orientation').addClass('small-7');
                    scope.orientation = 'landscape';
                }
            });
         

        });

        $scope.SaveBusiness = function() {
            var saveURL = ""
            if (scope.BusinessSaved === true)
            {
                saveURL = "http://app2.univercity3d.com/univercity/UnsaveBusiness?token=" + userToken
                    + "&id=" + businessID;

                scope.AdFavorited = '';
                console.log("UnSaving..")

                scope.BusinessSaved = false;

                engine.call("UnsaveBusiness", businessID);
            }

            else
            {
                saveURL = "http://app2.univercity3d.com/univercity/SaveBusiness?token=" + userToken
                    + "&id=" + businessID;

                scope.AdFavorited = 'favorited';

                scope.BusinessSaved = true;

                engine.call("SaveBusiness", businessID);
            }

            $http({
                type: 'POST',
                url: saveURL,
                success: function(result) {
                    console.log("Worked!~")
                }
            });  
        } 


        $scope.Init = function() {

            userToken = localStorage.getItem('userToken');

            businessID = urlParam("id");

            AdPlayerData.get(businessID).then(function(result){
                var data = result.data;
                console.log(data)
                for( var i = 0; i < data.pages.length; i++) {
                    if(data.pages[i].title === "")
                        data.pages.splice(i--,1);
                }
                $scope.AdPlayerData = data;
            });
            BusinessSaved.get(userToken).then(function(result) {
                $scope.BusinessSaved = false;
                var businesses = result.data;
                for(var i = 0; i < businesses.savedBusinesses.length; ++i)
                {
                    console.log(businessID);
                    if(businesses.savedBusinesses[i] == businessID)
                    {
                        $scope.BusinessSaved = true;
                        $scope.AdFavorited = 'favorited';
                        break;
                    }
                }
            });
        }



    })
    .directive('addVideo', function() {
        return {
            restrict: "A",
            link: function(scope, element, attributes) {
                element.html('<video src="' + attributes.addVideo + '" data-played="false" preload="none" controls class="htmlvid" ></video>');
            }       
        }
    })
    .directive('addAudio', function() {
        return {
            restrict: "A",
            link: function(scope, element, attributes) {
                element.html('<audio src="' +  attributes.addAudio + '" type="audio/ogg"></audio>');
            }       
        }
    })
    .directive('initiateRightMenu', function($timeout) {
        return {
            restrict: 'A',
            link: function(scope, element, attributes) {

                $timeout(function() {
/*
                    $(document).foundation({
                          offcanvas : {
                            open_method: 'move',
                            close_on_click : true
                          }
                        });*/
                })
            }
        }
    })
    .directive('initiateOrbitCarousel', function($timeout) {
        return {
            restrict: 'A',
            link: function(scope, element, attributes) {

                if(scope.$last) {

                    $timeout(function() {

                        element.parent().owlCarousel({
                            items: 1,
                            itemsMobile: [2000, 1],
                            itemsTablet: [2000, 1],
                            itemsScaleUp: true,
                            responsive: true,
                            responsiveRefreshRate: 200,
                            pagination: false,
                            afterInit: function() {
                                $($('.slide-btn.bottom')[0]).addClass('active');
                                $($('.slide-btn.side')[0]).addClass('active');
                                $('.spinner-modal').css('display', 'none');
                            },
                            afterMove: function(){
                                var currentSlide = $(".owl-carousel").data('owlCarousel').currentItem;

                                $('.active').each(function()
                                {
                                    $(this).removeClass('active');
                                });

                                $($('.slide-btn.bottom')[currentSlide]).addClass('active');
                                $($('.slide-btn.side')[currentSlide]).addClass('active');
                             
                                PlayVideoOnPage(currentSlide);

                            }
                        });

                        //TODO put in own directive without error*/  
                        $(document).foundation({
                            offcanvas : {
                                open_method: 'move',
                                close_on_click : true
                            }
                        });

                    })
                };
            }
        }
    })
    .directive('initiateOrbitButtons', function($timeout) {
    return {
        restrict: 'A',
        link: function(scope, element, attributes) {

            var parts = angular.fromJson(attributes.initiateOrbitButtons);

             switch (parts[0].type) {
                    case "image":
                        element.html('<img class="page-icon" src="' + mediaURL + parts[0].id + '"></img>');
                    break;

                    case "video":
                        element.html('<img class="page-icon" src="' + mediaURL + parts[0].id + '&thumbnail=1"></img>');
                    break;

                    case "audio":
                        element.html('<div add-audio="' + mediaURL + parts[0].id + '">');
                    break;


                    case "text":
                        element.html('<div class="icon-text"><div>'+scope.AdPlayerData.pages[scope.$index].title+'</div></div>');
                    break;
                }




            var height = rowHeight();
            if(parts[0].type === "text")
                height -= 6;
            element.height(height);
            element.width(height);
            element.nailthumb();

            element.click(function() {
                var owl = $(".owl-carousel").data('owlCarousel');
                owl.goTo(scope.$index);
            })


            if(scope.$last) {

                $timeout(function() {

                    if( element.parent().parent().hasClass('owl-carousel')) {

                        element.parent().parent().owlCarousel({
                            items: 5,
                            itemsMobile: [479, 3],
                            itemsTablet: [768, 3],
                            itemsScaleUp: false,
                            responsive: true,
                            responsiveRefreshRate: 200,
                            pagination: false
                        });
                    }


                });
                
            }

        }
    }
    })
    .directive('initLikeButton', function() {
        return {
            restrict: 'A',
            link: function(scope, element, attributes) {

                var height = rowHeight();
                element.css('line-height', height + 'px');
                //element.css('width', height + 'px');
            } 
        }
    })
    .directive('initFlowtype', function(){
        return {
            restrict: "A",
            link: function(scope, element, attributes) {
                element.flowtype({
                    minFont: 11,
                    maxFont: 50,
                    minimum : 300,
                    maximum : 1200
                })
            }
        }
    })
    .directive('singleImagePage', function() {
        return {

            restrict: "A",
            link: function(scope, element, attributes) {

                var parts = angular.fromJson(attributes.singleImagePage);

                //var size = ResizeImage(parts[i].width, parts[i].height, window.innerWidth, window.innerHeight, 0.8);

                switch (parts[0].type) {
                    case "image":
                        element.html('<img class="ad-page-img" src="' + mediaURL + parts[0].id + '"></img>');
                    break;

                    case "video":
                        element.html('<video data-played="false" poster="' + mediaURL + parts[0].id  + '&thumbnail=1' + '"class="ad-page-img" controls preload="auto"><source type="audio/ogg; codecs=\'vorbis\'" src="' + mediaURL + parts[0].id + '&alt=yes' + '"></source></video>');
                    break;

                    case "audio":
                        element.html('<div add-audio="' + mediaURL + parts[0].id + '">');
                    break;


                    case "text":
                        element.html(parts[0].text);
                    break;
                }
            }
        }
    })
    .directive('doubleImagePage', function() {
        return {

            restrict: "A",
            link: function(scope, element, attributes) {

            var parts = angular.fromJson(attributes.doubleImagePage);

            var html = '<div class="row">';

            for( var i = 0; i < parts.length; ++i ) {

                    var size = ResizeImage(parts[i].width, parts[i].height, window.innerWidth, window.innerHeight, 0.4);

                    switch (parts[i].type) {
                        case "image":
                            //html = '<div class="col-xs-5" style="background-image: url(' + mediaURL + parts[i].id + '); background-position: center center; background-size: contain; background-repeat: no-repeat; width: ' + size.width + 'px; height: ' + size.height + 'px;"></div>';
                            html += '<img class="col-xs-6 col-sm-6" src="' + mediaURL + parts[i].id + '"></img>';
                        break;

                        case "video":
                            html += '<div class="col-xs-6 col-sm-6" add-video="' + mediaURL + parts[i].id + '">';
                        break;

                        case "audio":
                            html += '<div class="col-xs-6 col-sm-6" add-audio="' + mediaURL + parts[i].id + '">';
                        break;

                        case "text":
                            html += '<div class="col-xs-6 col-sm-6">' + parts[i].text + '</div>';
                        break;
                    }
                }

            html += '</div>';

            element.append(html);

            }

        }
        
    })
    .directive('tripleImagePage', function() {

        return {

            restrict: "A",
            link: function(scope, element, attributes) {

            var parts = angular.fromJson(attributes.tripleImagePage);

            var html = '<div class="row">';

            for( var i = 0; i < parts.length; ++i ) {

                    var size = ResizeImage(parts[i].width, parts[i].height, window.innerWidth, window.innerHeight, 0.33);

                    switch (parts[i].type) {
                        case "image":
                            //html = '<div class="two" style="background-image: url(' + mediaURL + parts[i].id + '); background-position: center center; background-size: contain; background-repeat: no-repeat; width: ' + size.width + 'px; height: ' + size.height + 'px;"></div>';

                            //html += '<div class="col-xs-3" style="background-image: url(' + mediaURL + parts[i].id + '); background-position: center center; background-size: contain; background-repeat: no-repeat;></div>';
                            html += '<img class="col-xs-4 col-md-4" src="' + mediaURL + parts[i].id + '"></img>';

                        break;

                        case "video":
                            html += '<div class="col-xs-4 col-md-4" add-video="' + mediaURL + parts[i].id + '">';
                        break;

                        case "audio":
                            html += '<div class="col-xs-4 col-md-4" add-audio="' + mediaURL + parts[i].id + '">';
                        break;

                        case "text":
                            html += '<div class="col-xs-4 col-md-4">' + parts[i].text + '</div>';
                        break;
                    }
                }

            html += '</div>';
            element.append(html);

            }
        }
    })
    .directive('quadrupleImagePage', function() {

        return {

            restrict: "A",
            link: function(scope, element, attributes) {

            var parts = angular.fromJson(attributes.quadrupleImagePage);

            var html = '<div class="row">';

            for( var i = 0; i < parts.length; ++i ) {

                    var size = ResizeImage(parts[i].width, parts[i].height, window.innerWidth, window.innerHeight, 0.2);

                    if(i % 2 === 0)
                        html += '</div><div class="row">';

                    switch (parts[i].type) {

                        case "image":
                            //html = '<div class="two" style="background-image: url(' + mediaURL + parts[i].id + '); background-position: center center; background-size: contain; background-repeat: no-repeat; width: ' + size.width + 'px; height: ' + size.height + 'px;"></div>';

                             //html += '<div class="four" style="background-image: url(' + mediaURL + parts[i].id + '); background-position: center center; background-size: contain; background-repeat: no-repeat;></div>';
                             html += '<img class="col-xs-5 col-sm-5" src="' + mediaURL + parts[i].id + '"></img>';
                        break;
                        case "video":
                            html += '<div class="col-xs-5 col-sm-5" add-video="' + mediaURL + parts[i].id + '">';
                        break;

                        case "audio":
                            html += '<div class="col-xs-5 col-sm-5" add-audio="' + mediaURL + parts[i].id + '">';
                        break;

                        case "text":
                            html += '<div class="col-xs-5 col-sm-5">' + parts[i].text + '</div>';
                        break;
                    }
                }

            html += '</div>';

            element.append(html);

            }
        }
    })
    .directive('slide-button', function() {

    })
    .directive('autoMargin', function() {
        return {
            restrict: "C",
            link: function (scope, element, attrs) {
                element.css('margin-top', (  ( 100 - (element.height() / $('#adpages').height()) * 100) / 4 ) + '%');

                console.log(element.css('margin-top'));            

            }
        }
    })
    .filter('filterEmptyPages', function(){
        return function(page){
            console.log(page);
            return page.title.length > 0 ? 'true' : 'false';
        }
    });

    angular.element(document).ready(function() {
        shouldChangeMenu = false;
        HideMenu();
    });


var ResizeImage = function(width, height, cWidth, cHeight, ratioRed) {
    if ( cHeight > cWidth ) {
        var temp = cHeight;
        cHeight = cWidth;
        cWidth = temp;
    }
    var finalWidth = 0, finalHeight = 0;

    if ( width > height ) {
        var ratio = height / width;
        finalWidth = cWidth * ratioRed;
        finalHeight = cHeight * ratioRed * ratio;
    } else {
        var ratio = width / height;
        finalWidth = cWidth * ratioRed * ratio;
        finalHeight = cHeight * ratioRed;
    }

    return { width: Math.floor(finalWidth), height: Math.floor(finalHeight) };
}

var isMobile = {
    Android: function() {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
};

var rowHeight = function() {
    var screen = $(window);

    var adpageHeight = 0;

    if(screen.height() > screen.width()) {

        adpageHeight = screen.height() * 0.80;
        return $(window).height() - adpageHeight - $('.tab-bar').height() - 20;
    }
    else {

        adpageHeight = screen.width() * 0.80;
        return screen.width()  - adpageHeight - $('.tab-bar').height() - 20;
    }
}


var PlayVideoOnPage = function(pageIndex) {
    var videos = $('video');

    videos.each(function(){
        this.pause();
    });

    if(videos.length && videos[pageIndex].dataset.played === "false") {
        videos[pageIndex].dataset.played = "true";
        videos[pageIndex].play();
    }
}

var goBack = function(){
    window.history.back();
    engine.call('OnAdPlayerWasClosed');
}
