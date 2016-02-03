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

function USERS_USER_NAME_Changed(fldUSER_NAME)
{
	// 02/04/2007   We need to have an easy way to locate the correct text fields, 
	// so use the current field to determine the label prefix and send that in the userContact field. 
	// 08/24/2009   One of the base controls can contain NAME in the text, so just get the length minus 4. 
	var userContext = fldUSER_NAME.id.substring(0, fldUSER_NAME.id.length - 'USER_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'USER_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_USER_NAME = document.getElementById(userContext + 'PREV_USER_NAME');
	if ( fldPREV_USER_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_USER_NAME');
	}
	else if ( fldPREV_USER_NAME.value != fldUSER_NAME.value )
	{
		if ( fldUSER_NAME.value.length > 0 )
		{
			try
			{
				Taoqi.Users.AutoComplete.USERS_USER_NAME_Get(fldUSER_NAME.value, USERS_USER_NAME_Changed_OnSucceededWithContext, USERS_USER_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('USERS_USER_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010   If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			USERS_USER_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function USERS_USER_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors     = document.getElementById(userContext + 'USER_NAME_AjaxErrors');
		var fldUSER_ID        = document.getElementById(userContext + 'USER_ID'       );
		var fldUSER_NAME      = document.getElementById(userContext + 'USER_NAME'     );
		var fldPREV_USER_NAME = document.getElementById(userContext + 'PREV_USER_NAME');
		if ( fldUSER_ID        != null ) fldUSER_ID.value        = sID  ;
		if ( fldUSER_NAME      != null ) fldUSER_NAME.value      = sNAME;
		if ( fldPREV_USER_NAME != null ) fldPREV_USER_NAME.value = sNAME;
	}
	else
	{
		alert('result from Users.AutoComplete service is null');
	}
}

function USERS_USER_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'USER_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldUSER_ID        = document.getElementById(userContext + 'USER_ID'       );
	var fldUSER_NAME      = document.getElementById(userContext + 'USER_NAME'     );
	var fldPREV_USER_NAME = document.getElementById(userContext + 'PREV_USER_NAME');
	if ( fldUSER_ID        != null ) fldUSER_ID.value        = '';
	if ( fldUSER_NAME      != null ) fldUSER_NAME.value      = '';
	if ( fldPREV_USER_NAME != null ) fldPREV_USER_NAME.value = '';
}

function USERS_ASSIGNED_TO_Changed(fldASSIGNED_TO)
{
	// 02/04/2007   We need to have an easy way to locate the correct text fields, 
	// so use the current field to determine the label prefix and send that in the userContact field. 
	// 08/24/2009   One of the base controls can contain NAME in the text, so just get the length minus 4. 
	var userContext = fldASSIGNED_TO.id.substring(0, fldASSIGNED_TO.id.length - 'ASSIGNED_TO'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'ASSIGNED_TO_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ASSIGNED_TO = document.getElementById(userContext + 'PREV_ASSIGNED_TO');
	if ( fldPREV_ASSIGNED_TO == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ASSIGNED_TO');
	}
	else if ( fldPREV_ASSIGNED_TO.value != fldASSIGNED_TO.value )
	{
		if ( fldASSIGNED_TO.value.length > 0 )
		{
			try
			{
				Taoqi.Users.AutoComplete.USERS_ASSIGNED_TO_Get(fldASSIGNED_TO.value, USERS_ASSIGNED_TO_Changed_OnSucceededWithContext, USERS_ASSIGNED_TO_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('USERS_ASSIGNED_TO_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010   If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			USERS_ASSIGNED_TO_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function USERS_ASSIGNED_TO_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors       = document.getElementById(userContext + 'ASSIGNED_TO_AjaxErrors');
		var fldASSIGNED_USER_ID = document.getElementById(userContext + 'ASSIGNED_USER_ID');
		var fldASSIGNED_TO      = document.getElementById(userContext + 'ASSIGNED_TO'     );
		var fldPREV_ASSIGNED_TO = document.getElementById(userContext + 'PREV_ASSIGNED_TO');
		if ( fldASSIGNED_USER_ID != null ) fldASSIGNED_USER_ID.value = sID  ;
		if ( fldASSIGNED_TO      != null ) fldASSIGNED_TO.value      = sNAME;
		if ( fldPREV_ASSIGNED_TO != null ) fldPREV_ASSIGNED_TO.value = sNAME;
	}
	else
	{
		alert('result from Users.AutoComplete service is null');
	}
}

function USERS_ASSIGNED_TO_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'ASSIGNED_TO_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldASSIGNED_USER_ID = document.getElementById(userContext + 'ASSIGNED_USER_ID');
	var fldASSIGNED_TO      = document.getElementById(userContext + 'ASSIGNED_TO'     );
	var fldPREV_ASSIGNED_TO = document.getElementById(userContext + 'PREV_ASSIGNED_TO');
	if ( fldASSIGNED_USER_ID != null ) fldASSIGNED_USER_ID.value = '';
	if ( fldASSIGNED_TO      != null ) fldASSIGNED_TO.value      = '';
	if ( fldPREV_ASSIGNED_TO != null ) fldPREV_ASSIGNED_TO.value = '';
}

// 08/01/2010   Allow User lookup by FULL NAME. 
function USERS_ASSIGNED_TO_NAME_Changed(fldASSIGNED_TO_NAME)
{
	// 02/04/2007   We need to have an easy way to locate the correct text fields, 
	// so use the current field to determine the label prefix and send that in the userContact field. 
	// 08/24/2009   One of the base controls can contain NAME in the text, so just get the length minus 4. 
	var userContext = fldASSIGNED_TO_NAME.id.substring(0, fldASSIGNED_TO_NAME.id.length - 'ASSIGNED_TO_NAME'.length)
	var fldAjaxErrors = document.getElementById(userContext + 'ASSIGNED_TO_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '';
	
	var fldPREV_ASSIGNED_TO_NAME = document.getElementById(userContext + 'PREV_ASSIGNED_TO_NAME');
	if ( fldPREV_ASSIGNED_TO_NAME == null )
	{
		//alert('Could not find ' + userContext + 'PREV_ASSIGNED_TO_NAME');
	}
	else if ( fldPREV_ASSIGNED_TO_NAME.value != fldASSIGNED_TO_NAME.value )
	{
		if ( fldASSIGNED_TO_NAME.value.length > 0 )
		{
			try
			{
				Taoqi.Users.AutoComplete.USERS_ASSIGNED_TO_NAME_Get(fldASSIGNED_TO_NAME.value, USERS_ASSIGNED_TO_NAME_Changed_OnSucceededWithContext, USERS_ASSIGNED_TO_NAME_Changed_OnFailed, userContext);
			}
			catch(e)
			{
				alert('USERS_ASSIGNED_TO_NAME_Changed: ' + e.message);
			}
		}
		else
		{
			// 08/30/2010   If the name was cleared, then we must also clear the hidden ID field. 
			var result = { 'ID' : '', 'NAME' : '' };
			USERS_ASSIGNED_TO_NAME_Changed_OnSucceededWithContext(result, userContext, null);
		}
	}
}

function USERS_ASSIGNED_TO_NAME_Changed_OnSucceededWithContext(result, userContext, methodName)
{
	if ( result != null )
	{
		var sID   = result.ID  ;
		var sNAME = result.NAME;
		
		var fldAjaxErrors            = document.getElementById(userContext + 'ASSIGNED_TO_NAME_AjaxErrors');
		var fldASSIGNED_USER_ID      = document.getElementById(userContext + 'ASSIGNED_USER_ID'     );
		var fldASSIGNED_TO_NAME      = document.getElementById(userContext + 'ASSIGNED_TO_NAME'     );
		var fldPREV_ASSIGNED_TO_NAME = document.getElementById(userContext + 'PREV_ASSIGNED_TO_NAME');
		if ( fldASSIGNED_USER_ID      != null ) fldASSIGNED_USER_ID.value      = sID  ;
		if ( fldASSIGNED_TO_NAME      != null ) fldASSIGNED_TO_NAME.value      = sNAME;
		if ( fldPREV_ASSIGNED_TO_NAME != null ) fldPREV_ASSIGNED_TO_NAME.value = sNAME;
	}
	else
	{
		alert('result from Users.AutoComplete service is null');
	}
}

function USERS_ASSIGNED_TO_NAME_Changed_OnFailed(error, userContext)
{
	// Display the error.
	var fldAjaxErrors = document.getElementById(userContext + 'ASSIGNED_TO_NAME_AjaxErrors');
	if ( fldAjaxErrors != null )
		fldAjaxErrors.innerHTML = '<br />' + error.get_message();

	var fldASSIGNED_USER_ID = document.getElementById(userContext + 'ASSIGNED_USER_ID');
	var fldASSIGNED_TO_NAME      = document.getElementById(userContext + 'ASSIGNED_TO_NAME'     );
	var fldPREV_ASSIGNED_TO_NAME = document.getElementById(userContext + 'PREV_ASSIGNED_TO_NAME');
	if ( fldASSIGNED_USER_ID != null ) fldASSIGNED_USER_ID.value = '';
	if ( fldASSIGNED_TO_NAME      != null ) fldASSIGNED_TO_NAME.value      = '';
	if ( fldPREV_ASSIGNED_TO_NAME != null ) fldPREV_ASSIGNED_TO_NAME.value = '';
}

if ( typeof(Sys) !== 'undefined' )
	Sys.Application.notifyScriptLoaded();



