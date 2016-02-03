<%@ Control CodeBehind="ListView.ascx.cs" Language="c#" AutoEventWireup="false" Inherits="Taoqi.Calendar.html5.ListView" %>
<script runat="server">
/**********************************************************************************************************************
 * Taoqi is a Customer Relationship Management program created by Taoqi Software, Inc. 
 * Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved.
 * 
 * This program is free software: you can redistribute it and/or modify it under the terms of the 
 * GNU Affero General Public License as published by the Free Software Foundation, either version 3 
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * See the GNU Affero General Public License for more details.
 * 
 * You should have received a copy of the GNU Affero General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>. 
 * 
 * You can contact Taoqi Software, Inc. at email address support@Taoqi.com. 
 * 
 * In accordance with Section 7(b) of the GNU Affero General Public License version 3, 
 * the Appropriate Legal Notices must display the following words on all interactive user interfaces: 
 * "Copyright (C) 2005-2011 Taoqi Software, Inc. All rights reserved."
 *********************************************************************************************************************/
</script>
<div id="divListView">
	<%@ Register TagPrefix="Taoqi" Tagname="ModuleHeader" Src="~/_controls/ModuleHeader.ascx" %>
	<Taoqi:ModuleHeader ID="ctlModuleHeader" Module="Calendar" Title="Calendar.LBL_MODULE_TITLE" EnableModuleLabel="false" EnablePrint="true" HelpName="index" EnableHelp="true" Runat="Server" />
	
	<div id="divError" class="error"></div>
	<div id="divCalendar" style="width: 100%" align="center"></div>
	<Taoqi:InlineScript runat="server">
		<script type="text/javascript">
// 08/25/2013   Move sREMOTE_SERVER definition to the master pages. 
//var sREMOTE_SERVER  = '<%# Application["rootURL"] %>';
var sAUTHENTICATION = '';
var L10n = new Object();
var TERMINOLOGY = new Object();
var TERMINOLOGY_LISTS = new Object();
// 11/06/2013   Make sure to JavaScript escape the text as the various languages may introduce accents. 
TERMINOLOGY['.LBL_NONE'                      ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_NONE"                      )) %>';
TERMINOLOGY['Calendar.LNK_VIEW_CALENDAR'     ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LNK_VIEW_CALENDAR"     )) %>';
TERMINOLOGY['Calendar.LBL_MONTH'             ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_MONTH"             )) %>';
TERMINOLOGY['Calendar.LBL_WEEK'              ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_WEEK"              )) %>';
TERMINOLOGY['Calendar.LBL_DAY'               ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_DAY"               )) %>';
TERMINOLOGY['Calendar.LBL_SHARED'            ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_SHARED"            )) %>';
TERMINOLOGY['Calendar.LBL_ALL_DAY'           ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_ALL_DAY"           )) %>';
TERMINOLOGY['Calendar.YearMonthPattern'      ] = '<%# Sql.EscapeJavaScript(DateTimeFormat.YearMonthPattern             ) %>';
TERMINOLOGY['Calendar.MonthDayPattern'       ] = '<%# Sql.EscapeJavaScript(DateTimeFormat.MonthDayPattern              ) %>';
TERMINOLOGY['Calendar.LongDatePattern'       ] = '<%# Sql.EscapeJavaScript(DateTimeFormat.LongDatePattern              ) %>';
TERMINOLOGY['Calendar.FirstDayOfWeek'        ] = '<%# (int) DateTimeFormat.FirstDayOfWeek       %>';
TERMINOLOGY['Calendar.LNK_NEW_APPOINTMENT'   ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LNK_NEW_APPOINTMENT"   )) %>';
TERMINOLOGY['Calls.LNK_NEW_CALL'             ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LNK_NEW_CALL"             )) %>';
TERMINOLOGY['Calls.LNK_NEW_MEETING'          ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LNK_NEW_MEETING"          )) %>';
TERMINOLOGY['Calls.LBL_SUBJECT'              ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_SUBJECT"              )) %>';
TERMINOLOGY['Calls.LBL_DATE_TIME'            ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_DATE_TIME"            )) %>';
TERMINOLOGY['Calls.LBL_DURATION'             ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_DURATION"             )) %>';
TERMINOLOGY['Calls.LBL_HOURS_MINUTES'        ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_HOURS_MINUTES"        )) %>';
TERMINOLOGY['Calls.LBL_ALL_DAY'              ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_ALL_DAY"              )) %>';
TERMINOLOGY['.LBL_REQUIRED_SYMBOL'           ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_REQUIRED_SYMBOL"           )) %>';
TERMINOLOGY['.ERR_REQUIRED_FIELD'            ] = '<%# Sql.EscapeJavaScript(L10n.Term(".ERR_REQUIRED_FIELD"            )) %>';
TERMINOLOGY['.LBL_SAVE_BUTTON_LABEL'         ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_SAVE_BUTTON_LABEL"         )) %>';
TERMINOLOGY['.LBL_SAVE_BUTTON_TITLE'         ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_SAVE_BUTTON_TITLE"         )) %>';
TERMINOLOGY['.LBL_CANCEL_BUTTON_LABEL'       ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_CANCEL_BUTTON_LABEL"       )) %>';
TERMINOLOGY['.LBL_CANCEL_BUTTON_TITLE'       ] = '<%# Sql.EscapeJavaScript(L10n.Term(".LBL_CANCEL_BUTTON_TITLE"       )) %>';
TERMINOLOGY_LISTS['month_names_dom'          ] = ['<%# String.Join("', '", Sql.EscapeJavaScript(DateTimeFormat.MonthNames           )) %>'];
TERMINOLOGY_LISTS['short_month_names_dom'    ] = ['<%# String.Join("', '", Sql.EscapeJavaScript(DateTimeFormat.AbbreviatedMonthNames)) %>'];
TERMINOLOGY_LISTS['day_names_dom'            ] = ['<%# String.Join("', '", Sql.EscapeJavaScript(DateTimeFormat.DayNames             )) %>'];
TERMINOLOGY_LISTS['short_day_names_dom'      ] = ['<%# String.Join("', '", Sql.EscapeJavaScript(DateTimeFormat.AbbreviatedDayNames  )) %>'];
TERMINOLOGY_LISTS['repeat_type_dom'          ] = ['Daily', 'Weekly', 'Monthly', 'Yearly'];
TERMINOLOGY['.repeat_type_dom.Daily'         ] = '<%# Sql.EscapeJavaScript(L10n.Term(".repeat_type_dom.Daily"         )) %>';
TERMINOLOGY['.repeat_type_dom.Weekly'        ] = '<%# Sql.EscapeJavaScript(L10n.Term(".repeat_type_dom.Weekly"        )) %>';
TERMINOLOGY['.repeat_type_dom.Monthly'       ] = '<%# Sql.EscapeJavaScript(L10n.Term(".repeat_type_dom.Monthly"       )) %>';
TERMINOLOGY['.repeat_type_dom.Yearly'        ] = '<%# Sql.EscapeJavaScript(L10n.Term(".repeat_type_dom.Yearly"        )) %>';

TERMINOLOGY['Calendar.LBL_REPEAT_TAB'        ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_REPEAT_TAB"        )) %>';
TERMINOLOGY['Calendar.LBL_REPEAT_END_AFTER'  ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_REPEAT_END_AFTER"  )) %>';
TERMINOLOGY['Calendar.LBL_REPEAT_OCCURRENCES'] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_REPEAT_OCCURRENCES")) %>';
TERMINOLOGY['Calendar.LBL_REPEAT_INTERVAL'   ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calendar.LBL_REPEAT_INTERVAL"   )) %>';
TERMINOLOGY['Calls.LBL_REPEAT_TYPE'          ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_REPEAT_TYPE"          )) %>';
TERMINOLOGY['Calls.LBL_REPEAT_UNTIL'         ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_REPEAT_UNTIL"         )) %>';
TERMINOLOGY['Calls.LBL_REPEAT_DOW'           ] = '<%# Sql.EscapeJavaScript(L10n.Term("Calls.LBL_REPEAT_DOW"           )) %>';

L10n.Term = function(sTerm)
{
	if ( TERMINOLOGY[sTerm] === undefined )
		return sTerm;
	return TERMINOLOGY[sTerm];
};

L10n.GetList = function(sListName)
{
	if ( TERMINOLOGY_LISTS[sListName] === undefined )
		return sListName;
	return TERMINOLOGY_LISTS[sListName];
};

L10n.GetListTerms = function(sListName)
{
	if ( TERMINOLOGY_LISTS[sListName] === undefined )
		return sListName;
	return TERMINOLOGY_LISTS[sListName];
};
L10n.ListTerm = function(sLIST_NAME, sNAME)
{
	var sEntryName = '.' + sLIST_NAME + '.' + sNAME;
	return L10n.Term(sEntryName);
}

var Security = new Object();
Security.USER_ID = function()
{
	return '<%# Security.USER_ID %>';
};
Security.TEAM_ID = function()
{
	return '<%# Security.TEAM_ID %>';
};
Security.USER_TIME_FORMAT = function()
{
	return '<%# Session["USER_SETTINGS/TIMEFORMAT"] %>';
};
Security.USER_DATE_FORMAT = function()
{
	return '<%# Session["USER_SETTINGS/DATEFORMAT"] %>';
};

var SplendidError = new Object();
SplendidError.FormatError = function(e, method)
{
	return e.message + '<br>\n' + dumpObj(e, method);
};
SplendidError.SystemAlert = function(e, method)
{
	alert(dumpObj(e, method));
};
SplendidError.SystemMessage = function(message)
{
	var divError = document.getElementById('divError');
	divError.innerHTML = message;
};

function DetailViewUI()
{
	this.MODULE  = null;
	this.ID      = null;
}
DetailViewUI.prototype.Load = function(sLayoutPanel, sActionsPanel, sMODULE_NAME, sID, callback)
{
	window.location.href = sREMOTE_SERVER + sMODULE_NAME + '/view.aspx?ID=' + sID;
}

var CONFIG = new Object();
CONFIG['calendar.hour_start'            ] = '<%# Application["CONFIG.calendar.hour_start"            ] %>';
CONFIG['GoogleCalendar.HolidayCalendars'] = '<%# Application["CONFIG.GoogleCalendar.HolidayCalendars"] %>';
var background = new Object();
background.SplendidCache = new Object();
background.SplendidCache.Config = function(sName)
{
	if ( CONFIG[sName] === undefined )
		return null;
	return CONFIG[sName];
};
background.CalendarView_GetCalendar = function(dtDATE_START, dtDATE_END, gASSIGNED_USER_ID, callback, context)
{
	var xhr = CreateSplendidRequest('Rest.svc/GetCalendar?DATE_START=' + encodeURIComponent(dtDATE_START) + '&DATE_END=' + encodeURIComponent(dtDATE_END)  + '&ASSIGNED_USER_ID=' + encodeURIComponent(gASSIGNED_USER_ID), 'GET');
	xhr.onreadystatechange = function()
	{
		if ( xhr.readyState == 4 )
		{
			GetSplendidResult(xhr, function(result)
			{
				try
				{
					if ( result.status == 200 )
					{
						if ( result.d !== undefined )
						{
							callback.call(context||this, 1, result.d.results);
						}
						else
						{
							callback.call(context||this, -1, xhr.responseText);
						}
					}
					else
					{
						if ( result.ExceptionDetail !== undefined )
							callback.call(context||this, -1, result.ExceptionDetail.Message);
						else
							callback.call(context||this, -1, xhr.responseText);
					}
				}
				catch(e)
				{
					callback.call(context||this, -1, SplendidError.FormatError(e, 'CalendarView_GetCalendar'));
				}
			}, context||this);
		}
	}
	try
	{
		xhr.send();
	}
	catch(e)
	{
		if ( e.number != -2146697208 )
			callback.call(context||this, -1, SplendidError.FormatError(e, 'CalendarView_GetCalendar'));
	}
};
background.UpdateModule= function(sMODULE_NAME, row, sID, callback, context)
{
	if ( sMODULE_NAME == null )
	{
		callback.call(context||this, -1, 'UpdateModule: sMODULE_NAME is invalid.');
		return;
	}
	else if ( row == null )
	{
		callback.call(context||this, -1, 'UpdateModule: row is invalid.');
		return;
	}
	var xhr = CreateSplendidRequest('Rest.svc/UpdateModule?ModuleName=' + sMODULE_NAME, 'POST', 'application/octet-stream');
	xhr.onreadystatechange = function()
	{
		if ( xhr.readyState == 4 )
		{
			GetSplendidResult(xhr, function(result)
			{
				try
				{
					if ( result.status == 200 )
					{
						if ( result.d !== undefined )
						{
							sID = result.d;
							callback.call(context||this, 1, sID);
						}
						else
						{
							callback.call(context||this, -1, xhr.responseText);
						}
					}
					else
					{
						if ( result.ExceptionDetail !== undefined )
							callback.call(context||this, -1, result.ExceptionDetail.Message);
						else
							callback.call(context||this, -1, xhr.responseText);
					}
				}
				catch(e)
				{
					callback.call(context||this, -1, SplendidError.FormatError(e, 'UpdateModule'));
				}
			});
		}
	}
	try
	{
		xhr.send(JSON.stringify(row));
	}
	catch(e)
	{
		callback.call(context||this, -1, SplendidError.FormatError(e, 'UpdateModule'));
	}
};

var chrome = new Object();
chrome.extension = new Object();
chrome.extension.getBackgroundPage = function()
{
	return background;
};

function CreateSplendidRequest(sPath, sMethod, sContentType)
{
	// http://www.w3.org/TR/XMLHttpRequest/
	var xhr = null;
	try
	{
		if ( window.XMLHttpRequest )
			xhr = new XMLHttpRequest();
		else if ( window.ActiveXObject )
			xhr = new ActiveXObject("Msxml2.XMLHTTP");
		
		var url = sREMOTE_SERVER + sPath;
		if ( sMethod === undefined )
			sMethod = 'POST';
		if ( sContentType === undefined )
			sContentType = 'application/json; charset=utf-8';
		xhr.open(sMethod, url, true);
		if ( sAUTHENTICATION == 'Basic' )
			xhr.setRequestHeader('Authorization', 'Basic ' + Base64.encode(sUSER_NAME + ':' + sPASSWORD));
		xhr.setRequestHeader('content-type', sContentType);
		// 09/27/2011   Add the URL to the object for debugging purposes. 
		// 10/19/2011   IE6 does not allow this. 
		if ( window.XMLHttpRequest )
		{
			xhr.url    = url;
			xhr.Method = sMethod;
		}
	}
	catch(e)
	{
		SplendidError.SystemAlert(e, 'CreateSplendidRequest');
	}
	return xhr;
}

function GetSplendidResult(xhr, callback, context)
{
	var result = null;
	try
	{
		//alert(dumpObj(xhr, 'xhr.status = ' + xhr.status));
		if ( xhr.responseText.length > 0 )
		{
			result = JSON.parse(xhr.responseText);
			result.status = xhr.status;
			callback.call(context||this, result);
		}
		else if ( xhr.status == 0 || xhr.status == 2 || xhr.status == 12002 || xhr.status == 12007 || xhr.status == 12029 || xhr.status == 12030 || xhr.status == 12031 || xhr.status == 12152 )
		{
		}
		else if ( xhr.status == 405 )
		{
			var sMessage = 'Method Not Allowed.  ' + xhr.url;
			result = { 'status': xhr.status, 'ExceptionDetail': { 'status': xhr.status, 'Message': sMessage } };
			callback.call(context||this, result);
		}
		else
		{
			result = { 'status': xhr.status, 'ExceptionDetail': { 'status': xhr.status, 'Message': xhr.statusText + '(' + xhr.status + ')' } };
			callback.call(context||this, result);
		}
	}
	catch(e)
	{
		SplendidError.SystemAlert(e, 'GetSplendidResult');
		callback.call(context||this, result);
	}
}

$(document).ready(function()
{
	var oCalendarViewUI = new CalendarViewUI();
	oCalendarViewUI.Render(null, null, function(status, message)
	{
	}, oCalendarViewUI);
});
		</script>
	</Taoqi:InlineScript>
</div>


