﻿$(document).ready(function(){$("#sl_frmClipImage")!="undefined"&&$("#sl_frmClipImage")!=null&&$("#sl_frmClipImage #shareRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers",{preventDuplicates:true,propertyToSearch:"name",emptyText:"Find people or type email address...",hintText:"Type here to search by name or provide an email address...",resultsFormatter:function(a){return"<li><img src='"+(a.userprofileimageurl==null?"/Images/user_white.png":a.userprofileimageurl)+"' title='"+a.name+"' height='25px' width='25px' /><div style='display: inline-block; padding-left: 10px;'><div class='full_name'>"+a.name+"</div><div class='email'>"+a.email+"</div></div></li>"},tokenFormatter:function(a){return"<li><p>"+a.name+"</p></li>"}});$("#sl_frmClipStory")!="undefined"&&$("#sl_frmClipStory")!=null&&$("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers",{preventDuplicates:true,propertyToSearch:"name",emptyText:"Find people or type email address...",hintText:"Type here to search by name or provide an email address...",resultsFormatter:function(a){return"<li><img src='"+(a.userprofileimageurl==null?"/Images/user_white.png":a.userprofileimageurl)+"' title='"+a.name+"' height='25px' width='25px' /><div style='display: inline-block; padding-left: 10px;'><div class='full_name'>"+a.name+"</div><div class='email'>"+a.email+"</div></div></li>"},tokenFormatter:function(a){return"<li><p>"+a.name+"</p></li>"}});$("#sl_frmClipImage #sl_addclipbtn").click(function(){$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');$("#sl_frmClipImage #sl_processingcaption").show();$("#sl_frmClipImage :input").attr("disabled",true);$(this).hide();if($("#sl_frmClipImage")!="undefined"&&$("#sl_frmClipImage")!=null)if($("#sl_frmClipImage").validationEngine("validate")){$("#sl_frmClipImage").validationEngine("hideAll");sl_addClipToScrapbook(this)}if($("#sl_frmClipStory")!="undefined"&&$("#sl_frmClipStory")!=null)if($("#sl_frmClipStory").validationEngine("validate")){$("#sl_frmClipStory").validationEngine("hideAll");sl_addClipToScrapbook(this)}return false});$("#sl_frmClipImage #sl_addclipdesc").focus(function(){$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');return false});$("#sl_frmClipImage #token-input-shareRecipientInput").focus(function(){$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>')});$("#sl_frmClipStory #sl_addarticlecomments").focus(function(){$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');return false});$("#sl_frmClipStory #token-input-shareArticleRecipientInput").focus(function(){$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>')});$("#sl_frmClipStory #sl_addarticleclipbtn").click(function(){$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');$("#sl_frmClipStory #sl_processingcaption").show();$("#sl_frmClipStory :input").attr("disabled",true);$(this).hide();sl_addArticleClipToScrapbook(this);return false})});function sl_addClipToScrapbook(c){var g=$("#sl_frmClipImage #clipimagephototag").attr("src"),e=$("#sl_frmClipImage #clipimagephototag").attr("data-width"),d=$("#sl_frmClipImage #clipimagephototag").attr("data-height"),j=$("#sl_frmClipImage #sl_addclipbtn").attr("data-vid"),i=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-url"),f=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-title"),h=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-desc"),k=$("#sl_frmClipImage #sl_addclipdesc").val(),b=$("#sl_frmClipImage #shareRecipientInput").tokenInput("get"),a=$("#sl_frmClipImage #sharelinkfbbtn").val();if(a=="on")a=true;else a=false;var l="/clip/ClipPhotoVideo";$.ajax({type:"POST",url:l,data:{recipients:JSON.stringify(b),pageUrl:i,pageTitle:f,pageDescription:h,imageUrl:g,imageWidth:e,imageHeight:d,videoId:j,clipDesc:k,postOnFB:a},success:function(a){if(a.successful=="true"){if(b.length>0)showSuccessNotification("Clip has been shared.");else showSuccessNotification("Clip has been added.");$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>')}else{showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipImage #sl_processingcaption").hide()}$("#sl_frmClipImage :input").attr("disabled",false);$(c).show()},error:function(){showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage :input").attr("disabled",false);$(c).show()}})}function sl_addArticleClipToScrapbook(b){var f=$("#sl_frmClipStory #sl_addarticleclipbtn").attr("data-article-id"),c=$("#sl_frmClipStory #sl_addarticlecomments").val(),d=$("#sl_frmClipStory #articletypenews").val(),e=$("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("get"),a=$("#sl_frmClipStory #sharelinkfbbtn").val();if(a=="on")a=true;else a=false;var g="/article/PublishArticleClip";$.ajax({type:"POST",url:g,data:{articleId:f,articleDesc:c,articleType:d,recipients:JSON.stringify(e),postOnFB:a},success:function(a){if(a.successful=="true"){showSuccessNotification("Clip has been added.");$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>')}else{showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipStory #sl_processingcaption").hide()}$("#sl_frmClipStory :input").attr("disabled",false);$(b).show()},error:function(){showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory :input").attr("disabled",false);$(b).show()}})}$(document).ready(function(){$("#sl_frmClipImage")!="undefined"&&$("#sl_frmClipImage")!=null&&$("#sl_frmClipImage #shareRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers",{preventDuplicates:true,propertyToSearch:"name",emptyText:"Find people or type email address...",hintText:"Type here to search by name or provide an email address...",resultsFormatter:function(a){return"<li><img src='"+(a.userprofileimageurl==null?"/Images/user_white.png":a.userprofileimageurl)+"' title='"+a.name+"' height='25px' width='25px' /><div style='display: inline-block; padding-left: 10px;'><div class='full_name'>"+a.name+"</div><div class='email'>"+a.email+"</div></div></li>"},tokenFormatter:function(a){return"<li><p>"+a.name+"</p></li>"}});$("#sl_frmClipStory")!="undefined"&&$("#sl_frmClipStory")!=null&&$("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("/webpage/GetTokenSearchShareLinkUsers",{preventDuplicates:true,propertyToSearch:"name",emptyText:"Find people or type email address...",hintText:"Type here to search by name or provide an email address...",resultsFormatter:function(a){return"<li><img src='"+(a.userprofileimageurl==null?"/Images/user_white.png":a.userprofileimageurl)+"' title='"+a.name+"' height='25px' width='25px' /><div style='display: inline-block; padding-left: 10px;'><div class='full_name'>"+a.name+"</div><div class='email'>"+a.email+"</div></div></li>"},tokenFormatter:function(a){return"<li><p>"+a.name+"</p></li>"}});$("#sl_frmClipImage #sl_addclipbtn").click(function(){$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');$("#sl_frmClipImage #sl_processingcaption").show();$("#sl_frmClipImage :input").attr("disabled",true);$(this).hide();if($("#sl_frmClipImage")!="undefined"&&$("#sl_frmClipImage")!=null)if($("#sl_frmClipImage").validationEngine("validate")){$("#sl_frmClipImage").validationEngine("hideAll");sl_addClipToScrapbook(this)}if($("#sl_frmClipStory")!="undefined"&&$("#sl_frmClipStory")!=null)if($("#sl_frmClipStory").validationEngine("validate")){$("#sl_frmClipStory").validationEngine("hideAll");sl_addClipToScrapbook(this)}return false});$("#sl_frmClipImage #sl_addclipdesc").focus(function(){$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');return false});$("#sl_frmClipImage #token-input-shareRecipientInput").focus(function(){$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>')});$("#sl_frmClipStory #sl_addarticlecomments").focus(function(){$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');return false});$("#sl_frmClipStory #token-input-shareArticleRecipientInput").focus(function(){$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>')});$("#sl_frmClipStory #sl_addarticleclipbtn").click(function(){$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/transparent_loader.gif" width="16px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Processing Clip...</span>');$("#sl_frmClipStory #sl_processingcaption").show();$("#sl_frmClipStory :input").attr("disabled",true);$(this).hide();sl_addArticleClipToScrapbook(this);return false})});function sl_addClipToScrapbook(c){var g=$("#sl_frmClipImage #clipimagephototag").attr("src"),e=$("#sl_frmClipImage #clipimagephototag").attr("data-width"),d=$("#sl_frmClipImage #clipimagephototag").attr("data-height"),j=$("#sl_frmClipImage #sl_addclipbtn").attr("data-vid"),i=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-url"),f=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-title"),h=$("#sl_frmClipImage #sl_addclipbtn").attr("data-page-desc"),k=$("#sl_frmClipImage #sl_addclipdesc").val(),b=$("#sl_frmClipImage #shareRecipientInput").tokenInput("get"),a=$("#sl_frmClipImage #sharelinkfbbtn").val();if(a=="on")a=true;else a=false;var l="/clip/ClipPhotoVideo";$.ajax({type:"POST",url:l,data:{recipients:JSON.stringify(b),pageUrl:i,pageTitle:f,pageDescription:h,imageUrl:g,imageWidth:e,imageHeight:d,videoId:j,clipDesc:k,postOnFB:a},success:function(a){if(a.successful=="true"){if(b.length>0)showSuccessNotification("Clip has been shared.");else showSuccessNotification("Clip has been added.");$("#sl_frmClipImage #sl_processingcaption").html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>')}else{showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipImage #sl_processingcaption").hide()}$("#sl_frmClipImage :input").attr("disabled",false);$(c).show()},error:function(){showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipImage #sl_processingcaption").hide();$("#sl_frmClipImage :input").attr("disabled",false);$(c).show()}})}function sl_addArticleClipToScrapbook(b){var f=$("#sl_frmClipStory #sl_addarticleclipbtn").attr("data-article-id"),c=$("#sl_frmClipStory #sl_addarticlecomments").val(),d=$("#sl_frmClipStory #articletypenews").val(),e=$("#sl_frmClipStory #shareArticleRecipientInput").tokenInput("get"),a=$("#sl_frmClipStory #sharelinkfbbtn").val();if(a=="on")a=true;else a=false;var g="/article/PublishArticleClip";$.ajax({type:"POST",url:g,data:{articleId:f,articleDesc:c,articleType:d,recipients:JSON.stringify(e),postOnFB:a},success:function(a){if(a.successful=="true"){showSuccessNotification("Clip has been added.");$("#sl_frmClipStory #sl_processingcaption").html('<img src="/images/tick.png" width="20px" alt /><span style="padding-left: 10px;position: relative; top: -5px;">Successfully Clipped</span>')}else{showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipStory #sl_processingcaption").hide()}$("#sl_frmClipStory :input").attr("disabled",false);$(b).show()},error:function(){showErrorNotification("Error: Clip could not be added. Please try again later.");$("#sl_frmClipStory #sl_processingcaption").hide();$("#sl_frmClipStory :input").attr("disabled",false);$(b).show()}})}