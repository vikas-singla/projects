﻿(function(a){a.fn.tipTip=function(g){var f={activation:"hover",keepAlive:false,maxWidth:"200px",edgeOffset:3,defaultPosition:"bottom",delay:400,fadeIn:200,fadeOut:200,attribute:"title",content:false,enter:function(){},exit:function(){}},b=a.extend(f,g);if(a("#tiptip_holder").length<=0){var c=a('<div id="tiptip_holder" style="max-width:'+b.maxWidth+';"></div>'),e=a('<div id="tiptip_content"></div>'),d=a('<div id="tiptip_arrow"></div>');a("body").append(c.html(e).prepend(d.html('<div id="tiptip_arrow_inner"></div>')))}else var c=a("#tiptip_holder"),e=a("#tiptip_content"),d=a("#tiptip_arrow");return this.each(function(){var f=a(this);if(b.content)var j=b.content;else var j=f.attr(b.attribute);if(j!=""){!b.content&&f.removeAttr(b.attribute);var h=false;if(b.activation=="hover"){f.hover(function(){i()},function(){!b.keepAlive&&g()});b.keepAlive&&c.hover(function(){},function(){g()})}else if(b.activation=="focus")f.focus(function(){i()}).blur(function(){g()});else if(b.activation=="click"){f.click(function(){i();return false}).hover(function(){},function(){!b.keepAlive&&g()});b.keepAlive&&c.hover(function(){},function(){g()})}function i(){b.enter.call(this);e.html(j);c.hide().removeAttr("class").css("margin","0");d.removeAttr("style");var l=parseInt(f.offset().top),n=parseInt(f.offset().left),y=parseInt(f.outerWidth()),o=parseInt(f.outerHeight()),m=c.outerWidth(),k=c.outerHeight(),r=Math.round((y-m)/2),x=Math.round((o-k)/2),q=Math.round(n+r),i=Math.round(l+o+b.edgeOffset),g="",p="",u=Math.round(m-12)/2;if(b.defaultPosition=="bottom")g="_bottom";else if(b.defaultPosition=="top")g="_top";else if(b.defaultPosition=="left")g="_left";else if(b.defaultPosition=="right")g="_right";var v=r+n<parseInt(a(window).scrollLeft()),w=m+n>parseInt(a(window).width());if(v&&r<0||g=="_right"&&!w||g=="_left"&&n<m+b.edgeOffset+5){g="_right";p=Math.round(k-13)/2;u=-12;q=Math.round(n+y+b.edgeOffset);i=Math.round(l+x)}else if(w&&r<0||g=="_left"&&!v){g="_left";p=Math.round(k-13)/2;u=Math.round(m);q=Math.round(n-(m+b.edgeOffset+5));i=Math.round(l+x)}var t=l+o+b.edgeOffset+k+8>parseInt(a(window).height()+a(window).scrollTop()),s=l+o-(b.edgeOffset+k+8)<0;if(t||g=="_bottom"&&t||g=="_top"&&!s){if(g=="_top"||g=="_bottom")g="_top";else g=g+"_top";p=k;i=Math.round(l-(k+5+b.edgeOffset))}else if(s|(g=="_top"&&s)||g=="_bottom"&&!t){if(g=="_top"||g=="_bottom")g="_bottom";else g=g+"_bottom";p=-12;i=Math.round(l+o+b.edgeOffset)}if(g=="_right_top"||g=="_left_top")i=i+5;else if(g=="_right_bottom"||g=="_left_bottom")i=i-5;if(g=="_left_top"||g=="_left_bottom")q=q+5;d.css({"margin-left":u+"px","margin-top":p+"px"});c.css({"margin-left":q+"px","margin-top":i+"px"}).attr("class","tip"+g);h&&clearTimeout(h);h=setTimeout(function(){c.stop(true,true).fadeIn(b.fadeIn)},b.delay)}function g(){b.exit.call(this);h&&clearTimeout(h);c.fadeOut(b.fadeOut)}}})}})(jQuery);(function(a){a.fn.emptyText=function(c){var b=arguments[1];if(a(this).val()==null||a.trim(a(this).val())==""){b&&a(this).addClass(b);a(this).val(c)}a(this).attr("emptytext",c);a(this).focus(function(){if(a(this).val()==c){b&&a(this).removeClass(b);a(this).val("")}});a(this).blur(function(){if(a(this).val()==null||a.trim(a(this).val())==""){b&&a(this).addClass(b);a(this).val(c)}})}})(jQuery);function clearDefaultIfEmpty(b,c){var a=$("#"+b);if(a.val()==c){a.val("");a.removeClass("emptytextstyle")}}function AddToList(b,a){if(b==null)b=[];if(a!=null){a=$.trim(a);$.inArray(a,b)==-1&&b.push(a)}}String.prototype.format=function(){var a=this;for(arg in arguments)a=a.replace("{"+arg+"}",arguments[arg]);return a};function DisableEmptyFormFieldsForSubmission(a){a.find(':input[value=""]').attr("disabled",true)}(function(a){a.fn.serializeJSON=function(){var b={};jQuery.map(a(this).serializeArray(),function(a){b[a.name]=a.value});return b}})(jQuery);function show(a){$(a).show()}function hide(a){$(a).hide()}function isIE(){return $.browser.msie}var showingSignInDD=false,showingRegisterDD=false,showingUserSessionDD=false,showingCitySearchDD=false,notificationId=1,globalsearchemptytext="Type here to search...";function htmlEncode(a){return a?encodeURIComponent(a):""}function loadVimeoThumb(a){$.getJSON("http://www.vimeo.com/api/v2/video/"+a+".json?callback=?",{format:"jsonp"},function(b){if(b!=null&&b[0]!=null){var c="#vimeo-"+b[0].id;$(c).attr("src",b[0].thumbnail_medium)}else{var c="#vimeo-"+a;$(c).attr("src","/images/videothumbnail.gif")}})}function loadEditPortfolioVimeoThumb(a){$.getJSON("http://www.vimeo.com/api/v2/video/"+a+".json?callback=?",{format:"jsonp"},function(b){if(b!=null&&b[0]!=null){var c="#editportfoliovimeo-"+b[0].id;$(c).attr("src",b[0].thumbnail_medium)}else{var c="#editportfoliovimeo-"+a;$(c).attr("src","/images/videothumbnail.gif")}})}function loadEditPortfolioDetailsVimeoThumb(a){$.getJSON("http://www.vimeo.com/api/v2/video/"+a+".json?callback=?",{format:"jsonp"},function(b){if(b!=null&&b[0]!=null){var c="#editportfoliodetailsvimeo-"+b[0].id;$(c).attr("src",b[0].thumbnail_medium)}else{var c="#editportfoliodetailsvimeo-"+a;$(c).attr("src","/images/videothumbnail.gif")}})}function loadDeletePortfolioDetailsVimeoThumb(a){$.getJSON("http://www.vimeo.com/api/v2/video/"+a+".json?callback=?",{format:"jsonp"},function(b){if(b!=null&&b[0]!=null){var c="#deleteportfoliodetailsvimeo-"+b[0].id;$(c).attr("src",b[0].thumbnail_medium)}else{var c="#deleteportfoliodetailsvimeo-"+a;$(c).attr("src","/images/videothumbnail.gif")}})}function showErrorNotification(a){if(document.useJQueryNotify!="undefined"&&document.useJQueryNotify)$.notify({inline:true,html:"<div style='clear:both;overflow:auto;'><div class='fancynotifyicon'><img src='/images/error.png' alt /></div><div class='fancynotifytext'><span style='font-size:18px;'>"+a+"</span></div></div>"},5e3);else{var b="<div class='error' id='notifid"+notificationId+"'><span class='hpadtext'><span class='notificonalign'><img src='/images/error.png' alt /></span>"+a+"</span><span class='notifalignright'><a class='notifclosebtn' data-refid='"+notificationId+"'></a></span></div>";$("#notification").append(b);$("#scrollToTopLink").click();notificationId=notificationId+1}}function showSuccessNotification(a){if(document.useJQueryNotify!="undefined"&&document.useJQueryNotify)$.notify({inline:true,html:"<div style='clear:both;overflow:auto;'><div class='fancynotifyicon'><img src='/images/tick.png' alt /></div><div class='fancynotifytext'><span style='font-size:18px;'>"+a+"</span></div></div>"},5e3);else{var b="<div class='success' id='notifid"+notificationId+"'><span class='hpadtext'><span class='notificonalign'><img src='/images/tick.png' alt /></span>"+a+"</span><span class='notifalignright'><a class='notifclosebtn' data-refid='"+notificationId+"'></a></span></div>";$("#notification").append(b);$("#scrollToTopLink").click();notificationId=notificationId+1}}$(document).ready(function(){$(".searchinputstyle").emptyText(globalsearchemptytext,"emptytextstyle");$("#hdrinvitefriends").click(function(){if(!loggedin){$("#loginbtn").click();return false}});$("#scrollToTopLink").click(function(a){a.preventDefault();var b=this;$.smoothScroll({scrollTarget:b.hash})});$(".searchinputstyle").focus(function(){$("#searchdisplay").attr("style","background:#fff;border: 1px solid #b9b9b9;color: #555;");$(".searchinputstyle").attr("style","background:#fff; border: none; color: #333;")});$(".searchinputstyle").blur(function(){$("#searchdisplay").attr("style","background:#fff;border: 1px solid #b9b9b9;color:#555");$(".searchinputstyle").attr("style","background:#fff; border: none; color: #555;")});$("#searchsiteinput").keypress(function(a){code=a.keyCode?a.keyCode:a.which;if(code==13)window.location="/s/"+encodeURIComponent($("#searchsiteinput").val())});$("#searchsitebtn").click(function(){window.location="/s/"+encodeURIComponent($("#searchsiteinput").val())});$("#loginbtn").fancybox({modal:false,padding:0,fitToView:false,scrolling:"no",transitionIn:"fade",transitionOut:"fade"});$("#fbfriendpromptlink").fancybox({modal:false,padding:0,scrolling:"no",transitionIn:"fade",transitionOut:"fade",fitToView:false,onComplete:function(){var a=$(".expandnetworkscrollpanel");a!=null&&a.tinyscrollbar_update()}});$("#hdrinvitefriends").fancybox({modal:false,padding:0,scrolling:"no",transitionIn:"fade",transitionOut:"fade"});$(".cliplink").fancybox({type:"ajax",padding:0,topRatio:0,nextEffect:"none",prevEffect:"none",fitToView:false,scrolling:"no"});$(".notifclosebtn").live("click",function(){var a=$(this).attr("data-refid")+"";hide("#notifid"+a);return false});$("#cityddmenu").click(function(){if(!showingCitySearchDD){show("#cityselectdropdown");$("#cityddmenu").attr("class","citysearchsel");showingCitySearchDD=true}else{hide("#cityselectdropdown");$("#cityddmenu").attr("class","");showingCitySearchDD=false}return false});$("#signindisplay").click(function(){if(!showingSignInDD){show("#signindropdown");hide("#registerdropdown");hide("#usersessiondropdown");$("#signindisplay").attr("class","sessionsel");$("#joinowdisplay").attr("class","");$("#userlogged").attr("class","");showingSignInDD=true;showingRegisterDD=false;showingUserSessionDD=false}else{hide("#signindropdown");$("#signindisplay").attr("class","");showingSignInDD=false}return false});$("#joinowdisplay").click(function(){if(!showingRegisterDD){show("#registerdropdown");hide("#signindropdown");hide("#usersessiondropdown");$("#signindisplay").attr("class","");$("#joinowdisplay").attr("class","sessionsel");$("#userlogged").attr("class","");showingRegisterDD=true;showingSignInDD=false;showingUserSessionDD=false}else{hide("#registerdropdown");$("#joinowdisplay").attr("class","");showingRegisterDD=false}return false});$("#userlogged").click(function(){if(!showingUserSessionDD){show("#usersessiondropdown");hide("#registerdropdown");hide("#signindropdown");$("#signindisplay").attr("class","");$("#joinowdisplay").attr("class","");$("#userlogged").attr("class","sessionsel");showingUserSessionDD=true;showingRegisterDD=false;showingSignInDD=false}else{hide("#usersessiondropdown");$("#userlogged").attr("class","");showingUserSessionDD=false}return false});$(document).click(function(a){if(!($(a.target).is("#signindropdown *")||$(a.target).is("#registerdropdown *")||$(a.target).is("#usersessiondropdown *"))){hide("#usersessiondropdown");hide("#registerdropdown");hide("#signindropdown");$("#signindisplay").attr("class","");$("#joinowdisplay").attr("class","");$("#userlogged").attr("class","");showingUserSessionDD=false;showingRegisterDD=false;showingSignInDD=false}})});(function(a){var f={method:"GET",queryParam:"q",searchDelay:300,minChars:1,propertyToSearch:"name",jsonContainer:null,contentType:"json",prePopulate:null,processPrePopulate:false,hintText:"Type in a search term",noResultsText:"No results",searchingText:"Searching...",deleteText:"&times;",animateDropdown:true,theme:null,zindex:11111,resultsFormatter:function(a){return"<li>"+a[this.propertyToSearch]+"</li>"},tokenFormatter:function(a){return"<li><p>"+a[this.propertyToSearch]+"</p></li>"},tokenLimit:null,tokenDelimiter:",",preventDuplicates:false,tokenValue:"id",onResult:null,onAdd:null,onDelete:null,onReady:null,idPrefix:"token-input-",disabled:false},d={tokenList:"token-input-list",token:"token-input-token",tokenDelete:"token-input-delete-token",selectedToken:"token-input-selected-token",highlightedToken:"token-input-highlighted-token",dropdown:"token-input-dropdown",dropdownItem:"token-input-dropdown-item",dropdownItem2:"token-input-dropdown-item2",selectedDropdownItem:"token-input-selected-dropdown-item",inputToken:"token-input-input-token",disabled:"token-input-disabled",emptyTextStyle:"emptytextstyle"},c={BEFORE:0,AFTER:1,END:2},b={BACKSPACE:8,TAB:9,ENTER:13,ESCAPE:27,SPACE:32,PAGE_UP:33,PAGE_DOWN:34,END:35,HOME:36,LEFT:37,UP:38,RIGHT:39,DOWN:40,NUMPAD_ENTER:108,COMMA:188},e={init:function(b,d){var c=a.extend({},f,d||{});return this.each(function(){a(this).data("tokenInputObject",new a.TokenList(this,b,c))})},clear:function(){this.data("tokenInputObject").clear();return this},add:function(a){this.data("tokenInputObject").add(a);return this},remove:function(a){this.data("tokenInputObject").remove(a);return this},"get":function(){return this.data("tokenInputObject").getTokens()},toggleDisabled:function(a){this.data("tokenInputObject").toggleDisabled(a);return this}};a.fn.tokenInput=function(a){return e[a]?e[a].apply(this,Array.prototype.slice.call(arguments,1)):e.init.apply(this,arguments)};a.TokenList=function(L,s,e){if(a.type(s)==="string"||a.type(s)==="function"){e.url=s;var D=A();if(e.crossDomain===undefined&&typeof D==="string")if(D.indexOf("://")===-1)e.crossDomain=false;else e.crossDomain=location.href.split(/\/+/g)[1]!==D.split(/\/+/g)[1]}else if(typeof s==="object")e.local_data=s;if(e.classes)e.classes=a.extend({},d,e.classes);else if(e.theme){e.classes={};a.each(d,function(b,a){e.classes[b]=a+"-"+e.theme})}else e.classes=d;var m=[],n=0,C=new a.TokenList.Cache,K,B,f=a('<input type="text"  autocomplete="off">').css({outline:"none"}).attr("id",e.idPrefix+L.id).addClass(e.classes.emptyTextStyle).val(e.emptyText).focus(function(){if(e.disabled)return false;else(e.tokenLimit===null||e.tokenLimit!==n)&&Q();if(a(this).val()===e.emptyText){a(this).removeClass(e.classes.emptyTextStyle);a(this).val("")}}).blur(function(){i();if(a.trim(f.val())!=""){var b={};b.id="-1";b.fbuid="-1";b.name=f.val();b.email=f.val();b.userprofileimageurl="";t(b)}h.change();a(this).val("");if(n>0)a(this).val("");else{a(this).addClass(e.classes.emptyTextStyle);a(this).val(e.emptyText)}}).bind("keyup keydown blur update",S).keydown(function(d){var e,l;switch(d.keyCode){case b.LEFT:case b.RIGHT:case b.UP:case b.DOWN:if(!a(this).val()){e=p.prev();l=p.next();if(e.length&&e.get(0)===g||l.length&&l.get(0)===g)if(d.keyCode===b.LEFT||d.keyCode===b.UP)o(a(g),c.BEFORE);else o(a(g),c.AFTER);else if((d.keyCode===b.LEFT||d.keyCode===b.UP)&&e.length)x(a(e.get(0)));else(d.keyCode===b.RIGHT||d.keyCode===b.DOWN)&&l.length&&x(a(l.get(0)))}else{var m=null;if(d.keyCode===b.DOWN||d.keyCode===b.RIGHT)m=a(k).next();else m=a(k).prev();m.length&&y(m)}return false;break;case b.BACKSPACE:e=p.prev();if(!a(this).val().length){if(g){w(a(g));h.change()}else e.length&&x(a(e.get(0)));return false}else if(a(this).val().length===1)i();else setTimeout(function(){J()},5);break;case b.TAB:case b.ENTER:case b.NUMPAD_ENTER:case b.COMMA:if(k){t(a(k).data("tokeninput"));h.change();return false}else{if(a.trim(f.val())!=""){var j={};j.id="-1";j.fbuid="-1";j.name=f.val();j.email=f.val();j.userprofileimageurl="";t(j)}h.change();if(d.keyCode==b.TAB)i();else return false}break;case b.ESCAPE:i();return true;default:String.fromCharCode(d.which)&&setTimeout(function(){J()},5)}}),h=a(L).hide().val("").focus(function(){r(f)}).blur(function(){f.blur()}),g=null,l=0,k=null,j=a("<ul />").addClass(e.classes.tokenList).click(function(d){var b=a(d.target).closest("li");if(b&&b.get(0)&&a.data(b.get(0),"tokeninput"))P(b);else{g&&o(a(g),c.END);r(f)}}).mouseover(function(c){var b=a(c.target).closest("li");b&&g!==this&&b.addClass(e.classes.highlightedToken)}).mouseout(function(c){var b=a(c.target).closest("li");b&&g!==this&&b.removeClass(e.classes.highlightedToken)}).insertBefore(h),p=a("<li />").addClass(e.classes.inputToken).appendTo(j).append(f),q=a("<div>").addClass(e.classes.dropdown).appendTo("body").hide(),H=a("<tester/>").insertAfter(f).css({position:"absolute",top:-9999,left:-9999,width:"auto",fontSize:f.css("fontSize"),fontFamily:f.css("fontFamily"),fontWeight:f.css("fontWeight"),letterSpacing:f.css("letterSpacing"),whiteSpace:"nowrap"});h.val("");var u=e.prePopulate||h.data("pre");if(e.processPrePopulate&&a.isFunction(e.onResult))u=e.onResult.call(h,u);u&&u.length&&a.each(u,function(b,a){I(a);F()});e.disabled&&G(true);a.isFunction(e.onReady)&&e.onReady.call();this.clear=function(){j.children("li").each(function(){a(this).children("input").length===0&&w(a(this))})};this.add=function(a){t(a)};this.remove=function(b){j.children("li").each(function(){if(a(this).children("input").length===0){var e=a(this).data("tokeninput"),c=true;for(var d in b)if(b[d]!==e[d]){c=false;break}c&&w(a(this))}})};this.getTokens=function(){return m};this.toggleDisabled=function(a){G(a)};function G(b){if(typeof b==="boolean")e.disabled=b;else e.disabled=!e.disabled;f.prop("disabled",e.disabled);j.toggleClass(e.classes.disabled,e.disabled);g&&o(a(g),c.END);h.prop("disabled",e.disabled)}function F(){if(e.tokenLimit!==null&&n>=e.tokenLimit){f.hide();i();return}}function S(){if(B===(B=f.val()))return;var a=B.replace(/&/g,"&amp;").replace(/\s/g," ").replace(/</g,"&lt;").replace(/>/g,"&gt;");H.html(a);f.width(H.width()+40)}function I(c){var b=e.tokenFormatter(c);b=a(b).addClass(e.classes.token).insertBefore(p);a("<span>"+e.deleteText+"</span>").addClass(e.classes.tokenDelete).appendTo(b).click(function(){if(!e.disabled){w(a(this).parent());h.change();return false}});var d=c;a.data(b.get(0),"tokeninput",c);m=m.slice(0,l).concat([d]).concat(m.slice(l));l++;E(m,h);n+=1;if(e.tokenLimit!==null&&n>=e.tokenLimit){f.hide();i()}return b}function t(b){var d=e.onAdd;if(n>0&&e.preventDuplicates){var c=null;j.children().each(function(){var e=a(this),d=a.data(e.get(0),"tokeninput");if(d&&(d.id!=-1&&b.id!=-1&&d.id===b.id||d.name===b.name)){c=e;return false}});if(c)return}if(e.tokenLimit==null||n<e.tokenLimit){I(b);F()}f.val("");i();a.isFunction(d)&&d.call(h,b)}function x(a){if(!e.disabled){a.addClass(e.classes.selectedToken);g=a.get(0);f.val("");i()}}function o(a,b){a.removeClass(e.classes.selectedToken);g=null;if(b===c.BEFORE){p.insertBefore(a);l--}else if(b===c.AFTER){p.insertAfter(a);l++}else{p.appendTo(j);l=n}r(f)}function P(b){var d=g;g&&o(a(g),c.END);if(d===b.get(0))o(b,c.END);else x(b)}function w(c){var i=a.data(c.get(0),"tokeninput"),d=e.onDelete,b=c.prevAll().length;if(b>l)b--;c.remove();g=null;r(f);m=m.slice(0,b).concat(m.slice(b+1));if(b<l)l--;E(m,h);n-=1;if(e.tokenLimit!==null){f.show().val("");r(f)}a.isFunction(d)&&d.call(h,i)}function E(c,b){var d=a.map(c,function(a){return typeof e.tokenValue=="function"?e.tokenValue.call(this,a):a[e.tokenValue]});b.val(d.join(e.tokenDelimiter))}function i(){q.hide().empty();k=null}function v(){q.css({position:"absolute",top:a(j).offset().top+a(j).outerHeight(),left:a(j).offset().left,width:a(j).outerWidth(),"z-index":e.zindex}).show()}function N(){if(e.searchingText){q.html("<p>"+e.searchingText+"</p>");v()}}function Q(){if(e.hintText){q.html("<p>"+e.hintText+"</p>");v()}}function R(a,b){return a.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)("+b+")(?![^<>]*>)(?![^&;]+;)","gi"),"<b>$1</b>")}function M(b,a,c){return b.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)("+a+")(?![^<>]*>)(?![^&;]+;)","g"),R(a,c))}function z(d,c){if(c&&c.length){q.empty();var b=a("<ul>").appendTo(q).mouseover(function(b){y(a(b.target).closest("li"))}).mousedown(function(b){t(a(b.target).closest("li").data("tokeninput"));h.change();return false}).hide();a.each(c,function(g,f){var c=e.resultsFormatter(f);c=M(c,f[e.propertyToSearch],d);c=a(c).appendTo(b);if(g%2)c.addClass(e.classes.dropdownItem);else c.addClass(e.classes.dropdownItem2);g===0&&y(c);a.data(c.get(0),"tokeninput",f)});v();if(e.animateDropdown)b.slideDown("fast");else b.show()}else if(e.noResultsText){q.html("<p>"+e.noResultsText+"</p>");v()}}function y(b){if(b){k&&O(a(k));b.addClass(e.classes.selectedDropdownItem);k=b.get(0)}}function O(a){a.removeClass(e.classes.selectedDropdownItem);k=null}function J(){var b=f.val();if(b&&b.length){g&&o(a(g),c.AFTER);if(b.length>=e.minChars){N();clearTimeout(K);K=setTimeout(function(){T(b)},e.searchDelay)}else i()}}function T(c){var g=c+A(),k=C.get(g);if(k)z(c,k);else if(e.url){var j=A(),b={};b.data={};if(j.indexOf("?")>-1){var l=j.split("?");b.url=l[0];var m=l[1].split("&");a.each(m,function(d,c){var a=c.split("=");b.data[a[0]]=a[1]})}else b.url=j;b.data[e.queryParam]=c;b.type=e.method;b.dataType=e.contentType;if(e.crossDomain)b.dataType="jsonp";b.success=function(b){if(a.isFunction(e.onResult))b=e.onResult.call(h,b);C.add(g,e.jsonContainer?b[e.jsonContainer]:b);f.val()===c&&z(c,e.jsonContainer?b[e.jsonContainer]:b)};b.error=function(){i()};a.ajax(b)}else if(e.local_data){var d=a.grep(e.local_data,function(a){return a[e.propertyToSearch].toLowerCase().indexOf(c.toLowerCase())>-1});if(a.isFunction(e.onResult))d=e.onResult.call(h,d);C.add(g,d);z(c,d)}}function A(){var a=e.url;if(typeof e.url=="function")a=e.url.call(e);return a}function r(a){setTimeout(function(){a.focus()},50)}};a.TokenList.Cache=function(e){var d=a.extend({max_size:500},e),b={},c=0,f=function(){b={};c=0};this.add=function(a,e){c>d.max_size&&f();if(!b[a])c+=1;b[a]=e};this.get=function(a){return b[a]}}})(jQuery);(function(a){var d,h,k=1,b,c=this,e=!1,f="postMessage",i="addEventListener",g,j=c[f]&&!a.browser.opera;a[f]=function(b,c,d){if(!c)return;b=typeof b==="string"?b:a.param(b);d=d||parent;if(j)d[f](b,c.replace(/([^:]+:\/\/[^\/]+).*/,"$1"));else if(c)d.location=c.replace(/#.*$/,"")+"#"+ +new Date+k+++"&"+b};a.receiveMessage=g=function(f,k,l){if(j){if(f){b&&g();b=function(b){if(typeof k==="string"&&b.origin!==k||a.isFunction(k)&&k(b.origin)===e)return e;f(b)}}if(c[i])c[f?i:"removeEventListener"]("message",b,e);else c[f?"attachEvent":"detachEvent"]("onmessage",b)}else{d&&clearInterval(d);d=null;if(f){l=typeof k==="number"?k:typeof l==="number"?l:100;d=setInterval(function(){var a=document.location.hash,b=/^#?\d+&/;if(a!==h&&b.test(a)){h=a;f({data:a.replace(b,"")})}},l)}}}})(jQuery)