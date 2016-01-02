<%@ control language="VB" autoeventwireup="false" codefile="LeftPanel__member.ascx.vb" inherits="dir_member_Shared_Sections_LeftPanel__member" %>
<nav class="navbar-default navbar-static-side" role="navigation">
	<div class="sidebar-collapse">
		<ul class="nav" id="side-menu">
			<li>
				<asp:hyperlink id="hyp_dashboard" runat="server" font-bold="true" navigateurl="~/story">Pet Dashboard</asp:hyperlink>
			</li>
			<asp:repeater id="rpt_pet" runat="server">
				<itemtemplate>
					<li>
						<a title='<%# Eval("tagline")%>' href='<%# Eval("pk_anthology", ns_enterprise.cls_controller.fnc_get_root() & "story/default.aspx?pka={0}") %>'><%# Eval("Name")%></a>
					</li>
				</itemtemplate>
			</asp:repeater>
			<li>
				<a href="#">
					<i class="fa fa-file-code-o fa-fw"></i>
					Settings
                    <span class="fa fa-angle-right angle"></span>
				</a>
				<ul class="nav nav-second-level">
					<li>
						<a href="../../../user/account.aspx">Account
						</a>
					</li>
					<li>
						<a href="../../../user/option.aspx">Options
						</a>
					</li>
				</ul>
			</li>
		</ul>
	</div>
</nav>
