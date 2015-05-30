
var MLM = {}
MLM.MapCanvas = (function () {
    
    var flgWasHere = "<div class='makeMeDraggable flgWasHere masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   '></div>";
    var flgHereNow = "<div class='makeMeDraggable flgHereNow masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   '></div>";
    var flgPlanToGo = "<div class='makeMeDraggable flgPlanToGo masterTooltip' data-xpos='50' data-ypos='50'  title='Name:      Date:   '></div>";
    var flgCustomLogo = "<div class='makeMeDraggable flgCustomLogo masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   ' style='height:75px; width:550px;'></div>"
    var flagId = 1;

    return {

        init: function () {
            $('body').css('background', 'url("/images/maps/' + $('#mapFileName').val() + '") no-repeat');

            MLM.MapCanvas.wireUpButtonEvents();
            MLM.MapCanvas.wireUpTooltips();
            MLM.MapCanvas.wireUpFlags();
        },

        wireUpButtonEvents: function () {
            $("#btnWasHere").click(function () {
                $(flgWasHere).attr("id", "flag" + flagId++).appendTo("#content");
                //$("#content").append(flgWasHere.attr);
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
                if ($(flag).hasClass('flgWasHere')) template = 'WasHere.html';
                else if ($(flag).hasClass('flgHereNow')) template = 'HereNow.html';
                else if ($(flag).hasClass('flgPlanToGo')) template = 'PlanToGo.html';
                else if ($(flag).hasClass('flgCustomLogo')) template = 'CustomLogo.html';
                else return;

                $.get('../../templates/modals/' + template, function (data) {
                    MLM.Modal.open({ content: data, element: flag });
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
    $close;

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
        $content.empty().append(settings.content);

        //stash flag coordinates for use on post
        $modal.attr('data-xpos', $(settings.element).attr('data-xpos'));
        $modal.attr('data-ypos', $(settings.element).attr('data-ypos'));
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