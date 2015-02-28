<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProfileOverviewImageControl.ascx.cs"
    Inherits="Rainforest.Assets.Controls.ProfileOverviewImageControl" %>
<ul id="slideshow" style="display:none">
    <li>
        <h3>
            Chicago, Illinois, USA</h3>
        <span>/Assets/Images/CompanyProfiles/Fullsize400x300/DeloitteOfficeChicago.gif</span>
        <p>
            Brief description of the office goes here</p>
        <a href="#">
            <img src="/Assets/Images/CompanyProfiles/Thumbnails125x94/DeloitteOfficeChicago.gif"
                alt="Chicago Office" /></a> </li>
    <li>
        <h3>
            Pennsylvania, USA</h3>
        <span>/Assets/Images/CompanyProfiles/Fullsize400x300/deloittePennslyvania.gif</span>
        <p>
            Brief description of the office goes here</p>
        <a href="#">
            <img src="/Assets/Images/CompanyProfiles/Thumbnails125x94/deloittePennslyvania.gif"
                alt="Pennsylvania Office" /></a> </li>
    <li>
        <h3>
            Milwaukee, Wisconsin, USA</h3>
        <span>/Assets/Images/CompanyProfiles/Fullsize400x300/Deloitte-Milwaukee.gif</span>
        <p>
            Brief description of the office goes here</p>
        <a href="#">
            <img src="/Assets/Images/CompanyProfiles/Thumbnails125x94/Deloitte-Milwaukee.gif"
                alt="Milwaukee Office" /></a> </li>
    <li>
        <h3>
            Deloitte Scooter</h3>
        <span>/Assets/Images/CompanyProfiles/Fullsize400x300/deloittescooter.gif</span>
        <p>
            Brief description of the scooter goes here</p>
        <a href="#">
            <img src="/Assets/Images/CompanyProfiles/Thumbnails125x94/deloittescooter.gif" alt="Deloitte Scooter" /></a>
    </li>
</ul>
<div id="wrapper" style="display:block">
    <div id="fullsize">
        <div id="imgprev" class="imgnav" title="Previous Image">
        </div>
        <div id="imglink">
        </div>
        <div id="imgnext" class="imgnav" title="Next Image">
        </div>
        <div id="image">
        </div>
        <div id="information">
            <h3>
            </h3>
            <p>
            </p>
        </div>
    </div>
    <div id="thumbnails">
        <div id="slideleft" title="Slide Left">
        </div>
        <div id="slidearea">
            <div id="slider">
            </div>
        </div>
        <div id="slideright" title="Slide Right">
        </div>
    </div>
</div>
<script type="text/javascript">
    var slideshow = new TINY.slideshow("slideshow");
    window.onload = function () {
        slideshow.auto = true;
        slideshow.speed = 500000000000;
        slideshow.link = "linkhover";
        slideshow.info = "information";
        slideshow.thumbs = "slider";
        slideshow.left = "slideleft";
        slideshow.right = "slideright";
        slideshow.scrollSpeed = 4;
        slideshow.spacing = 5;
        slideshow.active = "#fff";
        slideshow.init("slideshow", "image", "imgprev", "imgnext", "imglink");
    }
</script>
