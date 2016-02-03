<%@ Control Language="c#" AutoEventWireup="false" CodeBehind="LoginView.ascx.cs" Inherits="Taoqi.Users.LoginView" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>

<div id="divLoginView">

    <div class="row" style="margin-bottom: 30px;">
        <div class="col-md-12">
            <div style="font-size: 20px; color: #ff6400; font-weight: bold;text-align:center;">517������</div>
        </div>
    </div>

    <div class="form-group" style="margin-bottom: 25px;">

        <div class="col-md-12 ">
            <asp:TextBox ID="txtUSER_NAME" CssClass="form-control" runat="server" Style="height: auto; padding: 13px 50px; background: url(../include/images/user.jpg) no-repeat;" placeholder="�ֻ���" />

            <%-- 
            <asp:HiddenField runat="server" ID="hiddenOpenId" />
            <asp:HiddenField runat="server" ID="hiddenAccessToken" />
            <asp:HiddenField runat="server" ID="hiddenNickname" />
                --%>
        </div>

    </div>


    <div class="form-group" style="margin-bottom: 25px;">

        <div class="col-md-12 ">
            <asp:TextBox ID="txtPASSWORD" CssClass="form-control" TextMode="Password" Style="height: auto; padding: 13px 50px; background: url(../include/images/pwd.jpg) no-repeat;" placeholder="����" runat="server" />
        </div>
    </div>

    <div class="form-group">
        <div class="col-md-6">
            <%-- <input type="checkbox" id="isAutoLogin" /> <span >�Զ���¼</span>--%>
        <div><span class="colorDarkGray">��û�������˺ţ�</span><a href="/member/account/register.aspx"><span class="colorOrange">����ע��</span></a></div>
        </div>
        <div class="col-md-6 textRight">
            <a href="ForgetPassword.aspx?from=/member/users/login.aspx"><span class="colorOrange">�������룿</span></a>
        </div>
    </div>


    <div class="form-group">
        <div class="col-md-12">

            <asp:Label ID="lblError" CssClass="error" EnableViewState="false" runat="server" />

            <asp:Button ID="btnLogin" CommandName="Login" OnCommand="Page_Command"
                CssClass="btn" Text='�� ¼' ToolTip='�� ¼' runat="server" Style="background-color: #ff6400; padding: 15px; color: white; height: auto; width: 100%;" />


            <%--<div style="text-align: left;">
                <div class="colorDarkGray" style="font-size: 14px; padding-top: 15px;">
                    ��Ҳ����ʹ�������˺ŵ�¼
                </div>

                <div style="padding-top: 10px;">
                    <img src="/images/Register/zc_07.png" border="0" alt="QQ��¼" title="QQ��¼" style="margin-left: 13px; cursor: pointer;" onclick="QQLoginIn();" />
                </div>

            </div>--%>

        </div>


    </div>



    <asp:Table HorizontalAlign="Center" CellPadding="0" CellSpacing="0" Style="width: 100%;" runat="server" Visible="false">

        <asp:TableRow>
            <asp:TableCell>
                <asp:Table Width="100%" CellPadding="0" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>


                            <%--
							<asp:Table ID="tblUser" Visible="<%# !Security.IsWindowsAuthentication() %>" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" HorizontalAlign="Center" Runat="server">
                                <asp:TableRow Height="25"></asp:TableRow>
                                <asp:TableRow Height="50">
									<asp:TableCell>
										<asp:Label ID="lblInstructions" Visible="<%# !Security.IsWindowsAuthentication() %>" Text='�����������û��������롣' Runat="server" />
									</asp:TableCell>
								</asp:TableRow>

								<asp:TableRow ID="trError" Visible="false" runat="server">
									<asp:TableCell ColumnSpan="2">
										<asp:Label ID="lblError" CssClass="error" EnableViewState="false" Runat="server" />
									</asp:TableCell>
								</asp:TableRow>

								<asp:TableRow>
									
									<asp:TableCell>
										<img src="../include/images/user.png" align="absmiddle" /> <asp:TextBox ID="txtUSER_NAME2" Height="28" Width="90%" Runat="server" placeholder="�û���" style="padding-left:4px;" /> &nbsp;<%# (Sql.IsEmptyString(Application["CONFIG.default_user_name"]) ? String.Empty : "(" + Sql.ToString(Application["CONFIG.default_user_name"]) + ")") %>
									</asp:TableCell>
								</asp:TableRow>
								<asp:TableRow>
									
									<asp:TableCell>
										<img src="../include/images/lock.png" align="absmiddle" /> <asp:TextBox ID="txtPASSWORD2" Height="28" Width="90%" TextMode="Password" placeholder="����" style="padding-left:4px;" Runat="server" /> &nbsp;<%# (Sql.IsEmptyString(Application["CONFIG.default_password"]) ? String.Empty : "(" + Sql.ToString(Application["CONFIG.default_password"]) + ")") %>
									</asp:TableCell>
								</asp:TableRow>

							</asp:Table>
                            --%>

                            <asp:Table Visible="false" Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" HorizontalAlign="Center" runat="server">
                                <asp:TableRow>

                                    <asp:TableCell Wrap="false">
                                        <asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Left" Style="padding-left: 20px;">
                                                    <asp:Button ID="btnLogin2" CommandName="Login" OnCommand="Page_Command" CssClass="button" Text='��¼' ToolTip='��¼' runat="server" />
                                                    &nbsp;
													<asp:HyperLink ID="lnkWorkOnline" Text='<%# L10n.Term("Offline.LNK_WORK_ONLINE"  ) %>' NavigateUrl="~/Users/ClientLogin.aspx" Visible="false" runat="server" />
                                                    <asp:HyperLink ID="lnkHTML5Client" Text='<%# L10n.Term(".LNK_HTML5_OFFLINE_CLIENT") %>' NavigateUrl="~/html5/default.aspx" Visible="false" runat="server" />
                                                </asp:TableCell>
                                                <asp:TableCell>
													
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Visible="false" runat="server">
                                    <asp:TableCell ColumnSpan="2" HorizontalAlign="Right" Style="padding-top: 10px;">
										<asp:HyperLink NavigateUrl=<%# "javascript:document.getElementById('" + txtFORGOT_USER_NAME.ClientID + "').value = document.getElementById('" + txtUSER_NAME.ClientID + "').value; toggleDisplay('" + pnlForgotPassword.ClientID + "');" %> CssClass="utilsLink" runat="server">
											<asp:Image SkinID="advanced_search" runat="server" />&nbsp;<asp:Label Text='<%# L10n.Term("Users.LBL_FORGOT_PASSWORD") %>' runat="server" />
										</asp:HyperLink>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Panel ID="pnlForgotPassword" Visible="false" Style="display: none" runat="server">
                                <asp:Table Width="100%" BorderWidth="0" CellPadding="0" CellSpacing="2" HorizontalAlign="Center" runat="server">
                                    <asp:TableRow ID="trForgotError" Visible="false" runat="server">
                                        <asp:TableCell ColumnSpan="2">
                                            <asp:Label ID="lblForgotError" CssClass="error" EnableViewState="false" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Width="30%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_USER_NAME") %></asp:TableCell>
                                        <asp:TableCell Width="70%">
                                            <asp:TextBox ID="txtFORGOT_USER_NAME" Width="180px" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell Width="30%" CssClass="dataLabel"><%# L10n.Term("Users.LBL_EMAIL") %></asp:TableCell>
                                        <asp:TableCell Width="70%">
                                            <asp:TextBox ID="txtFORGOT_EMAIL" Width="180px" runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell />
                                        <asp:TableCell>
                                            <asp:Button ID="btnForgotPassword" CommandName="ForgotPassword" OnCommand="Page_Command" CssClass="button" Text='<%# " "  + L10n.Term(".LBL_SUBMIT_BUTTON_LABEL") + " "  %>' ToolTip='<%# L10n.Term(".LBL_SUBMIT_BUTTON_TITLE") %>' runat="server" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>

        </asp:TableRow>

    </asp:Table>



</div>


