﻿
var MLM = {}
MLM.MapCanvas = (function () {
    
    var flgWasHere = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgWasHere masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='1' title='Name:      Date:   '></div></a>";
    var flgHereNow = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgHereNow masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='2' title='Name:      Date:   '></div></a>";
    var flgPlanToGo = "<a href='#' class='mlm-flag-lnk'><div class='makeMeDraggable flgPlanToGo masterTooltip' data-xpos='50' data-ypos='50' data-flagtypeid='3'  title='Name:      Date:   '></div></a>";
    var step = .05;
    var slider = $('#zoomSlider');
    var timer;
    var touchduration = 1000;

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
            
            $(".mlm-flag-lnk").click(function (e) {
                var flag = this.firstChild;
                MLM.MapCanvas.handleOpenModal(flag, false);
                e.preventDefault();
            });

            jQuery(document).on('touchstart', '.mlm-flag-lnk', function (e) {
                var flag = this.firstChild;
                timer = setTimeout(MLM.MapCanvas.handleOpenModal(flag, false), 2000);//2 seconds long hold               
                e.preventDefault();
            });

            jQuery(document).on('touchend', '.mlm-flag-lnk', function (e) {
                e.preventDefault();
                //stops short touches from firing the event
                if (timer)
                    clearTimeout(timer); // clearTimeout, not cleartimeout..
            });
        },

        wireUpFlagOnClickEvents: function () {
            $(".mlm-flag-lnk").click(function (e) {
                var flag = this.firstChild;
                MLM.MapCanvas.handleOpenModal(flag, true);
                e.preventDefault();
            });

            jQuery(document).on('touchstart', '.mlm-flag-lnk', function (e) {
                var flag = this.firstChild;
                timer = setTimeout(MLM.MapCanvas.handleOpenModalReadOnly(flag, true), 2000);//2 seconds long hold               
                e.preventDefault();
            });

            jQuery(document).on('touchend', '.mlm-flag-lnk', function (e) {
                e.preventDefault();
                //stops short touches from firing the event
                if (timer)
                    clearTimeout(timer); // clearTimeout, not cleartimeout..
            });
    
        },

        handleOpenModal: function(draggableFlagElem, readonly){
            var flag = draggableFlagElem;
            var template = "";
            var adoptedMapId = ($('#adoptedMapId') != null) ? $('#adoptedMapId').val() : 0;
            var flagId = ($(flag).attr("data-flagid") != null) ? $(flag).attr("data-flagid") : 0;
                     

            if ($(flag).hasClass('flgWasHere')) template = (readonly) ? 'WasHereReadOnly.html' : 'WasHere.html';
            else if ($(flag).hasClass('flgHereNow')) template = (readonly) ? 'HereNowReadOnly.html' : 'HereNow.html';
            else if ($(flag).hasClass('flgPlanToGo')) template = (readonly) ? 'PlanToGoReadOnly.html' : 'PlanToGo.html';
            else return;
          

            if (!readonly) {
                if (adoptedMapId > 0 && flagId > 0) {
                    template = "Edit" + template;
                }
            }

            $.get('../../templates/modals/' + template, function (data) {
                MLM.Modal.open({ content: data, element: flag, adoptedMapId: adoptedMapId });
            });
        },

        handleDragStop: function (event, ui) {
            var offsetXPos = parseInt(ui.offset.left);
            var offsetYPos = parseInt(ui.offset.top);
            var xPos = Math.round(offsetXPos - $('#canvas').offset().left);
            var yPos = Math.round(offsetYPos - $('#canvas').offset().top);
            //position relative to the canvas element
            $(this).attr('data-xpos', xPos);
            $(this).attr('data-ypos', yPos);

        },
    };
})();
MLM.MapCanvas.init();


