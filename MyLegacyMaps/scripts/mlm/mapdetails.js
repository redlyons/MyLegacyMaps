
var MLM = (function () {

    var flgWasHere = "<div class='makeMeDraggable flgWasHere masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   '></div>";
    var flgHereNow = "<div class='makeMeDraggable flgHereNow masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   '></div>";
    var flgPlanToGo = "<div class='makeMeDraggable flgPlanToGo masterTooltip' data-xpos='50' data-ypos='50'  title='Name:      Date:   '></div>";
    var flgCustomLogo = "<div class='makeMeDraggable flgCustomLogo masterTooltip' data-xpos='50' data-ypos='50' title='Name:      Date:   ' style='height:75px; width:550px;'></div>"
    var flagId = 1;

    return {

        init: function () {
            $('body').css('background', 'url("/images/maps/' + $('#mapFileName').val() + '") no-repeat');

            MLM.wireUpButtonEvents();
            MLM.wireUpTooltips();
            MLM.wireUpFlags();
        },

        wireUpButtonEvents: function () {
            $("#btnWasHere").click(function () {
                $(flgWasHere).attr("id", "flag" + flagId++).appendTo("#content");
                //$("#content").append(flgWasHere.attr);
                MLM.wireUpFlags();
            });
            $("#btnHereNow").click(function () {
                $("#content").append(flgHereNow);
                MLM.wireUpFlags();
            });
            $("#btnWantToGoHere").click(function () {
                $("#content").append(flgPlanToGo);
                MLM.wireUpFlags();
            });
            $("#btnCustomLogo").click(function () {
                $("#content").append(flgCustomLogo);
                MLM.wireUpFlags();
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
                stop: MLM.handleDragStop
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
                    modal.open({ content: data, element: flag });
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
MLM.init();