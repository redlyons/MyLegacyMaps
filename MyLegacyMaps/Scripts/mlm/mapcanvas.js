
var MLM = {}
MLM.MapCanvas = (function () {
    
    var flgWasHere = "<div class='makeMeDraggable flgWasHere masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='1' title='Name:      Date:   '></div>";
    var flgHereNow = "<div class='makeMeDraggable flgHereNow masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='2' title='Name:      Date:   '></div>";
    var flgPlanToGo = "<div class='makeMeDraggable flgPlanToGo masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='3'  title='Name:      Date:   '></div>";
    var flgCustomLogo = "<div class='makeMeDraggable flgCustomLogo masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='4' title='Name:      Date:   ' style='height:75px; width:550px;'></div>"
    
    return {

        init: function () {
            $('body').css('background', 'url("/images/maps/' + $('#mapFileName').val() + '") no-repeat');

            MLM.MapCanvas.wireUpButtonEvents();
            MLM.MapCanvas.wireUpTooltips();
            MLM.MapCanvas.wireUpFlags();
        },

        wireUpButtonEvents: function () {
            $("#btnWasHere").click(function () {
                $(flgWasHere).appendTo("#content");
                MLM.MapCanvas.wireUpFlags();
            });
            $("#btnHereNow").click(function () {
                $("#content").append(flgHereNow);
                MLM.MapCanvas.wireUpFlags();
            });
            $("#btnWantToGoHere").click(function () {
                $("#content").append(flgPlanToGo);
                MLM.MapCanvas.wireUpFlags();
            });
            $("#btnCustomLogo").click(function () {
                $("#content").append(flgCustomLogo);
                MLM.MapCanvas.wireUpFlags();
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
                snap: '#content',
                stop: MLM.MapCanvas.handleDragStop
            });

            $(".makeMeDraggable").click(function () {
                var flag = this;
                var template = "";
                var adoptedMapId = ($('#adoptedMapId') != null)?  $('#adoptedMapId').val() : 0;
                var flagId = ($(flag).attr("data-flagid") != null)? $(flag).attr("data-flagid") : 0;
                
                if ($(flag).hasClass('flgWasHere')) template = 'WasHere.html';
                else if ($(flag).hasClass('flgHereNow')) template = 'HereNow.html';
                else if ($(flag).hasClass('flgPlanToGo')) template = 'PlanToGo.html';
                else if ($(flag).hasClass('flgCustomLogo')) template = 'CustomLogo.html';
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
            console.log("x = ", offsetXPos, "y = ", offsetYPos);
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