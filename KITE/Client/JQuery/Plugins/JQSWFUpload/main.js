/*
 * jQuery upload - main file
 */
$(document).ready(function(){
	$("#upload-container").jqswfupload({
		onFileSuccess: function(file,data,response) {
			$('#debug-bar').append('<p>File success: '+file.name+'</p><pre>'+data+'</pre>');
		},
		onFileError: function(file,code,message){
			$('#debug-bar').append('<p>File Error: '+file.name+'</p><pre>'+message+'</pre>');
		}
//		upload_url: 'AttachmentHandler.ashx',
//		flash_url : 'Images/swfupload.swf'
	});
});

