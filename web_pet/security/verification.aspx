<%@ page language="VB" autoeventwireup="false" codefile="verification.aspx.vb" inherits="security_verification" masterpagefile="~/dir_public/Shared/MasterPages/Site.Master" title="Security | Verification" %>

<asp:content id="headPage" contentplaceholderid="head" runat="server">
</asp:content>
<asp:content id="bodyPage" contentplaceholderid="ContentBody" runat="server">
	<form runat="server">
		<div class="container desc">
			<div class="col-lg-10">
				<asp:label id="lbl_pk_person_user" runat="server" visible="false"></asp:label>
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
								<asp:label id="lbl_error" runat="server"></asp:label>
							</div>
						</div>
					</div>
				</asp:placeholder>
				<div class="row well">
					<h4>Hello
					<asp:label id="lbl_first_name" runat="server"></asp:label>
						(and
						<asp:label id="lbl_pet_name" runat="server"></asp:label>)!  &nbsp;&nbsp;Welcome to our Verification page.
					</h4>
					<br />
					<p>
						You are here until you've had the chance to verify your phone number (<asp:label id="lbl_phone_number" cssclass="text-info" runat="server"></asp:label><asp:label id=lbl_phone_number__verified runat=server cssclass="verified" visible="false">verified!</asp:label>) and email address (<asp:label id="lbl_email_address" cssclass="text-info" runat="server"></asp:label><asp:label id=lbl_email_adress__verified runat=server cssclass="verified" visible="false"> verified!</asp:label>). &nbsp;Once you verify your phone number, you'll start receiving questions and can respond with answers. &nbsp;Once you confirm your email address, you can login to see your Pet's story online (please allow 24 hours for email verification).
					</p>

					<p>
						If you'd like to have us to re-send your verification text, you can click
						<asp:linkbutton id="lbtn_send_sms" runat="server" text="Re-send initial SMS"></asp:linkbutton>. &nbsp;Remember to respond with <span class="text-info">YES</span> so we can verify your number. If you'd like us to resend your email verificationyou can click
						<asp:linkbutton id="lbtn_resend_initial_email" runat="server" text="Re-send initial Email"></asp:linkbutton>.  &nbsp;Finally, if you are having any issues, please email us at <span class="text-info">Support@TryPetFolio.com.</span>
					</p>
					Once you verify successfully, simply Sign Out, then login again.
				</div>

				<div class="row">
					<asp:button id="btn_signout" runat="server" causesvalidation="false" text="Sign Out" cssclass="btn btn-primary" />
					<asp:button id="btn_setting" runat="server" causesvalidation="false" text="Change Settings" cssclass="btn btn-primary" />
				</div>
				<br />
				<br />
			</div>
		</div>
	</form>
</asp:content>

