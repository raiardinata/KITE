
function ShowPopUp(Div) {
    $(Div).PopUps({
        appendTo:"form",
        easing: 'easeOutBounce',
        speed: 1300,
        transition: 'slideDown'
    });
}


function Close(Div) {
    $(Div).PopUps().close();
}