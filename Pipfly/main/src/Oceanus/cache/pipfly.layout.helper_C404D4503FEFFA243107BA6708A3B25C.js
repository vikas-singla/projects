﻿function layoutPhotosCollage(c,a,b,d){if(!document.photoCollageLayoutDefined)if($(a+" .photocollage")!="undefined"&&$(a+" .photocollage").length>0){document.photoCollageLayoutDefined=1;$(a+" .photocollage").isotope({itemSelector:".collagephoto",masonry:{columnWidth:1},layoutMode:"masonry",resizable:false,animationEngine:"best-available"});$(window).load(function(){$(a+" .photocollage").isotope("reLayout")})}if(!document.photoCollageScollLinked){document.photoCollageScollLinked=1;setPhotoInfiniteScroll(c,a,b,d)}}function setPhotoInfiniteScroll(d,a,b,e){var c=$(a).attr("data-since-date");$(a).unlimitedscroll({url:d,load_callback:b,since_date:c,user_id:e,start_page:1,item_selector:".collagephoto",overlay:"#loading_overlay"})}function layoutArticlesCollage(c,a,b,d){if($(a+" .articlemaincontainer")!="undefined"&&$(a+" .articlemaincontainer").length>0)if(!document.articleCollageLayoutDefined){document.articleCollageLayoutDefined=1;$(a+" .articlemaincontainer").isotope({itemSelector:".articlecontainer",masonry:{columnWidth:320},layoutMode:"masonry",resizable:true,animationEngine:"best-available"})}if(!document.articleCollageScollLinked){document.articleCollageScollLinked=1;setArticleInfiniteScroll(c,a,b,d)}}function setArticleInfiniteScroll(d,a,b,e){var c=$(a).attr("data-since-date");$(a).unlimitedscroll({url:d,load_callback:b,since_date:c,user_id:e,start_page:1,item_selector:".articlecontainer",overlay:"#loading_overlay"})}function layoutVideosCollage(d,a,c,e){if($(a+" .photocollage")!="undefined"&&$(a+" .photocollage").length>0){var b=$(a+" .collagephoto");b.each(function(){$("img",this).each(function(){var a=$(this).width()*1/($(this).height()*1);if(a>=.75&&a<=1.34)if($(this).width()>200&&$(this).height()>200){$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");$(this).width(225);$(this).css("height","")}else{$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");if($(this).width()>$(this).height()){$(this).height(170);$(this).css("width","")}else{$(this).width(225);$(this).css("height","")}}else if(a<.75){$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");var c=170*1/$(this).height()*$(this).width();if(c<225){$(this).width(225);$(this).css("height","")}else{$(this).height(170);$(this).css("width","")}}else{$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");var b=225*1/$(this).width()*$(this).height();if(b<170){$(this).height(170);$(this).css("width","")}else{$(this).width(225);$(this).css("height","")}}})})}if(!document.videoCollageIsotopeLinked){document.videoCollageIsotopeLinked=1;$(a+" .photocollage").isotope({itemSelector:".collagephoto",masonry:{columnWidth:1},layoutMode:"masonry",resizable:false,animationEngine:"best-available"});$(window).load(function(){$(a+" .photocollage").isotope("reLayout")})}if(!document.videoCollageScollLinked){document.videoCollageScollLinked=1;setVideoInfiniteScroll(d,a,c,e)}}function setVideoInfiniteScroll(d,a,b,e){var c=$(a).attr("data-since-date");$(a).unlimitedscroll({url:d,load_callback:b,since_date:c,user_id:e,start_page:1,item_selector:".collagephoto",overlay:"#loading_overlay"})}