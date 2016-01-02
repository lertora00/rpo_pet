<%@ page language="VB" autoeventwireup="false" codefile="error.aspx.vb" inherits="cls_error" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="System | Error" %>

<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">

	<div class="container wb">
		<div class="row centered">
			<div class="col-lg-8 col-lg-offset-2">
				<h4>Ooops.  Something went wrong.
				</h4>
				<p>
					No worries...we've notified the authorities.  
				</p>

				<div class="alert alert-warning">
					<b>Technical mumbo jumbo: </b>
					<p>
						<br />
						<asp:label id="lbl_error" runat="server"></asp:label>
					</p>
				</div>
			</div>
		</div>
	</div>
</asp:content>
