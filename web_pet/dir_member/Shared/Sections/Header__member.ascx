<%@ control language="VB" autoeventwireup="false" codefile="Header__member.ascx.vb" inherits="dir_member_Shared_Sections_Header__member" %>
<nav class="navbar navbar-default navbar-static-top navigation-blue" role="navigation" style="margin-bottom: 0">
	<div class="navbar-header">
		<button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".sidebar-collapse">
			<span class="sr-only">Toggle navigation
			</span>
			<span class="icon-bar"></span>
			<span class="icon-bar"></span>
			<span class="icon-bar"></span>
		</button>
		<a class="navbar-brand white-title" href="../../../default.aspx">PetFolio
            <i class="fa fa-home"></i>
		</a>
		<a id="a_pet" class="navbar-brand" style="color: #FF9B09;" href="/story" runat="server">Teddy
            <i class="fa fa-paw"></i>
		</a>
	</div>
	<ul class="nav navbar-top-links navbar-right notify-row">
		<li>
			<asp:label id="lbl_username" font-bold="false" runat="server" forecolor="#ffffff"></asp:label>
		</li>
		<li>
			<asp:linkbutton id="lbtn_logout" forecolor="#FF9B09" runat="server" text="Logout"></asp:linkbutton>
		</li>
	</ul>

	<asp:placeholder id="plc_setting" runat="server">
		<ul class="nav navbar-top-links navbar-right notify-row">
			<li class="dropdown">
				<a class="dropdown-toggle white-icon" data-toggle="dropdown" href="#">
					<i class="fa fa-sort-desc fa-fw"></i>
				</a>
				<ul class="dropdown-menu dropdown-user">
					<li>
						<a href="#">
							<span class="name-title">
								<i class="fa fa-gears fa-fw"></i>
								Pet Configuration
							</span>
						</a>
					</li>
					<li>
						<a href="/user/account.aspx">
							<span class="name-title">
								<i class="fa fa-gear fa-fw"></i>
								Settings
							</span>
						</a>
					</li>
					<li class="divider"></li>
					<li>
						<a href="#">
							<span class="name-title">
								<i class="fa fa-lock fa-fw"></i>
								Privacy
							</span>
						</a>
					</li>
				</ul>
			</li>
		</ul>
	</asp:placeholder>

</nav>
