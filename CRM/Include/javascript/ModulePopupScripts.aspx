<%@ Page language="c#" Codebehind="ModulePopupScripts.aspx.cs" AutoEventWireup="false" Inherits="Taoqi.JavaScript.ModulePopupScripts" %>
<%@ Import Namespace="System.Data" %>

<head visible="false" runat="server" />
var sCHANGE_MODULE_ID   = null;
var sCHANGE_MODULE_NAME = null;
var sCHANGE_QUERY       = null;
var bCHANGE_SUBMIT      = null;
var sCHANGE_CLICK_FIELD = null;

function ChangeAlert()
{
	alert('There was an error setting the Change callback.');
}

<%
if ( Security.IsAuthenticated() )
{
	foreach ( DataRowView row in vwModulePopups )
	{
		string sSINGULAR_NAME = Sql.ToString(row["SINGULAR_NAME"]);
		Response.Write("var Change" + sSINGULAR_NAME + " = ChangeAlert;\r\n");
	}
}
%>

function ChangeModule(sPARENT_ID, sPARENT_NAME)
{
	// 09/03/2009   Also clear any error messages returned by AJAX. 
	var fldAjaxErrors = document.getElementById(sCHANGE_MODULE_NAME + '_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';

	var fldCHANGE_MODULE_ID   = document.getElementById(sCHANGE_MODULE_ID  );
	if ( sCHANGE_MODULE_NAME != null )
	{
		var fldCHANGE_MODULE_NAME = document.getElementById(sCHANGE_MODULE_NAME);
		if ( fldCHANGE_MODULE_NAME != null )
		{
			fldCHANGE_MODULE_NAME.value = sPARENT_NAME;
		}
	}
	if ( fldCHANGE_MODULE_ID != null )
	{
		fldCHANGE_MODULE_ID.value   = sPARENT_ID  ;
		if ( bCHANGE_SUBMIT )
			document.forms[0].submit();
		// 09/18/2010   Add the CLICK_FIELD parameter so that an UpdatePanel can be submitted. 
		else if ( sCHANGE_CLICK_FIELD != null )
		{
			var fldCHANGE_CLICK_FIELD = document.getElementById(sCHANGE_CLICK_FIELD);
			if ( fldCHANGE_CLICK_FIELD != null )
				fldCHANGE_CLICK_FIELD.click();
		}
	}
	else
	{
		alert('Could not find ' + sCHANGE_MODULE_ID + ' in the form.');
	}
}

function ModuleTypePopup(sPopupURL, sPopupTitle)
{
	if ( sCHANGE_QUERY != null )
		sPopupURL += '?' + sCHANGE_QUERY;


	//return window.open(sPopupURL, sPopupTitle, '<%= Taoqi.Crm.Config.PopupWindowOptions() %>');

    var iWidth=680; //弹出窗口的宽度;
    var iHeight=450; //弹出窗口的高度;
    var iTop = (window.screen.availHeight-30-iHeight)/2; //获得窗口的垂直位置;
    var iLeft = (window.screen.availWidth-10-iWidth)/2; //获得窗口的水平位置;
    return window.open(sPopupURL,sPopupTitle,"scrollbars=yes,resizable=yes,height="+iHeight+", width="+iWidth+", top="+iTop+", left="+iLeft); 

}

// 09/18/2010   Add the CLICK_FIELD parameter so that an UpdatePanel can be submitted. 
function ModulePopup(sMODULE_TYPE, sMODULE_ID, sMODULE_NAME, sQUERY, bSUBMIT, sPOPUP_FILE, sCLICK_FIELD)
{
	// 05/18/2009   Simplify code.  Only assign change function specific to the task. 
	sCHANGE_MODULE_ID   = sMODULE_ID  ;
	sCHANGE_MODULE_NAME = sMODULE_NAME;
	sCHANGE_QUERY       = sQUERY      ;
	bCHANGE_SUBMIT      = bSUBMIT     ;
	sCHANGE_POPUP_FILE  = sPOPUP_FILE ;
	sCHANGE_CLICK_FIELD = sCLICK_FIELD;
	if ( sCHANGE_POPUP_FILE == null )
		sCHANGE_POPUP_FILE = 'Popup.aspx';
	switch(sMODULE_TYPE)
	{
<%
if ( Security.IsAuthenticated() )
{
	foreach ( DataRowView row in vwModulePopups )
	{
		string sMODULE_NAME   = Sql.ToString(row["MODULE_NAME"  ]);
		string sSINGULAR_NAME = Sql.ToString(row["SINGULAR_NAME"]);
		string sRELATIVE_PATH = Sql.ToString(row["RELATIVE_PATH"]);
		Response.Write("		case '" + sMODULE_NAME + "':  Change" + sSINGULAR_NAME + " = ChangeModule;  ModuleTypePopup('" + sRELATIVE_PATH + "' + sCHANGE_POPUP_FILE, '" + sSINGULAR_NAME + "Popup');  break;\r\n");
	}
}
%>
		default:
			alert('Unknown type. Add ' + sMODULE_TYPE + ' to Include/javascript/ModulePopupScripts.aspx');
			break;
	}
	return false;
}

// 07/27/2010   Add the ability to submit after clear. 
function ClearModuleType(sMODULE_TYPE, sMODULE_ID, sMODULE_NAME, bSUBMIT)
{
	sCHANGE_MODULE_ID   = sMODULE_ID  ;
	sCHANGE_MODULE_NAME = sMODULE_NAME;
	bCHANGE_SUBMIT      = bSUBMIT     ;
	ChangeModule('', '');
	return false;
}


