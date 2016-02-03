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

function Taoqi_ChangeFavorites(fld, sMODULE, gID)
{
	var fldAdd = document.getElementsByName('favAdd_' + gID);
	var fldRem = document.getElementsByName('favRem_' + gID);
	if ( fldAdd[0].style.display == 'none' )
		Taoqi_RemoveFromFavorites(fld, sMODULE, gID);
	else
		Taoqi_AddToFavorites(fld, sMODULE, gID);
}

function Taoqi_AddToFavorites(fld, sMODULE, gID)
{
	var userContext = gID;
	try
	{
		Taoqi.Utilities.Modules.AddToFavorites(sMODULE, gID, Taoqi_AddToFavorites_OnSucceededWithContext, Taoqi_AddToFavorites_OnFailed, userContext);
	}
	catch(e)
	{
		alert('Taoqi_AddToFavorites: ' + e.message);
	}
	return false;
}

function Taoqi_AddToFavorites_OnSucceededWithContext(result, userContext)
{
	if ( result )
	{
		var fldAdd = document.getElementsByName('favAdd_' + userContext);
		var fldRem = document.getElementsByName('favRem_' + userContext);
		fldAdd[0].style.display = 'none'  ;
		fldRem[0].style.display = 'inline';
	}
}

function Taoqi_AddToFavorites_OnFailed(error, userContext)
{
	alert('Taoqi_AddToFavorites_OnFailed: ' + error.Message);
}

function Taoqi_RemoveFromFavorites(fld, sMODULE, gID)
{
	var userContext = gID;
	try
	{
		Taoqi.Utilities.Modules.RemoveFromFavorites(sMODULE, gID, Taoqi_RemoveFromFavorites_OnSucceededWithContext, Taoqi_RemoveFromFavorites_OnFailed, userContext);
	}
	catch(e)
	{
		alert('Taoqi_RemoveFromFavorites: ' + e.message);
	}
	return false;
}

function Taoqi_RemoveFromFavorites_OnSucceededWithContext(result, userContext)
{
	if ( result )
	{
		var fldAdd = document.getElementsByName('favAdd_' + userContext);
		var fldRem = document.getElementsByName('favRem_' + userContext);
		fldAdd[0].style.display = 'inline';
		fldRem[0].style.display = 'none'  ;
	}
}

function Taoqi_RemoveFromFavorites_OnFailed(error, userContext)
{
	alert('Taoqi_RemoveFromFavorites_OnFailed: ' + error.Message);
}

if ( typeof(Sys) !== 'undefined' )
	Sys.Application.notifyScriptLoaded();



