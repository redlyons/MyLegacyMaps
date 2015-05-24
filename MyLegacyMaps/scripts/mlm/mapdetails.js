
var redflag = "<div class='makeMeDraggable redflag masterTooltip' title='Name:      Date:   '></div>";
var blueflag = "<div class='makeMeDraggable blueflag masterTooltip'  title='Name:      Date:   '></div>";
var greenflag = "<div class='makeMeDraggable greenflag masterTooltip'  title='Name:      Date:   '></div>";
var customLogo = "<div class='makeMeDraggable customLogo masterTooltip' title='Name:      Date:   ' style='height:75px; width:550px;'></div>"

function init() {
    $('.makeMeDraggable').draggable({
        cursor: 'move',
        snap: '#content',
        stop: handleDragStop
    });

    $(".makeMeDraggable").click(function () {
        $.get('../../templates/modals/WasHere.html', function (data) {
            modal.open({ content: data });
        });
    });

   
}

function handleDragStop(event, ui) {
    var offsetXPos = parseInt(ui.offset.left);
    var offsetYPos = parseInt(ui.offset.top);
    //alert("Pin Location: (" + offsetXPos + ", " + offsetYPos + ")\n");
}

$(document).ready(function () {
    $('body').css('background', 'url("/images/maps/' + $('#mapFileName').val() + '") no-repeat');

    $("#btnWasHere").click(function () {
        $("#content").append(redflag);
        init();
    });
    $("#btnHereNow").click(function () {
        $("#content").append(blueflag);
        init();
    });
    $("#btnWantToGoHere").click(function () {
        $("#content").append(greenflag);
        init();
    });
    $("#btnCustomLogo").click(function () {
        $("#content").append(customLogo);
        init();
    });

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
});