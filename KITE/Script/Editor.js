
function ShowGridEditorWindow(editor)
{
    var keys = { };
    
    var width = 350;
    var height = 300;
    var retVal = window.showModalDialog(editor
                 , keys, 'dialogWidth: '+ width +'px; dialogHeight: '+ height +'px;');
    retVal = null;
}

function ShowInputModal(url, width, height, defaultPostBack)
{
    var config = 'dialogWidth: 750px; dialogHeight: 450px;';
    if(width != null && height != null)
        config = 'dialogWidth: ' + width + 'px; dialogHeight: ' + height + 'px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config);
    if(defaultPostBack == null)    
        return true;
    else 
        return defaultPostBack;
}

function ShowInputModal(url, width, height)
{
    var config = 'dialogWidth: 150px; dialogHeight: 150px;';
    if(width != null && height != null)
        config = 'dialogWidth: ' + width + 'px; dialogHeight: ' + height + 'px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config);
    return true;
}

function ShowInputModal(url)
{
    var config = 'dialogWidth: 450px; dialogHeight: 550px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config); 
    return true;
}

function ShowInputModal1(url)
{
    var config = 'dialogWidth: 1150px; dialogHeight: 550px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , window, config);
    return true;
}

function ShowInputModal2(url)
{
    var config = 'dialogWidth: 750px; dialogHeight: 550px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config); 
    return true;
}

function ShowInputModalSmall(url)
{
    var config = 'dialogWidth: 350px; dialogHeight: 150px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config); 
    return true;
}

function ShowInputModalSmall(url)
{
    var config = 'dialogWidth: 350px; dialogHeight: 150px;';
    //url = url+'&seed='+Date();
    var retVal = window.showModalDialog(url , null, config); 
    return true;
}