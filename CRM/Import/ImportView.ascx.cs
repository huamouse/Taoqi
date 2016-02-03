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
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Xml;
using System.Text;
using System.Workflow.Activities.Rules;

namespace Taoqi.Import
{
	/// <summary>
	///		Summary description for ImportView.
	/// </summary>
	public class ImportView : SplendidControl
	{
		#region Properties
		protected _controls.ModuleHeader   ctlModuleHeader ;
		protected _controls.DynamicButtons ctlDynamicButtons;
		protected _controls.ListHeader     ctlListHeader   ;
		protected _controls.Chooser        ctlDuplicateFilterChooser;
		protected PlaceHolder              phDefaultsView  ;
		protected SplendidControl          ctlDefaultsView ;

		protected Guid                   gID                          ;
		protected TextBox                txtNAME                      ;
		protected RequiredFieldValidator reqNAME                      ;

		protected RadioButton            radEXCEL                     ;
		protected RadioButton            radXML_SPREADSHEET           ;
		protected RadioButton            radXML                       ;
		protected RadioButton            radSALESFORCE                ;
		protected RadioButton            radACT_2005                  ;
		protected RadioButton            radDBASE                     ;
		protected RadioButton            radCUSTOM_CSV                ;
		protected RadioButton            radCUSTOM_TAB                ;
		protected RadioButton            radCUSTOM_DELIMITED          ;
		protected TextBox                txtCUSTOM_DELIMITER_VAL      ;
		// 04/08/2012   Add LinkedIn to the source. 
		protected RadioButton            radLINKEDIN                  ;
		protected RadioButton            radTWITTER                   ;
		protected RadioButton            radFACEBOOK                  ;
		protected RadioButton            radQUICKBOOKS                ;
		protected Button                 btnSignIn                    ;
		protected Button                 btnConnect                   ;
		protected Button                 btnSignOut                   ;
		protected HiddenField            txtOAUTH_TOKEN               ;
		protected HiddenField            txtOAUTH_SECRET              ;
		protected HiddenField            txtOAUTH_VERIFIER            ;
		protected HiddenField            txtOAUTH_ACCESS_TOKEN        ;
		protected HiddenField            txtOAUTH_ACCESS_SECRET       ;
		protected Button                 btnOAuthChanged              ;

		protected DataView               vwMain                       ;
		protected DataView               vwColumns                    ;
		protected SplendidGrid           grdMain                      ;
		protected DataView               vwMySaved                    ;
		protected SplendidGrid           grdMySaved                   ;

		protected XmlDocument            xml                          ;
		protected XmlDocument            xmlMapping                   ;
		protected string                 sImportModule                ;
		protected HtmlInputFile          fileIMPORT                   ;
		protected RequiredFieldValidator reqFILENAME                  ;
		protected CheckBox               chkHasHeader                 ;
		protected HtmlTable              tblImportMappings            ;
		protected StringBuilder          sbImport                     ;

		protected Label                  lblStatus                    ;
		protected Label                  lblSuccessCount              ;
		protected Label                  lblDuplicateCount            ;
		protected Label                  lblFailedCount               ;
		protected CheckBox               chkUseTransaction            ;

		protected HiddenField            txtACTIVE_TAB                ;
		protected bool                   bDuplicateFields = false;
		protected int                    nMAX_ERRORS = 200;

		// 09/17/2013   Add Business Rules to import. 
		protected DataTable       dtRules               ;
		protected DataGrid        dgRules               ;
		protected HiddenField     txtRULE_ID            ;
		protected TextBox         txtRULE_NAME          ;
		protected TextBox         txtPRIORITY           ;
		protected DropDownList    lstREEVALUATION       ;
		protected CheckBox        chkACTIVE             ;
		protected TextBox         txtCONDITION          ;
		protected TextBox         txtTHEN_ACTIONS       ;
		protected TextBox         txtELSE_ACTIONS       ;
		protected RequiredFieldValidator reqRULE_NAME   ;
		protected RequiredFieldValidator reqCONDITION   ;
		protected RequiredFieldValidator reqTHEN_ACTIONS;

		protected DataTable       dtRuleColumns  ;
		protected Repeater        ctlConditionSchemaRepeater;
		protected Repeater        ctlThenSchemaRepeater;
		protected Repeater        ctlElseSchemaRepeater;

		public string Module
		{
			get { return sImportModule; }
			set { sImportModule = value; }
		}
		#endregion

		#region Helper methods
		protected string SourceType()
		{
			string sSourceType = "";
			if      ( radEXCEL           .Checked ) sSourceType = "excel";
			else if ( radXML_SPREADSHEET .Checked ) sSourceType = "xmlspreadsheet";
			else if ( radXML             .Checked ) sSourceType = "xml";
			else if ( radSALESFORCE      .Checked ) sSourceType = "salesforce";
			else if ( radACT_2005        .Checked ) sSourceType = "act";
			else if ( radDBASE           .Checked ) sSourceType = "dbase";
			else if ( radCUSTOM_CSV      .Checked ) sSourceType = "other";
			else if ( radCUSTOM_TAB      .Checked ) sSourceType = "other_tab";
			else if ( radCUSTOM_DELIMITED.Checked ) sSourceType = "custom_delimited";
			else if ( radLINKEDIN        .Checked ) sSourceType = "LinkedIn";
			else if ( radTWITTER         .Checked ) sSourceType = "Twitter";
			else if ( radFACEBOOK        .Checked ) sSourceType = "Facebook";
			else if ( radQUICKBOOKS      .Checked ) sSourceType = "QuickBooks";
			return sSourceType;
		}

		protected void SourceType(string sSOURCE)
		{
			switch ( sSOURCE.ToLower() )
			{
				case "excel"           :  radEXCEL           .Checked = true;  break;
				case "xmlspreadsheet"  :  radXML_SPREADSHEET .Checked = true;  break;
				case "xml"             :  radXML             .Checked = true;  break;
				case "salesforce"      :  radSALESFORCE      .Checked = true;  break;
				case "act"             :  radACT_2005        .Checked = true;  break;
				case "dbase"           :  radDBASE           .Checked = true;  break;
				case "other"           :  radCUSTOM_CSV      .Checked = true;  break;
				case "other_tab"       :  radCUSTOM_TAB      .Checked = true;  break;
				case "custom_delimited":  radCUSTOM_DELIMITED.Checked = true;  break;
				case "LinkedIn"        :  radLINKEDIN        .Checked = true;  break;
				case "Twitter"         :  radTWITTER         .Checked = true;  break;
				case "Facebook"        :  radFACEBOOK        .Checked = true;  break;
				case "QuickBooks"      :  radQUICKBOOKS      .Checked = true;  break;
			}
		}

		protected void DuplicateFilterUpdate()
		{
			try
			{
				DataTable dtFields = ctlDuplicateFilterChooser.LeftValuesTable;
				if ( dtFields != null )
				{
					DataView vwFields = new DataView(dtFields);
					XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
					foreach ( XmlNode xField in nlFields )
					{
						string sName = xField.Attributes.GetNamedItem("Name").Value;
						vwFields.RowFilter = "value = '" + sName + "'";
						bool bDuplicateFilter = (vwFields.Count > 0);
						XmlUtil.SetSingleNode(xmlMapping, xField, "DuplicateFilter", bDuplicateFilter.ToString());
					}
				}
			}
			catch(Exception ex)
			{
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		private void ctlDuplicateFilterChooser_Bind()
		{
			StringBuilder sbFieldsList = new StringBuilder();
			// 12/17/2008   There are some common fields that are unlikely to be filtered on. 
			sbFieldsList.Append("'ID', 'MODIFIED_USER_ID', 'ASSIGNED_USER_ID', 'TEAM_ID'");

			// 04/22/2012   vwColumns is global and does not need to be fetched here. 
			vwColumns.RowFilter = "NAME not in (" + sbFieldsList.ToString() + ")";

			ListBox lstLeft = ctlDuplicateFilterChooser.LeftListBox;
			lstLeft.Items.Clear();

			XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field[DuplicateFilter='True']");
			foreach ( XmlNode xField in nlFields )
			{
				ListItem item = new ListItem();
				item.Value = xField.Attributes["Name"].Value;
				item.Text  = Utils.TableColumnName(L10n, sImportModule, item.Value);
				lstLeft.Items.Add(item);

				// 12/17/2008   Build the filter for the right-hand-side.
				if ( sbFieldsList.Length > 0 )
					sbFieldsList.Append(", ");
				string sName = xField.Attributes.GetNamedItem("Name").Value;
				sbFieldsList.Append("'" + sName + "'");
			}
			vwColumns.RowFilter = "NAME not in (" + sbFieldsList.ToString() + ")";
			
			ListBox lstRight = ctlDuplicateFilterChooser.RightListBox;
			lstRight.DataValueField = "NAME";
			lstRight.DataTextField  = "DISPLAY_NAME";
			lstRight.DataSource     = vwColumns;
			lstRight.DataBind();

			// 04/22/2012   Clear filter after use. 
			vwColumns.RowFilter = "";
		}

		protected void UpdateImportMappings(XmlDocument xml, bool bInitialize, bool bUpdateMappings)
		{
			Hashtable hashFieldMappings = new Hashtable();

			tblImportMappings.Rows.Clear();
			HtmlTableRow rowHeader = new HtmlTableRow();
			tblImportMappings.Rows.Add(rowHeader);
			HtmlTableCell cellField  = new HtmlTableCell();
			HtmlTableCell cellRowHdr = new HtmlTableCell();
			HtmlTableCell cellRow1   = new HtmlTableCell();
			HtmlTableCell cellRow2   = new HtmlTableCell();
			rowHeader.Cells.Add(cellField );
			if ( chkHasHeader.Checked || radXML.Checked )
				rowHeader.Cells.Add(cellRowHdr);
			rowHeader.Cells.Add(cellRow1  );
			rowHeader.Cells.Add(cellRow2  );
			cellField .Attributes.Add("class", "tabDetailViewDL");
			cellRowHdr.Attributes.Add("class", "tabDetailViewDL");
			cellRow1  .Attributes.Add("class", "tabDetailViewDL");
			cellRow2  .Attributes.Add("class", "tabDetailViewDL");
			cellField .Attributes.Add("style", "TEXT-ALIGN: left");
			cellRowHdr.Attributes.Add("style", "TEXT-ALIGN: left");
			cellRow1  .Attributes.Add("style", "TEXT-ALIGN: left");
			cellRow2  .Attributes.Add("style", "TEXT-ALIGN: left");
			Label lblField  = new Label();
			Label lblRowHdr = new Label();
			Label lblRow1   = new Label();
			Label lblRow2   = new Label();
			cellField .Controls.Add(lblField );
			cellRowHdr.Controls.Add(lblRowHdr);
			cellRow1  .Controls.Add(lblRow1  );
			cellRow2  .Controls.Add(lblRow2  );
			lblField .Font.Bold = true;
			lblRowHdr.Font.Bold = true;
			lblRow1  .Font.Bold = true;
			lblRow2  .Font.Bold = true;
			lblField .Text = L10n.Term("Import.LBL_DATABASE_FIELD");
			lblRowHdr.Text = L10n.Term("Import.LBL_HEADER_ROW"    );
			lblRow1  .Text = L10n.Term("Import.LBL_ROW"           ) + " 1";
			lblRow2  .Text = L10n.Term("Import.LBL_ROW"           ) + " 2";
			
			if ( xml.DocumentElement != null )
			{
				XmlNodeList nl = xml.DocumentElement.SelectNodes(sImportModule.ToLower());
				if ( nl.Count > 0 )
				{
					vwColumns.Sort = "DISPLAY_NAME";
					XmlNode nodeH = nl[0];
					XmlNode node1 = nl[0];
					XmlNode node2 = null;
					// 08/22/2006   An XML Spreadsheet will have a header record, 
					// so don't assume that an XML file will use the tag names as the header. 
					if ( chkHasHeader.Checked )
					{
						if ( nl.Count > 1 )
							node1 = nl[1];
						if ( nl.Count > 2 )
							node2 = nl[2];
					}
					else
					{
						if ( nl.Count > 1 )
							node2 = nl[1];
					}
					bDuplicateFields = false;
					Hashtable hashSelectedFields = new Hashtable();
					for ( int i = 0 ; i < nodeH.ChildNodes.Count ; i++ )
					{
						rowHeader = new HtmlTableRow();
						tblImportMappings.Rows.Add(rowHeader);
						cellField  = new HtmlTableCell();
						cellRowHdr = new HtmlTableCell();
						cellRow1   = new HtmlTableCell();
						cellRow2   = new HtmlTableCell();
						rowHeader.Cells.Add(cellField );
						if ( chkHasHeader.Checked || radXML.Checked )
							rowHeader.Cells.Add(cellRowHdr);
						rowHeader.Cells.Add(cellRow1  );
						if ( node2 != null && i < node2.ChildNodes.Count )
							rowHeader.Cells.Add(cellRow2);
						cellField .Attributes.Add("class", "tabDetailViewDF");
						cellRowHdr.Attributes.Add("class", "tabDetailViewDF");
						cellRow1  .Attributes.Add("class", "tabDetailViewDF");
						cellRow2  .Attributes.Add("class", "tabDetailViewDF");
						// 04/25/2008   Use KeySortDropDownList instead of ListSearchExtender. 
						DropDownList lstField  = new KeySortDropDownList();
						// 07/23/2010   Lets try the latest version of the ListSearchExtender. 
						// 07/28/2010   We are getting an undefined exception on the Accounts List Advanced page. 
						// Lets drop back to using KeySort. 
						//DropDownList lstField  = new DropDownList();
						lblRowHdr = new Label();
						lblRow1   = new Label();
						lblRow2   = new Label();
						cellField .Controls.Add(lstField );
						cellRowHdr.Controls.Add(lblRowHdr);
						cellRow1  .Controls.Add(lblRow1  );
						cellRow2  .Controls.Add(lblRow2  );
						
						// 08/20/2006   Clear any previous filters. 
						vwColumns.RowFilter = null;
						// 08/20/2006   Don't use real column names as they may collide.
						lstField.ID             = "ImportField" + i.ToString("000");
						lstField.DataValueField = "NAME";
						lstField.DataTextField  = "DISPLAY_NAME";
						lstField.DataSource     = vwColumns;
						lstField.DataBind();
						
						// 04/25/2008   Add AJAX searching. 
						// 04/25/2008   ListSearchExtender needs work.  I don't like the delay when a list is selected
						// and there are problems when the browser window is scrolled.  KeySortDropDownList is a better solution. 
						// 07/23/2010   Lets try the latest version of the ListSearchExtender. 
						// 07/28/2010   We are getting an undefined exception on the Accounts List Advanced page. 
						/*
						AjaxControlToolkit.ListSearchExtender extField = new AjaxControlToolkit.ListSearchExtender();
						extField.ID              = lstField.ID + "_ListSearchExtender";
						extField.TargetControlID = lstField.ID;
						extField.PromptText      = L10n.Term(".LBL_TYPE_TO_SEARCH");
						extField.PromptCssClass  = "ListSearchExtenderPrompt";
						cellField .Controls.Add(extField );
						*/
						
						lstField.Items.Insert(0, new ListItem(L10n.Term("Import.LBL_DONT_MAP"), String.Empty));
						try
						{
							if ( bInitialize )
							{
								if ( chkHasHeader.Checked )
								{
									// 08/22/2006   If Has Header is checked, then always expect the body to contain the header names. 
									string sFieldName = nodeH.ChildNodes[i].InnerText.Trim();
									// 08/20/2006   Use the DataView to locate matching fields so that we don't have to worry about case significance. 
									// 05/09/2010   Also match against a custom field name. 
									// 04/22/2012   Calculate a display name without spaces to get better hits with Salesforce field names. 
									sFieldName = Sql.EscapeSQL(sFieldName);
									vwColumns.RowFilter = "NAME = '" + sFieldName + "' or NAME_NOUNDERSCORE = '" + sFieldName + "' or DISPLAY_NAME = '" + sFieldName + "' or DISPLAY_NAME_NOSPACE = '" + sFieldName + "' or NAME = '" + sFieldName.Replace(" ", "_") + "_C" + "'";
									if ( vwColumns.Count > 0 )
									{
										hashFieldMappings.Add(i, Sql.ToString(vwColumns[0]["NAME"]));
										// 08/19/2010   Check the list before assigning the value. 
										Utils.SetSelectedValue(lstField, Sql.ToString(vwColumns[0]["NAME"]));
									}
								}
								else if ( radXML.Checked )
								{
									// 08/22/2006   If Has Header is not checked for XML, then use the tag ame as the field name. 
									string sFieldName = nodeH.ChildNodes[i].Name;
									// 08/20/2006   Use the DataView to locate matching fields so that we don't have to worry about case significance. 
									// 05/09/2010   Also match against a custom field name. 
									// 04/22/2012   Calculate a display name without spaces to get better hits with Salesforce field names. 
									sFieldName = Sql.EscapeSQL(sFieldName);
									vwColumns.RowFilter = "NAME = '" + sFieldName + "' or NAME_NOUNDERSCORE = '" + sFieldName + "' or DISPLAY_NAME = '" + sFieldName + "' or DISPLAY_NAME_NOSPACE = '" + sFieldName + "' or NAME = '" + sFieldName.Replace(" ", "_") + "_C" + "'";
									if ( vwColumns.Count > 0 )
									{
										hashFieldMappings.Add(i, Sql.ToString(vwColumns[0]["NAME"]));
										// 08/19/2010   Check the list before assigning the value. 
										Utils.SetSelectedValue(lstField, Sql.ToString(vwColumns[0]["NAME"]));
									}
								}
								else
									hashFieldMappings.Add(i, "ImportField" + i.ToString("000"));
							}
							else
							{
								// 08/20/2006   Manually set the last value. 
								hashFieldMappings.Add(i, Sql.ToString(Request[lstField.UniqueID]));
								// 08/19/2010   Check the list before assigning the value. 
								Utils.SetSelectedValue(lstField, Sql.ToString(Request[lstField.UniqueID]));
								if ( lstField.SelectedValue.Length > 0 )
								{
									if ( hashSelectedFields.ContainsKey(lstField.SelectedValue) )
									{
										bDuplicateFields = true;
									}
									else
									{
										hashSelectedFields.Add(lstField.SelectedValue, null);
									}
								}
							}
						}
						catch //(Exception ex)
						{
						}
						// XML data will use the node-name as the header. 
						if ( chkHasHeader.Checked )
						{
							// 08/22/2006   If Has Header is checked, then always expect the body to contain the header names. 
							lblRowHdr.Text = nodeH.ChildNodes[i].InnerText;
						}
						else if ( radXML.Checked )
						{
							// 08/22/2006   If Has Header is not checked for XML, then use the tag name as the field name. 
							lblRowHdr.Text = nodeH.ChildNodes[i].Name;
						}
						
						if ( node1 != null && i < node1.ChildNodes.Count )
							lblRow1.Text = node1.ChildNodes[i].InnerText;
						if ( node2 != null && i < node2.ChildNodes.Count )
							lblRow2.Text = node2.ChildNodes[i].InnerText;
					}
					if ( bDuplicateFields )
					{
						throw(new Exception(L10n.Term("Import.ERR_MULTIPLE")));
					}
					
					if ( bUpdateMappings )
					{
						DuplicateFilterUpdate();
						XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
						foreach ( XmlNode xField in nlFields )
						{
							XmlUtil.SetSingleNode(xmlMapping, xField, "Mapping", String.Empty);
						}
						// 08/22/2006   We should always use the header mappings instead of an index as nodes may move around. 
						XmlNode node = nl[0];
						for ( int j = 0; j < node.ChildNodes.Count; j++ )
						{
							XmlNode xField = xmlMapping.DocumentElement.SelectSingleNode("Fields/Field[@Name='" + hashFieldMappings[j] + "']");
							if ( xField != null )
							{
								XmlUtil.SetSingleNode(xmlMapping, xField, "Mapping", node.ChildNodes[j].Name);
							}
						}
					}
					else
					{
						// 12/17/2008   Apply current filter fields. 
						ctlDuplicateFilterChooser_Bind();
						// 10/12/2006   If we are not updating the mappings, then we are setting the mappings. 
						XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
						foreach ( XmlNode xField in nlFields )
						{
							string sName    = xField.Attributes.GetNamedItem("Name").Value;
							string sMapping = XmlUtil.SelectSingleNode(xField, "Mapping");
							if ( !Sql.IsEmptyString(sMapping) )
							{
								DropDownList lstField = tblImportMappings.FindControl(sMapping) as DropDownList;
								if ( lstField != null )
								{
									try
									{
										// 08/19/2010   Check the list before assigning the value. 
										Utils.SetSelectedValue(lstField, sName);
									}
									catch
									{
									}
								}
							}
						}
					}
				}
			}
		}

		protected void ValidateMappings()
		{
			switch ( sImportModule )
			{
				case "Accounts":
				{
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Accounts.LBL_LIST_ACCOUNT_NAME")) );
					break;
				}
				case "Contacts":
				{
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='LAST_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Contacts.LBL_LIST_LAST_NAME")) );
					break;
				}
				case "Leads":
				{
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='LAST_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Contacts.LBL_LIST_LAST_NAME")) );
					break;
				}
				case "Prospects":
				{
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='LAST_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						throw ( new Exception(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Prospects.LBL_LIST_LAST_NAME")) );
					break;
				}
				case "Opportunities":
				{
					StringBuilder sb = new StringBuilder();
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_NAME"));
					// 11/02/2006   Allow mapping of ACCOUNT_NAME or ACCOUNT_ID. 
					sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_NAME']/Mapping");
					// 03/04/2010   A default value is valid when checking for required fields. 
					string sDefault = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_NAME']/Default");
					if ( Sql.IsEmptyString(sMapping) && Sql.IsEmptyString(sDefault) )
					{
						sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_ID']/Mapping");
						// 03/04/2010   A default value is valid when checking for required fields. 
						sDefault = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_ID']/Default");
						if ( Sql.IsEmptyString(sMapping) && Sql.IsEmptyString(sDefault) )
							sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_ACCOUNT_NAME"));
					}
					sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='DATE_CLOSED']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_DATE_CLOSED"));
					sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='SALES_STAGE']/Mapping");
					// 03/04/2010   A default value is valid when checking for required fields. 
					sDefault = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='SALES_STAGE']/Default");
					if ( Sql.IsEmptyString(sMapping) && Sql.IsEmptyString(sDefault) )
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Opportunities.LBL_LIST_SALES_STAGE"));
					if ( sb.Length > 0 )
						throw ( new Exception(sb.ToString()) );
					break;
				}
				case "Cases":
				{
					StringBuilder sb = new StringBuilder();
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Cases.LBL_LIST_NAME"));
					// 11/02/2006   Allow mapping of ACCOUNT_NAME or ACCOUNT_ID. 
					sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
					{
						sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_ID']/Mapping");
						// 03/04/2010   A default value is valid when checking for required fields. 
						string sDefault = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='ACCOUNT_ID']/Default");
						if ( Sql.IsEmptyString(sMapping) && Sql.IsEmptyString(sDefault) )
							sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Cases.LBL_LIST_ACCOUNT_NAME"));
					}
					if ( sb.Length > 0 )
						throw ( new Exception(sb.ToString()) );
					break;
				}
				case "Users":
				{
					StringBuilder sb = new StringBuilder();
					string sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='USER_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Users.LBL_USER_NAME"));
					sMapping = XmlUtil.SelectSingleNode(xmlMapping.DocumentElement, "Fields/Field[@Name='LAST_NAME']/Mapping");
					if ( Sql.IsEmptyString(sMapping) )
					{
						sb.AppendLine(L10n.Term(".ERR_MISSING_REQUIRED_FIELDS") + " " + L10n.Term("Users.LBL_LAST_NAME"));
					}
					if ( sb.Length > 0 )
						throw ( new Exception(sb.ToString()) );
					break;
				}
			}
		}

		protected void GenerateImport(string sTempFileName, bool bPreview)
		{
			try
			{
				XmlDocument xmlImport = new XmlDocument();
				xmlImport.Load(Path.Combine(Path.GetTempPath(), sTempFileName));
				
				XmlNodeList nlRows = xmlImport.DocumentElement.SelectNodes(sImportModule.ToLower());
				if ( nlRows.Count == 0 )
					throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
				
				// 09/17/2013   Add Business Rules to import. 
				SplendidRulesTypeProvider typeProvider = new SplendidRulesTypeProvider();
				RuleValidation validation = new RuleValidation(typeof(SplendidImportThis), typeProvider);
				RuleSet rules = null;
				// 06/02/2014   No sense in building the rules if the rows are empty. 
				if ( dtRules != null && dtRules.Rows.Count > 0 )
				{
					rules = RulesUtil.BuildRuleSet(dtRules, validation);
				}
				
				// 08/20/2006   Also map the header names to allow for a flexible XML. 
				StringDictionary hashHeaderMappings   = new StringDictionary();
				StringDictionary hashReverseMappings  = new StringDictionary();
				StringDictionary hashDuplicateFilters = new StringDictionary ();
				Hashtable hashDefaultMappings = new Hashtable();
				XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
				foreach ( XmlNode xField in nlFields )
				{
					string sName    = xField.Attributes.GetNamedItem("Name").Value;
					string sMapping = XmlUtil.SelectSingleNode(xField, "Mapping");
					string sDefault = XmlUtil.SelectSingleNode(xField, "Default");
					if ( !Sql.IsEmptyString(sMapping) )
					{
						// 11/02/2009 Rick.  We need to protect against duplicate dictionary entries. 
						if ( !hashHeaderMappings.ContainsKey(sMapping) )
							hashHeaderMappings.Add(sMapping, sName);
						if ( !hashReverseMappings.ContainsKey(sName) )
							hashReverseMappings.Add(sName, sMapping);
					}
					if ( !Sql.IsEmptyString(sDefault) )
					{
						// 11/02/2009 Rick.  We need to protect against duplicate dictionary entries. 
						if ( !hashDefaultMappings.ContainsKey(sName) )
							hashDefaultMappings.Add(sName, sDefault);
					}
					bool bDuplicateFilter = Sql.ToBoolean(XmlUtil.SelectSingleNode(xField, "DuplicateFilter"));
					if ( bDuplicateFilter )
					{
						// 11/02/2009 Rick.  We need to protect against duplicate dictionary entries. 
						if ( !hashDuplicateFilters.ContainsKey(sName) )
							hashDuplicateFilters.Add(sName, String.Empty);
					}
				}
				StringBuilder sbDuplicateFilters = new StringBuilder();
				foreach ( string sDuplicateField in hashDuplicateFilters.Keys )
				{
					if ( sbDuplicateFilters.Length > 0 )
						sbDuplicateFilters.Append(", ");
					sbDuplicateFilters.Append(sDuplicateField.ToUpper());
				}
				
				// 11/01/2006   Use a hash for quick access to required fields. 
				Hashtable hashColumns = new Hashtable();
				foreach ( DataRowView row in vwColumns )
				{
					// 11/02/2009 Rick.  We need to protect against duplicate dictionary entries. 
					if ( !hashColumns.ContainsKey(row["NAME"]) )
						hashColumns.Add(row["NAME"], row["DISPLAY_NAME"]);
				}
				
				Hashtable hashRequiredFields = new Hashtable();
				DataTable dtRequiredFields = SplendidCache.EditViewFields(sImportModule + ".EditView");
				DataView dvRequiredFields = new DataView(dtRequiredFields);
				dvRequiredFields.RowFilter = "UI_REQUIRED = 1";
				foreach(DataRowView row in dvRequiredFields)
				{
					string sDATA_FIELD = Sql.ToString (row["DATA_FIELD"]);
					if (!Sql.IsEmptyString(sDATA_FIELD) )
					{
						if ( !hashRequiredFields.ContainsKey(sDATA_FIELD) )
							hashRequiredFields.Add(sDATA_FIELD, null);
					}
				}
				dvRequiredFields = null;
				dtRequiredFields = null;
				
				int nImported   = 0;
				int nFailed     = 0;
				int nDuplicates = 0;
				//int nSkipped  = 0;
				DataTable dtProcessed = new DataTable();
				dtProcessed.Columns.Add("IMPORT_ROW_STATUS", typeof(bool));
				dtProcessed.Columns.Add("IMPORT_ROW_NUMBER", typeof(Int32));
				dtProcessed.Columns.Add("IMPORT_ROW_ERROR"  );
				dtProcessed.Columns.Add("IMPORT_LAST_COLUMN");
				dtProcessed.Columns.Add("ID");  // 10/10/2006   Every record will have an ID, either implied or specified. 
				SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Import Database Table: " + sImportModule);
				DbProviderFactory dbf = DbProviderFactories.GetFactory();
				using ( IDbConnection con = dbf.CreateConnection() )
				{
					con.Open();
					Hashtable hashTeamNames = new Hashtable();
					if ( Crm.Config.enable_team_management() )
					{
						string sSQL;
						sSQL = "select ID          " + ControlChars.CrLf
						     + "     , NAME        " + ControlChars.CrLf
						     + "  from vwTEAMS_List" + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							using ( IDataReader rdr = cmd.ExecuteReader() )
							{
								while ( rdr.Read() )
								{
									Guid   gTEAM_ID   = Sql.ToGuid  (rdr["ID"  ]);
									string sTEAM_NAME = Sql.ToString(rdr["NAME"]);
									sTEAM_NAME = sTEAM_NAME.Trim().ToUpper();
									if ( !Sql.IsEmptyString(sTEAM_NAME) )
									{
										// 11/02/2009 Rick.  We need to protect against duplicate dictionary entries. 
										if ( !hashTeamNames.ContainsKey(sTEAM_NAME) )
											hashTeamNames.Add(sTEAM_NAME, gTEAM_ID);
									}
								}
							}
						}
					}

					// 11/01/2006   The transaction is optional, just make sure to always dispose it. 
					//using ( IDbTransaction trn = Sql.BeginTransaction(con) )
					{
						IDbTransaction trn = null;
						try
						{
							string sTABLE_NAME = Sql.ToString(Application["Modules." + sImportModule + ".TableName"]);
							if ( Sql.IsEmptyString(sTABLE_NAME) )
								sTABLE_NAME = sImportModule.ToUpper();
							
							// 03/13/2008   Allow the use of a special Import procedure. 
							// This is so that we can convert text values to their associated GUID value. 
							IDbCommand cmdImport = null;
							try
							{
								// 03/13/2008   The factory will throw an exception if the procedure is not found. 
								// Catching an exception is expensive, but trivial considering all the other processing that will occur. 
								// We need this same logic in SplendidCache.ImportColumns. 
								cmdImport = SqlProcs.Factory(con, "sp" + sTABLE_NAME + "_Import");
							}
							catch
							{
								cmdImport = SqlProcs.Factory(con, "sp" + sTABLE_NAME + "_Update");
							}
							// 02/02/2010   ACT! import will also import Notes and Activities (Calls or Meetings). 
							IDbCommand cmdNOTES_Import    = null;
							IDbCommand cmdCALLS_Import    = null;
							IDbCommand cmdMEETINGS_Import = null;
							IDbCommand cmdPROSPECT_LISTS_Import = null;
							IDbCommand cmdPROSPECT_LISTS_CONTACTS_Import  = null;
							// 01/11/2011   Use a separate procedure as it has different parameters. 
							IDbCommand cmdPROSPECT_LISTS_LEADS_Import     = null;
							IDbCommand cmdPROSPECT_LISTS_PROSPECTS_Import = null;
							if ( radACT_2005.Checked )
							{
								try
								{
									cmdNOTES_Import = SqlProcs.Factory(con, "spNOTES_Import");
								}
								catch
								{
									cmdNOTES_Import = SqlProcs.Factory(con, "spNOTES_Update");
								}
								try
								{
									cmdCALLS_Import = SqlProcs.Factory(con, "spCALLS_Import");
								}
								catch
								{
									cmdCALLS_Import = SqlProcs.Factory(con, "spCALLS_Update");
								}
								try
								{
									cmdMEETINGS_Import = SqlProcs.Factory(con, "spMEETINGS_Import");
								}
								catch
								{
									cmdMEETINGS_Import = SqlProcs.Factory(con, "spMEETINGS_Update");
								}
								try
								{
									cmdPROSPECT_LISTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_Import");
								}
								catch
								{
									cmdPROSPECT_LISTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_Update");
								}
							}
							// 01/10/2011   When importing into the Leads module, we need to use the Leads relationship table. 
							// 10/24/2013   These import procedures need to be available to all imports and not just ACT import 
							// to allow direct import into a Prospect List.
							if ( sTABLE_NAME == "CONTACTS" )
							{
								try
								{
									cmdPROSPECT_LISTS_CONTACTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_CONTACTS_Import");
								}
								catch
								{
									cmdPROSPECT_LISTS_CONTACTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_CONTACTS_Update");
								}
							}
							else if ( sTABLE_NAME == "LEADS" )
							{
								try
								{
									cmdPROSPECT_LISTS_LEADS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_LEADS_Import");
								}
								catch
								{
									cmdPROSPECT_LISTS_LEADS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_LEADS_Update");
								}
							}
							else if ( sTABLE_NAME == "PROSPECTS" )
							{
								try
								{
									cmdPROSPECT_LISTS_PROSPECTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_PROSPECTS_Import");
								}
								catch
								{
									cmdPROSPECT_LISTS_PROSPECTS_Import = SqlProcs.Factory(con, "spPROSPECT_LISTS_PROSPECTS_Update");
								}
							}
							IDbCommand cmdImportCSTM = null;
							//IDbCommand cmdImportTeam = null;
							// 09/17/2007   Only activate the custom field code if there are fields in the custom fields table. 
							vwColumns.RowFilter = "CustomField = 1";
							if ( vwColumns.Count > 0 )
							{
								vwColumns.Sort = "colid";
								cmdImportCSTM = con.CreateCommand();
								cmdImportCSTM.CommandType = CommandType.Text;
								cmdImportCSTM.CommandText = "update " + sTABLE_NAME + "_CSTM" + ControlChars.CrLf;
								int nFieldIndex = 0;
								foreach ( DataRowView row in vwColumns )
								{
									// 01/11/2006   Uppercase looks better. 
									string sNAME   = Sql.ToString(row["ColumnName"]).ToUpper();
									string sCsType = Sql.ToString(row["ColumnType"]);
									// 01/13/2007   We need to truncate any long strings to prevent SQL error. 
									// String or binary data would be truncated. The statement has been terminated. 
									int    nMAX_SIZE = Sql.ToInteger(row["Size"]);
									if ( nFieldIndex == 0 )
										cmdImportCSTM.CommandText += "   set ";
									else
										cmdImportCSTM.CommandText += "     , ";
									// 01/10/2006   We can't use a StringBuilder because the Sql.AddParameter function
									// needs to be able to replace the @ with the appropriate database specific token. 
									cmdImportCSTM.CommandText += sNAME + " = @" + sNAME + ControlChars.CrLf;
									
									IDbDataParameter par = null;
									switch ( sCsType )
									{
										// 09/19/2007   In order to leverage the existing AddParameter functions, we need to provide default values. 
										case "Guid"    :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, Guid.Empty             );  break;
										case "short"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "Int32"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "Int64"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0                      );  break;
										case "float"   :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, 0.0f                   );  break;
										case "decimal" :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, new Decimal()          );  break;
										case "bool"    :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, false                  );  break;
										case "DateTime":  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, DateTime.MinValue      );  break;
										default        :  par = Sql.AddParameter(cmdImportCSTM, "@" + sNAME, String.Empty, nMAX_SIZE);  break;
									}
									nFieldIndex++;
								}
								// 09/19/2007   Exclude ID_C as it is expect and required. We don't want it to appear in the mapping table. 
								cmdImportCSTM.CommandText += " where ID_C = @ID_C" + ControlChars.CrLf;
								Sql.AddParameter(cmdImportCSTM, "@ID_C", Guid.Empty);
								// 10/24/2010   This execute does not seem correct, so remove it. 
								//cmdImportCSTM.ExecuteNonQuery();
							}
							vwColumns.RowFilter = "";
							/*
							if ( Crm.Config.enable_team_management() )
							{
								cmdImportTeam = con.CreateCommand();
								cmdImportTeam.CommandType = CommandType.Text;
								cmdImportTeam.CommandText  = "update " + sTABLE_NAME     + ControlChars.CrLf;
								cmdImportTeam.CommandText += "   set TEAM_ID = @TEAM_ID" + ControlChars.CrLf;
								cmdImportTeam.CommandText += " where ID      = @ID     " + ControlChars.CrLf;
								Sql.AddParameter(cmdImportTeam, "@TEAM_ID", Guid.Empty);
								Sql.AddParameter(cmdImportTeam, "@ID"     , Guid.Empty);
							}
							*/
							
							// 11/01/2006   The transaction is optional, but on by default. 
							if ( chkUseTransaction.Checked || bPreview )
							{
								// 10/07/2009   We need to create our own global transaction ID to support auditing and workflow on SQL Azure, PostgreSQL, Oracle, DB2 and MySQL. 
								trn = Sql.BeginTransaction(con);
								cmdImport.Transaction = trn;
								if ( cmdImportCSTM != null )
									cmdImportCSTM.Transaction = trn;
								//if ( cmdImportTeam != null )
								//	cmdImportTeam.Transaction = trn;
							}
							int i = 0;
							if ( chkHasHeader.Checked )
								i++;
							// 06/04/2009   If Team is required, then make sure to initialize the TEAM_ID.  Same is true for ASSIGNED_USER_ID. 
							bool bEnableTeamManagement  = Crm.Config.enable_team_management();
							// 01/11/2011   Ignore the Required flag. 
							//bool bRequireTeamManagement = Crm.Config.require_team_management();
							//bool bRequireUserAssignment = Crm.Config.require_user_assignment();
							// 02/04/2010   An ACT! group should be treated as a Prospect List. 
							Hashtable hashProspectLists = new Hashtable();
							if ( radACT_2005.Checked && cmdPROSPECT_LISTS_Import != null )
							{
								if ( chkUseTransaction.Checked || bPreview )
								{
									cmdPROSPECT_LISTS_Import.Transaction = trn;
								}
								// 02/04/2010   Prospect Lists should assume the owner of the parent record. 
								Guid gTEAM_ID          = Security.TEAM_ID;
								Guid gASSIGNED_USER_ID = Security.USER_ID;
								// 03/27/2010   Use FindParameter as the Parameter Name may start with @. 
								// 01/10/2011   This logic is the source of a bug where the Prospect List owner was not getting set. 
								// The problem is that cmdImport has not been initialized at this stage, so it does not make sense to use it as the base. 
								/*
								IDbDataParameter parTEAM_ID          = Sql.FindParameter(cmdImport, "@TEAM_ID"         );
								IDbDataParameter parASSIGNED_USER_ID = Sql.FindParameter(cmdImport, "@ASSIGNED_USER_ID");
								if ( parTEAM_ID != null )
									gTEAM_ID = Sql.ToGuid(parTEAM_ID.Value);
								if ( parASSIGNED_USER_ID != null )
									gASSIGNED_USER_ID = Sql.ToGuid(parASSIGNED_USER_ID.Value);
								*/
								
								IDbDataParameter parID = Sql.FindParameter(cmdPROSPECT_LISTS_Import, "ID");
								if ( parID != null )
								{
									XmlNodeList nlGroups = xmlImport.DocumentElement.SelectNodes("groups");
									foreach ( XmlNode xGroup in nlGroups )
									{
										foreach(IDbDataParameter par in cmdPROSPECT_LISTS_Import.Parameters)
										{
											// 03/27/2010   The ParameterName will start with @, so we need to remove it. 
											string sParameterName = Sql.ExtractDbName(cmdPROSPECT_LISTS_Import, par.ParameterName).ToUpper();
											if ( sParameterName == "TEAM_ID" && bEnableTeamManagement ) // 01/11/2011   Ignore the Required flag. && bRequireTeamManagement )
												par.Value = Sql.ToDBGuid(gTEAM_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
											else if ( sParameterName == "ASSIGNED_USER_ID" ) // 01/11/2011   Always set the Assigned User ID. && bRequireUserAssignment )
												par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
											// 02/20/2013   We need to set the MODIFIED_USER_ID. 
											else if ( sParameterName == "MODIFIED_USER_ID" )
												par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);
											else
												par.Value = DBNull.Value;
										}
										// 05/10/2010   We now support ACT! etime. 
										DateTime dtDATE_MODIFIED = Sql.ToDateTime(XmlUtil.SelectSingleNode(xGroup, "etime"      ));
										string   sNAME           = Sql.ToString  (XmlUtil.SelectSingleNode(xGroup, "grp_name"   )).Trim();
										string   sDESCRIPTION    = Sql.ToString  (XmlUtil.SelectSingleNode(xGroup, "description")).Trim();
										if ( !Sql.IsEmptyString(sNAME) && !hashProspectLists.ContainsKey(sNAME) )
										{
											// 02/04/2010   The modified user is always the person who imported the data. 
											Sql.SetParameter(cmdPROSPECT_LISTS_Import, "@DATE_MODIFIED"   , dtDATE_MODIFIED );
											Sql.SetParameter(cmdPROSPECT_LISTS_Import, "@MODIFIED_USER_ID", Security.USER_ID);
											Sql.SetParameter(cmdPROSPECT_LISTS_Import, "@NAME"            , sNAME           );
											Sql.SetParameter(cmdPROSPECT_LISTS_Import, "@DESCRIPTION"     , sDESCRIPTION    );
											
											sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_Import));
											sbImport.AppendLine(";");
											cmdPROSPECT_LISTS_Import.ExecuteNonQuery();
											
											hashProspectLists.Add(sNAME, Sql.ToGuid(parID.Value).ToString());
										}
									}
								}
							}
							for ( int iRowNumber = 1; i < nlRows.Count ; i++ )
							{
								XmlNode node = nlRows[i];
								int nEmptyColumns = 0;
								for ( int j = 0; j < node.ChildNodes.Count; j++ )
								{
									string sText = node.ChildNodes[j].InnerText;
									if ( sText == String.Empty )
										nEmptyColumns++;
								}
								// 09/04/2006   If all columns are empty, then skip the row. 
								if ( nEmptyColumns == node.ChildNodes.Count )
									continue;
								DataRow row = dtProcessed.NewRow();
								row["IMPORT_ROW_NUMBER"] = iRowNumber ;
								iRowNumber++;
								dtProcessed.Rows.Add(row);
								try
								{
									if ( !Response.IsClientConnected )
									{
										break;
									}
									foreach(IDbDataParameter par in cmdImport.Parameters)
									{
										// 06/04/2009   If Team is required, then make sure to initialize the TEAM_ID.  Same is true for ASSIGNED_USER_ID. 
										// 03/27/2010   The ParameterName will start with @, so we need to remove it. 
										string sParameterName = Sql.ExtractDbName(cmdImport, par.ParameterName).ToUpper();
										if ( sParameterName == "TEAM_ID" && bEnableTeamManagement ) // 01/11/2011   Ignore the Required flag. && bRequireTeamManagement )
											par.Value = Sql.ToDBGuid(Security.TEAM_ID);  // 02/26/2011   Make sure to convert Guid.Empty to DBNull. 
										else if ( sParameterName == "ASSIGNED_USER_ID" ) // 01/11/2011   Always set the Assigned User ID. && bRequireUserAssignment )
											par.Value = Sql.ToDBGuid(Security.USER_ID);  // 02/26/2011   Make sure to convert Guid.Empty to DBNull. 
										// 02/20/2013   We need to set the MODIFIED_USER_ID. 
										else if ( sParameterName == "MODIFIED_USER_ID" )
											par.Value = Sql.ToDBGuid(Security.USER_ID);
										else
											par.Value = DBNull.Value;
									}
									if ( cmdImportCSTM != null )
									{
										foreach(IDbDataParameter par in cmdImportCSTM.Parameters)
										{
											par.Value = DBNull.Value;
										}
									}
									/*
									if ( cmdImportTeam != null )
									{
										foreach(IDbDataParameter par in cmdImportTeam.Parameters)
										{
											par.Value = DBNull.Value;
										}
									}
									*/
									// 09/19/2007   parID and parID_C are frequently used, so obtain outside the import loop. 
									IDbDataParameter parID   = Sql.FindParameter(cmdImport, "ID");
									IDbDataParameter parID_C = null;
									if ( cmdImportCSTM != null )
										parID_C = Sql.FindParameter(cmdImportCSTM, "ID_C");

									// 10/31/2006   The modified user is always the person who imported the data. 
									// 11/01/2006   The real problem with importing a contact is that the SYNC_CONTACT flag was null, and treated as 1. 
									// It still makes sense to set the modified id. 
									Sql.SetParameter(cmdImport, "@MODIFIED_USER_ID", Security.USER_ID);
									foreach(string sName in hashDefaultMappings.Keys)
									{
										string sDefault = Sql.ToString(hashDefaultMappings[sName]);
										if ( !dtProcessed.Columns.Contains(sName) )
										{
											dtProcessed.Columns.Add(sName);
										}
										row["IMPORT_ROW_STATUS" ] = true ;
										row["IMPORT_LAST_COLUMN"] = sName;
										row[sName] = sDefault;
										Sql.SetParameter(cmdImport, sName, sDefault);
										if ( cmdImportCSTM != null )
											Sql.SetParameter(cmdImportCSTM, sName, sDefault);
										//if ( cmdImportTeam != null && sName == "team_id" )
										//	Sql.SetParameter(cmdImportTeam, "@TEAM_ID", sDefault);
									}
									for ( int j = 0; j < node.ChildNodes.Count; j++ )
									{
										string sText = node.ChildNodes[j].InnerText;
										string sName = String.Empty;
										// 08/22/2006   We should always use the header mappings instead of an index as nodes may move around. 
										sName = Sql.ToString(hashHeaderMappings[node.ChildNodes[j].Name]);
										// 09/08/2006   There is no need to set the field if the value is empty. 
										if ( sName.Length > 0 && sText.Length > 0 )
										{
											sName = sName.ToUpper();
											// 08/20/2006   Fix IDs. 
											// 09/30/2006   CREATED_BY counts as an ID. 
											if ( sName == "ID" || sName.EndsWith("_ID") || sName == "CREATED_BY" )
											{
												// 09/30/2006   IDs must be in upper case.  This is primarily for platforms that are case-significant. 
												// 10/05/2006   We need to use upper case for SQL Server as well so that the SugarCRM user names are correctly replaced. 
												sText = sText.ToUpper();
												if ( sText.Length < 36 && sText.Length > 0 )
												{
													sText = "00000000-0000-0000-0000-000000000000".Substring(0, 36 - sText.Length) + sText;
													switch ( sText )
													{
														case "00000000-0000-0000-0000-000000JIM_ID":  sText = "00000000-0000-0000-0001-000000000000";  break;
														case "00000000-0000-0000-0000-000000MAX_ID":  sText = "00000000-0000-0000-0002-000000000000";  break;
														case "00000000-0000-0000-0000-00000WILL_ID":  sText = "00000000-0000-0000-0003-000000000000";  break;
														case "00000000-0000-0000-0000-0000CHRIS_ID":  sText = "00000000-0000-0000-0004-000000000000";  break;
														case "00000000-0000-0000-0000-0000SALLY_ID":  sText = "00000000-0000-0000-0005-000000000000";  break;
														case "00000000-0000-0000-0000-0000SARAH_ID":  sText = "00000000-0000-0000-0006-000000000000";  break;
														// 11/30/2006   The following mappings will really only help when importing SugarCRM sample data. 
														case "00000000-0000-0000-0000-000000000001":  sText = "00000000-0000-0001-0000-000000000000";  break;
														case "00000000-0000-0000-0000-0PRIVATE.JIM":  sText = "00000000-0000-0001-0001-000000000000";  break;
														case "00000000-0000-0000-0000-0PRIVATE.MAX":  sText = "00000000-0000-0001-0002-000000000000";  break;
														case "00000000-0000-0000-0000-PRIVATE.WILL":  sText = "00000000-0000-0001-0003-000000000000";  break;
														case "00000000-0000-0000-0000PRIVATE.CHRIS":  sText = "00000000-0000-0001-0004-000000000000";  break;
														case "00000000-0000-0000-0000PRIVATE.SALLY":  sText = "00000000-0000-0001-0005-000000000000";  break;
														case "00000000-0000-0000-0000PRIVATE.SARAH":  sText = "00000000-0000-0001-0006-000000000000";  break;
														case "00000000-0000-0000-0000-00000000EAST":  sText = "00000000-0000-0001-0101-000000000000";  break;
														case "00000000-0000-0000-0000-00000000WEST":  sText = "00000000-0000-0001-0102-000000000000";  break;
														case "00000000-0000-0000-0000-0000000NORTH":  sText = "00000000-0000-0001-0103-000000000000";  break;
														case "00000000-0000-0000-0000-0000000SOUTH":  sText = "00000000-0000-0001-0104-000000000000";  break;
														// 07/09/2010   New IDs used in a prepopulated SugarCRM database. 
														case "00000000-0000-0000-0000-0SEED_JIM_ID":  sText = "00000000-0000-0000-0011-000000000000";  break;
														case "00000000-0000-0000-0000-0SEED_MAX_ID":  sText = "00000000-0000-0000-0012-000000000000";  break;
														case "00000000-0000-0000-0000-SEED_WILL_ID":  sText = "00000000-0000-0000-0013-000000000000";  break;
														case "00000000-0000-0000-0000SEED_CHRIS_ID":  sText = "00000000-0000-0000-0014-000000000000";  break;
														case "00000000-0000-0000-0000SEED_SALLY_ID":  sText = "00000000-0000-0000-0015-000000000000";  break;
														case "00000000-0000-0000-0000SEED_SARAH_ID":  sText = "00000000-0000-0000-0016-000000000000";  break;
													}
												}
											}
											// 02/20/2008   Most modules have the TEAM_ID in the main update procedure, 
											// so we need to translate the TEAM_NAME to TEAM_ID inside this loop. 
											else if ( sName == "TEAM_NAME" && Crm.Config.enable_team_management() )
											{
												Guid gTEAM_ID = Guid.Empty;
												string sTEAM_NAME = sText.Trim().ToUpper();
												if ( hashTeamNames.ContainsKey(sTEAM_NAME) )
												{
													gTEAM_ID = Sql.ToGuid(hashTeamNames[sTEAM_NAME]);
												}
												sName = "TEAM_ID";
												sText = gTEAM_ID.ToString();
											}
											if ( !dtProcessed.Columns.Contains(sName) )
											{
												dtProcessed.Columns.Add(sName);
											}
											row["IMPORT_ROW_STATUS" ] = true ;
											row["IMPORT_LAST_COLUMN"] = sName;
											row[sName] = sText;
											Sql.SetParameter(cmdImport, sName, sText);
											if ( cmdImportCSTM != null )
												Sql.SetParameter(cmdImportCSTM, sName, sText);
										}
									}
									
									// 09/17/2013   Add Business Rules to import. 
									// Apply rules before Required Fields or Duplicates check. 
									// For efficiency, don't apply rules engine if no rules were defined. 
									if ( rules != null && dtRules != null && dtRules.Rows.Count > 0 )
									{
										row["IMPORT_LAST_COLUMN"] = "Business Rules Engine";
										SplendidImportThis swThis = new SplendidImportThis(L10n, sImportModule, row, cmdImport, cmdImportCSTM);
										RuleExecution exec = new RuleExecution(validation, swThis);
										rules.Execute(exec);
									}
									
									StringBuilder sbRequiredFieldErrors = new StringBuilder();
									foreach ( string sRequiredField in hashRequiredFields.Keys )
									{
										IDbDataParameter par = Sql.FindParameter(cmdImport, sRequiredField);
										if ( par == null && cmdImportCSTM != null )
											par = Sql.FindParameter(cmdImportCSTM, sRequiredField);
										if ( par != null )
										{
											if ( par.Value == DBNull.Value || par.Value.ToString() == String.Empty )
											{
												// 02/05/2010   If this is an ACT! import of contacts, then there may not be a Last Name. 
												// In this case, use the Account Name as we want to keep the record. 
												if ( radACT_2005.Checked && sRequiredField == "LAST_NAME" && sImportModule == "Contacts" )
												{
													IDbDataParameter parACCOUNT_NAME = Sql.FindParameter(cmdImport, "ACCOUNT_NAME");
													if ( parACCOUNT_NAME != null )
													{
														// 02/05/2010   Check the value not the parameter. 
														if ( !Sql.IsEmptyString(parACCOUNT_NAME.Value) )
														{
															par.Value = parACCOUNT_NAME.Value;
															continue;
														}
													}
												}
												// 11/02/2006   If ACCOUNT_ID is required, then also allow ACCOUNT_NAME. 
												else if ( sRequiredField == "ACCOUNT_ID" && (sImportModule == "Cases " || sImportModule == "Opportunities") )
												{
													par = Sql.FindParameter(cmdImport, "ACCOUNT_NAME");
													if ( par != null )
													{
														if ( par.Value != DBNull.Value && par.Value.ToString() != String.Empty )
														{
															continue;
														}
													}
												}
												if ( sbRequiredFieldErrors.Length > 0 )
													sbRequiredFieldErrors.Append(", ");
												if ( hashColumns.ContainsKey(sRequiredField) )
													sbRequiredFieldErrors.Append(hashColumns[sRequiredField]);
												else
													sbRequiredFieldErrors.Append(sRequiredField);
											}
										}
									}
									// 12/17/2008   Now that all the data is available in cmdImport, we can use the data in a filter. 
									if ( hashDuplicateFilters.Count > 0 )
									{
										string sSQL = String.Empty;
										sSQL = "select count(*)        " + ControlChars.CrLf
										     + "  from vw" + sTABLE_NAME + ControlChars.CrLf
										     + " where 1 = 1           " + ControlChars.CrLf;
										using ( IDbCommand cmdDuplicate = con.CreateCommand() )
										{
											cmdDuplicate.Transaction = trn;
											cmdDuplicate.CommandText = sSQL;
											foreach ( string sDuplicateField in hashDuplicateFilters.Keys )
											{
												string sFieldName = sDuplicateField.ToUpper();
												IDbDataParameter par = Sql.FindParameter(cmdImport, sFieldName);
												if ( par == null )
												{
													par = Sql.FindParameter(cmdImportCSTM, sFieldName);
												}
												if ( par != null )
												{
													if ( par.Value == DBNull.Value )
													{
														cmdDuplicate.CommandText += "   and " + sFieldName + " is null" + ControlChars.CrLf;
													}
													else
													{
														cmdDuplicate.CommandText += "   and " + sFieldName + " = @" + sFieldName + ControlChars.CrLf;
														IDbDataParameter parDup = Sql.CreateParameter(cmdDuplicate, "@" + sFieldName);
														parDup.DbType    = par.DbType   ;
														parDup.Size      = par.Size     ;
														parDup.Scale     = par.Scale    ;
														parDup.Precision = par.Precision;
														parDup.Value     = par.Value    ;
													}
												}
											}
											sbImport.Append(Sql.ExpandParameters(cmdDuplicate));
											sbImport.AppendLine(";");
											
											int nDuplicateCount = Sql.ToInteger(cmdDuplicate.ExecuteScalar());
											if ( nDuplicateCount > 0 )
											{
												nDuplicates++;
												row["IMPORT_ROW_STATUS"] = false;
												row["IMPORT_ROW_ERROR" ] = L10n.Term("Import.ERR_DUPLICATE_FIELDS") + " " + sbDuplicateFilters.ToString();
												continue;
											}
										}
									}
									if ( sbRequiredFieldErrors.Length > 0 )
									{
										row["IMPORT_ROW_STATUS"] = false;
										row["IMPORT_ROW_ERROR" ] = L10n.Term("Import.ERR_MISSING_REQUIRED_FIELDS") + " " + sbRequiredFieldErrors.ToString();
										nFailed++;
										// 10/31/2006   Abort after 200 errors. 
										if ( nFailed >= nMAX_ERRORS )
										{
											ctlDynamicButtons.ErrorText += L10n.Term("Import.LBL_MAX_ERRORS");
											break;
										}
									}
									else
									{
										sbImport.Append(Sql.ExpandParameters(cmdImport));
										sbImport.AppendLine(";");
										cmdImport.ExecuteNonQuery();
										if ( parID != null )
										{
											row["ID"] = parID.Value;

											Guid gID = Sql.ToGuid(parID.Value);
											if ( cmdImportCSTM != null && parID_C != null )
											{
												parID_C.Value = gID;
												sbImport.Append(Sql.ExpandParameters(cmdImportCSTM));
												sbImport.AppendLine(";");
												cmdImportCSTM.ExecuteNonQuery();
											}
											if ( radACT_2005.Checked )
											{
												// 02/02/2010   Notes and Activities should assume the owner of the parent record. 
												Guid gTEAM_ID          = Security.TEAM_ID;
												Guid gASSIGNED_USER_ID = Security.USER_ID;
												// 03/27/2010   Use FindParameter as the Parameter Name may start with @. 
												IDbDataParameter parTEAM_ID          = Sql.FindParameter(cmdImport, "@TEAM_ID"         );
												IDbDataParameter parASSIGNED_USER_ID = Sql.FindParameter(cmdImport, "@ASSIGNED_USER_ID");
												if ( parTEAM_ID != null )
													gTEAM_ID = Sql.ToGuid(parTEAM_ID.Value);
												if ( parASSIGNED_USER_ID != null )
													gASSIGNED_USER_ID = Sql.ToGuid(parASSIGNED_USER_ID.Value);
												// 02/02/2010   If this is an ACT! import, then we need to look for Notes and Activities. 
												if ( cmdNOTES_Import != null )
												{
													// 02/04/2010   The Note and Activity import must also be part of the transaction. 
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdNOTES_Import.Transaction = trn;
													}
													XmlNodeList nlNotes = node.SelectNodes("notes");
													foreach ( XmlNode xNote in nlNotes )
													{
														foreach(IDbDataParameter par in cmdNOTES_Import.Parameters)
														{
															// 03/27/2010   The ParameterName will start with @, so we need to remove it. 
															string sParameterName = Sql.ExtractDbName(cmdNOTES_Import, par.ParameterName).ToUpper();
															if ( sParameterName == "TEAM_ID" && bEnableTeamManagement ) // 01/11/2011   Ignore the Required flag. && bRequireTeamManagement )
																par.Value = Sql.ToDBGuid(gTEAM_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															else if ( sParameterName == "ASSIGNED_USER_ID" ) // 01/11/2011   Always set the Assigned User ID. && bRequireUserAssignment )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															// 02/20/2013   We need to set the MODIFIED_USER_ID. 
															else if ( sParameterName == "MODIFIED_USER_ID" )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);
															else
																par.Value = DBNull.Value;
														}
														DateTime dtDATE_MODIFIED = Sql.ToDateTime(XmlUtil.SelectSingleNode(xNote, "user_time"  ));
														string   sDESCRIPTION    = Sql.ToString  (XmlUtil.SelectSingleNode(xNote, "description")).Trim();
														string   sNAME           = sDESCRIPTION;
														if ( sNAME.IndexOf(ControlChars.CrLf) > 0 )
															sNAME = sNAME.Substring(0, sNAME.IndexOf(ControlChars.CrLf));
														if ( Sql.IsEmptyString(sNAME) )
															sNAME = "Note";
														// 02/04/2010   The modified user is always the person who imported the data. 
														Sql.SetParameter(cmdNOTES_Import, "@MODIFIED_USER_ID", Security.USER_ID);
														Sql.SetParameter(cmdNOTES_Import, "@DATE_MODIFIED"   , dtDATE_MODIFIED );
														Sql.SetParameter(cmdNOTES_Import, "@NAME"            , sNAME           );
														Sql.SetParameter(cmdNOTES_Import, "@PARENT_TYPE"     , sImportModule   );
														Sql.SetParameter(cmdNOTES_Import, "@PARENT_ID"       , gID             );
														Sql.SetParameter(cmdNOTES_Import, "@DESCRIPTION"     , sDESCRIPTION    );
														
														sbImport.Append(Sql.ExpandParameters(cmdNOTES_Import));
														sbImport.AppendLine(";");
														cmdNOTES_Import.ExecuteNonQuery();
													}
												}
												if ( cmdCALLS_Import != null && cmdMEETINGS_Import != null )
												{
													// 02/04/2010   The Note and Activity import must also be part of the transaction. 
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdCALLS_Import.Transaction = trn;
														cmdMEETINGS_Import.Transaction = trn;
													}
													XmlNodeList nlActivities = node.SelectNodes("activities");
													foreach ( XmlNode xActivity in nlActivities )
													{
														foreach(IDbDataParameter par in cmdCALLS_Import.Parameters)
														{
															// 03/27/2010   The ParameterName will start with @, so we need to remove it. 
															string sParameterName = Sql.ExtractDbName(cmdCALLS_Import, par.ParameterName).ToUpper();
															if ( sParameterName == "TEAM_ID" && bEnableTeamManagement ) // 01/11/2011   Ignore the Required flag. && bRequireTeamManagement )
																par.Value = Sql.ToDBGuid(gTEAM_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															else if ( sParameterName == "ASSIGNED_USER_ID" ) // 01/11/2011   Always set the Assigned User ID. && bRequireUserAssignment )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															// 02/20/2013   We need to set the MODIFIED_USER_ID. 
															else if ( sParameterName == "MODIFIED_USER_ID" )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);
															else
																par.Value = DBNull.Value;
														}
														foreach(IDbDataParameter par in cmdMEETINGS_Import.Parameters)
														{
															// 03/27/2010   The ParameterName will start with @, so we need to remove it. 
															string sParameterName = Sql.ExtractDbName(cmdMEETINGS_Import, par.ParameterName).ToUpper();
															if ( sParameterName == "TEAM_ID" && bEnableTeamManagement ) // 01/11/2011   Ignore the Required flag. && bRequireTeamManagement )
																par.Value = Sql.ToDBGuid(gTEAM_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															else if ( sParameterName == "ASSIGNED_USER_ID" ) // 01/11/2011   Always set the Assigned User ID. && bRequireUserAssignment )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);  // 01/10/2011   Make sure to convert Guid.Empty to DBNull. 
															// 02/20/2013   We need to set the MODIFIED_USER_ID. 
															else if ( sParameterName == "MODIFIED_USER_ID" )
																par.Value = Sql.ToDBGuid(gASSIGNED_USER_ID);
															else
																par.Value = DBNull.Value;
														}
														int      nTYPE             = Sql.ToInteger (XmlUtil.SelectSingleNode(xActivity, "type"      ));
														DateTime dtSTART_TIME      = Sql.ToDateTime(XmlUtil.SelectSingleNode(xActivity, "start_time"));
														DateTime dtEND_TIME        = Sql.ToDateTime(XmlUtil.SelectSingleNode(xActivity, "end_time"  ));
														// 02/04/2010   An activity does not have a user_time, so use the start time. 
														// 05/10/2010   We now support ACT! etime. 
														DateTime dtDATE_MODIFIED   = Sql.ToDateTime(XmlUtil.SelectSingleNode(xActivity, "etime"     ));
														int      nREMINDER_TIME    = Sql.ToInteger (XmlUtil.SelectSingleNode(xActivity, "lead_time" ));
														int      nDURATION         = Sql.ToInteger (XmlUtil.SelectSingleNode(xActivity, "duration"  ));
														int      nDURATION_HOURS   = nDURATION / 60;
														int      nDURATION_MINUTES = nDURATION % 60;
														string   sDESCRIPTION      = Sql.ToString  (XmlUtil.SelectSingleNode(xActivity, "description")).Trim();
														string   sNAME             = Sql.ToString  (XmlUtil.SelectSingleNode(xActivity, "regarding"  ));
														if ( Sql.IsEmptyString(sNAME) )
															sNAME = "Activity";
														if ( nTYPE == 0 )  // Call when TYPE == 0. 
														{
															// 02/04/2010   The modified user is always the person who imported the data. 
															Sql.SetParameter(cmdCALLS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
															Sql.SetParameter(cmdCALLS_Import, "@DATE_MODIFIED"   , dtDATE_MODIFIED  );
															Sql.SetParameter(cmdCALLS_Import, "@NAME"            , sNAME            );
															Sql.SetParameter(cmdCALLS_Import, "@DURATION_HOURS"  , nDURATION_HOURS  );
															Sql.SetParameter(cmdCALLS_Import, "@DURATION_MINUTES", nDURATION_MINUTES);
															Sql.SetParameter(cmdCALLS_Import, "@DATE_TIME"       , dtSTART_TIME     );
															Sql.SetParameter(cmdCALLS_Import, "@PARENT_TYPE"     , sImportModule    );
															Sql.SetParameter(cmdCALLS_Import, "@PARENT_ID"       , gID              );
															Sql.SetParameter(cmdCALLS_Import, "@REMINDER_TIME"   , nREMINDER_TIME   );
															Sql.SetParameter(cmdCALLS_Import, "@DESCRIPTION"     , sDESCRIPTION     );
															
															sbImport.Append(Sql.ExpandParameters(cmdCALLS_Import));
															sbImport.AppendLine(";");
															cmdCALLS_Import.ExecuteNonQuery();
														}
														else // Meeting when TYPE == 1, TO-DO when TYPE == 2. 
														{
															// 02/04/2010   The modified user is always the person who imported the data. 
															Sql.SetParameter(cmdMEETINGS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
															Sql.SetParameter(cmdMEETINGS_Import, "@DATE_MODIFIED"   , dtDATE_MODIFIED  );
															Sql.SetParameter(cmdMEETINGS_Import, "@NAME"            , sNAME            );
															Sql.SetParameter(cmdMEETINGS_Import, "@DURATION_HOURS"  , nDURATION_HOURS  );
															Sql.SetParameter(cmdMEETINGS_Import, "@DURATION_MINUTES", nDURATION_MINUTES);
															Sql.SetParameter(cmdMEETINGS_Import, "@DATE_TIME"       , dtSTART_TIME     );
															Sql.SetParameter(cmdMEETINGS_Import, "@PARENT_TYPE"     , sImportModule    );
															Sql.SetParameter(cmdMEETINGS_Import, "@PARENT_ID"       , gID              );
															Sql.SetParameter(cmdMEETINGS_Import, "@REMINDER_TIME"   , nREMINDER_TIME   );
															Sql.SetParameter(cmdMEETINGS_Import, "@DESCRIPTION"     , sDESCRIPTION     );
															
															sbImport.Append(Sql.ExpandParameters(cmdMEETINGS_Import));
															sbImport.AppendLine(";");
															cmdMEETINGS_Import.ExecuteNonQuery();
														}
													}
												}
												if ( cmdPROSPECT_LISTS_CONTACTS_Import != null )
												{
													// 02/04/2010   The Note and Activity import must also be part of the transaction. 
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_CONTACTS_Import.Transaction = trn;
													}
													XmlNodeList nlProspectLists = node.SelectNodes("prospect_lists");
													foreach ( XmlNode xProspectList in nlProspectLists )
													{
														string sNAME = Sql.ToString(XmlUtil.SelectSingleNode(xProspectList, "name"));
														if ( hashProspectLists.ContainsKey(sNAME) )
														{
															Guid gPROSPECT_LIST_ID = Sql.ToGuid(hashProspectLists[sNAME]);
															foreach(IDbDataParameter par in cmdPROSPECT_LISTS_CONTACTS_Import.Parameters)
															{
																par.Value = DBNull.Value;
															}
															// 02/04/2010   The modified user is always the person who imported the data. 
															Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
															Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
															Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@CONTACT_ID"      , gID              );
															
															sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_CONTACTS_Import));
															sbImport.AppendLine(";");
															cmdPROSPECT_LISTS_CONTACTS_Import.ExecuteNonQuery();
														}
													}
												}
												// 01/11/2011   Use a separate procedure as it has different parameters. 
												if ( cmdPROSPECT_LISTS_LEADS_Import != null )
												{
													// 02/04/2010   The Note and Activity import must also be part of the transaction. 
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_LEADS_Import.Transaction = trn;
													}
													XmlNodeList nlProspectLists = node.SelectNodes("prospect_lists");
													foreach ( XmlNode xProspectList in nlProspectLists )
													{
														string sNAME = Sql.ToString(XmlUtil.SelectSingleNode(xProspectList, "name"));
														if ( hashProspectLists.ContainsKey(sNAME) )
														{
															Guid gPROSPECT_LIST_ID = Sql.ToGuid(hashProspectLists[sNAME]);
															foreach(IDbDataParameter par in cmdPROSPECT_LISTS_LEADS_Import.Parameters)
															{
																par.Value = DBNull.Value;
															}
															// 02/04/2010   The modified user is always the person who imported the data. 
															Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
															Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
															Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@LEAD_ID"         , gID              );
															
															sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_LEADS_Import));
															sbImport.AppendLine(";");
															cmdPROSPECT_LISTS_LEADS_Import.ExecuteNonQuery();
														}
													}
												}
												// 01/11/2011   Use a separate procedure as it has different parameters. 
												if ( cmdPROSPECT_LISTS_PROSPECTS_Import != null )
												{
													// 02/04/2010   The Note and Activity import must also be part of the transaction. 
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_PROSPECTS_Import.Transaction = trn;
													}
													XmlNodeList nlProspectLists = node.SelectNodes("prospect_lists");
													foreach ( XmlNode xProspectList in nlProspectLists )
													{
														string sNAME = Sql.ToString(XmlUtil.SelectSingleNode(xProspectList, "name"));
														if ( hashProspectLists.ContainsKey(sNAME) )
														{
															Guid gPROSPECT_LIST_ID = Sql.ToGuid(hashProspectLists[sNAME]);
															foreach(IDbDataParameter par in cmdPROSPECT_LISTS_PROSPECTS_Import.Parameters)
															{
																par.Value = DBNull.Value;
															}
															// 02/04/2010   The modified user is always the person who imported the data. 
															Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
															Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
															Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@PROSPECT_ID"     , gID              );
															
															sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_PROSPECTS_Import));
															sbImport.AppendLine(";");
															cmdPROSPECT_LISTS_PROSPECTS_Import.ExecuteNonQuery();
														}
													}
												}
											}
											// 09/06/2012   Allow direct import into prospect list. 
											else if ( (sImportModule == "Contacts" || sImportModule == "Leads" || sImportModule == "Prospects") && !Sql.IsEmptyGuid(ViewState["PROSPECT_LIST_ID"]) )
											{
												Guid gPROSPECT_LIST_ID = Sql.ToGuid(ViewState["PROSPECT_LIST_ID"]);
												if ( cmdPROSPECT_LISTS_CONTACTS_Import != null )
												{
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_CONTACTS_Import.Transaction = trn;
													}
													foreach(IDbDataParameter par in cmdPROSPECT_LISTS_CONTACTS_Import.Parameters)
													{
														par.Value = DBNull.Value;
													}
													Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
													Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
													Sql.SetParameter(cmdPROSPECT_LISTS_CONTACTS_Import, "@CONTACT_ID"      , gID              );
													
													sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_CONTACTS_Import));
													sbImport.AppendLine(";");
													cmdPROSPECT_LISTS_CONTACTS_Import.ExecuteNonQuery();
												}
												if ( cmdPROSPECT_LISTS_LEADS_Import != null )
												{
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_LEADS_Import.Transaction = trn;
													}
													foreach(IDbDataParameter par in cmdPROSPECT_LISTS_LEADS_Import.Parameters)
													{
														par.Value = DBNull.Value;
													}
													Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
													Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
													Sql.SetParameter(cmdPROSPECT_LISTS_LEADS_Import, "@LEAD_ID"         , gID              );
													
													sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_LEADS_Import));
													sbImport.AppendLine(";");
													cmdPROSPECT_LISTS_LEADS_Import.ExecuteNonQuery();
												}
												if ( cmdPROSPECT_LISTS_PROSPECTS_Import != null )
												{
													if ( chkUseTransaction.Checked || bPreview )
													{
														cmdPROSPECT_LISTS_PROSPECTS_Import.Transaction = trn;
													}
													foreach(IDbDataParameter par in cmdPROSPECT_LISTS_PROSPECTS_Import.Parameters)
													{
														par.Value = DBNull.Value;
													}
													Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@MODIFIED_USER_ID", Security.USER_ID );
													Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@PROSPECT_LIST_ID", gPROSPECT_LIST_ID);
													Sql.SetParameter(cmdPROSPECT_LISTS_PROSPECTS_Import, "@PROSPECT_ID"     , gID              );
													
													sbImport.Append(Sql.ExpandParameters(cmdPROSPECT_LISTS_PROSPECTS_Import));
													sbImport.AppendLine(";");
													cmdPROSPECT_LISTS_PROSPECTS_Import.ExecuteNonQuery();
												}
											}
										}
										nImported++;
										row["IMPORT_LAST_COLUMN"] = DBNull.Value;
									}
									Response.Write(" ");
								}
								catch(Exception ex)
								{
									row["IMPORT_ROW_STATUS"] = false;
									row["IMPORT_ROW_ERROR" ] = L10n.Term("Import.LBL_ERROR") + " " + Sql.ToString(row["IMPORT_LAST_COLUMN"]) + ". " + ex.Message;
									nFailed++;
									// 10/31/2006   Abort after 200 errors. 
									if ( nFailed >= nMAX_ERRORS )
									{
										ctlDynamicButtons.ErrorText += L10n.Term("Import.LBL_MAX_ERRORS");
										break;
									}
								}
							}
							// 10/29/2006   Save the processed table so that the result can be browsed. 
							string sProcessedFileID   = Guid.NewGuid().ToString();
							string sProcessedFileName = Security.USER_ID.ToString() + " " + Guid.NewGuid().ToString() + ".xml";
							DataSet dsProcessed = new DataSet();
							dsProcessed.Tables.Add(dtProcessed);
							dsProcessed.WriteXml(Path.Combine(Path.GetTempPath(), sProcessedFileName), XmlWriteMode.WriteSchema);
							Session["TempFile." + sProcessedFileID] = sProcessedFileName;
							ViewState["ProcessedFileID"] = sProcessedFileID;

							// 10/31/2006   The transaction should rollback if it is not explicitly committed. 
							// Manually rolling back is causing a timeout. 
							//if ( bPreview || nFailed > 0 )
							//	trn.Rollback();
							//else
							if ( trn != null && !bPreview && nFailed == 0 )
							{
								trn.Commit();
							}
						}
						catch(Exception ex)
						{
							// 10/31/2006   The transaction should rollback if it is not explicitly committed. 
							//if ( trn.Connection != null )
							//	trn.Rollback();
							// 10/31/2006   Don't throw this exception.  We want to be able to display the failed count. 
							nFailed++;
							//throw(new Exception(ex.Message, ex.InnerException));
							ctlDynamicButtons.ErrorText += ex.Message;
						}
						finally
						{
							if ( trn != null )
								trn.Dispose();
						}
					}
				}
				lblStatus.Text = String.Empty;
				// 03/20/2011   Include a preview indicator. 
				if ( bPreview )
					lblStatus.Text += L10n.Term("Import.LBL_PREVIEW_BUTTON_LABEL") + " ";
				if ( nFailed == 0 )
					lblStatus.Text += L10n.Term("Import.LBL_SUCCESS");
				else
					lblStatus.Text += L10n.Term("Import.LBL_FAIL"   );
				lblSuccessCount  .Text = nImported.ToString()   + " " + L10n.Term("Import.LBL_SUCCESSFULLY" );
				lblFailedCount   .Text = nFailed.ToString()     + " " + L10n.Term("Import.LBL_FAILED_IMPORT");
				lblDuplicateCount.Text = nDuplicates.ToString() + " " + L10n.Term("Import.LBL_DUPLICATES_IGNORED");

				grdMain.SortColumn = "IMPORT_ROW_STATUS, IMPORT_ROW_NUMBER";
				grdMain.SortOrder  = "asc" ;
				PreviewGrid(dtProcessed);
			}
			catch ( Exception ex )
			{
				ctlDynamicButtons.ErrorText += ex.Message;
			}
		}

		private void PreviewGrid(DataTable dtProcessed)
		{
			vwColumns.Sort = "DISPLAY_NAME";
			Hashtable hashColumns = new Hashtable();
			foreach ( DataRowView row in vwColumns )
				hashColumns.Add(row["NAME"], row["DISPLAY_NAME"]);

			// 10/31/2006   Always reset columns before adding them. 
			grdMain.Columns.Clear();
			BoundColumn bnd = new BoundColumn();
			bnd.DataField  = "IMPORT_ROW_NUMBER";
			bnd.SortExpression = bnd.DataField;
			bnd.HeaderText = L10n.Term("Import.LBL_ROW");
			grdMain.Columns.Add(bnd);

			bnd = new BoundColumn();
			bnd.DataField  = "IMPORT_ROW_ERROR";
			bnd.SortExpression = bnd.DataField;
			bnd.HeaderText = L10n.Term("Import.LBL_ROW_STATUS");
			grdMain.Columns.Add(bnd);

			for ( int i = 4; i < dtProcessed.Columns.Count; i++ )
			{
				bnd = new BoundColumn();
				bnd.DataField = dtProcessed.Columns[i].ColumnName;
				bnd.SortExpression = bnd.DataField;
				if ( hashColumns.ContainsKey(bnd.DataField) )
					bnd.HeaderText = hashColumns[bnd.DataField] as string;
				else
					bnd.HeaderText = bnd.DataField;
				grdMain.Columns.Add(bnd);
			}
			
			grdMain.DataSource = new DataView(dtProcessed);
			grdMain.ApplySort();
			grdMain.DataBind();
		}

		private void GetOAuthAccessTokens()
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				con.Open();
				string sSQL;
				sSQL = "select *                                   " + ControlChars.CrLf
				     + "  from vwOAUTH_TOKENS                      " + ControlChars.CrLf
				     + " where NAME             = @NAME            " + ControlChars.CrLf
				     + "   and ASSIGNED_USER_ID = @ASSIGNED_USER_ID" + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@NAME"            , this.SourceType());
					Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);
					if ( bDebug )
						RegisterClientScriptBlock("vwOAUTH_TOKENS", Sql.ClientScriptBlock(cmd));

					using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
					{
						if ( rdr.Read() )
						{
							txtOAUTH_ACCESS_TOKEN .Value = Sql.ToString(rdr["TOKEN" ]);
							txtOAUTH_ACCESS_SECRET.Value = Sql.ToString(rdr["SECRET"]);
						}
					}
				}
			}
		}
		#endregion

		// 09/17/2013   Add Business Rules to import. 
		#region Business Rules Helper methods
		protected void ResetRuleText()
		{
			txtRULE_ID     .Value         = String.Empty;
			txtRULE_NAME   .Text          = String.Empty;
			txtPRIORITY    .Text          = "0";
			lstREEVALUATION.SelectedIndex = 0;
			chkACTIVE      .Checked       = true;
			txtCONDITION   .Text          = String.Empty;
			txtTHEN_ACTIONS.Text          = String.Empty;
			txtELSE_ACTIONS.Text          = String.Empty;
		}

		protected void RulesGet(Guid gID, ref string sRULE_NAME, ref int nPRIORITY, ref string sREEVALUATION, ref bool bACTIVE, ref string sCONDITION, ref string sTHEN_ACTIONS, ref string sELSE_ACTIONS)
		{
			DataView vwRules = new DataView(dtRules);
			vwRules.RowFilter = "ID = '" + gID.ToString() + "'";
			if ( vwRules.Count > 0 )
			{
				sRULE_NAME    = Sql.ToString (vwRules[0]["RULE_NAME"   ]);
				nPRIORITY     = Sql.ToInteger(vwRules[0]["PRIORITY"    ]);
				sREEVALUATION = Sql.ToString (vwRules[0]["REEVALUATION"]);
				bACTIVE       = Sql.ToBoolean(vwRules[0]["ACTIVE"      ]);
				sCONDITION    = Sql.ToString (vwRules[0]["CONDITION"   ]);
				sTHEN_ACTIONS = Sql.ToString (vwRules[0]["THEN_ACTIONS"]);
				sELSE_ACTIONS = Sql.ToString (vwRules[0]["ELSE_ACTIONS"]);
			}
		}

		protected void RulesUpdate(Guid gID, string sRULE_NAME, int nPRIORITY, string sREEVALUATION, bool bACTIVE, string sCONDITION, string sTHEN_ACTIONS, string sELSE_ACTIONS)
		{
			DataView vwRules = new DataView(dtRules);
			vwRules.RowFilter = "ID = '" + gID.ToString() + "'";
			try
			{
				if ( vwRules.Count > 0 )
				{
					vwRules[0]["RULE_NAME"   ] = sRULE_NAME   ;
					vwRules[0]["PRIORITY"    ] = nPRIORITY    ;
					vwRules[0]["REEVALUATION"] = sREEVALUATION;
					vwRules[0]["ACTIVE"      ] = bACTIVE      ;
					vwRules[0]["CONDITION"   ] = sCONDITION   ;
					vwRules[0]["THEN_ACTIONS"] = sTHEN_ACTIONS;
					vwRules[0]["ELSE_ACTIONS"] = sELSE_ACTIONS;
				}
				else
				{
					DataRow row = dtRules.NewRow();
					dtRules.Rows.Add(row);
					row["ID"          ] = Guid.NewGuid();
					row["RULE_NAME"   ] = sRULE_NAME   ;
					row["PRIORITY"    ] = nPRIORITY    ;
					row["REEVALUATION"] = sREEVALUATION;
					row["ACTIVE"      ] = bACTIVE      ;
					row["CONDITION"   ] = sCONDITION   ;
					row["THEN_ACTIONS"] = sTHEN_ACTIONS;
					row["ELSE_ACTIONS"] = sELSE_ACTIONS;
				}
				dgRules.DataSource = dtRules;
				dgRules.DataBind();
			}
			catch(Exception ex)
			{
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		protected void RulesDelete(Guid gID)
		{
			dgRules.EditItemIndex = -1;
			for ( int i = 0; i < dtRules.Rows.Count; i++ )
			{
				DataRow row = dtRules.Rows[i];
				if ( gID == Sql.ToGuid(row["ID"]) )
				{
					row.Delete();
					break;
				}
			}
			dtRules.AcceptChanges();
			dgRules.DataSource = dtRules;
			dgRules.DataBind();
		}
		#endregion

		protected void Page_Command(Object sender, CommandEventArgs e)
		{
			try
			{
				if ( e.CommandName == "Import.Load" )
				{
					gID = Sql.ToGuid(e.CommandArgument);
					Response.Redirect(Request.Path + "?ID=" + gID.ToString());
				}
				else if ( e.CommandName == "Import.Delete" )
				{
					gID = Sql.ToGuid(e.CommandArgument);
					SqlProcs.spIMPORT_MAPS_Delete(gID);
					BindSaved();
					txtACTIVE_TAB.Value = "1";
					ViewState["ID"] = Guid.Empty;
				}
				else if ( e.CommandName == "Import.Save" )
				{
					reqNAME.Enabled = true;
					reqNAME.Validate();
					if ( Page.IsValid )
					{
						// 03/16/2010   The ViewState will be the primary location for the ID. 
						gID = Sql.ToGuid(ViewState["ID"]);
						// 10/12/2006   Save the sample data with the mappings. 
						XmlUtil.SetSingleNode(xmlMapping, "Sample", xml.OuterXml);
						
						// 09/17/2013   Add Business Rules to import. 
						StringBuilder sbRulesXML = new StringBuilder();
						if ( dtRules != null && dtRules.Rows.Count > 0 )
						{
							SplendidRulesTypeProvider typeProvider = new SplendidRulesTypeProvider();
							RuleValidation validation = new RuleValidation(typeof(SplendidImportThis), typeProvider);
							RuleSet rules = RulesUtil.BuildRuleSet(dtRules, validation);
						
							string sXOML = RulesUtil.Serialize(rules);
							using ( StringWriter wtr = new StringWriter(sbRulesXML, System.Globalization.CultureInfo.InvariantCulture) )
							{
								dtRules.WriteXml(wtr, XmlWriteMode.WriteSchema, false);
							}
						}
						SqlProcs.spIMPORT_MAPS_Update
							( ref gID
							, Security.USER_ID
							, txtNAME.Text
							, SourceType()
							, sImportModule
							, chkHasHeader.Checked
							, false
							, xmlMapping.OuterXml
							, sbRulesXML.ToString()
							);
						XmlUtil.SetSingleNode(xmlMapping, "Sample", String.Empty);
						// 03/16/2010   Preserve the name.
						//txtNAME.Text = String.Empty;
						BindSaved();
						ViewState["ID"] = gID;
					}
					else
					{
						txtACTIVE_TAB.Value = "1";
					}
				}
				else if ( e.CommandName == "Import.Run" || e.CommandName == "Import.Preview" )
				{
					if ( Page.IsValid && !bDuplicateFields )
					{
						// 10/10/2006   The temp file name is stored in the session so that it is impossible for a hacker to access. 
						string sTempFileID   = Sql.ToString(ViewState["TempFileID"]);
						string sTempFileName = Sql.ToString(Session["TempFile." + sTempFileID]);
						if ( Sql.IsEmptyString(sTempFileID) || Sql.IsEmptyString(sTempFileName) )
						{
							txtACTIVE_TAB.Value = "3";
							throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
						}

						// 10/10/2006   If there is a validation error, we want to display the mappings page. 
						// If there is no error, or if the error was during import, then show the results page. 
						txtACTIVE_TAB.Value = "4";
						ValidateMappings();
						// 12/17/2008   The results tab is now 6. 
						// 09/17/2013   Add Business Rules to import. Results tab is now 7. 
						txtACTIVE_TAB.Value = "7";
						// 02/05/2010   An ACT! import can take a long time. 
						Server.ScriptTimeout = 20 * 60;
						
						// 09/04/2010   Log the import time. 
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Begin Import");
						GenerateImport(sTempFileName, e.CommandName == "Import.Preview");
						SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "End Import");
					}
				}
				else if ( e.CommandName == "Import.Upload" )
				{
					reqFILENAME.Enabled = true;
					reqFILENAME.Validate();
					if ( Page.IsValid )
					{
						HttpPostedFile pstIMPORT = fileIMPORT.PostedFile;
						if ( pstIMPORT != null )
						{
							if ( pstIMPORT.FileName.Length > 0 )
							{
								string sFILENAME       = Path.GetFileName (pstIMPORT.FileName);
								string sFILE_EXT       = Path.GetExtension(sFILENAME);
								string sFILE_MIME_TYPE = pstIMPORT.ContentType;
								
								// 09/04/2010   ACT Imports are taking a long time.  Time the stream conversion to see where the problem lies. 
								SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "Begin Upload: " + sFILENAME);
								// 05/06/2011   We need to be able to distinguish between Excel 2003 and Excel 2007. 
								xml = SplendidImport.ConvertStreamToXml(sImportModule, SourceType(), txtCUSTOM_DELIMITER_VAL.Text, pstIMPORT.InputStream, sFILE_EXT);
								
								if ( xml.DocumentElement == null )
									throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
								
								// 08/21/2006   Don't move to next step if there is no data. 
								XmlNodeList nlRows = xml.DocumentElement.SelectNodes(sImportModule.ToLower());
								if ( nlRows.Count == 0 )
									throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
								
								// 10/10/2006   Don't store the file name in the ViewState because a hacker could find a way to access and alter it.
								// Storing the file name in the session and an ID in the view state should be sufficiently safe. 
								string sTempFileID   = Guid.NewGuid().ToString();
								string sTempFileName = Security.USER_ID.ToString() + " " + Guid.NewGuid().ToString() + " " + sFILENAME + ".xml";
								xml.Save(Path.Combine(Path.GetTempPath(), sTempFileName));
								// 01/30/2010   Were were not storing the full path in the Session for cleanup. 
								Session["TempFile." + sTempFileID] = Path.Combine(Path.GetTempPath(), sTempFileName);
								ViewState["TempFileID"] = sTempFileID;
								
								// 10/10/2006   We only need to save a small portion of the imported data as a sample. 
								// Trying to save too much data in ViewState can cause memory errors. 
								// 10/31/2006   It is taking too long to reduce the size of a large XML file. 
								// Instead, extract the three rows and attach to a new XML document. 
								XmlDocument xmlSample = new XmlDocument();
								xmlSample.AppendChild(xmlSample.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
								xmlSample.AppendChild(xmlSample.CreateElement("xml"));
								// 10/31/2006   Select only the nodes that apply.  We need to make sure to skip unrelated nodes. 
								for ( int i = 0; i < nlRows.Count && i < 3 ; i++ )
								{
									XmlNode node = nlRows[i];
									xmlSample.DocumentElement.AppendChild(xmlSample.ImportNode(node, true));
								}
								// 10/31/2006   We are getting an OutOfMemoryException.  Try to free the large XML file. 
								xml = null;
								nlRows = null;
								xml = xmlSample;
								GC.Collect();
								// 09/04/2010   Store the sample data in the Session to prevent a huge download. 
								// We are seeing a 13M html file for an 8M import file. 
								Session[sImportModule + ".xmlSample." + sFILENAME] = xml.OuterXml;
								ViewState["xmlSample"] = sImportModule + ".xmlSample." + sFILENAME;

								bool bUpdateMapping = (Request["ID"] == null);
								UpdateImportMappings(xml, bUpdateMapping, bUpdateMapping);
								txtACTIVE_TAB.Value = "4";
								SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), "End Upload: " + sFILENAME);
							}
						}
					}
					if ( xml.DocumentElement == null )
						throw(new Exception(L10n.Term("Import.LBL_NOTHING")));
				}
				else if ( e.CommandName == "Cancel" )
				{
					string sRelativePath = Sql.ToString(Application["Modules." + sImportModule + ".RelativePath"]);
					if ( Sql.IsEmptyString(sRelativePath) )
						sRelativePath = "~/" + sImportModule + "/";
					Response.Redirect(sRelativePath);
				}
				// 04/08/2012   LinkedIn OAuth events. 
				// 04/13/2012   Move the authorization URL lookup to the Soure Type change event so that the button click will do the action.  
				// Attempting window.open() after a postback has issues with popup blockers. 
				/*
				else if ( e.CommandName == "Import.SignIn" )
				{
					string sRedirectURL = Request.Url.Scheme + "://" + Request.Url.Host + Sql.ToString(Application["rootURL"]) + "Import/OAuthLanding.aspx";
					if ( this.SourceType() == "LinkedIn" )
					{
						string sLinkedInApiKey    = Sql.ToString(Application["CONFIG.LinkedIn.APIKey"   ]);
						string sLinkedInApiSecret = Sql.ToString(Application["CONFIG.LinkedIn.SecretKey"]);
						Spring.Social.LinkedIn.Connect.LinkedInServiceProvider linkedInServiceProvider = new Spring.Social.LinkedIn.Connect.LinkedInServiceProvider(sLinkedInApiKey, sLinkedInApiSecret);
						Spring.Social.OAuth1.OAuthToken oauthToken = linkedInServiceProvider.OAuthOperations.FetchRequestToken(sRedirectURL, null);
						string authenticateUrl = linkedInServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
						txtOAUTH_TOKEN        .Value = oauthToken.Value ;
						txtOAUTH_SECRET       .Value = oauthToken.Secret;
						txtOAUTH_VERIFIER     .Value = String.Empty     ;
						txtOAUTH_ACCESS_TOKEN .Value = String.Empty     ;
						txtOAUTH_ACCESS_SECRET.Value = String.Empty     ;
						
						RegisterClientScriptBlock(this.SourceType() + "Popup", "<script type=\"text/javascript\">window.open('" + authenticateUrl + "', '" + this.SourceType() + "Popup" + "', 'width=600,height=360,status=1,toolbar=0,location=0');</script>");
					}
					else if ( this.SourceType() == "Twitter" )
					{
						// 04/08/2012   We were getting (401) Unauthorized until we specified a valid Callback URL in the Twitter Application (http://dev.twitter.com). 
						string sTwitterConsumerKey    = Sql.ToString(Application["CONFIG.Twitter.ConsumerKey"   ]);
						string sTwitterConsumerSecret = Sql.ToString(Application["CONFIG.Twitter.ConsumerSecret"]);
						Spring.Social.Twitter.Connect.TwitterServiceProvider twitterServiceProvider = new Spring.Social.Twitter.Connect.TwitterServiceProvider(sTwitterConsumerKey, sTwitterConsumerSecret);
						Spring.Social.OAuth1.OAuthToken oauthToken = twitterServiceProvider.OAuthOperations.FetchRequestToken(sRedirectURL, null);
						string authenticateUrl = twitterServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
						txtOAUTH_TOKEN        .Value = oauthToken.Value ;
						txtOAUTH_SECRET       .Value = oauthToken.Secret;
						txtOAUTH_VERIFIER     .Value = String.Empty     ;
						txtOAUTH_ACCESS_TOKEN .Value = String.Empty     ;
						txtOAUTH_ACCESS_SECRET.Value = String.Empty     ;
						
						RegisterClientScriptBlock(this.SourceType() + "Popup", "<script type=\"text/javascript\">window.open('" + authenticateUrl + "', '" + this.SourceType() + "Popup" + "', 'width=600,height=360,status=1,toolbar=0,location=0');</script>");
					}
					else if ( this.SourceType() == "Facebook" )
					{
						string sFacebookAppID     = Sql.ToString(Application["CONFIG.facebook.AppID"    ]);
						string sFacebookAppSecret = Sql.ToString(Application["CONFIG.facebook.AppSecret"]);

						Spring.Social.Facebook.Connect.FacebookServiceProvider facebookServiceProvider = new Spring.Social.Facebook.Connect.FacebookServiceProvider(sFacebookAppID, sFacebookAppSecret);
						Spring.Social.OAuth2.OAuth2Parameters parameters = new Spring.Social.OAuth2.OAuth2Parameters()
						{
							RedirectUrl = sRedirectURL,
							Scope = "publish_stream"
						};
						string authenticateUrl = facebookServiceProvider.OAuthOperations.BuildAuthorizeUrl(Spring.Social.OAuth2.GrantType.ImplicitGrant, parameters);
						txtOAUTH_TOKEN        .Value = String.Empty;
						txtOAUTH_SECRET       .Value = String.Empty;
						txtOAUTH_VERIFIER     .Value = String.Empty;
						txtOAUTH_ACCESS_TOKEN .Value = String.Empty;
						txtOAUTH_ACCESS_SECRET.Value = String.Empty;
						
						RegisterClientScriptBlock(this.SourceType() + "Popup", "<script type=\"text/javascript\">window.open('" + authenticateUrl + "', '" + this.SourceType() + "Popup" + "', 'width=600,height=360,status=1,toolbar=0,location=0');</script>");
					}
					else if ( this.SourceType() == "salesforce" )
					{
						string sSalesforceConsumerKey    = Sql.ToString(Application["CONFIG.Salesforce.ConsumerKey"   ]);
						string sSalesforceConsumerSecret = Sql.ToString(Application["CONFIG.Salesforce.ConsumerSecret"]);
						Spring.Social.Salesforce.Connect.SalesforceServiceProvider salesforceServiceProvider = new Spring.Social.Salesforce.Connect.SalesforceServiceProvider(sSalesforceConsumerKey, sSalesforceConsumerSecret);
						Spring.Social.OAuth1.OAuthToken oauthToken = salesforceServiceProvider.OAuthOperations.FetchRequestToken(sRedirectURL, null);
						string authenticateUrl = salesforceServiceProvider.OAuthOperations.BuildAuthorizeUrl(oauthToken.Value, null);
						txtOAUTH_TOKEN        .Value = oauthToken.Value ;
						txtOAUTH_SECRET       .Value = oauthToken.Secret;
						txtOAUTH_VERIFIER     .Value = String.Empty     ;
						txtOAUTH_ACCESS_TOKEN .Value = String.Empty     ;
						txtOAUTH_ACCESS_SECRET.Value = String.Empty     ;
						
						RegisterClientScriptBlock(this.SourceType() + "Popup", "<script type=\"text/javascript\">window.open('" + authenticateUrl + "', '" + this.SourceType() + "Popup" + "', 'width=600,height=360,status=1,toolbar=0,location=0');</script>");
					}
				}
				*/
				else if ( e.CommandName == "Import.SignOut" )
				{
					// 04/08/2012   When the OAuth key is deleted, the access tokens become invalid, so delete them. 
					SqlProcs.spOAUTHKEYS_Delete(Security.USER_ID, this.SourceType());
					btnSignIn.Visible  = true;
					btnConnect.Visible = !btnSignIn.Visible;
					btnSignOut.Visible = !btnSignIn.Visible;
					txtOAUTH_TOKEN        .Value = String.Empty;
					txtOAUTH_SECRET       .Value = String.Empty;
					txtOAUTH_VERIFIER     .Value = String.Empty;
					txtOAUTH_ACCESS_TOKEN .Value = String.Empty;
					txtOAUTH_ACCESS_SECRET.Value = String.Empty;
					SOURCE_TYPE_CheckedChanged(null, null);
				}
				// 09/17/2013   Add Business Rules to import. 
				else if ( e.CommandName == "Rules.Cancel" )
				{
					ResetRuleText();
				}
				else if ( e.CommandName == "Rules.Add" )
				{
					ResetRuleText();
				}
				else if ( e.CommandName == "Rules.Delete" )
				{
					RulesDelete(Sql.ToGuid(e.CommandArgument));
					ResetRuleText();
				}
				else if ( e.CommandName == "Rules.Edit" )
				{
					Guid   gRULE_ID = Sql.ToGuid(e.CommandArgument);
					string sRULE_NAME    = String.Empty;
					int    nPRIORITY     = 0           ;
					string sREEVALUATION = String.Empty;
					bool   bACTIVE       = true        ;
					string sCONDITION    = String.Empty;
					string sTHEN_ACTIONS = String.Empty;
					string sELSE_ACTIONS = String.Empty;
					RulesGet(gRULE_ID, ref sRULE_NAME, ref nPRIORITY, ref sREEVALUATION, ref bACTIVE, ref sCONDITION, ref sTHEN_ACTIONS, ref sELSE_ACTIONS);
					txtRULE_ID     .Value   = gRULE_ID.ToString() ;
					txtRULE_NAME   .Text    = sRULE_NAME          ;
					txtPRIORITY    .Text    = nPRIORITY.ToString();
					chkACTIVE      .Checked = bACTIVE             ;
					txtCONDITION   .Text    = sCONDITION          ;
					txtTHEN_ACTIONS.Text    = sTHEN_ACTIONS       ;
					txtELSE_ACTIONS.Text    = sELSE_ACTIONS       ;
					Utils.SetSelectedValue(lstREEVALUATION, sREEVALUATION);
				}
				else if ( e.CommandName == "Rules.Update" )
				{
					// 12/07/2010   There does not seem to be a compelling reason to have a rule name. 
					if ( Sql.IsEmptyString(txtRULE_NAME.Text) )
						txtRULE_NAME.Text = Guid.NewGuid().ToString();
					
					Guid   gRULE_ID      = Sql.ToGuid(txtRULE_ID.Value);
					string sRULE_NAME    = txtRULE_NAME   .Text   ;
					int    nPRIORITY     = Sql.ToInteger(txtPRIORITY.Text);
					string sREEVALUATION = lstREEVALUATION.SelectedValue;
					bool   bACTIVE       = chkACTIVE      .Checked;
					string sCONDITION    = txtCONDITION   .Text   ;
					string sTHEN_ACTIONS = txtTHEN_ACTIONS.Text   ;
					string sELSE_ACTIONS = txtELSE_ACTIONS.Text   ;
					
					//reqRULE_NAME   .Enabled = true;
					reqCONDITION   .Enabled = true;
					reqTHEN_ACTIONS.Enabled = true;
					reqRULE_NAME   .Validate();
					reqCONDITION   .Validate();
					reqTHEN_ACTIONS.Validate();
					if ( reqRULE_NAME.IsValid && reqCONDITION.IsValid && reqTHEN_ACTIONS.IsValid )
					{
						// 12/12/2012   For security reasons, we want to restrict the data types available to the rules wizard. 
						SplendidRulesTypeProvider typeProvider = new SplendidRulesTypeProvider();
						RulesUtil.RulesValidate(gRULE_ID, sRULE_NAME, nPRIORITY, sREEVALUATION, bACTIVE, sCONDITION, sTHEN_ACTIONS, sELSE_ACTIONS, typeof(SplendidImportThis), typeProvider);
						RulesUpdate  (gRULE_ID, sRULE_NAME, nPRIORITY, sREEVALUATION, bACTIVE, sCONDITION, sTHEN_ACTIONS, sELSE_ACTIONS);
						ResetRuleText();
						
						// 10/23/2010   Build the ruleset so that the entire set will get validated. 
						// 12/12/2012   For security reasons, we want to restrict the data types available to the rules wizard. 
						RuleValidation validation = new RuleValidation(typeof(SplendidImportThis), typeProvider);
						RuleSet rules = RulesUtil.BuildRuleSet(dtRules, validation);
					}
				}
			}
			catch(Exception ex)
			{
				//SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
				ctlDynamicButtons.ErrorText += ex.Message;
				return;
			}
		}

		private void InitMapping()
		{
			// 10/18/2006   Initalize the fields. 
			XmlUtil.RemoveAllChildren(xmlMapping, "Fields");
			XmlNode xFields = xmlMapping.DocumentElement.SelectSingleNode("Fields");
			if ( xFields == null )
			{
				xFields = xmlMapping.CreateElement("Fields");
				xmlMapping.DocumentElement.AppendChild(xFields);
			}

			vwColumns.Sort = "colid";
			foreach ( DataRowView row in vwColumns )
			{
				XmlNode xField = xmlMapping.CreateElement("Field");
				xFields.AppendChild(xField);
				
				string sColumnName = Sql.ToString(row["Name"]);
				XmlUtil.SetSingleNodeAttribute(xmlMapping, xField, "Name", sColumnName);
				XmlUtil.SetSingleNode(xmlMapping, xField, "Type"   , Sql.ToString(row["ColumnType"]));
				XmlUtil.SetSingleNode(xmlMapping, xField, "Length" , Sql.ToString(row["Size"]));
				XmlUtil.SetSingleNode(xmlMapping, xField, "Default", String.Empty);
				XmlUtil.SetSingleNode(xmlMapping, xField, "Mapping", String.Empty);
				XmlUtil.SetSingleNode(xmlMapping, xField, "DuplicateFilter", "false");
			}
			// 12/17/2008   Display the available columns. 
			ctlDuplicateFilterChooser_Bind();
		}

		private void BindSaved()
		{
			DbProviderFactory dbf = DbProviderFactories.GetFactory();
			using ( IDbConnection con = dbf.CreateConnection() )
			{
				string sSQL;
				sSQL = "select *                                   " + ControlChars.CrLf
				     + "  from vwIMPORT_MAPS_List                  " + ControlChars.CrLf
				     + " where MODULE           = @MODULE          " + ControlChars.CrLf
				     + "   and ASSIGNED_USER_ID = @ASSIGNED_USER_ID" + ControlChars.CrLf
				     + " order by NAME                             " + ControlChars.CrLf;
				using ( IDbCommand cmd = con.CreateCommand() )
				{
					cmd.CommandText = sSQL;
					Sql.AddParameter(cmd, "@MODULE"          , sImportModule   );
					Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);

					if ( bDebug )
						RegisterClientScriptBlock("vwIMPORT_MAPS_List", Sql.ClientScriptBlock(cmd));

					using ( DbDataAdapter da = dbf.CreateDataAdapter() )
					{
						((IDbDataAdapter)da).SelectCommand = cmd;
						using ( DataTable dt = new DataTable() )
						{
							da.Fill(dt);
							vwMySaved = new DataView(dt);
							grdMySaved.DataSource = vwMySaved ;
							grdMySaved.DataBind();
						}
					}
				}
			}
		}

		protected void SOURCE_TYPE_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				// 04/08/2012   Show and hide the SignIn and Connect buttons based on the OAuth Keys. 
				btnSignIn.Visible  = true;
				btnConnect.Visible = !btnSignIn.Visible;
				btnSignOut.Visible = !btnSignIn.Visible;
				txtOAUTH_TOKEN        .Value = String.Empty;
				txtOAUTH_SECRET       .Value = String.Empty;
				txtOAUTH_VERIFIER     .Value = String.Empty;
				txtOAUTH_ACCESS_TOKEN .Value = String.Empty;
				txtOAUTH_ACCESS_SECRET.Value = String.Empty;
				if ( this.SourceType() == "LinkedIn" || this.SourceType() == "Twitter" || this.SourceType() == "Facebook" || this.SourceType() == "salesforce" )
				{
					DbProviderFactory dbf = DbProviderFactories.GetFactory();
					using ( IDbConnection con = dbf.CreateConnection() )
					{
						con.Open();
						string sSQL;
						sSQL = "select *                                   " + ControlChars.CrLf
						     + "  from vwOAUTHKEYS                         " + ControlChars.CrLf
						     + " where NAME             = @NAME            " + ControlChars.CrLf
						     + "   and ASSIGNED_USER_ID = @ASSIGNED_USER_ID" + ControlChars.CrLf;
						using ( IDbCommand cmd = con.CreateCommand() )
						{
							cmd.CommandText = sSQL;
							Sql.AddParameter(cmd, "@NAME"            , this.SourceType());
							Sql.AddParameter(cmd, "@ASSIGNED_USER_ID", Security.USER_ID);
							if ( bDebug )
								RegisterClientScriptBlock("vwOAUTHKEYS", Sql.ClientScriptBlock(cmd));

							using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
							{
								if ( rdr.Read() )
								{
									txtOAUTH_TOKEN   .Value = Sql.ToString(rdr["TOKEN"   ]);
									txtOAUTH_SECRET  .Value = Sql.ToString(rdr["SECRET"  ]);
									txtOAUTH_VERIFIER.Value = Sql.ToString(rdr["VERIFIER"]);
									btnSignIn.Visible  = false;
									btnConnect.Visible = !btnSignIn.Visible;
									btnSignOut.Visible = !btnSignIn.Visible;
								}
							}
						}
					}
				}
				else if ( this.SourceType() == "QuickBooks" )
				{
					btnSignIn.Visible  = false;
					btnConnect.Visible = !btnSignIn.Visible;
					btnSignOut.Visible = false;
					//txtACTIVE_TAB.Value = "3";
				}

			}
			catch(Exception ex)
			{
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		// 04/22/2012   Calculate a display name without spaces to get better hits with Salesforce field names. 
		private void UpdateImportColumns()
		{
			DataTable dtColumns = SplendidCache.ImportColumns(sImportModule).Copy();
			dtColumns.Columns.Add("DISPLAY_NAME_NOSPACE", Type.GetType("System.String"));
			dtColumns.Columns.Add("NAME_NOUNDERSCORE"   , Type.GetType("System.String"));
			foreach ( DataRow row in dtColumns.Rows )
			{
				string sDISPLAY_NAME = Utils.TableColumnName(L10n, sImportModule, Sql.ToString(row["DISPLAY_NAME"]));
				row["DISPLAY_NAME"        ] = sDISPLAY_NAME;
				row["DISPLAY_NAME_NOSPACE"] = sDISPLAY_NAME.Replace(" ", "");
				row["NAME_NOUNDERSCORE"   ] = Sql.ToString(row["NAME"]).Replace("_", "");
			}
			vwColumns = new DataView(dtColumns);
		}

		private void Page_Load(object sender, System.EventArgs e)
		{
			SetPageTitle(L10n.Term(sImportModule + ".LBL_MODULE_NAME"));
			// 06/04/2006   Visibility is already controlled by the ASPX page, but it is probably a good idea to skip the load. 
			// 03/17/2010   Access rights must be in reference to the Import Module. 
			this.Visible = Security.isAdmin || (Taoqi.Security.GetUserAccess(sImportModule, "import") >= 0);
			if ( !this.Visible )
			{
				// 03/17/2010   We need to rebind the parent in order to get the error message to display. 
				Parent.DataBind();
				return;
			}

			xml = new XmlDocument();
			xmlMapping = new XmlDocument();
			xmlMapping.AppendChild(xmlMapping.CreateProcessingInstruction("xml" , "version=\"1.0\" encoding=\"UTF-8\""));
			xmlMapping.AppendChild(xmlMapping.CreateElement("Import"));

			sbImport = new StringBuilder();
			try
			{
				// 11/01/2006   Max errors is now a config value. 
				nMAX_ERRORS = Sql.ToInteger(Application["CONFIG.import_max_errors"]);
				if ( nMAX_ERRORS <= 0 )
					nMAX_ERRORS = 200;

				// 07/02/2006   The required fields need to be bound manually. 
				reqNAME    .DataBind();
				reqFILENAME.DataBind();
				// 12/17/2005   Don't buffer so that the connection can be kept alive. 
				Response.BufferOutput = false;

				BindSaved();
				// 10/08/2006   Columns table is used in multiple locations.  Make sure to load only once. 
				// 04/22/2012   Calculate a display name without spaces to get better hits with Salesforce field names. 
				UpdateImportColumns();

				gID = Sql.ToGuid(Request["ID"]);
				if ( !IsPostBack )
				{
					radEXCEL.Checked = true;
					chkHasHeader.Checked = true;
					txtACTIVE_TAB.Value = "1";

					// 09/06/2012   Allow direct import into prospect list. 
					// 10/22/2013   Title was not getting set properly. 
					ViewState["ctlModuleHeader.Title"] = L10n.Term(sImportModule + ".LBL_MODULE_NAME");
					if ( sImportModule == "Contacts" || sImportModule == "Leads" || sImportModule == "Prospects" )
					{
						Guid gPROSPECT_LIST_ID = Sql.ToGuid(Request["PROSPECT_LIST_ID"]);
						if ( !Sql.IsEmptyGuid(gPROSPECT_LIST_ID) )
						{
							try
							{
								string sPROSPECT_LIST_NAME = Crm.Modules.ItemName(Application, "ProspectLists", gPROSPECT_LIST_ID);
								ViewState["PROSPECT_LIST_ID"] = gPROSPECT_LIST_ID;
								ctlModuleHeader.Title = String.Format(L10n.Term("Import.LBL_IMPORT_INTO"), sPROSPECT_LIST_NAME);
								ViewState["ctlModuleHeader.Title"] = ctlModuleHeader.Title;
								SetPageTitle(ctlModuleHeader.Title);
							}
							catch
							{
							}
						}
					}

					// 09/17/2013   Add Business Rules to import. 
					lstREEVALUATION.DataSource = SplendidCache.List("rules_reevaluation_dom");
					lstREEVALUATION.DataBind();
					foreach ( DataGridColumn col in dgRules.Columns )
					{
						if ( !Sql.IsEmptyString(col.HeaderText) )
						{
							col.HeaderText = L10n.Term(col.HeaderText);
						}
					}
					string sMODULE_TABLE = Sql.ToString(Application["Modules." + sImportModule + ".TableName"]);
					dtRuleColumns = SplendidCache.SqlColumns("vw" + sMODULE_TABLE + "_List");
					ViewState["RULE_COLUMNS"] = dtRuleColumns;
					ctlConditionSchemaRepeater.DataSource = dtRuleColumns;
					ctlThenSchemaRepeater     .DataSource = dtRuleColumns;
					ctlElseSchemaRepeater     .DataSource = dtRuleColumns;
					ctlConditionSchemaRepeater.DataBind();
					ctlThenSchemaRepeater     .DataBind();
					ctlElseSchemaRepeater     .DataBind();

					radEXCEL           .DataBind();
					radXML_SPREADSHEET .DataBind();
					radXML             .DataBind();
					radSALESFORCE      .DataBind();
					radACT_2005        .DataBind();
					radDBASE           .DataBind();
					radCUSTOM_CSV      .DataBind();
					radCUSTOM_TAB      .DataBind();
					radCUSTOM_DELIMITED.DataBind();
					radLINKEDIN        .DataBind();
					radTWITTER         .DataBind();
					radFACEBOOK        .DataBind();
					radQUICKBOOKS      .DataBind();

					//radEXCEL           .Attributes.Add("onclick", "SelectSourceFormat()");
					//radXML_SPREADSHEET .Attributes.Add("onclick", "SelectSourceFormat()");
					//radXML             .Attributes.Add("onclick", "SelectSourceFormat()");
					//radSALESFORCE      .Attributes.Add("onclick", "SelectSourceFormat()");
					//radACT_2005        .Attributes.Add("onclick", "SelectSourceFormat()");
					//radDBASE           .Attributes.Add("onclick", "SelectSourceFormat()");
					//radCUSTOM_CSV      .Attributes.Add("onclick", "SelectSourceFormat()");
					//radCUSTOM_TAB      .Attributes.Add("onclick", "SelectSourceFormat()");
					//radCUSTOM_DELIMITED.Attributes.Add("onclick", "SelectSourceFormat()");
					//radLINKEDIN        .Attributes.Add("onclick", "SelectSourceFormat()");
					//radTWITTER         .Attributes.Add("onclick", "SelectSourceFormat()");
					//radFACEBOOK        .Attributes.Add("onclick", "SelectSourceFormat()");
					//radQUICKBOOKS      .Attributes.Add("onclick", "SelectSourceFormat()");
					ctlListHeader.Title = L10n.Term("Import.LBL_LAST_IMPORTED") + " " + L10n.Term(".moduleList.", sImportModule);

					SOURCE_TYPE_CheckedChanged(null, null);
					if ( !Sql.IsEmptyGuid(gID) )
					{
						DbProviderFactory dbf = DbProviderFactories.GetFactory();
						using ( IDbConnection con = dbf.CreateConnection() )
						{
							string sSQL;
							sSQL = "select *                 " + ControlChars.CrLf
							     + "  from vwIMPORT_MAPS_Edit" + ControlChars.CrLf
							     + " where ID = @ID          " + ControlChars.CrLf;
							using ( IDbCommand cmd = con.CreateCommand() )
							{
								cmd.CommandText = sSQL;
								Sql.AddParameter(cmd, "@ID", gID);
								con.Open();

								if ( bDebug )
									RegisterClientScriptBlock("vwIMPORT_MAPS_Edit", Sql.ClientScriptBlock(cmd));

								using ( IDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow) )
								{
									if ( rdr.Read() )
									{
										// 03/16/2010   The ViewState will be the primary location for the ID. 
										ViewState["ID"] = gID;
										SourceType(Sql.ToString(rdr["SOURCE"]));
										// 03/16/2010   Update the name of the loaded import. 
										txtNAME.Text = Sql.ToString(rdr["NAME"]);
										chkHasHeader.Checked = Sql.ToBoolean(rdr["HAS_HEADER"]);
										
										string sXmlMapping = Sql.ToString (rdr["CONTENT"]);
										ViewState["xmlMapping"] = sXmlMapping;
										xmlMapping.LoadXml(sXmlMapping);
										
										// 03/16/2010   Update the loaded default values. 
										XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
										foreach ( XmlNode xField in nlFields )
										{
											string sFieldName = xField.Attributes.GetNamedItem("Name").Value;
											DynamicControl ctl = new DynamicControl(ctlDefaultsView, sFieldName);
											if ( ctl.Exists )
											{
												string sDefaultValue = XmlUtil.SelectSingleNode(xField, "Default");
												ctl.Text = sDefaultValue;
											}
										}
										
										// 10/12/2006   Extract the sample from the mapping. 
										xml = new XmlDocument();
										string sXmlSample = XmlUtil.SelectSingleNode(xmlMapping, "Sample");
										// 09/04/2010   Store the sample data in the Session to prevent a huge download. 
										// We are seeing a 13M html file for an 8M import file. 
										Session[sImportModule + ".xmlSample." + gID.ToString()] = sXmlSample;
										ViewState["xmlSample"] = sImportModule + ".xmlSample." + gID.ToString();
										XmlUtil.SetSingleNode(xmlMapping, "Sample", String.Empty);
										if ( sXmlSample.Length > 0 )
											xml.LoadXml(sXmlSample);
										
										// 09/17/2013   Add Business Rules to import. 
										dtRules = new DataTable();
										string sRULES_XML = Sql.ToString(rdr["RULES_XML"]);
										// 11/08/2013   Make sure rules exist before trying to read XML. 
										if ( !Sql.IsEmptyString(sRULES_XML) )
										{
											using ( StringReader srdr = new StringReader(sRULES_XML) )
											{
												dtRules.ReadXml(srdr);
											}
										}
										// 06/02/2014   If no rules were loaded, then we need to clear the rules table so that it gets initialized properly. 
										if ( dtRules.Rows.Count == 0 )
										{
											dtRules = null;
										}
										
										// 03/16/2010   The duplicate filters should be updated inside UpdateImportMappings. 
										UpdateImportMappings(xml, false, false);
										txtACTIVE_TAB.Value = "3";
									}
								}
							}
						}
					}
					else
					{
						XmlUtil.SetSingleNodeAttribute(xmlMapping, xmlMapping.DocumentElement, "Name", String.Empty);
						XmlUtil.SetSingleNode(xmlMapping, "Module"    , sImportModule);
						XmlUtil.SetSingleNode(xmlMapping, "SourceType", SourceType() );
						XmlUtil.SetSingleNode(xmlMapping, "HasHeader" , chkHasHeader.Checked.ToString());
						InitMapping();
					}
					
					// 09/17/2013   Add Business Rules to import. 
					if ( dtRules == null )
					{
						dtRules = new DataTable();
						DataColumn colID           = new DataColumn("ID"          , typeof(System.Guid   ));
						DataColumn colRULE_NAME    = new DataColumn("RULE_NAME"   , typeof(System.String ));
						DataColumn colPRIORITY     = new DataColumn("PRIORITY"    , typeof(System.Int32  ));
						DataColumn colREEVALUATION = new DataColumn("REEVALUATION", typeof(System.String ));
						DataColumn colACTIVE       = new DataColumn("ACTIVE"      , typeof(System.Boolean));
						DataColumn colCONDITION    = new DataColumn("CONDITION"   , typeof(System.String ));
						DataColumn colTHEN_ACTIONS = new DataColumn("THEN_ACTIONS", typeof(System.String ));
						DataColumn colELSE_ACTIONS = new DataColumn("ELSE_ACTIONS", typeof(System.String ));
						dtRules.Columns.Add(colID          );
						dtRules.Columns.Add(colRULE_NAME   );
						dtRules.Columns.Add(colPRIORITY    );
						dtRules.Columns.Add(colREEVALUATION);
						dtRules.Columns.Add(colACTIVE      );
						dtRules.Columns.Add(colCONDITION   );
						dtRules.Columns.Add(colTHEN_ACTIONS);
						dtRules.Columns.Add(colELSE_ACTIONS);
					}
					ViewState["RulesDataTable"] = dtRules;

					dgRules.DataSource = dtRules;
					dgRules.DataBind();
				}
				else
				{
					ctlModuleHeader.Title = Sql.ToString(ViewState["ctlModuleHeader.Title"]);
					SetPageTitle(ctlModuleHeader.Title);
					string sXmlMapping = Sql.ToString(ViewState["xmlMapping"]);
					if ( sXmlMapping.Length > 0 )
						xmlMapping.LoadXml(sXmlMapping);
					
					XmlUtil.SetSingleNodeAttribute(xmlMapping, xmlMapping.DocumentElement, "Name", txtNAME.Text);
					XmlUtil.SetSingleNode(xmlMapping, "Module"    , sImportModule);
					XmlUtil.SetSingleNode(xmlMapping, "SourceType", SourceType() );
					XmlUtil.SetSingleNode(xmlMapping, "HasHeader" , chkHasHeader.Checked.ToString());
					// 12/17/2008   Update the duplicate filter and then rebind. 
					DuplicateFilterUpdate();
					ctlDuplicateFilterChooser_Bind();

					// 10/10/2006   This loop updates the default values. Field mappings are updated inside UpdateImportMappings(). 
					XmlNodeList nlFields = xmlMapping.DocumentElement.SelectNodes("Fields/Field");
					foreach ( XmlNode xField in nlFields )
					{
						string sFieldName = xField.Attributes.GetNamedItem("Name").Value;
						DynamicControl ctl = new DynamicControl(ctlDefaultsView, sFieldName);
						// 08/01/2010   Fixed bug in Import.  The Exist check was failing because we were not converting TEAM_SET_LIST to TEAM_SET_NAME. 
						if ( ctl.Exists )
						{
							XmlUtil.SetSingleNode(xmlMapping, xField, "Default", ctl.Text);
						}
					}

					// 09/04/2010   Store the sample data in the Session to prevent a huge download. 
					// We are seeing a 13M html file for an 8M import file. 
					string sXmlSample = Sql.ToString(Session[Sql.ToString(ViewState["xmlSample"])]);
					if ( sXmlSample.Length > 0 )
					{
						xml.LoadXml(sXmlSample);
						UpdateImportMappings(xml, false, true);
					}
					
					string sProcessedFileID   = Sql.ToString(ViewState["ProcessedFileID"]);
					string sProcessedFileName = Sql.ToString(Session["TempFile." + sProcessedFileID]);
					string sProcessedPathName = Path.Combine(Path.GetTempPath(), sProcessedFileName);
					if ( File.Exists(sProcessedPathName) )
					{
						DataSet dsProcessed = new DataSet();
						dsProcessed.ReadXml(sProcessedPathName);
						if ( dsProcessed.Tables.Count == 1 )
						{
							PreviewGrid(dsProcessed.Tables[0]);
						}
					}
					// 09/17/2013   Add Business Rules to import. 
					dtRuleColumns = ViewState["RULE_COLUMNS"] as DataTable;
					dtRules = ViewState["RulesDataTable"] as DataTable;
					dgRules.DataSource = dtRules;
					dgRules.DataBind();
				}
				// 09/17/2013   Add Business Rules to import. 
				reqRULE_NAME   .DataBind();
				reqCONDITION   .DataBind();
				reqTHEN_ACTIONS.DataBind();
			}
			catch ( Exception ex )
			{
				ctlDynamicButtons.ErrorText = ex.Message;
			}
		}

		private void Page_PreRender(object sender, System.EventArgs e)
		{
			// 09/17/2013   Add Business Rules to import. 
			ViewState["RulesDataTable"] = dtRules;
			ViewState["xmlMapping"] = xmlMapping.OuterXml;
			// 09/04/2010   Store the sample data in the Session to prevent a huge download. 
			// We are seeing a 13M html file for an 8M import file. 
			if ( Sql.ToString(ViewState["xmlSample"]).StartsWith(sImportModule) )
				Session[Sql.ToString(ViewState["xmlSample"])] = xml.OuterXml;
			//ViewState["xmlSample" ] = xml.OuterXml;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
			this.PreRender += new System.EventHandler(this.Page_PreRender);
			ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
			this.m_sMODULE = "Import";
			// 07/21/2010   Make sure to highlight the correct menu item. 
			SetMenu(sImportModule);
			
			string sRelativePath = Sql.ToString(Application["Modules." + sImportModule + ".RelativePath"]);
			if ( Sql.IsEmptyString(sRelativePath) )
				sRelativePath = "~/" + sImportModule + "/";
			ctlDefaultsView = LoadControl(sRelativePath + "ImportDefaultsView.ascx") as SplendidControl;
			if ( ctlDefaultsView != null )
				phDefaultsView.Controls.Add(ctlDefaultsView);
			// 04/29/2008   Make use of dynamic buttons. 
			ctlDynamicButtons.AppendButtons(m_sMODULE + ".ImportView", Guid.Empty, Guid.Empty);
		}
		#endregion
	}
}


