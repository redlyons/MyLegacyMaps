
var MLM = {}
MLM.MapCanvas = (function () {
    
    var flgWasHere = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgWasHere masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='1' title='Name:      Date:   '></div></a>";
    var flgHereNow = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgHereNow masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='2' title='Name:      Date:   '></div></a>";
    var flgPlanToGo = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgPlanToGo masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='3'  title='Name:      Date:   '></div></a>";
    var step = .05;
    var slider = $('#zoomSlider');

    return {

        init: function () {      
            $('#canvas').css('background', 'url("' + $('#mapImageUrl').val() + '") no-repeat');
            var ht = "1450px";
            var wd =  "2000px";
            if ($('#mapOrientation').val() == 2)
            {
                ht = "2000px";
                wd = "1800px";
            }
            $('#canvas').css('height', ht);
            $('#canvas').css('width', wd);

            MLM.MapCanvas.wireUpButtonEvents();
            MLM.MapCanvas.wireUpTooltips();
            MLM.MapCanvas.wireUpFlags();
            MLM.MapCanvas.wireUpZoom();

            if ($('#adoptedMapId').val() != null) {
                $('#lnkSaveMap').hide();
            }
           
      
        },

        wireUpZoom: function () {
          
            slider.change(function () {
                window.parent.document.body.style.zoom = slider.val();
            });
            $("#zoomReset").click(function () {
                window.parent.document.body.style.zoom = 0;
            });
            $("#zoomIn").click(function () {
                var curr = parseFloat(slider.val());
                var zoomInVal = (curr - step);
                slider.val(zoomInVal);
                window.parent.document.body.style.zoom = zoomInVal;
              
            });
            $("#zoomOut").click(function () {
                var curr = parseFloat(slider.val());
                var zoomOutVal = (curr + step);
                slider.val(zoomOutVal);
                window.parent.document.body.style.zoom = zoomOutVal;
            });


        },

        wireUpButtonEvents: function () {
            $("#btnWasHere").click(function () {
                $("#canvas").append(flgWasHere);
                MLM.MapCanvas.wireUpFlags();
            });
            $("#btnHereNow").click(function () {
                $("#canvas").append(flgHereNow);
                MLM.MapCanvas.wireUpFlags();
            });
            $("#btnWantToGoHere").click(function () {
                $("#canvas").append(flgPlanToGo);
                MLM.MapCanvas.wireUpFlags();
            });            
            $("#lnkSaveMap").click(function () {
                $("#frmAdpotMap").submit();
            });

        },

        wireUpTooltips: function () {

            // Tooltip only Text
            $('.masterTooltip').hover(function () {
                // Hover over code
                var title = $(this).attr('title');
                $(this).data('tipText', title).removeAttr('title');
                $('<p class="tooltip"></p>')
                .text(title)
                .appendTo('body')
                .fadeIn('slow');
            }, function () {
                // Hover out code
                $(this).attr('title', $(this).data('tipText'));
                $('.tooltip').remove();
            }).mousemove(function (e) {
                var mousex = e.pageX + 20; //Get X coordinates
                var mousey = e.pageY + 10; //Get Y coordinates
                $('.tooltip')
                .css({ top: mousey, left: mousex })
            });

        },

        wireUpFlags: function () {
            $('.makeMeDraggable').draggable({
                cursor: 'move',
                snap: '#canvas',
                stop: MLM.MapCanvas.handleDragStop
            });

            $(".mlm-flag-lnk").click(function () {
                var flag = this.firstChild;
                var template = "";
                var adoptedMapId = ($('#adoptedMapId') != null)?  $('#adoptedMapId').val() : 0;
                var flagId = ($(flag).attr("data-flagid") != null)? $(flag).attr("data-flagid") : 0;
                
                if ($(flag).hasClass('flgWasHere')) template = 'WasHere.html';
                else if ($(flag).hasClass('flgHereNow')) template = 'HereNow.html';
                else if ($(flag).hasClass('flgPlanToGo')) template = 'PlanToGo.html';
                else return;
                
                if (adoptedMapId > 0 && flagId > 0) {
                    template = "Edit" + template;
                }               

                $.get('../../templates/modals/' + template, function (data) {
                    MLM.Modal.open({ content: data, element: flag, adoptedMapId: adoptedMapId });
                });
            });

        },

        handleDragStop: function (event, ui) {
            var offsetXPos = parseInt(ui.offset.left);
            var offsetYPos = parseInt(ui.offset.top);
            //console.log("x = ", offsetXPos, "y = ", offsetYPos);
            $(this).attr('data-xpos', offsetXPos);
            $(this).attr('data-ypos', offsetYPos);

        },
    };
})();
MLM.MapCanvas.init();


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
        date: ''

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
        flagElement = $(settings.element);

        if (flag.flagId != null){
            method.getFlag(flag.flagId);           
        }
       
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

    method.buyNow = function () {
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
                date: $('#txtFlagDate').val(),
                xPos: flag.xPos,
                yPos: flag.yPos
            },
            success: function (data) {
                $(flagElement).attr("title", method.getToolTip(data));
                method.close();
            },
            error: function (data) {
                console.log("error on save");
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
                flag.date = data.Date;

                if (flag.name != null)
                    $('#txtFlagName').val(flag.name).html();
                if (flag.videoUrl != null)
                    $('#txtFlagVideoUrl').val(flag.videoUrl).html();
                if (flag.description != null)
                    $('#txtFlagDescription').val(flag.description).html();
                if (flag.date != null)
                    $('#txtFlagDate').datepicker("setDate", new Date( parseInt(flag.date.substr(6))));
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
        var displayName = (response.Name != null)? response.Name : "";        
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