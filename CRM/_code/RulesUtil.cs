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
using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Workflow.Activities.Rules;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Diagnostics;
// 09/18/2011   Upgrade to CKEditor 3.6.2. 
using CKEditor.NET;

namespace Taoqi
{
	// 03/11/2014   Provide a way to control the dynamic buttons. 
	public class SafeDynamicButtons
	{
		protected _controls.DynamicButtons ctlDynamicButtons;
		protected SplendidControl ctlPARENT ;
		protected DataRow         rowCurrent;

		public SafeDynamicButtons(SplendidControl ctlPARENT, DataRow row)
		{
			this.ctlPARENT  = ctlPARENT ;
			this.rowCurrent = row       ;
			this.ctlDynamicButtons = ctlPARENT.FindControl("ctlDynamicButtons") as _controls.DynamicButtons;
		}

		public SafeDynamicButtons(SplendidControl ctlPARENT, string sNAME, DataRow row)
		{
			this.ctlPARENT  = ctlPARENT ;
			this.rowCurrent = row       ;
			this.ctlDynamicButtons = ctlPARENT.FindControl(sNAME) as _controls.DynamicButtons;
		}

		public void DisableAll()
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.DisableAll();
		}

		public void HideAll()
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.HideAll();
		}

		public void ShowAll()
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.ShowAll();
		}

		public void ShowButton(string sCommandName, bool bVisible)
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.ShowButton(sCommandName, bVisible);
		}

		public void EnableButton(string sCommandName, bool bEnabled)
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.EnableButton(sCommandName, bEnabled);
		}

		public void SetButtonText(string sCommandName, string sText)
		{
			if ( this.ctlDynamicButtons != null )
				this.ctlDynamicButtons.SetButtonText(sCommandName, sText);
		}

		public bool ShowRequired
		{
			get
			{
				if ( this.ctlDynamicButtons != null )
					return this.ctlDynamicButtons.ShowRequired;
				else
					return false;
			}
			set
			{
				if ( this.ctlDynamicButtons != null )
					this.ctlDynamicButtons.ShowRequired = value;
			}
		}

		public bool ShowError
		{
			get
			{
				if ( this.ctlDynamicButtons != null )
					return this.ctlDynamicButtons.ShowError;
				else
					return false;
			}
			set
			{
				if ( this.ctlDynamicButtons != null )
					this.ctlDynamicButtons.ShowError = value;
			}
		}

		public string ErrorText
		{
			get
			{
				if ( this.ctlDynamicButtons != null )
					return this.ctlDynamicButtons.ErrorText;
				else
					return String.Empty;
			}
			set
			{
				if ( this.ctlDynamicButtons != null )
					this.ctlDynamicButtons.ErrorText = value;
			}
		}

		public string ErrorClass
		{
			get
			{
				if ( this.ctlDynamicButtons != null )
					return this.ctlDynamicButtons.ErrorClass;
				else
					return String.Empty;
			}
			set
			{
				if ( this.ctlDynamicButtons != null )
					this.ctlDynamicButtons.ErrorClass = value;
			}
		}

	}

	// 11/10/2010   Make sure to add the RulesValidator early in the pipeline. 
	public class RulesValidator : IValidator
	{
		protected SplendidControl Container;

		public RulesValidator(SplendidControl Container)
		{
			this.Container = Container;
		}

		// 11/10/2010   We can return the error, but it does not get displayed because we do not have a summary control. 
		public string ErrorMessage
		{
			get { return Container.RulesErrorMessage; }
			set { Container.RulesErrorMessage = value; }
		}

		public bool IsValid
		{
			get { return Container.RulesIsValid; }
			set { Container.RulesIsValid = value; }
		}

		public void Validate()
		{
		}
	}

	public class SplendidControlThis : SqlObj
	{
		private SplendidControl Container;
		private L10N            L10n     ;
		private DataRow         Row      ;
		private DataTable       Table    ;
		private string          Module   ;
		
		public SplendidControlThis(SplendidControl Container, string sModule, DataRow Row)
		{
			this.Container = Container;
			this.Module    = sModule  ;
			this.Row       = Row      ;
			if ( Row != null )
				this.Table = Row.Table;
			this.L10n      = Container.GetL10n();
		}

		public SplendidControlThis(SplendidControl Container, string sModule, DataTable Table)
		{
			this.Container = Container;
			this.Module    = sModule  ;
			this.Table     = Table    ;
			this.L10n      = Container.GetL10n();
		}

		public object this[string columnName]
		{
			get
			{
				if ( Row != null )
					return Row[columnName];
				return null;
			}
			set
			{
				if ( Row != null )
					Row[columnName] = value;
			}
		}

		// 02/15/2014   Provide access to the Request object so that we can determine if the record is new. 
		public HttpRequest Request
		{
			get
			{
				return HttpContext.Current.Request;
			}
		}

		public void AddColumn(string columnName, string typeName)
		{
			if ( Table != null )
			{
				if ( !Table.Columns.Contains(columnName) )
				{
					if ( Sql.IsEmptyString(typeName) )
						Table.Columns.Add(columnName);
					else
						Table.Columns.Add(columnName, Type.GetType(typeName));
				}
			}
		}

		// http://msdn.microsoft.com/en-us/library/system.data.datacolumn.expression(v=VS.80).aspx
		public void AddColumnExpression(string columnName, string typeName, string sExpression)
		{
			if ( Table != null )
			{
				if ( !Table.Columns.Contains(columnName) )
				{
					Table.Columns.Add(columnName, Type.GetType(typeName), sExpression);
				}
			}
		}

		public DynamicControl GetDynamicControl(string columnName)
		{
			return new DynamicControl(Container, columnName);
		}

		// 03/11/2014   Provide a way to control the dynamic buttons. 
		public SafeDynamicButtons GetDynamicButtons()
		{
			return new SafeDynamicButtons(this.Container, this.Row);
		}

		public SafeDynamicButtons GetDynamicButtons(string sName)
		{
			return new SafeDynamicButtons(this.Container, sName, this.Row);
		}

		public string ListTerm(string sListName, string oField)
		{
			return Sql.ToString(L10n.Term(sListName, oField));
		}

		public string Term(string sEntryName)
		{
			return L10n.Term(sEntryName);
		}

		public string RedirectURL
		{
			get { return Container.RulesRedirectURL; }
			set { Container.RulesRedirectURL = value; }
		}

		public string ErrorMessage
		{
			get { return Container.RulesErrorMessage; }
			set { Container.RulesErrorMessage = value; }
		}

		public bool IsValid
		{
			get { return Container.RulesIsValid; }
			set { Container.RulesIsValid = value; }
		}

		// 11/14/2013   A customer wants to hide a row if it matches a certain criteria. 
		public void Delete()
		{
			if ( Row != null )
			{
				Row.Delete();
			}
		}

		// 02/13/2013   Allow the business rules to change the layout. 
		public string LayoutListView
		{
			get { return Container.LayoutListView; }
			set { Container.LayoutListView = value; }
		}

		public string LayoutEditView
		{
			get { return Container.LayoutEditView; }
			set { Container.LayoutEditView = value; }
		}

		public string LayoutDetailView
		{
			get { return Container.LayoutDetailView; }
			set { Container.LayoutDetailView = value; }
		}

		// 11/10/2010   Throwing an exception will be the preferred method of displaying an error. 
		public void Throw(string sMessage)
		{
			throw(new Exception(sMessage));
		}

		public bool UserIsAdmin()
		{
			return Taoqi.Security.isAdmin;
		}

		public int UserModuleAccess(string sACCESS_TYPE)
		{
			return Taoqi.Security.GetUserAccess(Module, sACCESS_TYPE);
		}

		public bool UserRoleAccess(string sROLE_NAME)
		{
			return Security.GetACLRoleAccess(sROLE_NAME);
		}

		public bool UserTeamAccess(string sTEAM_NAME)
		{
			return Security.GetTeamAccess(sTEAM_NAME);
		}

		public bool UserFieldIsReadable(string sFIELD_NAME, Guid gASSIGNED_USER_ID)
		{
			Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, sFIELD_NAME, gASSIGNED_USER_ID);
			return acl.IsReadable();
		}

		public bool UserFieldIsWriteable(string sFIELD_NAME, Guid gASSIGNED_USER_ID)
		{
			Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, sFIELD_NAME, gASSIGNED_USER_ID);
			return acl.IsWriteable();
		}

		public void LayoutShowButton(string sCommandName, bool bVisible)
		{
			_controls.DynamicButtons ctlDynamicButtons = Container.FindControl("ctlDynamicButtons") as _controls.DynamicButtons;
			_controls.DynamicButtons ctlFooterButtons  = Container.FindControl("ctlFooterButtons" ) as _controls.DynamicButtons;
			if ( ctlDynamicButtons != null )
				ctlDynamicButtons.ShowButton(sCommandName, bVisible);
			if ( ctlFooterButtons != null )
				ctlFooterButtons .ShowButton(sCommandName, bVisible);
		}

		public void LayoutEnableButton(string sCommandName, bool bEnabled)
		{
			_controls.DynamicButtons ctlDynamicButtons = Container.FindControl("ctlDynamicButtons") as _controls.DynamicButtons;
			_controls.DynamicButtons ctlFooterButtons  = Container.FindControl("ctlFooterButtons" ) as _controls.DynamicButtons;
			if ( ctlDynamicButtons != null )
				ctlDynamicButtons.EnableButton(sCommandName, bEnabled);
			if ( ctlFooterButtons != null )
				ctlFooterButtons .EnableButton(sCommandName, bEnabled);
		}

		public void LayoutShowField(string sDATA_FIELD, bool bVisible)
		{
			Control ctl = Container.FindControl(sDATA_FIELD);
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_LABEL");
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_PARENT_TYPE");
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_btnChange");
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_btnClear");
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_TOOLTIP_IMAGE");
			if ( ctl != null )
				ctl.Visible = bVisible;
			ctl = Container.FindControl(sDATA_FIELD + "_TOOLTIP_PANEL");
			if ( ctl != null )
				ctl.Visible = bVisible;
		}

		public void LayoutEnableField(string sDATA_FIELD, bool bEnabled)
		{
			Control ctl = Container.FindControl(sDATA_FIELD);
			if ( ctl != null )
			{
				if ( ctl is TextBox )
					(ctl as TextBox).Enabled = bEnabled;
				// 11/11/2010   The FCKeditor cannot be disabled, so just hide. 
				// 09/18/2011   Upgrade to CKEditor 3.6.2. 
				else if ( ctl is CKEditorControl )
					(ctl as CKEditorControl).Visible = bEnabled;
				else if ( ctl is ListControl )
					(ctl as ListControl).Enabled = bEnabled;
				else if ( ctl is CheckBox )
					(ctl as CheckBox).Enabled = bEnabled;
				else if ( ctl is HtmlInputButton )
					(ctl as HtmlInputButton).Disabled = !bEnabled;
				else if ( ctl is HtmlInputFile )
					(ctl as HtmlInputFile).Disabled = !bEnabled;
				else if ( ctl is _controls.DatePicker )
					(ctl as _controls.DatePicker).Enabled = bEnabled;
				else if ( ctl is _controls.DateTimePicker )
					(ctl as _controls.DateTimePicker).Enabled = bEnabled;
				else if ( ctl is _controls.TimePicker )
					(ctl as _controls.TimePicker).Enabled = bEnabled;
				else if ( ctl is _controls.DateTimeEdit )
					(ctl as _controls.DateTimeEdit).Enabled = bEnabled;
				else if ( ctl is _controls.TeamSelect )
					(ctl as _controls.TeamSelect).Enabled = bEnabled;
				else if ( ctl is _controls.KBTagSelect )
					(ctl as _controls.KBTagSelect).Enabled = bEnabled;
			}
		}

		public void LayoutRequiredField(string sDATA_FIELD, bool bRequired)
		{
			Control req = Container.FindControl(sDATA_FIELD + "_REQUIRED");
			if ( req != null )
			{
				if ( req is RequiredFieldValidator )
					(req as RequiredFieldValidator).Enabled = bRequired;
				else if ( req is RequiredFieldValidatorForHiddenInputs )
					(req as RequiredFieldValidatorForHiddenInputs).Enabled = bRequired;
			}
			else
			{
				Control ctl = Container.FindControl(sDATA_FIELD);
				if ( ctl != null )
				{
					if ( ctl is _controls.DatePicker )
						(ctl as _controls.DatePicker).Validate(bRequired);
					else if ( ctl is _controls.DateTimePicker )
						(ctl as _controls.DateTimePicker).Validate(bRequired);
					else if ( ctl is _controls.TimePicker )
						(ctl as _controls.TimePicker).Validate(bRequired);
					else if ( ctl is _controls.DateTimeEdit )
						(ctl as _controls.DateTimeEdit).Validate(bRequired);
					else if ( ctl is _controls.TeamSelect )
						(ctl as _controls.TeamSelect).Validate(bRequired);
					else if ( ctl is _controls.KBTagSelect )
						(ctl as _controls.KBTagSelect).Validate(bRequired);
				}
			}
		}

		// 07/05/2012   Provide access to the current user. 
		public Guid USER_ID()
		{
			return Taoqi.Security.USER_ID;
		}

		public string USER_NAME()
		{
			return Taoqi.Security.USER_NAME;
		}

		public string FULL_NAME()
		{
			return Taoqi.Security.FULL_NAME;
		}

		public Guid TEAM_ID()
		{
			return Taoqi.Security.TEAM_ID;
		}

		public string TEAM_NAME()
		{
			return Taoqi.Security.TEAM_NAME;
		}

		// 05/12/2013   Provide a way to decrypt inside a business rule.  
		// The business rules do not have access to the config variables, so the Guid values will need to be hard-coded in the rule. 
		public string DecryptPassword(string sPASSWORD, Guid gKEY, Guid gIV)
		{
			return Taoqi.Security.DecryptPassword(sPASSWORD, gKEY, gIV);
		}
	}

	public class SplendidWizardThis : SqlObj
	{
		private L10N            L10n             ;
		private DataRow         Row              ;
		private string          Module           ;
		private Guid            gASSIGNED_USER_ID;
		
		public SplendidWizardThis(L10N L10n, string sModule, DataRow Row)
		{
			this.L10n              = L10n      ;
			this.Row               = Row       ;
			this.Module            = sModule   ;
			this.gASSIGNED_USER_ID = Guid.Empty;
			if ( Row.Table != null && Row.Table.Columns.Contains("ASSIGNED_USER_ID") )
				gASSIGNED_USER_ID = Sql.ToGuid(Row["ASSIGNED_USER_ID"]);
		}
		
		public object this[string columnName]
		{
			get
			{
				bool bIsReadable  = true;
				if ( SplendidInit.bEnableACLFieldSecurity && !Sql.IsEmptyString(columnName) )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsReadable  = acl.IsReadable();
				}
				if ( bIsReadable )
					return Row[columnName];
				else
					return DBNull.Value;
			}
			set
			{
				bool bIsWriteable = true;
				if ( SplendidInit.bEnableACLFieldSecurity )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsWriteable = acl.IsWriteable();
				}
				if ( bIsWriteable )
					Row[columnName] = value;
			}
		}

		public string ListTerm(string sListName, string oField)
		{
			return Sql.ToString(L10n.Term(sListName, oField));
		}

		public string Term(string sEntryName)
		{
			return L10n.Term(sEntryName);
		}

		// 07/05/2012   Provide access to the current user. 
		public Guid USER_ID()
		{
			return Taoqi.Security.USER_ID;
		}

		public string USER_NAME()
		{
			return Taoqi.Security.USER_NAME;
		}

		public string FULL_NAME()
		{
			return Taoqi.Security.FULL_NAME;
		}

		public Guid TEAM_ID()
		{
			return Taoqi.Security.TEAM_ID;
		}

		public string TEAM_NAME()
		{
			return Taoqi.Security.TEAM_NAME;
		}
	}

	// 09/17/2013   Add Business Rules to import. 
	public class SplendidImportThis : SqlObj
	{
		private L10N            L10n             ;
		private DataRow         Row              ;
		private IDbCommand      Import           ;
		private IDbCommand      ImportCSTM       ;
		private string          Module           ;
		private Guid            gASSIGNED_USER_ID;
		
		public SplendidImportThis(L10N L10n, string sModule, DataRow Row, IDbCommand cmdImport, IDbCommand cmdImportCSTM)
		{
			this.L10n              = L10n         ;
			this.Row               = Row          ;
			this.Import            = cmdImport    ;
			this.ImportCSTM        = cmdImportCSTM;
			this.Module            = sModule      ;
			this.gASSIGNED_USER_ID = Guid.Empty   ;
			
			IDbDataParameter par = Sql.FindParameter(cmdImport, "ASSIGNED_USER_ID");
			if ( par != null )
				gASSIGNED_USER_ID = Sql.ToGuid(par.Value);
		}
		
		public object this[string columnName]
		{
			get
			{
				bool bIsReadable  = true;
				if ( SplendidInit.bEnableACLFieldSecurity && !Sql.IsEmptyString(columnName) )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsReadable  = acl.IsReadable();
				}
				if ( bIsReadable )
				{
					IDbDataParameter par = Sql.FindParameter(Import, columnName);
					if ( par != null )
					{
						return par.Value;
					}
					else if ( ImportCSTM != null )
					{
						par = Sql.FindParameter(ImportCSTM, columnName);
						if ( par != null )
							return par.Value;
					}
				}
				return DBNull.Value;
			}
			set
			{
				bool bIsWriteable = true;
				if ( SplendidInit.bEnableACLFieldSecurity )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsWriteable = acl.IsWriteable();
				}
				if ( bIsWriteable )
				{
					IDbDataParameter par = Sql.FindParameter(Import, columnName);
					if ( par != null )
					{
						Sql.SetParameter(par, value);
					}
					if ( ImportCSTM != null )
					{
						// 09/17/2013   If setting the ID, then also set the related custom field ID. 
						if ( String.Compare(columnName, "ID", true) == 0 )
							columnName = "ID_C";
						par = Sql.FindParameter(ImportCSTM, columnName);
						if ( par != null )
							Sql.SetParameter(par, value);
					}
					// 09/17/2013   The Row is displayed in the Results tab while the parameters are used to update the database. 
					Row[columnName] = value;
				}
			}
		}

		public string ListTerm(string sListName, string oField)
		{
			return Sql.ToString(L10n.Term(sListName, oField));
		}

		public string Term(string sEntryName)
		{
			return L10n.Term(sEntryName);
		}

		// 07/05/2012   Provide access to the current user. 
		public Guid USER_ID()
		{
			return Taoqi.Security.USER_ID;
		}

		public string USER_NAME()
		{
			return Taoqi.Security.USER_NAME;
		}

		public string FULL_NAME()
		{
			return Taoqi.Security.FULL_NAME;
		}

		public Guid TEAM_ID()
		{
			return Taoqi.Security.TEAM_ID;
		}

		public string TEAM_NAME()
		{
			return Taoqi.Security.TEAM_NAME;
		}
	}

	public class SplendidReportThis : SqlObj
	{
		private HttpApplicationState Application ;
		private L10N            L10n             ;
		private DataRow         Row              ;
		private DataTable       Table            ;
		private string          Module           ;
		private Guid            gASSIGNED_USER_ID;
		
		public SplendidReportThis(HttpApplicationState Application, L10N L10n, string sModule, DataRow Row)
		{
			this.Application       = Application;
			this.L10n              = L10n       ;
			this.Module            = sModule    ;
			this.Row               = Row        ;
			this.gASSIGNED_USER_ID = Guid.Empty ;
			if ( Row != null )
			{
				this.Table = Row.Table;
				if ( Table != null && Table.Columns.Contains("ASSIGNED_USER_ID") )
					gASSIGNED_USER_ID = Sql.ToGuid(Row["ASSIGNED_USER_ID"]);
			}
		}

		public SplendidReportThis(HttpApplicationState Application, L10N L10n, string sModule, DataTable Table)
		{
			this.Application       = Application;
			this.L10n              = L10n       ;
			this.Module            = sModule    ;
			this.Table             = Table      ;
			this.gASSIGNED_USER_ID = Guid.Empty ;
		}

		public object this[string columnName]
		{
			get
			{
				bool bIsReadable  = true;
				if ( SplendidInit.bEnableACLFieldSecurity && !Sql.IsEmptyString(columnName) )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsReadable  = acl.IsReadable();
				}
				if ( bIsReadable )
					return Row[columnName];
				else
					return DBNull.Value;
			}
			set
			{
				bool bIsWriteable = true;
				if ( SplendidInit.bEnableACLFieldSecurity )
				{
					Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, columnName, gASSIGNED_USER_ID);
					bIsWriteable = acl.IsWriteable();
				}
				if ( bIsWriteable )
					Row[columnName] = value;
			}
		}

		public void AddColumn(string columnName, string typeName)
		{
			if ( Table != null )
			{
				if ( !Table.Columns.Contains(columnName) )
				{
					if ( Sql.IsEmptyString(typeName) )
						Table.Columns.Add(columnName);
					else
						Table.Columns.Add(columnName, Type.GetType(typeName));
				}
			}
		}

		// http://msdn.microsoft.com/en-us/library/system.data.datacolumn.expression(v=VS.80).aspx
		public void AddColumnExpression(string columnName, string typeName, string sExpression)
		{
			if ( Table != null )
			{
				if ( !Table.Columns.Contains(columnName) )
				{
					Table.Columns.Add(columnName, Type.GetType(typeName), sExpression);
				}
			}
		}

		public string ListTerm(string sListName, string oField)
		{
			// 12/04/2010   We need to use the static version of Term as a report can get rendered inside a workflow, which has issues accessing the context. 
			//return Sql.ToString(L10n.Term(sListName, oField));
			return Sql.ToString(L10N.Term(Application, L10n.NAME, sListName, oField));
		}

		public string Term(string sEntryName)
		{
			// 12/04/2010   We need to use the static version of Term as a report can get rendered inside a workflow, which has issues accessing the context. 
			//return L10n.Term(sEntryName);
			return L10N.Term(Application, L10n.NAME, sEntryName);
		}

		// 11/10/2010   Throwing an exception will be the preferred method of displaying an error. 
		public void Throw(string sMessage)
		{
			throw(new Exception(sMessage));
		}

		public bool UserIsAdmin()
		{
			return Taoqi.Security.isAdmin;
		}

		public int UserModuleAccess(string sACCESS_TYPE)
		{
			return Taoqi.Security.GetUserAccess(Module, sACCESS_TYPE);
		}

		public bool UserRoleAccess(string sROLE_NAME)
		{
			return Security.GetACLRoleAccess(sROLE_NAME);
		}

		public bool UserTeamAccess(string sTEAM_NAME)
		{
			return Security.GetTeamAccess(sTEAM_NAME);
		}

		public bool UserFieldIsReadable(string sFIELD_NAME, Guid gASSIGNED_USER_ID)
		{
			Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, sFIELD_NAME, gASSIGNED_USER_ID);
			return acl.IsReadable();
		}

		public bool UserFieldIsWriteable(string sFIELD_NAME, Guid gASSIGNED_USER_ID)
		{
			Security.ACL_FIELD_ACCESS acl = Security.GetUserFieldSecurity(Module, sFIELD_NAME, gASSIGNED_USER_ID);
			return acl.IsWriteable();
		}

		// 07/05/2012   Provide access to the current user. 
		public Guid USER_ID()
		{
			return Taoqi.Security.USER_ID;
		}

		public string USER_NAME()
		{
			return Taoqi.Security.USER_NAME;
		}

		public string FULL_NAME()
		{
			return Taoqi.Security.FULL_NAME;
		}

		public Guid TEAM_ID()
		{
			return Taoqi.Security.TEAM_ID;
		}

		public string TEAM_NAME()
		{
			return Taoqi.Security.TEAM_NAME;
		}
	}

	// 12/12/2012   For security reasons, we want to restrict the data types available to the rules wizard. 
	// http://www.codeproject.com/Articles/12675/How-to-reuse-the-Windows-Workflow-Foundation-WF-co
	// 09/16/2013   ITypeProvider is obsolete in .NET 4.5, but we have not found the alternative. 
#pragma warning disable 618
	public class SplendidRulesTypeProvider : System.Workflow.ComponentModel.Compiler.ITypeProvider
	{
		public event EventHandler TypeLoadErrorsChanged;
		public event EventHandler TypesChanged         ;
		private Dictionary<string, Type> availableTypes;
		private Dictionary<object, Exception> typeErrors;
		private List<System.Reflection.Assembly> availableAssemblies;

		public SplendidRulesTypeProvider()
		{
			typeErrors     = new Dictionary<object, Exception>();
			availableAssemblies = new List<System.Reflection.Assembly>();
			availableAssemblies.Add(this.GetType().Assembly);
			
			availableTypes = new Dictionary<string, Type>();
			availableTypes.Add(typeof(System.Boolean ).FullName, typeof(System.Boolean ));
			availableTypes.Add(typeof(System.Byte    ).FullName, typeof(System.Byte    ));
			availableTypes.Add(typeof(System.Char    ).FullName, typeof(System.Char    ));
			availableTypes.Add(typeof(System.DateTime).FullName, typeof(System.DateTime));
			availableTypes.Add(typeof(System.Decimal ).FullName, typeof(System.Decimal ));
			availableTypes.Add(typeof(System.Double  ).FullName, typeof(System.Double  ));
			availableTypes.Add(typeof(System.Guid    ).FullName, typeof(System.Guid    ));
			availableTypes.Add(typeof(System.Int16   ).FullName, typeof(System.Int16   ));
			availableTypes.Add(typeof(System.Int32   ).FullName, typeof(System.Int32   ));
			availableTypes.Add(typeof(System.Int64   ).FullName, typeof(System.Int64   ));
			availableTypes.Add(typeof(System.SByte   ).FullName, typeof(System.SByte   ));
			availableTypes.Add(typeof(System.Single  ).FullName, typeof(System.Single  ));
			availableTypes.Add(typeof(System.String  ).FullName, typeof(System.String  ));
			availableTypes.Add(typeof(System.TimeSpan).FullName, typeof(System.TimeSpan));
			availableTypes.Add(typeof(System.UInt16  ).FullName, typeof(System.UInt16  ));
			availableTypes.Add(typeof(System.UInt32  ).FullName, typeof(System.UInt32  ));
			availableTypes.Add(typeof(System.UInt64  ).FullName, typeof(System.UInt64  ));
			availableTypes.Add(typeof(System.DBNull  ).FullName, typeof(System.DBNull  ));
			// 03/11/2014   Provide a way to control the dynamic buttons. 
			availableTypes.Add(typeof(SafeDynamicButtons).FullName, typeof(SafeDynamicButtons));
			// 12/12/2012   Use TypesChanged to avoid a compiler warning; 
			if ( TypesChanged != null )
				TypesChanged(this, null);
		}

		public Type GetType(string name, bool throwOnError)
		{
			if ( String.IsNullOrEmpty(name) )
			{
				return null;
			}

			if ( availableTypes.ContainsKey(name) )
			{
				Type type = availableTypes[name];
				return type;
			}
			else
			{
				if ( !typeErrors.ContainsKey(name) )
				{
					typeErrors.Add(name, new Exception("SplendidRulesTypeProvider: " + name + " is not a supported data type. "));
				}
				if ( throwOnError )
				{
					throw new TypeLoadException();
				}
				else
				{
					if ( TypeLoadErrorsChanged != null )
					{
						try
						{
							EventArgs args = new EventArgs();
							TypeLoadErrorsChanged(this, args);
						}
						catch
						{
						}
					}
					return null;
				}
			}
		}

		public Type GetType(string name)
		{
			return GetType(name, false);
		}

		public Type[] GetTypes() 
		{
			Type[] result = new Type[availableTypes.Count];
			availableTypes.Values.CopyTo(result, 0);
			return result;
		}

		public System.Reflection.Assembly LocalAssembly
		{
			get { return this.GetType().Assembly; }
		}

		public IDictionary<object, Exception> TypeLoadErrors
		{
			get { return typeErrors; }
		}

		public ICollection<System.Reflection.Assembly> ReferencedAssemblies
		{
			get { return availableAssemblies; }
		}
	}
#pragma warning restore 618

	/// <summary>
	/// Summary description for RulesUtil.
	/// </summary>
	public class RulesUtil
	{
		public static RuleSet Deserialize(string sXOML)
		{
			RuleSet rules = null;
			using ( StringReader stm = new StringReader(sXOML) )
			{
				using ( XmlTextReader xrdr = new XmlTextReader(stm) )
				{
					WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
					rules = (RuleSet) serializer.Deserialize(xrdr);
				}
			}
			return rules;
		}

		public static string Serialize(RuleSet rules)
		{
			StringBuilder sbXOML = new StringBuilder();
			using ( StringWriter wtr = new StringWriter(sbXOML, System.Globalization.CultureInfo.InvariantCulture) )
			{
				using ( XmlTextWriter xwtr = new XmlTextWriter(wtr) )
				{
					WorkflowMarkupSerializer serializer = new WorkflowMarkupSerializer();
					serializer.Serialize(xwtr, rules);
				}
			}
			return sbXOML.ToString();
		}

		// 12/12/2012   For security reasons, we want to restrict the data types available to the rules wizard. 
		public static void RulesValidate(Guid gID, string sRULE_NAME, int nPRIORITY, string sREEVALUATION, bool bACTIVE, string sCONDITION, string sTHEN_ACTIONS, string sELSE_ACTIONS, Type thisType, SplendidRulesTypeProvider typeProvider)
		{
			RuleSet        rules      = new RuleSet("RuleSet 1");
			RuleValidation validation = new RuleValidation(thisType, typeProvider);
			RulesParser    parser     = new RulesParser(validation);
			RuleExpressionCondition condition      = parser.ParseCondition    (sCONDITION   );
			List<RuleAction>        lstThenActions = parser.ParseStatementList(sTHEN_ACTIONS);
			List<RuleAction>        lstElseActions = parser.ParseStatementList(sELSE_ACTIONS);

			System.Workflow.Activities.Rules.Rule r = new System.Workflow.Activities.Rules.Rule(sRULE_NAME, condition, lstThenActions, lstElseActions);
			r.Priority = nPRIORITY;
			r.Active   = bACTIVE  ;
			//r.ReevaluationBehavior = (RuleReevaluationBehavior) Enum.Parse(typeof(RuleReevaluationBehavior), sREEVALUATION);
			// 12/04/2010   Play it safe and never-reevaluate. 
			r.ReevaluationBehavior = RuleReevaluationBehavior.Never;
			rules.Rules.Add(r);
			rules.Validate(validation);
			if ( validation.Errors.HasErrors )
			{
				throw(new Exception(GetValidationErrors(validation)));
			}
		}

		public static string GetValidationErrors(RuleValidation validation)
		{
			StringBuilder sbErrors = new StringBuilder();
			foreach ( ValidationError err in validation.Errors )
			{
				sbErrors.AppendLine(err.ErrorText);
			}
			return sbErrors.ToString();
		}

		public static RuleSet BuildRuleSet(DataTable dtRules, RuleValidation validation)
		{
			RuleSet        rules = new RuleSet("RuleSet 1");
			RulesParser    parser = new RulesParser(validation);

			DataView vwRules = new DataView(dtRules);
			vwRules.RowFilter = "ACTIVE = 1";
			vwRules.Sort      = "PRIORITY asc";
			foreach ( DataRowView row in vwRules )
			{
				string sRULE_NAME    = Sql.ToString (row["RULE_NAME"   ]);
				int    nPRIORITY     = Sql.ToInteger(row["PRIORITY"    ]);
				string sREEVALUATION = Sql.ToString (row["REEVALUATION"]);
				bool   bACTIVE       = Sql.ToBoolean(row["ACTIVE"      ]);
				string sCONDITION    = Sql.ToString (row["CONDITION"   ]);
				string sTHEN_ACTIONS = Sql.ToString (row["THEN_ACTIONS"]);
				string sELSE_ACTIONS = Sql.ToString (row["ELSE_ACTIONS"]);
				
				RuleExpressionCondition condition      = parser.ParseCondition    (sCONDITION   );
				List<RuleAction>        lstThenActions = parser.ParseStatementList(sTHEN_ACTIONS);
				List<RuleAction>        lstElseActions = parser.ParseStatementList(sELSE_ACTIONS);
				System.Workflow.Activities.Rules.Rule r = new System.Workflow.Activities.Rules.Rule(sRULE_NAME, condition, lstThenActions, lstElseActions);
				r.Priority = nPRIORITY;
				r.Active   = bACTIVE  ;
				//r.ReevaluationBehavior = (RuleReevaluationBehavior) Enum.Parse(typeof(RuleReevaluationBehavior), sREEVALUATION);
				// 12/04/2010   Play it safe and never-reevaluate. 
				r.ReevaluationBehavior = RuleReevaluationBehavior.Never;
				rules.Rules.Add(r);
			}
			rules.Validate(validation);
			if ( validation.Errors.HasErrors )
			{
				throw(new Exception(RulesUtil.GetValidationErrors(validation)));
			}
			return rules;
		}

	}
}


