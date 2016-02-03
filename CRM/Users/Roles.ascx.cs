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
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;

namespace Taoqi.Users
{
    /// <summary>
    ///		Summary description for Roles.
    /// </summary>
    public class Roles : SplendidControl
    {
        protected _controls.DynamicButtons ctlDynamicButtons;
        protected UniqueStringCollection arrSelectFields;
        protected Guid gID;
        protected DataView vwMain;
        protected SplendidGrid grdMain;
        protected HtmlInputHidden txtROLE_ID;
        protected bool bMyAccount;

        public bool MyAccount
        {
            get
            {
                return bMyAccount;
            }
            set
            {
                bMyAccount = value;
            }
        }

        protected void Page_Command(object sender, CommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Roles.Edit":
                        {
                            Guid gROLE_ID = Sql.ToGuid(e.CommandArgument);
                            Response.Redirect("~/Administration/ACLRoles/edit.aspx?ID=" + gROLE_ID.ToString());
                            break;
                        }
                    case "Roles.Remove":
                        {
                            Guid gROLE_ID = Sql.ToGuid(e.CommandArgument);
                            SqlProcs.spACL_ROLES_USERS_Delete(gROLE_ID, gID);
                            //Response.Redirect("view.aspx?ID=" + gID.ToString());
                            // 05/16/2008   Instead of redirecting, just rebind the grid and AJAX will repaint. 
                            BindGrid();
                            // 03/17/2010   We can only reset the current user. 
                            SplendidInit.ClearUserACL();
                            SplendidInit.LoadUserACL(Security.USER_ID);
                            break;
                        }
                    default:
                        throw (new Exception("Unknown command: " + e.CommandName));
                }
            }
            catch (Exception ex)
            {
                SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                ctlDynamicButtons.ErrorText = ex.Message;
            }
        }

        protected void BindGrid()
        {
            DbProviderFactory dbf = DbProviderFactories.GetFactory();
            using (IDbConnection con = dbf.CreateConnection())
            {
                string sSQL;
                // 04/26/2008   Build the list of fields to use in the select clause.
                sSQL = "select " + Sql.FormatSelectFields(arrSelectFields)
                     + "  from vwUSERS_ACL_ROLES" + ControlChars.CrLf
                     + " where 1 = 1            " + ControlChars.CrLf;
                using (IDbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sSQL;
                    Sql.AppendParameter(cmd, gID, "USER_ID");
                    // 04/26/2008   Move Last Sort to the database.
                    cmd.CommandText += grdMain.OrderByClause("ROLE_NAME", "asc");

                    if (bDebug)
                        RegisterClientScriptBlock("vwUSER_ACL_ROLES", Sql.ClientScriptBlock(cmd));

                    try
                    {
                        using (DbDataAdapter da = dbf.CreateDataAdapter())
                        {
                            ((IDbDataAdapter)da).SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                // 03/07/2013   Apply business rules to subpanel. 
                                this.ApplyGridViewRules("Users." + m_sMODULE, dt);
                                vwMain = dt.DefaultView;
                                grdMain.DataSource = vwMain;
                                // 09/05/2005   LinkButton controls will not fire an event unless the the grid is bound. 
                                // 04/25/2008   Enable sorting of sub panel. 
                                // 04/26/2008   Move Last Sort to the database.
                                grdMain.DataBind();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                        ctlDynamicButtons.ErrorText = ex.Message;
                    }
                }
            }
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            gID = Sql.ToGuid(Request["ID"]);
            // 03/08/2007   We need to disable the buttons unless the user is an administrator. 
            if (bMyAccount)
            {
                gID = Security.USER_ID;
            }
            if (!Sql.IsEmptyString(txtROLE_ID.Value))
            {
                try
                {
                    SqlProcs.spUSERS_ACL_ROLES_MassUpdate(gID, txtROLE_ID.Value);
                    // 05/16/2008   Instead of redirecting, just rebind the grid and AJAX will repaint. 
                    //Response.Redirect("view.aspx?ID=" + gID.ToString());
                    // 05/16/2008   If we are not going to redirect,then we must clear the value. 
                    // ���ѡ�����в��쵼���߸߲��쵼��Ϣ�������ò���
                    txtROLE_ID.Value = String.Empty;
                }
                catch (Exception ex)
                {
                    SplendidError.SystemError(new StackTrace(true).GetFrame(0), ex);
                    ctlDynamicButtons.ErrorText = ex.Message;
                }
            }
            BindGrid();

            if (!IsPostBack)
            {
                // 06/09/2006   Remove data binding in the user controls.  Binding is required, but only do so in the ASPX pages. 
                //Page.DataBind();
                // 04/28/2008   Make use of dynamic buttons. 
                Guid gASSIGNED_USER_ID = Sql.ToGuid(Page.Items["ASSIGNED_USER_ID"]);
                ctlDynamicButtons.AppendButtons("Users." + m_sMODULE, gASSIGNED_USER_ID, gID);
                // 11/19/2008   HideAll must be after the buttons are appended.
                if (bMyAccount && !(Taoqi.Security.AdminUserAccess("Users", "edit") >= 0))
                {
                    ctlDynamicButtons.HideAll();
                }
            }
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
            ctlDynamicButtons.Command += new CommandEventHandler(Page_Command);
            m_sMODULE = "ACLRoles";
            // 04/26/2008   We need to build a list of the fields used by the search clause. 
            arrSelectFields = new UniqueStringCollection();
            arrSelectFields.Add("DATE_ENTERED");
            arrSelectFields.Add("ROLE_ID");
            arrSelectFields.Add("ROLE_NAME");
            arrSelectFields.Add("DESCRIPTION");
            // 04/28/2008   Make use of dynamic buttons. 
            if (IsPostBack)
                ctlDynamicButtons.AppendButtons("Users." + m_sMODULE, Guid.Empty, Guid.Empty);
        }
        #endregion
    }
}


