<%@ page language="VB" autoeventwireup="false" codefile="account.aspx.vb" inherits="user_account" masterpagefile="~/dir_member/Shared/MasterPages/SiteLayout__member.Master" title="User | Account" %>

<asp:content id="ctnBodyPage" contentplaceholderid="ctnBody" runat="server">
	<div id="page-wrapper">


		<div class="row">
			<div class="col-lg-12">
				<h1 class="page-header">Account Management
				</h1>
			</div>
		</div>
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
						<asp:validationsummary id="vsum_error" runat="server" headertext="Oops..take a look, something went amiss." />
					</div>
				</div>
			</div>
		</asp:placeholder>
		<div class="row">
			<div class="col-lg-9">
				<div class="panel panel-default content-panel">
					<div class="panel-heading">
						Account detail for:
						<asp:label id="lbl_first_name" runat="server"></asp:label>
					</div>
					<div class="panel-body">
						<div class="row">
							<div class="col-sm-2">First Name: </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_first_name" runat="server" cssclass="form-control" width="75%"></asp:textbox>
								<asp:requiredfieldvalidator id="rval_first_name" enableclientscript="false" controltovalidate="txt_first_name" runat="server" display="none" errormessage="First name is required.  Anything will do."></asp:requiredfieldvalidator>
							</div>
						</div>


						<div class="row">
							<div class="col-sm-2">Last Name: </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_last_name" runat="server" cssclass="form-control" width="75%"></asp:textbox>
								<asp:requiredfieldvalidator id="rval_last_name" runat="server" enableclientscript="false" controltovalidate="txt_last_name" errormessage="Last name is required.  Be creative." display="None"></asp:requiredfieldvalidator>
							</div>
						</div>


						<div class="row">
							<div class="col-sm-2">Phone Number: </div>
							<div class="col-sm-4">
								<asp:label id="lbl_phone_number_changed" runat="server" visible="false"></asp:label>
								<div class="tooltip-demo">
									<asp:textbox id="txt_phone_number" runat="server" cssclass="form-control" width="95%" data-toggle="tooltip" data-placement="right" title="Please enter a text capable, 10 digit phone number."></asp:textbox>
								</div>
								<asp:requiredfieldvalidator id="rval_phone_number" runat="server" controltovalidate="txt_phone_number" errormessage="We need a phone number to send you text messages." display="None"></asp:requiredfieldvalidator>
								<asp:customvalidator id="cval_phone_number" runat="server" enableclientscript="false" errormessage="Hmm...this number is already a member?  Trying to develop stories on multiple pets?  If so, email us at support@petfolio.com." controltovalidate="txt_phone_number" display="none"></asp:customvalidator>
								<asp:customvalidator id="cval_phone_number__format" runat="server" enableclientscript="false" errormessage="Please enter only digits...and 10 of them. &nbsp;&nbsp; :)" controltovalidate="txt_phone_number" display="none"></asp:customvalidator>
							</div>
						</div>

						<div class="row">
							<div class="col-sm-2">Email: </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_email_address" runat="server" cssclass="form-control" width="150%"></asp:textbox>
								<asp:requiredfieldvalidator id="rval_email_address" runat="server" controltovalidate="txt_email_address" errormessage="Please provide an email so we can keep you informed." display="None"></asp:requiredfieldvalidator>
								<asp:customvalidator id="cval_email_address__member" runat="server" enableclientscript="false" errormessage="Hmm...this email is already a member?  Trying to develop stories on multiple pets?  If so, email us at support@petfolio.com." controltovalidate="txt_email_address" display="none"></asp:customvalidator>
								<asp:customvalidator id="cval_email_address" runat="server" enableclientscript="false" errormessage="Sorry...your email address doesn't appear to be valid.  Try again?" controltovalidate="txt_email_address" display="none"></asp:customvalidator>
							</div>
						</div>
						<div class="row">
							<div class="col-sm-2">Disable SMS?: </div>
							<div class="col-sm-8">
								<asp:checkbox id="chk_disable_sms" runat="server" data-toggle="tooltip" data-placement="right" title="Disabling SMS will suspend prompts to you via text but questions will still be posted online." />
								&nbsp;&nbsp;<small><b>Note:</b> Disabling SMS will suspend text prompts but they will still be posted online.</small>
							</div>
						</div>
						<div class="row">
							<div class="col-sm-2">Referral Link?: </div>
							<div class="col-sm-8">
                <div class="well well-sm">
								<asp:label id=lbl_referral_address runat="server" text="PetFolio.com?ref="></asp:label><asp:label id="lbl_referral_key" runat="server"></asp:label> or <asp:hyperlink id="hyp_email" runat="server" navigateurl="mailto:[email_address]?subject=PetFolio Referral Link&body=Share this link with a friend and unlock PetFolio features when they sign up! &nbsp;%0D%0A%0D%0A[body]%0D%0A%0D%0A"><u>Email yourself the link</u></asp:hyperlink> 
                </div>
							</div>
						</div>

						<hr />

						<div class="row">
							<div class="col-sm-2">Current Password: </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_password__current" textmode="Password" runat="server" cssclass="form-control" width="75%"></asp:textbox>
								<asp:customvalidator id="cval_password_current" runat="server" errormessage="To change your password, first enter your current password." enableclientscript="false" controltovalidate="txt_password_confirm" display="none"></asp:customvalidator>
							</div>
						</div>
						<div class="row">
							<div class="col-sm-2">New Password: </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_password" textmode="Password" runat="server" cssclass="form-control" width="75%"></asp:textbox>
							</div>
						</div>

						<div class="row">
							<div class="col-sm-2">Pwd (confirm): </div>
							<div class="col-sm-4">
								<asp:textbox id="txt_password_confirm" textmode="Password" runat="server" cssclass="form-control" width="75%"></asp:textbox>
								<asp:customvalidator id="cval_password_confirm" runat="server" errormessage="Your password and confirm need to match." enableclientscript="false" controltovalidate="txt_password_confirm" display="none"></asp:customvalidator>
							</div>
						</div>

						<div class="row">
							<div class="col-sm-2">&nbsp;</div>
							<div class="col-sm-4">
								<br />
								<asp:button id="btn_save" runat="server" text="Save" cssclass="btn btn-primary" />
							</div>
						</div>
					</div>
				</div>
			</div>
			<div class="col-lg-3">
				<div class="panel panel-default content-panel">
					<div class="panel-heading">
						Instructions/Notes
					</div>
					<div class="panel-body">
						<ul>
							<li>Your email address is also your username.
							</li>
							<li>Phone number must be exactly 10 digits.
							</li>
							<li>To change password, provide your current, new and new confirmed password.
							</li>
							<hr />
							<li>Any change to phone number or email address will have to be verified.
							</li>

						</ul>
					</div>
				</div>
			</div>
		</div>

	</div>
</asp:content>
<asp:content id="x" runat="server" contentplaceholderid="ctnJavaScriptPage">
	<script>
		$('.tooltip-demo').tooltip({
			selector: "[data-toggle=tooltip]",
			container: "body"
		});
		$("[data-toggle=popover]").popover();
	</script>
</asp:content>
