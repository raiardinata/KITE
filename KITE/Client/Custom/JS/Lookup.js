//Custom functions for lookup modal
//To cancel popup customStartFunctionhas to return false
function LUCustomFunctions() 
{
	//SAMPLE
    this.CustomClose = function(retVal) {
        //Custom function process before closing lookup
        return false;
    }
	//SAMPLE
    this.CustomStart = function() {
        //Custom function process before opening lookup
        //return false to cancel popup
        if(LUGetValue('LuVoucherType') == '')
        {
            ShowMessage('Please select Voucher Type');
            return false;
        }
        else
            return true; 
    }
}

//clear LookupBox
function LUClear(luName)
{
    var hdValue = document.getElementById(luName + '_HdValue');
    var txtText = document.getElementById(luName + '_TxtText');

    hdValue.value = '';    
    txtText.value = '';
}

function setLU(luName,luValue,luText)
{
    var hdValue = document.getElementById(luName + '_HdValue');
    var txtText = document.getElementById(luName + '_TxtText');

    hdValue.value = luValue;    
    txtText.value = luText;
}

//get LookupBox value
function LUGetValue(luName)
{
    var hdValue = document.getElementById(luName + '_HdValue');
    return hdValue.value;
}

//get LookupBox text
function LUGetText(luName)
{
    var txtText = document.getElementById(luName + '_TxtText');
    return txtText.value;
}

function ShowLookupModal(targetTextName, targetValueName, url, keys, startFunction, closeFunction)
{
    if(startFunction != undefined && startFunction != '')
    {
        var cst = new LUCustomFunctions();
        var startFunc = cst[startFunction];
        var callStart = startFunc();   
        if(callStart == false)
            return;     
    }

    var retVal = window.showModalDialog(url , keys, 'dialogWidth: 1024px; dialogHeight: 450px;');
    if (retVal != null) 
    {
        var  targetText = document.getElementById(targetTextName);
        var  targetValue = document.getElementById(targetValueName);
        targetValue.value = retVal.value;
        targetText.value = retVal.text;
        if(closeFunction != undefined && closeFunction != '')
        {
            var cfs = new LUCustomFunctions();
            var closeFunc = cfs[closeFunction];
            var call = closeFunc(targetTextName, retVal);        
        }
        
        var btn = document.getElementById('ctl00_MainContent_btnNone');
        
        if(btn!=null)
        {
            //alert('tes');
            btn.click();
        }
    }
}

function ShowLookupModalNoClick(targetTextName, targetValueName, url, keys, startFunction, closeFunction)
{
    if(startFunction != undefined && startFunction != '')
    {
        var cst = new LUCustomFunctions();
        var startFunc = cst[startFunction];
        var callStart = startFunc();   
        if(callStart == false)
            return;     
    }

    var retVal = window.showModalDialog(url , keys, 'dialogWidth: 600px; dialogHeight: 450px;');
    if (retVal != null) 
    {
        var  targetText = document.getElementById(targetTextName);
        var  targetValue = document.getElementById(targetValueName);
        targetValue.value = retVal.value;
        targetText.value = retVal.text;
        if(closeFunction != undefined && closeFunction != '')
        {
            var cfs = new LUCustomFunctions();
            var closeFunc = cfs[closeFunction];
            var call = closeFunc(targetTextName, retVal);        
        }
    }
}

function GetReturnObj(value)
{        
    obj =new Object();                
    obj.value = value;
    window.returnValue = obj;				
    window.close ();
}

function ModalReturn(obj)
{
    window.returnValue = obj;				
    window.close ();
}

function ShowMessage(msg)
{
    Ext.MessageBox.alert('Alert', msg);
}

function ShowLookup(targetName, url)
{
    var keys = { };
    var retVal = window.showModalDialog(url , keys, 'dialogWidth: 600px; dialogHeight: 350px;');
    if (retVal != null) 
    {
        var  target = document.getElementById(targetName);
        target.value = retVal.value;
    }
}

function ShowLookup(targetName, url, config)
{
    var keys = { };
    var retVal = window.showModalDialog(url , keys, config);
    if (retVal != null) 
    {
        var  target = document.getElementById(targetName);
        target.value = retVal.value;
    }
}

function isNumberKey(evt)
{
    var charCode = (evt.which) ? evt.which : event.keyCode
    
    if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;
    return true;
}