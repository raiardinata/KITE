$(document).ready(function () {
    $('#nav .Menu').css({ visibility: "hidden" });
    $('#nav li').hover(function () {
        $(this).find('.Menu:first').stop(true, true).css({ visibility: "visible", display: "none" }).slideDown(300);
    }, function () {
        $(this).find('.Menu:first').stop(true, true).slideUp(200);
    })

    $("#nav .Menu a").click(function () {
        $('#nav .Menu').slideUp(0);
    });
})