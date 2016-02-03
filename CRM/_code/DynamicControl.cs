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
using System.Xml;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Diagnostics;
// 09/18/2011   Upgrade to CKEditor 3.6.2. 
using CKEditor.NET;

namespace Taoqi
{
	/// <summary>
	/// Summary description for DynamicControl.
	/// </summary>
	public class DynamicControl
	{
		protected string          sNAME     ;
		protected SplendidControl ctlPARENT ;
		protected DataRow         rowCurrent;

		// 08/01/2010   Fixed bug in Import.  The Exist check was failing because we were not converting TEAM_SET_LIST to TEAM_SET_NAME. 
		public bool Exists
		{
			get
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 08/01/2001   Allow TEAM_SET_LIST to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_SET_LIST" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				// 08/01/2001   Allow KBTAG_SET_LIST to also imply KBTAG_NAME. 
				else if ( ctl == null && sNAME == "KBTAG_SET_LIST" )
					ctl = ctlPARENT.FindControl("KBTAG_NAME");
				return (ctl != null);
			}
		}

		public string Type
		{
			get
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 08/01/2001   Allow TEAM_SET_LIST to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_SET_LIST" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				// 08/01/2001   Allow KBTAG_SET_LIST to also imply KBTAG_NAME. 
				else if ( ctl == null && sNAME == "KBTAG_SET_LIST" )
					ctl = ctlPARENT.FindControl("KBTAG_NAME");
				if ( ctl != null )
					return ctl.GetType().Name;
				return String.Empty;
			}
		}

		public string ClientID
		{
			get
			{
				string sClientID = ctlPARENT.ID + ":" + sNAME;
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 08/01/2001   Allow TEAM_SET_LIST to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_SET_LIST" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				// 08/01/2001   Allow KBTAG_SET_LIST to also imply KBTAG_NAME. 
				else if ( ctl == null && sNAME == "KBTAG_SET_LIST" )
					ctl = ctlPARENT.FindControl("KBTAG_NAME");
				if ( ctl != null )
				{
					sClientID = ctl.ClientID;
				}
				return sClientID;
			}
		}

		public string Text
		{
			get
			{
				string sVALUE = String.Empty;
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 08/24/2009   Allow TEAM_SET_LIST to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_SET_LIST" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				// 10/25/2009   Allow KBTAG_SET_LIST to also imply KBTAG_NAME. 
				if ( ctl == null && sNAME == "KBTAG_SET_LIST" )
					ctl = ctlPARENT.FindControl("KBTAG_NAME");
				// 01/10/2008   Simplify by using the IS clause instead of GetType(). 
				if ( ctl != null )
				{
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is TextBox )
					{
						TextBox txt = ctl as TextBox;
						// 01/29/2008   Lets trim the text everwhere.  The only place that trimming is not wanted is in the terminology editor. 
						sVALUE = txt.Text.Trim();
					}
					// 04/02/2009   Add support for FCKEditor to the EditView. 
					// 09/18/2011   Upgrade to CKEditor 3.6.2. 
					else if ( ctl is CKEditorControl )
					{
						CKEditorControl txt = ctl as CKEditorControl;
						sVALUE = txt.Text;
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is Label )
					{
						Label lbl = ctl as Label;
						sVALUE = lbl.Text;
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is DropDownList )
					{
						DropDownList lst = ctl as DropDownList;
						sVALUE = lst.SelectedValue;
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is HtmlInputHidden )
					{
						HtmlInputHidden txt = ctl as HtmlInputHidden;
						sVALUE = txt.Value;
					}
					// 05/28/2007   .NET 2.0 has a new control for the hidden field and we are starting to use it in the Payments module. 
					else if ( ctl is HiddenField )
					{
						HiddenField txt = ctl as HiddenField;
						sVALUE = txt.Value;
					}
					// 09/05/2006   DetailViews place the literal in a span. 
					else if ( ctl is HtmlGenericControl )
					{
						HtmlGenericControl spn = ctl as HtmlGenericControl;
						if ( spn.Controls.Count > 0 )
						{
							if ( spn.Controls[0] is Literal )
							{
								Literal txt = spn.Controls[0] as Literal;
								sVALUE = txt.Text;
							}
						}
						else
						{
							sVALUE = spn.InnerText;
						}
					}
					// 12/30/2007   A customer needed the ability to save and restore the multiple selection. 
					else if ( ctl is ListBox )
					{
						ListBox lst = ctl as ListBox;
						if ( lst.SelectionMode == ListSelectionMode.Multiple )
						{
							XmlDocument xml = new XmlDocument();
							// 12/30/2007   The XML declaration is important as it will be used to determine if the XML is valid during rendering. 
							xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
							xml.AppendChild(xml.CreateElement("Values"));
							int nSelected = 0;
							foreach(ListItem item in lst.Items)
							{
								if ( item.Selected )
									nSelected++;
							}
							if ( nSelected > 0 )
							{
								foreach(ListItem item in lst.Items)
								{
									if ( item.Selected )
									{
										XmlNode xValue = xml.CreateElement("Value");
										xml.DocumentElement.AppendChild(xValue);
										xValue.InnerText = item.Value;
									}
								}
								// 05/07/2014   Store NULL when nothing selected for multi-selection control. 
								sVALUE = xml.OuterXml;
							}
						}
						else
						{
							sVALUE = lst.SelectedValue;
						}
					}
					// 06/16/2010   Add support for CheckBoxList. 
					else if ( ctl is CheckBoxList )
					{
						CheckBoxList lst = ctl as CheckBoxList;
						// 03/22/2013   REPEAT_DOW is a special list that returns 0 = sunday, 1 = monday, etc. 
						if ( lst.ID == "REPEAT_DOW" )
						{
							sVALUE = String.Empty;
							for ( int i = 0; i < lst.Items.Count; i++ )
							{
								if ( lst.Items[i].Selected )
									sVALUE += i.ToString();
							}
						}
						else
						{
							XmlDocument xml = new XmlDocument();
							// 12/30/2007   The XML declaration is important as it will be used to determine if the XML is valid during rendering. 
							xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
							xml.AppendChild(xml.CreateElement("Values"));
							int nSelected = 0;
							foreach(ListItem item in lst.Items)
							{
								if ( item.Selected )
									nSelected++;
							}
							if ( nSelected > 0 )
							{
								foreach(ListItem item in lst.Items)
								{
									if ( item.Selected )
									{
										XmlNode xValue = xml.CreateElement("Value");
										xml.DocumentElement.AppendChild(xValue);
										xValue.InnerText = item.Value;
									}
								}
							}
							sVALUE = xml.OuterXml;
						}
					}
					// 06/16/2010   Add support for Radio buttons. 
					else if ( ctl is RadioButtonList )
					{
						RadioButtonList lst = ctl as RadioButtonList;
						sVALUE = lst.SelectedValue;
					}
					// 03/13/2009   We need to allow a date value to be stored in a text field. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DatePicker) )
					{
						TimeZone T10n = ctlPARENT.GetT10n();
						_controls.DatePicker dt = ctl as _controls.DatePicker;
						DateTime dtVALUE = T10n.ToServerTime(dt.Value);
						if ( dtVALUE != DateTime.MinValue )
							sVALUE = dtVALUE.ToString();
					}
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimePicker) )
					{
						TimeZone T10n = ctlPARENT.GetT10n();
						_controls.DateTimePicker dt = ctl as _controls.DateTimePicker;
						DateTime dtVALUE = T10n.ToServerTime(dt.Value);
						if ( dtVALUE != DateTime.MinValue )
							sVALUE = dtVALUE.ToString();
					}
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimeEdit) )
					{
						TimeZone T10n = ctlPARENT.GetT10n();
						_controls.DateTimeEdit dt = ctl as _controls.DateTimeEdit;
						DateTime dtVALUE = T10n.ToServerTime(dt.Value);
						if ( dtVALUE != DateTime.MinValue )
							sVALUE = dtVALUE.ToString();
					}
					// 08/24/2009   If we are getting a text string from TeamSelect, then it should return the list. 
					else if ( ctl.GetType().BaseType == typeof(_controls.TeamSelect) )
					{
						_controls.TeamSelect ts = ctl as _controls.TeamSelect;
						sVALUE = ts.TEAM_SET_LIST;
					}
					// 10/21/2009   If we are getting a text string from KBTagSelect, then it should return the list. 
					else if ( ctl.GetType().BaseType == typeof(_controls.KBTagSelect) )
					{
						_controls.KBTagSelect kbt = ctl as _controls.KBTagSelect;
						sVALUE = kbt.KBTAG_SET_LIST;
					}
					// 09/09/2009   A literal control should be the clue to pull from the existing recordset. 
					else if ( ctl is Literal )
					{
						// 09/20/2009   Always check if rowCurrent is not null.  It is null for a new record. 
						if ( rowCurrent != null )
						{
							// 11/18/2007   Use the current values for any that are not defined in the edit view. 
							if ( rowCurrent.Table.Columns.Contains(sNAME) )
								sVALUE = Sql.ToString(rowCurrent[sNAME]);
						}
						// 06/12/2014   If recordset not available, then pull from control. 
						else
						{
							Literal txt = ctl as Literal;
							sVALUE = txt.Text;
						}
					}
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						sVALUE = Sql.ToString(rowCurrent[sNAME]);
				}
				return sVALUE;
			}
			set
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 04/14/2013   Allow TEAM_SET_LIST to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_SET_LIST" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				// 04/14/2013   Allow KBTAG_SET_LIST to also imply KBTAG_NAME. 
				if ( ctl == null && sNAME == "KBTAG_SET_LIST" )
					ctl = ctlPARENT.FindControl("KBTAG_NAME");
				if ( ctl != null )
				{
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is TextBox )
					{
						TextBox txt = ctl as TextBox;
						txt.Text = value;
					}
					// 04/02/2009   Add support for FCKEditor to the EditView. 
						// 09/18/2011   Upgrade to CKEditor 3.6.2. 
					else if ( ctl is CKEditorControl )
					{
						CKEditorControl txt = ctl as CKEditorControl;
						txt.Text = value;
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is Label )
					{
						Label lbl = ctl as Label;
						lbl.Text = value;
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is DropDownList )
					{
						DropDownList lst = ctl as DropDownList;
						try
						{
							// 08/19/2010   Check the list before assigning the value. 
							Utils.SetValue(lst, value);
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex);
						}
					}
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					else if ( ctl is HtmlInputHidden )
					{
						HtmlInputHidden txt = ctl as HtmlInputHidden;
						txt.Value = value;
					}
					// 06/05/2007   .NET 2.0 has a new control for the hidden field and we are starting to use it in the Payments module. 
					else if ( ctl is HiddenField )
					{
						HiddenField txt = ctl as HiddenField;
						txt.Value = value;
					}
					// 07/24/2006   Allow the text of a literal to be set. 
					else if ( ctl is Literal )
					{
						Literal txt = ctl as Literal;
						txt.Text = value;
					}
					// 09/05/2006   DetailViews place the literal in a span. 
					else if ( ctl is HtmlGenericControl )
					{
						HtmlGenericControl spn = ctl as HtmlGenericControl;
						if ( spn.Controls.Count > 0 )
						{
							if ( spn.Controls[0] is Literal )
							{
								Literal txt = spn.Controls[0] as Literal;
								txt.Text = value;
							}
						}
						else
						{
							spn.InnerText = value;
						}
					}
					// 12/30/2007   A customer needed the ability to save and restore the multiple selection. 
					else if ( ctl is ListBox )
					{
						ListBox lst = ctl as ListBox;
						try
						{
							// 12/30/2007   Require the XML declaration in the data before trying to treat as XML. 
							string sVALUE = value;
							if ( lst.SelectionMode == ListSelectionMode.Multiple && sVALUE.StartsWith("<?xml") )
							{
								XmlDocument xml = new XmlDocument();
								xml.LoadXml(sVALUE);
								XmlNodeList nlValues = xml.DocumentElement.SelectNodes("Value");
								foreach ( XmlNode xValue in nlValues )
								{
									foreach ( ListItem item in lst.Items )
									{
										if ( item.Value == xValue.InnerText )
											item.Selected = true;
									}
								}
							}
							else
							{
								// 08/19/2010   Check the list before assigning the value. 
								Utils.SetValue(lst, sVALUE);
							}
						}
						catch(Exception ex)
						{
							SplendidError.SystemWarning(new StackTrace(true).GetFrame(0), ex);
						}
					}
					// 11/15/2011   Allow a date text value to be set. This is primarily for report parameters. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DatePicker) )
					{
						_controls.DatePicker dt = ctl as _controls.DatePicker;
						dt.DateText = value;
					}
					/*
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimePicker) )
					{
						_controls.DateTimePicker dt = ctl as _controls.DateTimePicker;
					}
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimeEdit) )
					{
						_controls.DateTimeEdit dt = ctl as _controls.DateTimeEdit;
					}
					*/
					// 04/14/2103   If we are getting a text string from TeamSelect, then it should return the list. 
					else if ( ctl.GetType().BaseType == typeof(_controls.TeamSelect) )
					{
						_controls.TeamSelect ts = ctl as _controls.TeamSelect;
						ts.TEAM_SET_LIST = value;
					}
					// 04/14/2103   If we are getting a text string from KBTagSelect, then it should return the list. 
					else if ( ctl.GetType().BaseType == typeof(_controls.KBTagSelect) )
					{
						_controls.KBTagSelect kbt = ctl as _controls.KBTagSelect;
						kbt.KBTAG_SET_LIST = value;
					}
				}
			}
		}

		public string SelectedValue
		{
			get
			{
				return this.Text;
			}
			set
			{
				this.Text = value;
			}
		}
		
		public Guid ID
		{
			get
			{
				// 12/03/2005   Don't catch the Guid conversion error as this should not happen. 
				Guid gVALUE = Guid.Empty;
				Control ctl = ctlPARENT.FindControl(sNAME);
				// 08/24/2009   Allow TEAM_ID to also imply TEAM_SET_NAME. 
				if ( ctl == null && sNAME == "TEAM_ID" )
					ctl = ctlPARENT.FindControl("TEAM_SET_NAME");
				if ( ctl != null )
				{
					// 08/24/2009   If we are getting an ID from TeamSelect, then it should return the primary team ID. 
					if ( ctl.GetType().BaseType == typeof(_controls.TeamSelect) )
					{
						_controls.TeamSelect ts = ctl as _controls.TeamSelect;
						gVALUE = ts.TEAM_ID;
					}
					else
					{
						string sVALUE = this.Text;
						if ( !Sql.IsEmptyString(sVALUE) )
						{
							// 05/11/2010   We have seen where a multi-selection listbox was turned off. 
							if ( sVALUE.StartsWith("<?xml") )
							{
								XmlDocument xml = new XmlDocument();
								xml.LoadXml(sVALUE);
								XmlNodeList nlValues = xml.DocumentElement.SelectNodes("Value");
								foreach ( XmlNode xValue in nlValues )
								{
									gVALUE = Sql.ToGuid(xValue.InnerText);
									break;
								}
							}
							else
							{
								gVALUE = Sql.ToGuid(sVALUE);
							}
						}
					}
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						gVALUE = Sql.ToGuid(rowCurrent[sNAME]);
				}
				return gVALUE;
			}
			set
			{
				this.Text = value.ToString();
			}
		}

		public int IntegerValue
		{
			get
			{
				// 12/03/2005   Don't catch the Integer conversion error as this should not happen. 
				int nVALUE = 0;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					string sVALUE = this.Text;
					if ( !Sql.IsEmptyString(sVALUE) )
						nVALUE = Sql.ToInteger(sVALUE);
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						nVALUE = Sql.ToInteger(rowCurrent[sNAME]);
				}
				return nVALUE;
			}
			set
			{
				this.Text = value.ToString();
			}
		}

		// 10/22/2013   A Twitter ID is a long. 
		public long LongValue
		{
			get
			{
				long nVALUE = 0;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					string sVALUE = this.Text;
					if ( !Sql.IsEmptyString(sVALUE) )
						nVALUE = Sql.ToLong(sVALUE);
				}
				else if ( rowCurrent != null )
				{
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						nVALUE = Sql.ToInteger(rowCurrent[sNAME]);
				}
				return nVALUE;
			}
			set
			{
				this.Text = value.ToString();
			}
		}

		public Decimal DecimalValue
		{
			get
			{
				// 12/03/2005   Don't catch the Decimal conversion error as this should not happen. 
				Decimal dVALUE = Decimal.Zero;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					string sVALUE = this.Text;
					if ( !Sql.IsEmptyString(sVALUE) )
						dVALUE = Sql.ToDecimal(sVALUE);
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						dVALUE = Sql.ToDecimal(rowCurrent[sNAME]);
				}
				return dVALUE;
			}
			set
			{
				this.Text = value.ToString();
			}
		}

		public float FloatValue
		{
			get
			{
				// 12/03/2005   Don't catch the float conversion error as this should not happen. 
				float fVALUE = 0;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					string sVALUE = this.Text;
					if ( !Sql.IsEmptyString(sVALUE) )
						fVALUE = Sql.ToFloat(sVALUE);
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						fVALUE = Sql.ToFloat(rowCurrent[sNAME]);
				}
				return fVALUE;
			}
			set
			{
				this.Text = value.ToString();
			}
		}

		public bool Checked
		{
			get
			{
				bool bVALUE = false;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is CheckBox )
					{
						CheckBox chk = ctl as CheckBox;
						bVALUE = chk.Checked;
					}
					// 09/09/2009   A literal control should be the clue to pull from the existing recordset. 
					else if ( ctl is Literal )
					{
						// 09/20/2009   Always check if rowCurrent is not null.  It is null for a new record. 
						if ( rowCurrent != null )
						{
							// 11/18/2007   Use the current values for any that are not defined in the edit view. 
							if ( rowCurrent.Table.Columns.Contains(sNAME) )
								bVALUE = Sql.ToBoolean(rowCurrent[sNAME]);
						}
					}
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						bVALUE = Sql.ToBoolean(rowCurrent[sNAME]);
				}
				return bVALUE;
			}
			set
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is CheckBox )
					{
						CheckBox chk = ctl as CheckBox;
						chk.Checked = value;
					}
				}
			}
		}

		public DateTime DateValue
		{
			get
			{
				DateTime dtVALUE = DateTime.MinValue;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					TimeZone T10n = ctlPARENT.GetT10n();
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is TextBox )
					{
						TextBox txt = ctl as TextBox;
						dtVALUE = T10n.ToServerTime(txt.Text);
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DatePicker) )
					{
						_controls.DatePicker dt = ctl as _controls.DatePicker;
						dtVALUE = T10n.ToServerTime(dt.Value);
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimePicker) )
					{
						_controls.DateTimePicker dt = ctl as _controls.DateTimePicker;
						dtVALUE = T10n.ToServerTime(dt.Value);
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimeEdit) )
					{
						_controls.DateTimeEdit dt = ctl as _controls.DateTimeEdit;
						dtVALUE = T10n.ToServerTime(dt.Value);
					}
					// 06/12/2014   A literal control should be the clue to pull from the existing recordset. 
					else if ( ctl is Literal || ctl is Label )
					{
						// 06/12/2014   Always check if rowCurrent is not null.  It is null for a new record. 
						if ( rowCurrent != null )
						{
							// 11/18/2007   Use the current values for any that are not defined in the edit view. 
							if ( rowCurrent.Table.Columns.Contains(sNAME) )
								dtVALUE = Sql.ToDateTime(rowCurrent[sNAME]);
						}
						// 06/12/2014   If recordset not available, then pull from control. 
						else if( ctl is Literal )
						{
							Literal txt = ctl as Literal;
							dtVALUE = T10n.ToServerTime(txt.Text);
						}
						else if( ctl is Label )
						{
							Label txt = ctl as Label;
							dtVALUE = T10n.ToServerTime(txt.Text);
						}
					}
				}
				else if ( rowCurrent != null )
				{
					// 11/18/2007   Use the current values for any that are not defined in the edit view. 
					if ( rowCurrent.Table.Columns.Contains(sNAME) )
						dtVALUE = Sql.ToDateTime(rowCurrent[sNAME]);
				}
				return dtVALUE;
			}
			set
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					TimeZone T10n = ctlPARENT.GetT10n();
					// 03/10/2006   Compare to the type as .NET 2.0 returns the name in lowercase. We don't want to have this problem again. 
					if ( ctl is TextBox )
					{
						TextBox txt = ctl as TextBox;
						txt.Text = T10n.FromServerTime(value).ToString();
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DatePicker) )
					{
						_controls.DatePicker dt = ctl as _controls.DatePicker;
						dt.Value = T10n.FromServerTime(value);
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimePicker) )
					{
						_controls.DateTimePicker dt = ctl as _controls.DateTimePicker;
						dt.Value = T10n.FromServerTime(value);
					}
					// 03/10/2006   User controls end in "_ascx".  Compare to the base type. 
					else if ( ctl.GetType().BaseType == typeof(_controls.DateTimeEdit) )
					{
						_controls.DateTimeEdit dt = ctl as _controls.DateTimeEdit;
						dt.Value = T10n.FromServerTime(value);
					}
				}
			}
		}

		public bool Visible
		{
			get
			{
				bool bVisible = false;
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					bVisible = ctl.Visible;
				}
				return bVisible;
			}
			set
			{
				Control ctl = ctlPARENT.FindControl(sNAME);
				if ( ctl != null )
				{
					ctl.Visible = value;
				}
			}
		}

		public override string ToString()
		{
			return this.Text;
		}
		
		// 10/11/2011   Add access to WebControl properties. 
		public bool Enabled
		{
			get
			{
				bool bEnabled = false;
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					bEnabled = ctl.Enabled;
				}
				return bEnabled;
			}
			set
			{
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					ctl.Enabled = value;
				}
			}
		}

		public string CssClass
		{
			get
			{
				string sCssClass = String.Empty;
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					sCssClass = ctl.CssClass;
				}
				return sCssClass;
			}
			set
			{
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					ctl.CssClass = value;
				}
			}
		}

		public string BackColor
		{
			get
			{
				string sBackColor = String.Empty;
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					sBackColor = System.Drawing.ColorTranslator.ToHtml(ctl.BackColor);
				}
				return sBackColor;
			}
			set
			{
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					ctl.BackColor = System.Drawing.ColorTranslator.FromHtml(value);
				}
			}
		}

		public string ForeColor
		{
			get
			{
				string sForeColor = String.Empty;
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					sForeColor = System.Drawing.ColorTranslator.ToHtml(ctl.ForeColor);
				}
				return sForeColor;
			}
			set
			{
				WebControl ctl = ctlPARENT.FindControl(sNAME) as WebControl;
				if ( ctl != null )
				{
					ctl.ForeColor = System.Drawing.ColorTranslator.FromHtml(value);
				}
			}
		}

		public DynamicControl(SplendidControl ctlPARENT, string sNAME)
		{
			this.ctlPARENT  = ctlPARENT ;
			this.sNAME      = sNAME     ;
			this.rowCurrent = null      ;
		}
		
		// 11/18/2007   Use the current values for any that are not defined in the edit view. 
		public DynamicControl(SplendidControl ctlPARENT, DataRow rowCurrent, string sNAME)
		{
			this.ctlPARENT  = ctlPARENT ;
			this.sNAME      = sNAME     ;
			this.rowCurrent = rowCurrent;
		}

	}
}


