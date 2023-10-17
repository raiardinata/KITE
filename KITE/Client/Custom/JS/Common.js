function popupwin(url,name,w,h){
    LeftPosition = (screen.width) ? (screen.width-w)/2 : 0;
    TopPosition = (screen.height) ? (screen.height-h)/2 : 0; 
    var settings ='dialogHeight: '+h+';dialogWidth: '+w+';';

    var retVal = window.showModalDialog(url,name,settings);
    
    return true;
}

function WindowModal(url, width, height)
{
    var config = 'dialogWidth: '+ width +'px; dialogHeight: ' + height + 'px;';
    var retVal = window.showModalDialog(url , window, config);
    return true;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

    return true;
}

function Close()
{
    var opener = null;

    if (window.dialogArguments) // Internet Explorer supports window.dialogArguments
    { 
        opener = window.dialogArguments;
    } 
    else // Firefox, Safari, Google Chrome and Opera supports window.opener
    {        
        if (window.opener) 
        {
            opener = window.opener;
        }
    }    
    
    opener.location.reload(true);
    window.close();
}

function Close2()
{
    var opener = null;

    if (window.dialogArguments) // Internet Explorer supports window.dialogArguments
    { 
        opener = window.dialogArguments;
    } 
    else // Firefox, Safari, Google Chrome and Opera supports window.opener
    {        
        if (window.opener) 
        {
            opener = window.opener;
        }
    }    
    //opener.document.getElementById("ctl00_MainContent_btmSave").click();
    opener.location.href = opener.location.href;
    window.close();
}