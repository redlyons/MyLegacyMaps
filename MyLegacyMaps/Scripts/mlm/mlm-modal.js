MLM.Modal = (function () {
    var
    method = {},
    $overlay,
    $modal,
    $content,
    $close,
    flag = {
        adoptedMapId: 0,
        flagId: 0,
        flagTypeId: 0,
        yPos: 0,
        xPos: 0,
        name: '',
        description: '',
        videoUrl: '',
        photosUrl: '',
        date: '',
        address1: '',
        city: '',
        state: '',
        postalCode: '',
        partnerLogoId: '',


    },
    flagElement,
    token;

    // Center the modal in the viewport
    method.center = function () {
        var top, left;

        top = Math.max($(window).height() - $modal.outerHeight(), 0) / 2;
        left = Math.max($(window).width() - $modal.outerWidth(), 0) / 2;

        $modal.css({
            top: top + $(window).scrollTop(),
            left: left + $(window).scrollLeft()
        });
    };

    // Open the modal
    method.open = function (settings) {

        var form = $('#__AjaxAntiForgeryForm');
        token = $('input[name="__RequestVerificationToken"]', form).val();

        $content.empty().append(settings.content);

        //stash flag coordinates for use on post
        flag.adoptedMapId = settings.adoptedMapId;
        flag.flagId = $(settings.element).attr('data-flagid');
        flag.flagTypeId = $(settings.element).attr('data-flagtypeid');
        flag.xPos = $(settings.element).attr('data-xpos');
        flag.yPos = $(settings.element).attr('data-ypos');
        flag.partnerLogoId = $(settings.element).attr('data-partnerlogoid');
        flagElement = $(settings.element);

        switch (settings.element.attributes["data-flagtypeid"].value) {
            case "1":
                $('#spCaption').text('Travel Way Point - Was Here');
                break;
            case "2":
                $('#spCaption').text('Travel Way Point - Here Now');
                break;
            case "3":
                $('#spCaption').text('Travel Way Point - Plan To Go');
                break;
            case "4":
                $('#spCaption').text('Real Estate');
                break;

        }

        if (flag.flagId != null) {
            method.getFlag(flag.flagId);
            
        }
        method.getTotalCredits();

        $modal.css({
            width: settings.width || 'auto',
            height: settings.height || 'auto'
        });

        method.center();
        $(window).bind('resize.modal', method.center);
        $modal.show();
        $overlay.show();
    };

    // Close the modal
    method.close = function () {
        $modal.hide();
        $overlay.hide();
        $content.empty();
        $(window).unbind('resize.modal');
    };

    method.createFlag = function () {
        $.ajax({
            url: window.location.origin + "/flags/create/",
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                flagTypeId: flag.flagTypeId,
                adoptedMapId: flag.adoptedMapId,
                name: $('#txtFlagName').val(),
                description: $('#txtFlagDescription').val(),
                videoUrl: $('#txtFlagVideoUrl').val(),
                photosUrl: $('#txtFlagPhotoUrl').val(),
                date: $('#txtFlagDate').val(),
                xPos: flag.xPos,
                yPos: flag.yPos
            },
            success: function (data) {
                $(flagElement).attr("data-flagId", data.FlagId);
                $(flagElement).attr("title", method.getToolTip(data));
                method.close();
            },
            error: function (data) {
                console.log('error on create');
            }
        });
    };

   method.createListing = function () {
        $.ajax({
            url: window.location.origin + "/flags/createListing/",
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                flagTypeId: flag.flagTypeId,
                adoptedMapId: flag.adoptedMapId,
                name: $('#txtFlagName').val(),
                description: $('#txtFlagDescription').val(),
                videoUrl: $('#txtFlagVideoUrl').val(),
                photosUrl: $('#txtFlagPhotoUrl').val(),
                xPos: flag.xPos,
                yPos: flag.yPos,
                address1: $('#txtAddress1').val(),
                city: $('#txtCity').val(),
                state: $('#txtState').val(),
                postalCode: $('#txtPostalCode').val(),
                partnerLogoId: flag.partnerLogoId,
            },
            success: function (data) {
                $(flagElement).attr("data-flagId", data.FlagId);
                $(flagElement).attr("title", method.getToolTip(data));
                method.close();
            },
            error: function (data) {
                console.log('error on create');
            }
        });
    };

    method.editFlag = function (data) {
        $('#txtFlagName').removeClass('readonly');
        $('#txtFlagDescription').removeClass('readonly');
        $('#txtFlagVideoUrl').removeClass('readonly');
        $('#txtFlagPhotoUrl').removeClass('readonly');

        $('#txtFlagVideoUrl').toggle();
        $('#txtFlagPhotoUrl').toggle();
        $('#lnkFlagVideoUrl').toggle();
        $('#lnkFlagPhotoUrl').toggle();

        $('#btnEditFlag').toggle();
        $('#btnSaveFlag').toggle();
    };

    method.editRealEstateFlag = function (data) {
        $('#txtFlagName').removeClass('readonly');
        $('#txtFlagDescription').removeClass('readonly');
        $('#txtFlagVideoUrl').removeClass('readonly');
        $('#txtFlagPhotoUrl').removeClass('readonly');
        $('#txtAddress1').removeClass('readonly');
        $('#txtCity').removeClass('readonly');
        $('#txtState').removeClass('readonly');
        $('#txtPostalCode').removeClass('readonly');

        $('#txtFlagVideoUrl').toggle();
        $('#txtFlagPhotoUrl').toggle();
        $('#lnkFlagVideoUrl').toggle();
        $('#lnkFlagPhotoUrl').toggle();

        $('#btnEditFlag').toggle();
        $('#btnSaveFlag').toggle();
    };

    method.saveFlag = function (data) {
        $.ajax({
            url: window.location.origin + "/flags/edit/",
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                flagId: flag.flagId,
                flagTypeId: flag.flagTypeId,
                adoptedMapId: flag.adoptedMapId,
                name: $('#txtFlagName').val(),
                description: $('#txtFlagDescription').val(),
                videoUrl: $('#txtFlagVideoUrl').val(),
                photosUrl: $('#txtFlagPhotoUrl').val(),
                date: $('#txtFlagDate').val(),
                xPos: flag.xPos,
                yPos: flag.yPos
            },
            success: function (data) {
                $(flagElement).attr("title", method.getToolTip(data));
                $('#txtFlagName').addClass('readonly');
                $('#txtFlagDescription').addClass('readonly');
                $('#txtFlagVideoUrl').addClass('readonly');
                $('#txtFlagPhotoUrl').addClass('readonly');

                $('#txtFlagVideoUrl').toggle();
                $('#txtFlagPhotoUrl').toggle();
                $('#lnkFlagVideoUrl').toggle();
                $('#lnkFlagPhotoUrl').toggle();

                $('#btnEditFlag').toggle();
                $('#btnSaveFlag').toggle();
            },
            error: function (data) {
                console.log("error on save");
            }
        });

    };

    method.saveRealEstateFlag = function (data) {
        $.ajax({
            url: window.location.origin + "/flags/editRealEstate/",
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
                flagId: flag.flagId,
                flagTypeId: flag.flagTypeId,
                adoptedMapId: flag.adoptedMapId,
                name: $('#txtFlagName').val(),
                description: $('#txtFlagDescription').val(),
                videoUrl: $('#txtFlagVideoUrl').val(),
                photosUrl: $('#txtFlagPhotoUrl').val(),
                xPos: flag.xPos,
                yPos: flag.yPos,
                partnerLogoId: flag.partnerLogoId,
                address1: $('#txtAddress1').val(),
                city: $('#txtCity').val(),
                state: $('#txtState').val(),
                postalCode: $('#txtPostalCode').val()

            },
            success: function (data) {
                $(flagElement).attr("title", method.getToolTip(data));
                $('#txtFlagName').addClass('readonly');
                $('#txtFlagDescription').addClass('readonly');
                $('#txtFlagVideoUrl').addClass('readonly');
                $('#txtFlagPhotoUrl').addClass('readonly');
                $('#txtAddress1').addClass('readonly');
                $('#txtCity').addClass('readonly');
                $('#txtState').addClass('readonly');
                $('#txtPostalCode').addClass('readonly');

                $('#txtFlagVideoUrl').toggle();
                $('#txtFlagPhotoUrl').toggle();
                $('#lnkFlagVideoUrl').toggle();
                $('#lnkFlagPhotoUrl').toggle();

                $('#btnEditFlag').toggle();
                $('#btnSaveFlag').toggle();
            },
            error: function (data) {
                console.log("error on save");
            }
        });

    };

    method.getTotalCredits = function () {
        $.ajax({
            url: window.location.origin + "/profile/totalcredits/",
            type: 'GET',
            success: function (data) {
                if (data > 0) {
                    var saveText = "Save (1 of " + data + " token";
                    if (data > 1) saveText += "s";
                    saveText += ")";

                    $('#btnCreateFlag').text(saveText);
                    $('#btnCreateFlag').show();
                    $('#btnGetMoreTokens').hide();
                }
                else {
                    $('#btnCreateFlag').hide();
                    $('#btnGetMoreTokens').show();
                    $('#btnGetMoreTokens').attr("href", window.location.origin + "/profile/credits")
                }
            },
            error: function (data) {
                $('#btnCreateFlag').hide();
                $('#btnGetMoreTokens').hide();
                      
            }
        });
    };

    method.getFlag = function (flagId) {
        $.ajax({
            url: window.location.origin + "/flags/details/" + flagId,
            type: 'GET',
            success: function (data) {
                flag.name = data.Name;
                flag.description = data.Description;
                flag.videoUrl = data.VideoUrl;
                flag.photosUrl = data.PhotosUrl;
                flag.date = data.Date;
                flag.address1 = data.Address1;
                flag.city = data.City;
                flag.state = data.State;
                flag.postalCode = data.PostalCode;
                flag.partnerLogoId = data.PartnerLogoId;

                if (flag.name != null)
                    $('#txtFlagName').val(flag.name).html();

                if (flag.videoUrl != null) {
                    if($('#txtFlagVideoUrl'))
                        $('#txtFlagVideoUrl').val(flag.videoUrl).html();

                    if ($('#lnkFlagVideoUrl')) {
                        $('#lnkFlagVideoUrl').attr("href", flag.videoUrl)
                    }
                    
                }
                else if ($('#lnkFlagVideoUrl')) {
                    $('#lnkFlagVideoUrl').hide()

                }

                if (flag.photosUrl != null) {
                    if ($('#txtFlagPhotoUrl'))
                        $('#txtFlagPhotoUrl').val(flag.photosUrl).html();

                    if ($('#lnkFlagPhotoUrl')) {
                        $('#lnkFlagPhotoUrl').attr("href", flag.photosUrl)
                    }
                }
                else if ($('#lnkFlagPhotoUrl')) {
                        $('#lnkFlagPhotoUrl').hide()
                    
                }
                

                if (flag.description != null)
                    $('#txtFlagDescription').val(flag.description).html();

                if (flag.date != null)
                    $('#txtFlagDate').datepicker("setDate", new Date(parseInt(flag.date.substr(6))));

                if (flag.address1 != null)
                    $('#txtAddress1').val(flag.address1).html();

                if (flag.city != null)
                    $('#txtCity').val(flag.city).html();

                if (flag.state != null)
                    $('#txtState').val(flag.state).html();

                if (flag.postalCode != null)
                    $('#txtPostalCode').val(flag.postalCode).html();
            },
            error: function (data) {
                console.log("get flag error");
            }
        });
    },

    method.deleteFlag = function () {
        $.ajax({
            url: window.location.origin + "/flags/delete/?id=" + flag.flagId,
            type: 'POST',
            data: {
                __RequestVerificationToken: token,
            },
            success: function (data) {
                flagElement.remove();
                method.close();
            },
            error: function (data) {
                console.log("delete flag error")
            }
        });

    };

    method.getToolTip = function (response) {
        var displayName = (response.Name != null) ? response.Name : "";
        return "Name: " + displayName + " Date: " + method.getDisplayDate(response);
    };

    method.getDisplayDate = function (response) {
        if (response.Date != null) {
            var dt = new Date(parseInt(response.Date.substr(6)));
            var yyyy = dt.getFullYear().toString();
            var mm = (dt.getMonth() + 1).toString(); // getMonth() is zero-based         
            var dd = dt.getDate().toString();
            return mm + "/" + dd + "/" + yyyy;
            // displayDate = yyyy + '-' + (mm[1]?mm:"0"+mm[0]) + '-' + (dd[1]?dd:"0"+dd[0]);
        }
        return "";
    };

    // Generate the HTML and add it to the document
    $overlay = $('<div id="overlay"></div>');
    $modal = $('<div id="modal"></div>');
    $content = $('<div id="modalContent"></div>');
    $close = $('<a id="close" href="#">close</a>');

    $modal.hide();
    $overlay.hide();
    $modal.append($content, $close);

    $(document).ready(function () {
        $('body').append($overlay, $modal);
    });

    $close.click(function (e) {
        e.preventDefault();
        method.close();
    });
    return method;
}());