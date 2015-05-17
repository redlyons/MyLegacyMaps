
var redflag = "<div class='makeMeDraggable redflag masterTooltip' title='Name:      Date:   '></div>";
var blueflag = "<div class='makeMeDraggable blueflag masterTooltip'  title='Name:      Date:   '></div>";
var greenflag = "<div class='makeMeDraggable greenflag masterTooltip'  title='Name:      Date:   '></div>";
var customLogo = "<div class='makeMeDraggable customLogo masterTooltip'  style='height:75px; width:550px;'></div>"

function init() {
    $('.makeMeDraggable').draggable({
        cursor: 'move',
        snap: '#content',
        stop: handleDragStop
    });

    $(".makeMeDraggable").click(function () {
        $.get('/modals/washere.html', function (data) {
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
});