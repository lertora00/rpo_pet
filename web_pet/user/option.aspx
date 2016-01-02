<%@ Page Language="VB" AutoEventWireup="false" CodeFile="option.aspx.vb" Inherits="user_option" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" title="Pet | Story" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">
	<div id="page-wrapper">
		<div class="row">
			<div class="col-lg-12">
				<h1 class="page-header">
					User Options  
				</h1>
			</div>
		</div>
		<div class="row">
			<div class="col-lg-9">
				<div class="panel panel-default content-panel">
					<div class="panel-heading">
						User options for: <asp:label id="lbl_first_name" runat="server"></asp:label>
					</div>
					<div class="panel-body">
						Notification (by pet) settings? Other things?
					</div>
				</div>
			</div>
			<div class="col-lg-3">
				<div class="panel panel-default content-panel">
					<div class="panel-heading">
						Context Help
					</div>
					<div class="panel-body">
						Panel Body 2
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:content>
