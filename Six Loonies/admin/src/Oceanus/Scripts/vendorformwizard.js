var prior_state = 0;
var current_state = 1;
var showServicePricePoint = false;
var showVendorImage = false;

$(document).ready(function () {

    changeWizardStep();

    // loads new state based on button clicked
    $('.error').hide();

    populateCountryStateDropdown();

    $("#CountryList").change(function () {
        $.ajax(
            {
                type: "POST",
                url: "/PhysicalLocations/AjaxStateList",
                data: "Country=" + htmlEncode($("#CountryList > option:selected").attr("value")),
                success: function (statesData) {
                    var items = "";
                    $.each(statesData, function (i, state) {
                        items += "<option value=\"" + state.Value + "\">" + state.Text + "</option>";
                    });
                    $("#StateList").html(items);
                },
                error: function (req, status, error) {
                    alert("Sorry! Retrieval of states failed");
                }
            });
    });

    $("#VirtualCountryList").change(function () {
        $.ajax(
            {
                type: "POST",
                url: "/VirtualLocation/AjaxStateList",
                data: "Country=" + htmlEncode($("#VirtualCountryList > option:selected").attr("value")),
                success: function (statesData) {
                    var items = "";
                    $.each(statesData, function (i, state) {
                        items += "<option value=\"" + state.Value + "\">" + state.Text + "</option>";
                    });
                    $("#VirtualStateList").html(items);
                },
                error: function (req, status, error) {
                    alert("Sorry! Retrieval of states failed");
                }
            });
    });

    $('#service_portfolio_link').click(function () {
        var vendorFormValidated = validateVendorForm();

        if (vendorFormValidated) {
            showVendorImage = true;
            showServicePricePoint = false;
            $('.error').hide();

            if ($('#vid').text().trim() == "") {
                createVendor();
            }
            else {
                editVendor();
            }
        }
    });

    $('#addLocation').click(function () {
        var physLocFormValidated = validatePhysicalLocationForm(true);

        if (physLocFormValidated) {
            $('.error').hide();
            if ($('#plid').text().trim() == "") {
                createPhysicalLocation();
            }
            else {
                editPhysicalLocation();
            }
        }

        return false;
    });

    $('#virtlocsubmit').click(function () {
        var virtLocFormValidated = validateVirtualLocationForm(true);

        if (virtLocFormValidated) {
            $('.error').hide();
            createVirtualLocation();

        }

        return false;
    });    

    $('#vendorSubmitBtn').click(function () {
        $('#wizard_2').click();

        return false;
    });

    $('#servicesSubmitBtn').click(function () {
        $('#wizard_3').click();

        return false;
    });


    $('.wizard_link').click(function () {
        // validate and process vendor form here

        $('.error').hide();

        current_state = $(this).attr('step');

        if (prior_state == 1 && current_state != 1) {
            var vendorFormValidated = validateVendorForm();

            if (vendorFormValidated) {
                $('.error').hide();
                if ($('#vid').text().trim() == "") {
                    createVendor();
                }
                else {
                    editVendor();
                }
            }
            else {
                current_state = 1;
            }

            changeWizardStep();
        }
        else {
            changeWizardStep();
        }

        return false;
    });
});

function loadServiceEdit(serviceId) {

    $('#serviceAdd').load("/Service/EditPartial?id=" + serviceId);
    $('.error').hide();
    return false;
}

function populateCountryStateDropdown() {
    $.ajax(
    {
        type: "POST",
        url: "/PhysicalLocations/AjaxCountryList",
        success: function (data) {
            var items = "";
            $.each(data, function (i, country) {
                items += "<option value=\"" + country.Value + "\">" + country.Text + "</option>";

                if (i == 0) {

                    $.ajax(
                    {
                        type: "POST",
                        url: "/PhysicalLocations/AjaxStateList",
                        data: "Country=" + htmlEncode(country.Value),
                        success: function (statesData) {
                            var stateItems = "";
                            $.each(statesData, function (i, state) {
                                stateItems += "<option value=\"" + state.Value + "\">" + state.Text + "</option>";
                            });
                            $("#StateList").html(stateItems);

                            $("#VirtualStateList").html(stateItems);
                        },
                        error: function (req, status, error) {
                            alert("Sorry! Vendor save failed");
                        }
                    });
                }
            });

            $("#CountryList").html(items);
            $("#VirtualCountryList").html(items);
        },
        error: function (req, status, error) {
            alert("Sorry! Retrieval of country names failed");
        }
    });
}

function changeWizardStep() {
    //reset the wizardcontent to hidden
    if (prior_state != current_state) {
        prior_state = current_state;

        $('.wizardcontent').hide();
        $('#wizardcontent').hide();
        load_state();
    }
}

function deleteVendor(vendorId) {
    $.ajax(
    {
        type: "POST",
        url: "/Vendor/AjaxDelete",
        data: "id=" + htmlEncode(vendorId),
        success: function (result) {
            alert("vendor deleted");
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete vendor");
        }
    });
    return false;
}

function deleteVirtualLocation(virtLocId) {
    $.ajax(
    {
        type: "POST",
        url: "/VirtualLocation/AjaxDelete",
        data: "id=" + htmlEncode(virtLocId),
        success: function (result) {
            reloadVirtualLocList($("#vid").text().trim());
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete virtual location");
        }
    });
    return false;
}

function deletePhysicalLocation(physLocId) {
    $.ajax(
    {
        type: "POST",
        url: "/PhysicalLocations/AjaxDelete",
        data: "id=" + htmlEncode(physLocId),
        success: function (result) {
            reloadPhysicalLocList($("#vid").text().trim());
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete physical location");
        }
    });
    return false;
}

function deleteService(serviceId) {
    $.ajax(
    {
        type: "POST",
        url: "/Service/AjaxDelete",
        data: "id=" + htmlEncode(serviceId),
        success: function (result) {
            reloadServiceList($("#vid").text().trim());
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete service");
        }
    });
    return false;
}

function deleteServicePkg(pkgId) {
    $.ajax(
    {
        type: "POST",
        url: "/ServicePackages/AjaxDelete",
        data: "id=" + htmlEncode(pkgId),
        success: function (result) {
            reloadServicePricePoints();
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete service package");
        }
    });
    return false;
}

function deleteVendorImage(imageId) {
    $.ajax(
    {
        type: "POST",
        url: "/VendorImage/AjaxDelete",
        //data: "id=" + htmlEncode(portfolioId),
        data: {id: imageId },
        success: function (result) {
            reloadVendorImages();
        },
        error: function (req, status, error) {
            alert("Sorry! Could not delete vendor portfolio image");
        }
    });

    return false;
}

function createVendor() {
    var vendorName = $("#vendorName").val();
    var vendorShortDesc = $("#vendorShortDesc").val();
    var vendorFullDesc = $("#vendorFullDesc").val();
    var websiteURL = $("#vendorWebsite").val();
    var email = $("#vendorEmail").val();
    var busPhone = $("#vendorBusPhone").val();
    var fax = $("#vendorFax").val();

    $.ajax(
    {
        type: "POST",
        url: "/Vendor/AjaxCreate",
        data: "vendorName=" + htmlEncode(vendorName) + "&vendorShortDesc=" + htmlEncode(vendorShortDesc) + "&vendorFullDesc=" + htmlEncode(vendorFullDesc) + "&websiteURL=" + htmlEncode(websiteURL) +"&email=" + htmlEncode(email) + "&busPhone=" + htmlEncode(busPhone) + "&fax=" + htmlEncode(fax),
        success: function (result) {
            $('#vid').text(result.vid);

            if (showVendorImage) {
                showVendorImage = false;

                $.fancybox({
                    'type': 'iframe',
                    'autoscale': true,
                    'href': ("/VendorImage/CreatePartial/" + $('#vid').text().trim())
                });
            }

            changeWizardStep();
        },
        error: function (req, status, error) {        
            showVendorImage = false;
            showServicePricePoint = false;
            alert("Sorry! Vendor save failed");
        }
    });
}

function editVendor() {
    var vendorName = $("#vendorName").val();
    var vendorShortDesc = $("#vendorShortDesc").val();
    var vendorFullDesc = $("#vendorFullDesc").val();
    var websiteURL = $("#vendorWebsite").val();
    var email = $("#vendorEmail").val();
    var busPhone = $("#vendorBusPhone").val();
    var fax = $("#vendorFax").val();

    $.ajax(
    {
        type: "POST",
        url: "/Vendor/AjaxEdit",
        data: "vendorId=" + htmlEncode($('#vid').text().trim()) + "&vendorName=" + htmlEncode(vendorName) + "&vendorShortDesc=" + htmlEncode(vendorShortDesc) + "&vendorFullDesc=" + htmlEncode(vendorFullDesc) + "&websiteURL=" + htmlEncode(websiteURL) + "&email=" + htmlEncode(email) + "&busPhone=" + htmlEncode(busPhone) + "&fax=" + htmlEncode(fax),
        success: function (result) {
            $('#vid').text(result.vid);

            if (showVendorImage) {
                showVendorImage = false;

                var vendorId = $('#vid').text().trim();

                $.fancybox({
                    type: 'iframe',
                    autoscale: true,
                    href: ("/VendorImage/CreatePartial/" + vendorId),
                    onCancel: function () {
                        $.ajax(
                        {
                            type: "POST",
                            url: "/VendorImage/AjaxSweep",
                            data: { id: vendorId }
                        });
                    }
                });
            }

            changeWizardStep();
        },
        error: function (req, status, error) {
            showVendorImage = false;
            showServicePricePoint = false;
            alert("Sorry! Vendor save failed");
        }
    });
}

function createVirtualLocation() {

    var vendorId = $("#vid").text().trim();
    var city = $("#virtLocCity").val();
    var refStateId = $("#VirtualStateList > option:selected").attr("value");

    $.ajax(
    {
        type: "POST",
        url: "/VirtualLocation/AjaxCreate",
        data: "vendorId=" + htmlEncode(vendorId) + "&city=" + htmlEncode(city) + "&refStateId=" + htmlEncode(refStateId),
        success: function (result) {

            reloadVirtualLocList($("#vid").text().trim());
        },
        error: function (req, status, error) {
            alert("Sorry! Virtual Location create save failed");
        }
    });

}


function createPhysicalLocation() {

    var vendorId = $("#vid").text().trim();
    var addressLine1 = $("#physicalLocAddressLine1").val();
    var addressLine2 = $("#physicalLocAddressLine2").val();
    var city = $("#physicalLocCity").val();
    var postalCode = $("#physicalLocPostalCode").val();
    var refStateId = $("#StateList > option:selected").attr("value");
    var phone = $("#physicalLocPhone").val();
    var fax = $("#physicalLocFax").val();
    var email = $("#physicalLocEmail").val();

    $.ajax(
    {
        type: "POST",
        url: "/PhysicalLocations/AjaxCreate",
        data: "vendorId=" + htmlEncode(vendorId) + "&addressLine1=" + htmlEncode(addressLine1) + "&addressLine2=" + htmlEncode(addressLine2) + "&city=" + htmlEncode(city) + "&postalCode=" + htmlEncode(postalCode) + "&refStateId=" + htmlEncode(refStateId) + "&phone=" + htmlEncode(phone) + "&fax=" + htmlEncode(fax) + "&email=" + htmlEncode(email),
        success: function (result) {
            $('#plid').text(result.plid);

            reloadPhysicalLocList($("#vid").text().trim());
            clearPhysLocationForm();
        },
        error: function (req, status, error) {
            alert("Sorry! Physical Location create save failed");
        }
    });

}

function editPhysicalLocation() {
    var vendorId = $("#vid").text().trim();
    var addressLine1 = $("#physicalLocAddressLine1").val();
    var addressLine2 = $("#physicalLocAddressLine2").val();
    var city = $("#physicalLocCity").val();
    var postalCode = $("#physicalLocPostalCode").val();
    var refStateId = $("#StateList > option:selected").attr("value");
    var phone = $("#physicalLocPhone").val();
    var fax = $("#physicalLocFax").val();
    var email = $("#physicalLocEmail").val();

    $.ajax(
    {
        type: "POST",
        url: "/PhysicalLocations/AjaxEdit",
        data: "physicalLocId=" + htmlEncode($('#plid').text().trim()) + "&vendorId=" + htmlEncode(vendorId) + "&addressLine1=" + htmlEncode(addressLine1) + "&addressLine2=" + htmlEncode(addressLine2) + "&city=" + htmlEncode(city) + "&postalCode=" + htmlEncode(postalCode) + "&refStateId=" + htmlEncode(refStateId) + "&phone=" + htmlEncode(phone) + "&fax=" + htmlEncode(fax) + "&email=" + htmlEncode(email),
        success: function (result) {
            $('#plid').text(result.plid);

            reloadPhysicalLocList($("#vid").text().trim());
            clearPhysLocationForm();
        },
        error: function (req, status, error) {
            alert("Sorry! Physical Location edit save failed");
        }
    });
}

function clearServiceForm() {
    $("#sid").text("");
    $("#sid").val("");
    $("#sid").html("");
    $("#serviceName").val("");
    $("#serviceShortDesc").val("");
    $("#serviceFullDesc").val("");

    $("#pricePackages").html("");

    $("#pricePackages").html("<p>There are currently no service price points associated.</p>");
}

function clearPhysLocationForm() {
    $("#physicalLocAddressLine1").val("");
    $("#physicalLocAddressLine2").val("");
    $("#physicalLocCity").val("");
    $("#physicalLocPostalCode").val("");
    $("#physicalLocPhone").val("");
    $("#physicalLocFax").val("");
    $("#physicalLocEmail").val("");
}

function validatePhysicalLocationForm(silent) {
    var isValid = true;

    var addressLine1 = $("#physicalLocAddressLine1").val();
    if (addressLine1 == "") {
        if (silent == false) {
            $("label#physicalLocAddressLine1_error").show();
            $("#physicalLocAddressLine1").focus();
        }
        isValid = false;
    }
    var city = $("#physicalLocCity").val();
    if (city == "") {
        if (silent == false) {
            $("#physicalLocCity_error").show();
            $("#physicalLocCity").focus();
        }
        isValid = false;
    }
    var postalCode = $("#physicalLocPostalCode").val();
    if (postalCode == "") {
        if (silent == false) {
            $("label#physicalLocPostalCode_error").show();
            $("#physicalLocPostalCode").focus();
        }
        isValid = false;
    }

    return isValid;
}

function validateVirtualLocationForm(silent) {
    var isValid = true;

    var city = $("#virtLocCity").val();
    if (city == "") {
        if (silent == false) {
            $("label#virtLocCity_error").show();
            $("#virtLocCity").focus();
        }
        isValid = false;
    }

    return isValid;
}

function validateVendorForm() {
    var isValid = true;

    var vendorName = $("#vendorName").val();
    if (vendorName == "") {
        $("#vendorName_error").show();
        $("#vendorName").focus();
        isValid = false;
    }

    return isValid;
}

function createService() {
    var vendorId = $("#vid").text().trim();
    var serviceName = $("#serviceName").val();
    var serviceShortDesc = $("#serviceShortDesc").val();
    var serviceFullDesc = $("#serviceFullDesc").val();

    $.ajax(
    {
        type: "POST",
        url: "/Service/AjaxCreate",
        data: "vendorId=" + htmlEncode(vendorId) + "&serviceName=" + htmlEncode(serviceName) + "&serviceShortDesc=" + htmlEncode(serviceShortDesc) + "&serviceFullDesc=" + htmlEncode(serviceFullDesc),
        success: function (result) {
            $('#sid').text(result.sid);

            if (showServicePricePoint) {
                showServicePricePoint = false;

                $.fancybox({
                    'type': 'iframe',
                    'autoscale': true,
                    'href': ("/ServicePackages/CreatePartial?id=" + $('#sid').text().trim())
                });
            }
            else {
                reloadServiceList($("#vid").text().trim());
            }
        },
        error: function (req, status, error) {
            showVendorImage = false;
            showServicePricePoint = false;
            alert("Sorry! Service save failed");
        }
    });
}

function editService() {
    var serviceName = $("#serviceName").val();
    var serviceShortDesc = $("#serviceShortDesc").val();
    var serviceFullDesc = $("#serviceFullDesc").val();

    $.ajax(
    {
        type: "POST",
        url: "/Service/AjaxEdit",
        data: "serviceId=" + htmlEncode($('#sid').text().trim()) + "&serviceName=" + htmlEncode(serviceName) + "&serviceShortDesc=" + htmlEncode(serviceShortDesc) + "&serviceFullDesc=" + htmlEncode(serviceFullDesc),
        success: function (result) {
            $('#sid').text(result.sid);

            if (showServicePricePoint) {
                showServicePricePoint = false;

                $.fancybox({
                    'type': 'iframe',
                    'autoscale': true,
                    'href': ("/ServicePackages/CreatePartial?id=" + $('#sid').text().trim())
                });
            }
            else {
                reloadServiceList($("#vid").text().trim());
            }
        },
        error: function (req, status, error) {
            showVendorImage = false;
            showServicePricePoint = false;
            alert("Sorry! Service save failed");
        }
    });
}


function validateServiceForm(silent) {
    var isValid = true;

    var vendorName = $("#serviceName").val();
    if (vendorName == "") {
        if (silent == false) {
            $("#serviceName_error").show();
            $("#serviceName").focus();
        }
        isValid = false;
    }

    return isValid;
}


function load_state() {
    //disable all buttons while loading the state
    $('#previous').attr("disabled", "disabled");
    $('#next').attr("disabled", "disabled")
    //load the content for this state into the wizard content div and fade in
    $('#step_' + current_state).fadeIn("slow");
    //set the wizard class to current state for next iteration
    $('#wizard').attr('class', 'step_' + current_state);
    var iterator = 1;
    // loop through the list items and set classes for css coloring
    $('#mainNav li').each(function () {
        var step = $(this)
        if (iterator == current_state) { step.attr('class', 'current'); }
        else if (current_state - iterator == 1) { step.attr('class', 'lastDone'); }
        else if (current_state - iterator > 1) { step.attr('class', 'done'); }
        else { step.attr('class', ''); }
        // special case for step 5 because it doesn't have bacground image
        if (iterator == 5) { step.addClass('mainNavNoBg'); }
        iterator++;
    });
}

function reloadPhysicalLocList(vendorId) {

    $('#physicalVendorLocationsList').load('/PhysicalLocations/IndexPartialFiltered?vendorId=' + vendorId);
}

function reloadVirtualLocList(vendorId) {

    $('#virtualVendorLocationsList').load('/VirtualLocation/IndexPartialFiltered?vendorId=' + vendorId);
}

function reloadServiceList(vendorId) {

    $('#serviceList').load('/Service/IndexPartialFiltered?vendorId=' + vendorId);
}

function htmlEncode(value) {
    if (value) {
        return encodeURIComponent(value);
    }
    else {
        return '';
    }
}
