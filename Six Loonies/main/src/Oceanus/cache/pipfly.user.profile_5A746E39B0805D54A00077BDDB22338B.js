﻿function formatArticleContents(a){$(a+" .oceanus-content").each(function(){var a=true;$("a",this).each(function(){var c=document.createElement("span");try{if($(".oceanus-title",this).length==0){c.innerHTML=this.innerHTML;this.parentNode.replaceChild(c,this)}else{var b=$(this).closest(".articlecontent");if(b!=null&&b!="undefined"){var d=$(b).attr("data-article-id");$(this).attr("href","/a/"+d);a=false}}}catch(e){}});a&&$(".oceanus-title",this).each(function(){var b=document.createElement("a");try{var a=$(this).closest(".articlecontent");if(a!=null&&a!="undefined"){var c=$(a).attr("data-article-id");$(b).attr("href","/a/"+c);this.parentNode.replaceChild(b,this);b.appendChild(this)}}catch(d){}});$("img",this).each(function(){try{var a=$(this).closest(".articlecontent");if(a!=null&&a!="undefined"){var c=$(a).attr("data-article-id"),b=document.createElement("a");$(b).attr("href","/a/"+c);this.parentNode.replaceChild(b,this);b.appendChild(this)}}catch(d){}})});$(a+" .articlewidthrect .oceanus-content").each(function(){$("img",this).each(function(){var c=$(this).width()*1/($(this).height()*1);if(c<=1){var b=170*1/$(this).height()*$(this).width(),a=170*1/$(this).width()*$(this).height();if(b<170){$(this).height(170);$(this).width(Math.floor(b))}else{$(this).height(Math.floor(a));$(this).width(170)}}else{var a=170*1/$(this).width()*$(this).height(),b=170*1/$(this).height()*$(this).width();if(a<170){$(this).width(170);$(this).height(Math.floor(a))}else{$(this).width(Math.floor(b));$(this).height(170)}}})});$(a+" .articlewidthsmallsquare .oceanus-content").each(function(){$("img",this).each(function(){var c=$(this).width()*1/($(this).height()*1);if(c<=1){var b=80*1/$(this).height()*$(this).width(),a=125*1/$(this).width()*$(this).height();if(b<125){$(this).height(80);$(this).width(Math.floor(b))}else{$(this).width(125);$(this).width(Math.floor(a))}}else{var a=125*1/$(this).width()*$(this).height(),b=80*1/$(this).height()*$(this).width();if(a<80){$(this).width(125);$(this).width(Math.floor(a))}else{$(this).height(80);$(this).width(Math.floor(b))}}})});$(a+" .articlecontainer .articlecontent .oceanus-content").each(function(){var j=$(this).parent().width(),b=$(this).parent().height(),d=0;$("img",this).each(function(){++d;d>1&&$(this).remove()});var a=getInnerText(this,false),g=a.length*.5,f=$("p",this);if(f.length>0){var h=f[0];$(h).attr("style","margin-top:0;")}var e=null;while(b>0&&$(this).height()>b&&e!=this.innerHTML){e=this.innerHTML;trimArticleContents(this,this,b);var i=this.innerHTML}var a=getInnerText(this,false),c=null;while(a.length>g&&c!=this.innerHTML){c=this.innerHTML;trimArticleContents(this,this,null);a=getInnerText(this,false)}})}function trimArticleContents(g,e,d){var b=null,f=$("p, span, div, h3, h4, h5, h6",g);if(f.length<=0)b=g;else{b=f[f.length-1];if($.trim(b.innerHTML)==""){b.parentNode.removeChild(b);trimArticleContents(g,e,d);return}}for(var h=b.childNodes.length-1;h>=0;h--){var a=b.childNodes[h];if(a.nodeType==Node.TEXT_NODE)if(d=="undefined"||d==null||d<=0){var c=a.nodeValue;if($.trim(c)==""||$.trim(c)=="...")a.parentNode.removeChild(a);else{c=c.replace(/\s*\S+\s*$/,"...");a.nodeValue=c;break}}else{while(d>0&&$(e).height()>d){var c=a.nodeValue;if($.trim(c)==""||$.trim(c)=="..."){a.parentNode.removeChild(a);break}else{c=c.replace(/\s*\S+\s*$/,"...");a.nodeValue=c}}if($(e).height()<d)break}else if(a.innerHTML=="")a.parentNode.removeChild(a);else{trimArticleContents(a,e,d);break}}$.trim(b.innerHTML)==""&&b.parentNode.removeChild(b)}function getInnerText(d,a){var b="",c=/^\s+|\s+$/g,e=/\s{2,}/g;a=typeof a=="undefined"?true:a;if(navigator.appName=="Microsoft Internet Explorer")b=d.innerText.replace(c,"");else b=d.textContent.replace(c,"");return a?b.replace(e," "):b}$(document).ready(function(){$(".articlecontainer .btnsect .actionbtns .likeaction").live("click",function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-article-id")+"";likeArticle(this,a)});$(".articlecontainer .btnsect .actionbtns .unlikeaction").live("click",function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-article-id")+"";unlikeArticle(this,a)});$(".articlecontainer .shareaction").live("click",function(){if(!loggedin){$("#loginbtn").click();return false}});loggedin&&$(".articlecontainer .shareaction").fancybox({modal:false,type:"ajax",padding:0,scrolling:"no",transitionIn:"fade",transitionOut:"fade",afterLoad:function(){try{$("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("http://www.pipfly.com/webpage/GetTokenSearchShareLinkUsers",{propertyToSearch:"name",emptyText:"Find people or type email address...",hintText:"Type here to search by name or provide an email address...",resultsFormatter:function(a){return"<li><img src='"+a.userprofileimageurl+"' title='"+a.name+"' height='25px' width='25px' /><div style='display: inline-block; padding-left: 10px;'><div class='full_name'>"+a.name+"</div><div class='email'>"+a.email+"</div></div></li>"},tokenFormatter:function(a){return"<li><p>"+a.name+"</p></li>"}})}catch(a){}}})});function likeArticle(b,a){$.ajax({type:"POST",url:"/article/LikeArticle",data:{articleId:a},success:function(){$(b).attr("class","action unlikeaction");$(b).html("<img class='actionbtnimg' src='/images/heart_filled_sm.png' alt /><span>Liked</span>");$("#articlelikes")!=null&&$("#articlelikes")!="undefined"&&$("#articlelikes").load("/article/GetArticleLikes?articleId="+a)},error:function(){showErrorNotification("Error: Error occured trying to save your like. Please try again later.")}})}function unlikeArticle(b,a){$.ajax({type:"POST",url:"/article/UnikeArticle",data:{articleId:a},success:function(){$(b).attr("class","action likeaction");$(b).html("<img class='actionbtnimg' src='/images/heart_empty_sm.png' alt /><span>Like</span>");$("#articlelikes")!=null&&$("#articlelikes")!="undefined"&&$("#articlelikes").load("/article/GetArticleLikes?articleId="+a)},error:function(){showErrorNotification("Error: Error occured trying to save your like. Please try again later.")}})}var gutterWidth=32,columnWidth=320;$(document).ready(function(){setViewPortWidth();if(!document.layoutUserTagCollage)if($("#utags").is(":visible")){document.layoutUserTagCollage=1;$(".tagcollage").isotope({itemSelector:".tagcollageitem",masonry:{columnWidth:1},layoutMode:"masonry",resizable:true,animationEngine:"best-available"});$(window).load(function(){$(".tagcollage").isotope("reLayout")})}$("#profilesectiontabs li a").click(function(){$("#profilesectiontabs li a").each(function(){$(this).parent().attr("class","");var a=$(this).attr("rel")+"";hide(a)});var a=$(this).attr("rel")+"";$(this).parent().attr("class","selected");show(a);if(!document.layoutUserPhotoCollage)if($("#uphotos").is(":visible")){document.layoutUserPhotoCollage=1;layoutUserPhotosCollage("#uphotos",loadNewUserPhotos)}if($("#uarticles").is(":visible"))if(!document.layoutUserArticlesCollage){document.layoutUserArticlesCollage=1;$(".articlemaincontainer").isotope({itemSelector:".articlecontainer",masonry:{columnWidth:320},layoutMode:"masonry",resizable:true,animationEngine:"best-available"});$(window).load(function(){$(".articlemaincontainer").isotope("reLayout")});formatArticleContents(".articlemaincontainer")}if($("#uvideos").is(":visible"))if(!document.layoutUserVideoCollage){layoutUserVideoCollage("#uvideos");document.layoutUserVideoCollage=1}return false});$("#dashboardfriendslink").click(function(){$("#friendstablink").click()});$(".unfollowuserbtn").live("click",function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"",b=$(this).attr("data-name")+"";a!=null&&unfollowUser(a,b);return false});$(".followuserbtn").live("click",function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"",b=$(this).attr("data-name")+"";a!=null&&followUser(a,b);return false});$(".btnbluestyle").click(function(){var b=$(this).attr("data-sid")+"";if(b=="addfbslfriend"){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"";a!=null&&sendFriendRequest(a);return false}});$("#uploadprofilephotobtn").click(function(){var a=$(this).attr("data-upid")+"";$("#userprofilephotouploadcontent").load("/account/GetUserProfilePhotoUploadForm?userProfileId="+htmlEncode(a));return false});$("#sendMsgToUserBtn").click(function(){var a=$(this).attr("data-uid")+"";$("#userprofilesendmsgformcontent").load("/message/GetPredefinedWriteMessageUI?recipientUID="+htmlEncode(a));return false});$("#addfriendbtn").click(function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"";a!=null&&sendFriendRequest(a);return false});$("#acceptfriendrequestbtn").click(function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"";a!=null&&acceptFriendRequest(a);return false});$("#rejectfriendrequestbtn").click(function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-uid")+"";a!=null&&rejectFriendRequest(a);return false});if(loggedin){$("#uploadprofilephotobtn").fancybox({autoSize:true,modal:false,padding:0,transitionIn:"fade",transitionOut:"fade",scrolling:"no"});$("#sendMsgToUserBtn").fancybox({autoSize:true,modal:false,padding:0,transitionIn:"fade",transitionOut:"fade",scrolling:"no"});$("#writemessagelink").fancybox({autoSize:true,modal:false,padding:0,transitionIn:"fade",transitionOut:"fade",scrolling:"no"})}});function rejectFriendRequest(a){$.ajax({type:"POST",url:"/Account/RejectFriendRequest",data:{friendId:a},async:false,success:function(){window.location.reload(true)},error:function(){showErrorNotification("Error: Friend request could not be rejected. We have recorded the error for analysis and apologize for inconvenience. Please try again later.")}})}function acceptFriendRequest(a){$.ajax({type:"POST",url:"/Account/AcceptFriendRequest",data:{friendId:a},async:false,success:function(){window.location.reload(true)},error:function(){showErrorNotification("Error: Friend request could not be accepted. We have recorded the error for analysis and apologize for inconvenience. Please try again later.")}})}function sendFriendRequest(a){$.ajax({type:"POST",url:"/Account/SendSixLooniesFriendRequest",data:{indicatedFriend:a},async:false,success:function(){window.location.reload(true)},error:function(){showErrorNotification("Error: You could not be added as a follower. We have recorded the error for analysis and apologize for inconvenience. Please try again later.")}})}function setViewPortWidth(){var a=$(".page").width();if(a<=1024)a=1024;a=a-20;var c=a/columnWidth;c=Math.floor(c);var b=c,d=a%columnWidth;if(d<gutterWidth)b=b-1;document.viewPortWidth=b*columnWidth+gutterWidth;a=document.viewPortWidth;$("#header .hcontainer").attr("style","width:"+a+"px");$(".breadcomb").attr("style","width:"+a+"px");$(".page .profile").attr("style","width:"+a+"px");$(".profile .userprofilewall").attr("style","width:"+(document.viewPortWidth-334-gutterWidth)+"px");$(".userwall .wallpost .postcontent").attr("style","width:"+(document.viewPortWidth-334-gutterWidth-100)+"px")}function layoutUserPhotosCollage(a,b){var c=document.viewPortWidth;if($(a+" .photocollage")!="undefined"&&$(a+" .photocollage").length>0){$(a+" .photocollage").isotope({itemSelector:".collagephoto",masonry:{columnWidth:1},layoutMode:"masonry",resizable:false,animationEngine:"best-available"});$(window).load(function(){$(a+" .photocollage").isotope("reLayout")})}if(!document.photoCollageScollLinked){document.photoCollageScollLinked=1;setUserPhotoInfiniteScroll(a,b)}}function setUserPhotoInfiniteScroll(a,b){var e="/clip/UserPhotos",c=$(a).attr("data-since-date"),d=$(a).attr("data-user-id");$(a).unlimitedscroll({url:e,load_callback:b,since_date:c,user_id:d,item_selector:".collagephoto",overlay:"#loading_overlay"})}function loadNewUserPhotos(b){if($("#uphotos .photocollage")!="undefined"&&$("#uphotos .photocollage").length>0){var a=$(".collagephoto",b);$("#uphotos .photocollage").append(a).isotope("appended",a)}else{$("#uphotos").append(b);layoutUserPhotosCollage("#uphotos",loadNewUserPhotos);$("#uphotos .photocollage").isotope("reLayout")}}function layoutUserVideoCollage(a){var c=document.viewPortWidth,b=$(a+" .collagephoto");b.each(function(){$("img",this).each(function(){var a=$(this).width()*1/($(this).height()*1);if(a>=.75&&a<=1.34)if($(this).width()>200&&$(this).height()>200){$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");$(this).width(225);$(this).css("height","")}else{$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");if($(this).width()>$(this).height()){$(this).height(170);$(this).css("width","")}else{$(this).width(225);$(this).css("height","")}}else if(a<.75){$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");var c=170*1/$(this).height()*$(this).width();if(c<225){$(this).width(225);$(this).css("height","")}else{$(this).height(170);$(this).css("width","")}}else{$(this).parent().parent().parent().attr("class",$(this).parent().parent().parent().attr("class")+" collagevideosquare");var b=225*1/$(this).width()*$(this).height();if(b<170){$(this).height(170);$(this).css("width","")}else{$(this).width(225);$(this).css("height","")}}})});$(a+" .photocollage").isotope({itemSelector:".collagephoto",masonry:{columnWidth:1},layoutMode:"masonry",resizable:false,animationEngine:"best-available"});$(window).load(function(){$(a+" .photocollage").isotope("reLayout")})}function followUser(a,b){$.ajax({type:"POST",url:"/Account/FollowUser",data:"userToFollowId="+htmlEncode(a),async:false,success:function(){showSuccessNotification("You have been added as a follower.");$(".userdetails #userstats").load("/Account/GetUserStats?userid="+a);$(".followuserbtn").attr("class","btnundo unfollowuserbtn");$(".unfollowuserbtn span").html("Unfollow "+b);$(".unfollowuserbtn img").attr("src","/images/tick.png")},error:function(){showErrorNotification("Error: You could not be added as a follower. Please try again later.")}})}function unfollowUser(a,b){$.ajax({type:"POST",url:"/Account/UnfollowUser",data:"followedUserId="+htmlEncode(a),async:false,success:function(){showSuccessNotification("You have been removed as a follower.");$(".userdetails #userstats").load("/Account/GetUserStats?userid="+a);$(".unfollowuserbtn").attr("class","btnstyle followuserbtn");$(".followuserbtn span").html("Follow "+b);$(".followuserbtn img").attr("src","/images/plus.gif")},error:function(){showErrorNotification("Error: You could not be removed as a follower. Please try again later.")}})}var newwallpostemptytext="Type here to share your opinion, start a discussion, or write something interesting.";$(document).ready(function(){$(".newwallpost #newwallpostinput").emptyText(newwallpostemptytext,"emptytextstyle");$(".newwallpost #newwallpostinput").focus(function(){$(".newwallpost .postbtns").show()});$(".wallpostcomment .postcommentcontent .newpostcommentinput").live("keydown",function(){if(!loggedin){$("#loginbtn").click();return false}var b=$(this).attr("data-post-id")+"",a=$(this).val();switch(event.keyCode){case 13:addUserWallPostComment(b,a)}});$(".newwallpost .postbtns #addnewuserpostbtn").click(function(){if(!loggedin){$("#loginbtn").click();return false}var a=$(this).attr("data-page-id");addUserWallPost(a);return false});$(".wallpost .showpostcommentsbtn").click(function(){var a=$(this).attr("data-post-comments");$("."+a).toggle();return false})});function addUserWallPost(a){var b=$(".newwallpost #newwallpostinput").val();$.ajax({type:"POST",url:"/Activity/AddWallTextPost",data:{postText:b,pageId:a},success:function(){showSuccessNotification("Post has been published.");$("#sl_discussionsect").load("/activity/GetBookmarkletUserWallPosts?pageId="+a)},error:function(){showErrorNotification("Error: Post could not be published. Please try again later.")}})}function addUserWallPostComment(a,b){$.ajax({type:"POST",url:"/Activity/AddWallTextPostComment",data:{wallPostId:a,commentText:b},success:function(){showSuccessNotification("Post comment has been published.");$(".pcb"+a).load("/Activity/GetUserWallPostComments?wallPostId="+a,function(){$(".wallpostcomment .postcommentcontent .newpostcommentinput").elastic()})},error:function(){showErrorNotification("Error: Post comment could not be published. Please try again later.")}})}