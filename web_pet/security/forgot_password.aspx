<%@ page language="VB" autoeventwireup="false" codefile="forgot_password.aspx.vb" inherits="security_forgot_password" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="Security | Login" %>

<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">
	<form runat="server">
		<div class="container desc">
			<div class="col-lg-8">
				<br />
				<asp:placeholder id="plc_success" runat="server" visible="false">
					<div class="row">
						<div class="col-lg-9">
							<div class="alert alert-success">
								<asp:label id="lbl_success" runat="server"></asp:label>
							</div>
						</div>
					</div>
				</asp:placeholder>
				<asp:placeholder id="plc_error" runat="server" visible="false">
					<div class="row">
						<div class="col-lg-9">
							<div class="alert alert-danger">
								<asp:label id="lbl_error" runat="server">Error. Invalid username or password.  Please try again.</asp:label>
							</div>
						</div>
					</div>
				</asp:placeholder>
				<div class="row">
					<div class="col-sm-2">Email: </div>
					<div class="col-sm-4">
						<asp:textbox cssclass="form-control" id="txt_email_address" runat="server"></asp:textbox>
					</div>
				</div>

				<div class="row">
					<div class="col-sm-2">&nbsp;</div>
					<div class="col-sm-4">
						<br />
						<asp:button id="btn_send" runat="server" text="Send Password" cssclass="btn btn-primary" />
					</div>
				</div>

				<br />
				<br />
			</div>
		</div>
	</form>
</asp:content>

